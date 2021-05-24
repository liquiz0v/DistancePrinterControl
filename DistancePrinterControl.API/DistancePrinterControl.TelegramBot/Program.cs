using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DistancePrinterControl.TelegramBot.DTO;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace DistancePrinterControl.TelegramBot
{
    class Program
    {
        private static TelegramBotClient Bot;
        private static RequestService _requestService;
        private static StringBuilder _stringBuilder;
        private static List<PrinterDTO> _printersCollection;
        public static async Task Main()
        {
            Bot = new TelegramBotClient(Configuration.BotToken);
            _requestService = new RequestService();
            
            _stringBuilder = new StringBuilder();

            var me = await Bot.GetMeAsync();
            Console.Title = me.Username;

            Bot.OnMessage += BotOnMessageReceived;
            Bot.OnMessageEdited += BotOnMessageReceived;
            Bot.OnCallbackQuery += BotOnCallbackQueryReceived;
            Bot.OnInlineQuery += BotOnInlineQueryReceived;
            Bot.OnInlineResultChosen += BotOnChosenInlineResultReceived;
            Bot.OnReceiveError += BotOnReceiveError;

            Bot.StartReceiving(Array.Empty<UpdateType>());
            Console.WriteLine($"Start listening for @{me.Username}");

            Console.ReadLine();
            Bot.StopReceiving();
        }

        private static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;
            if (message == null || message.Type != MessageType.Text)
                return;

            switch (message.Text.Split(' ').First())
            {
                // Send inline keyboard
                // case "/inline":
                //     await SendInlineKeyboard(message);
                //     break;

                // send custom keyboard
                case "/keyboard":
                    await SendReplyKeyboard(message);
                    break;

                // send a photo
                case "/photo":
                    await SendDocument(message);
                    break;

                // request location or contact
                case "/request":
                    await RequestContactAndLocation(message);
                    break;
                // Get list of all printers
                case "/getprinters":
                    _printersCollection = await _requestService.GetPrinters();
                    _printersCollection.ForEach(i => _stringBuilder.Append(i.Id + " "));
                    SendMessageAsync(message, _stringBuilder.ToString());
                    break;
                
                //Get current printer task
                case "/currentjob":
                    _printersCollection = await _requestService.GetPrinters();
                    _printersCollection.ForEach(i => _stringBuilder.Append(i.Id + " "));
                   
                    await SendInlineKeyboard(message, _printersCollection, typeof(JobDTO));
                    break;

                //Get files that's stored on printer
                case "/getfiles":
                    _printersCollection = await _requestService.GetPrinters();
                    _printersCollection.ForEach(i => _stringBuilder.Append(i.Id + " "));
                   
                    await SendInlineKeyboard(message, _printersCollection, typeof(FileDTO));
                    break;
                
                default:
                    await Usage(message);
                    break;
            }

            // Send inline keyboard
            // You can process responses in BotOnCallbackQueryReceived handler
            static async Task SendInlineKeyboard(Message message, List<PrinterDTO> printerDtos, Type messageType)
            {
                await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);

                // Simulate longer running task
                await Task.Delay(500);

                List<InlineKeyboardButton> keyboardButtons = new List<InlineKeyboardButton>();
                
                printerDtos.ForEach(
                    _ => keyboardButtons.Add(
                        InlineKeyboardButton.WithCallbackData(
                            _.Id.ToString(), _.Id.ToString() + " " + messageType.ToString()
                            )
                        )
                    );
                
                var inlineKeyboard = new InlineKeyboardMarkup(keyboardButtons);
                await Bot.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Choose",
                    replyMarkup: inlineKeyboard
                );
            }

            static async Task SendReplyKeyboard(Message message)
            {
                var replyKeyboardMarkup = new ReplyKeyboardMarkup(
                    new KeyboardButton[][]
                    {
                        new KeyboardButton[] {"1.1", "1.2"},
                        new KeyboardButton[] {"2.1", "2.2"},
                    },
                    resizeKeyboard: true
                );

                await Bot.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Choose printer",
                    replyMarkup: replyKeyboardMarkup

                );
            }

            static async Task SendDocument(Message message)
            {
                await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.UploadPhoto);

                const string filePath = @"Files/tux.png";
                using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                var fileName = filePath.Split(Path.DirectorySeparatorChar).Last();
                await Bot.SendPhotoAsync(
                    chatId: message.Chat.Id,
                    photo: new InputOnlineFile(fileStream, fileName),
                    caption: "Nice Picture"
                );
            }

            static async Task RequestContactAndLocation(Message message)
            {
                var RequestReplyKeyboard = new ReplyKeyboardMarkup(new[]
                {
                    KeyboardButton.WithRequestLocation("Location"),
                    KeyboardButton.WithRequestContact("Contact"),
                });
                await Bot.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Who or Where are you?",
                    replyMarkup: RequestReplyKeyboard
                );
            }

            static async Task Usage(Message message)
            {
                const string usage = "Usage:\n" +
                                     "/inline   - send inline keyboard\n" +
                                     "/keyboard - send custom keyboard\n" +
                                     "/photo    - send a photo\n" +
                                     "/request  - request location or contact";
                await Bot.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: usage,
                    replyMarkup: new ReplyKeyboardRemove()
                );
            }
            
            static async void SendMessageAsync(Message message, string text)
            {
                await Bot.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: text
                );
            }
        }

        // Process Inline Keyboard callback data
        private static async void BotOnCallbackQueryReceived(object sender,
            CallbackQueryEventArgs callbackQueryEventArgs)
        {
            var callbackQuery = callbackQueryEventArgs.CallbackQuery;
            
            if (callbackQueryEventArgs.CallbackQuery.Data.Contains("JobDTO"))
            {
                var chosenPrinterId = callbackQueryEventArgs.CallbackQuery.Data.Split(' ').First();
                var receivedData =  await _requestService.GetCurrentJob(chosenPrinterId);
                
                await Bot.SendTextMessageAsync(
                    chatId: callbackQuery.Message.Chat.Id,
                    text: $"{receivedData.State}"
                );
            }
            if (callbackQueryEventArgs.CallbackQuery.Data.Contains("FileDTO"))
            {
                var chosenPrinterId = callbackQueryEventArgs.CallbackQuery.Data.Split(' ').First();
                var receivedData =  await _requestService.GetFiles(chosenPrinterId);
                
                await Bot.SendTextMessageAsync(
                    chatId: callbackQuery.Message.Chat.Id,
                    text: $"Here's list of available files: "
                );
                
                receivedData.Files.ForEach(async i => 
                    await Bot.SendTextMessageAsync(
                    chatId: callbackQuery.Message.Chat.Id,
                    text: $"Filename - {receivedData.Files.FirstOrDefault().Name}"
                ));
                
                await Bot.SendTextMessageAsync(
                    chatId: callbackQuery.Message.Chat.Id,
                    text: $"Available memory: {receivedData.Free}, Total memory: {receivedData.Total}"
                );
            }

            // await Bot.AnswerCallbackQueryAsync(
            //     callbackQueryId: callbackQuery.Id,
            //     text: $"Received {callbackQuery.Data}"
            // );

            
        }

        #region Inline Mode

        private static async void BotOnInlineQueryReceived(object sender, InlineQueryEventArgs inlineQueryEventArgs)
        {
            Console.WriteLine($"Received inline query from: {inlineQueryEventArgs.InlineQuery.From.Id}");

            InlineQueryResultBase[] results =
            {
                // displayed result
                new InlineQueryResultArticle(
                    id: "3",
                    title: "TgBots",
                    inputMessageContent: new InputTextMessageContent(
                        "hello"
                    )
                )
            };
            await Bot.AnswerInlineQueryAsync(
                inlineQueryId: inlineQueryEventArgs.InlineQuery.Id,
                results: results,
                isPersonal: true,
                cacheTime: 0
            );
        }

        private static void BotOnChosenInlineResultReceived(object sender,
            ChosenInlineResultEventArgs chosenInlineResultEventArgs)
        {
            Console.WriteLine($"Received inline result: {chosenInlineResultEventArgs.ChosenInlineResult.ResultId}");
        }

        #endregion

        private static void BotOnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
        {
            Console.WriteLine("Received error: {0} — {1}",
                receiveErrorEventArgs.ApiRequestException.ErrorCode,
                receiveErrorEventArgs.ApiRequestException.Message
            );
        }
    }
}
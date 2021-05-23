using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DistancePrinterControl.TelegramBot.DTO;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace DistancePrinterControl.TelegramBot
{
    class Program
    {
        private static ITelegramBotClient _botClient;
        private static HttpClient _client;
        static void Main(string[] args)
        {
            _client = new HttpClient();
            
            _botClient = new TelegramBotClient("1703061729:AAElXnODZ5pFznHeqadQgpegbk31FI7Z0kY");
            var me = _botClient.GetMeAsync().Result;
            Console.WriteLine(
                $"Hello, World! I am user {me.Username} and my name is {me.FirstName}."
            );

            _botClient.OnMessage += OnMessageHandler;
            _botClient.StartReceiving();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            _botClient.StopReceiving();
        }
        private static async void OnMessageHandler(object sender, MessageEventArgs e)
        {
            var currentMessageText = e.Message.Text;
            var currentMessageChat = e.Message.Chat;
            if (e.Message.Text != null)
            {
                Console.WriteLine($"Received a text message in chat {e.Message.Chat.Id}.");
                if (currentMessageText == "/help")
                {
                    SendMessageAsync(e.Message.Chat, "Help method");
                }

                if (currentMessageText == "/version")
                {
                    HttpResponseMessage response = await _client.GetAsync($"http://localhost:5000/Printer/1/Version");
                    string responseBody = await response.Content.ReadAsStringAsync();

                    var deserializedData = JsonConvert.DeserializeObject<PrinterVersionDTO>(responseBody);
                    
                    SendMessageAsync(
                        currentMessageChat,
                        $@"The server version is: {deserializedData.Text}, API version is: {deserializedData.Api}");
                }

                if (currentMessageText == "/getprinters")
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    HttpResponseMessage response = await _client.GetAsync($"http://localhost:5000/Printer");
                    string responseBody = await response.Content.ReadAsStringAsync();

                    var deserializedData = JsonConvert.DeserializeObject<List<PrinterDTO>>(responseBody);
                    deserializedData.ForEach(i => stringBuilder.Append(i.Id + " "));
                    
                    SendMessageAsync(
                        currentMessageChat,
                        $@"List of available printers: {stringBuilder.ToString()}");
                }
            }
        }
        
        private static async void SendMessageAsync(Chat chat, string text)
        {
            await _botClient.SendTextMessageAsync(
                chatId: chat,
                text: text
            );
        }
    }
}
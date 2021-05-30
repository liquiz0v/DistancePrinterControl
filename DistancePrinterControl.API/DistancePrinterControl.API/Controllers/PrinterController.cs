using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DistancePrinterControl.API.Helpers;
using DistancePrinterControl.Database.Logic.ReadServices.Interfaces;
using DistancePrinterControl.Database.Models;
using DistancePrinterControl.Logic.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace DistancePrinterControl.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PrinterController : ControllerBase
    {
        private readonly IPrinterReadService _readService;
        
        // TODO: Remove this later, add new field to Printer model, in which will be stored creds or accs
        private readonly string _printerCreds;
        public PrinterController(IPrinterReadService readService, ICredsHelper credsHelper)
        {
            _readService = readService;
            _printerCreds = credsHelper.AuthToken;
        }
        
        [HttpGet]
        // GET PRINTERS LIST
        public List<Printer> GetPrintersAsync()
        {
            return _readService.GetPrinters();
        }
        
        [HttpGet("{printerId}/version")]
        // GET SERVER VERSION
        public async Task<object> GetServerVersion(int printerId)
        {
            StringBuilder responseBody = new StringBuilder();
            var printer = _readService.GetPrinter(printerId);

            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization
                    = new AuthenticationHeaderValue("Bearer", $@"{_printerCreds}");
                HttpResponseMessage response = await client.GetAsync($"{printer.PrinterUrl}/api/version");
                responseBody.Append(await response.Content.ReadAsStringAsync());
                
            }
            catch (Exception ex)
            {
                responseBody.Append(@$"Connection error to printer {printerId} encountered. Please, contact administrator.");
            }
            
            return responseBody.ToString();
        }

        [HttpPost("{printerId}/files")]
        public async Task<object> UploadFile([FromForm] UploadFileDTO uploadData, int printerId)
        {
            var printer = _readService.GetPrinter(printerId);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization
                    = new AuthenticationHeaderValue("Bearer", $@"{_printerCreds}");

                var multipartContent = new MultipartFormDataContent();

                if (uploadData.File.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        uploadData.File.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        var byteString = Convert.ToBase64String(fileBytes);
                        multipartContent.Add(new StringContent(byteString), uploadData.Name.ToString());
                    }
                }
            

                var response = await client.PostAsync($"{printer.PrinterUrl}/api/files/local", multipartContent);
                var responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
        }
        
        [HttpGet("{printerId}/files")]
        public async Task<object> GetFiles(int printerId) 
        {
            var printer = _readService.GetPrinter(printerId);
            
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", $@"{_printerCreds}");

            var response = await client.GetAsync($"{printer.PrinterUrl}/api/files/local");
            var responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
        
        // Get current job status
        [HttpGet("{printerId}/job")]
        public async Task<object> JobStatus(int printerId)
        {
            var printer = _readService.GetPrinter(printerId);
            
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", $@"{_printerCreds}");

            var response = await client.GetAsync($"{printer.PrinterUrl}/api/job");
            var responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
        
        [HttpGet("{printerId}/job/{command}")] 
        //TODO: handle 409 CONFLICT when printer is not operational + put available commands into AvailableOptionsEnum
        // Will be presented on demo, need to mock start action
        public async Task<object> JobIssueCommand(int printerId, string command)
        {
            var printer = _readService.GetPrinter(printerId);
            
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", $@"{_printerCreds}");

            var commandJsonObject = JsonConvert.SerializeObject(new {command = command});
            
            var content = new StringContent(commandJsonObject, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"{printer.PrinterUrl}/api/job", content);
            var responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
        
        [HttpGet("{printerId}/settings")]
        public async Task<object> GetSettings(int printerId)
        {
            var printer = _readService.GetPrinter(printerId);
            
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", $@"{_printerCreds}");
            var response = await client.GetAsync($"{printer.PrinterUrl}/api/settings");
            var responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
        
        
    }
}
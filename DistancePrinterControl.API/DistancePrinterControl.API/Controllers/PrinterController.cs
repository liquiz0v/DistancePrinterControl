using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Dapper;
using DistancePrinterControl.API.Helpers;
using DistancePrinterControl.Database.Logic.ReadServices.Interfaces;
using DistancePrinterControl.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

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
        
        [HttpGet("{printerId}/Version")]
        // GET SERVER VERSION
        public async Task<object> Test(int printerId)
        {
            var printer = _readService.GetPrinter(printerId);
            
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", $@"{_printerCreds}");
            HttpResponseMessage response = await client.GetAsync($"{printer.PrinterUrl}/api/version");
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
        
        [HttpGet("{printerId}/controls/")]
        public object Controls(int printerId)
        {
            return new
            {
                Up = "/up",
                Down = "/down",
                Left = "/left",
                Right = "/right",
                PrinterId = $@"{printerId}"
            };
        }
    }
}
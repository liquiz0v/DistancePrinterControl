using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Dapper;
using DistancePrinterControl.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace DistancePrinterControl.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PrinterController : ControllerBase
    {
        readonly string _connectionString = "Server=.;Database=DistancePrinterControl; Trusted_Connection=True; MultipleActiveResultSets=true";
        [HttpGet("{printerId}/Version")]
        // GET
        public async Task<object> Test()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", "4DB690C38FE74C2EB2BF1FC4BEDE3E06");
            HttpResponseMessage response = await client.GetAsync("http://192.168.1.125:5000/api/version");
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }

        [HttpGet]
        // GET
        public List<Printer> GetPrintersAsync()
        {
            var query = $@"select * from Printers;";

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return db.Query<Printer>(query).ToList();
            }
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
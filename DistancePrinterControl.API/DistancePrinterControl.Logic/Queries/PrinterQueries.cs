using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using DistancePrinterControl.Database.Logic.Helpers.Sql;
using DistancePrinterControl.Database.Logic.Queries.Interfaces;
using DistancePrinterControl.Database.Models;
using Microsoft.Data.SqlClient;

namespace DistancePrinterControl.Database.Logic.Queries
{
    public class PrinterQueries : IPrinterQueries
    {
        private readonly string _connectionString;
        public PrinterQueries(IConnectionStringHelper helper)
        {
            _connectionString = helper.ConnectionString;
        }
        
        public List<Printer> GetPrinters()
        {
            var query = $@"select * from Printers;";

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return db.Query<Printer>(query).ToList();
            }
        }

        public Printer GetPrinter(int printerId)
        {
            var query = $@"select * from Printers where Printers.Id = {printerId};";

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return db.Query<Printer>(query).FirstOrDefault();
            }
        }
    }
}
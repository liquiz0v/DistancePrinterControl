using System.Collections.Generic;
using DistancePrinterControl.Database.Logic.Queries;
using DistancePrinterControl.Database.Logic.Queries.Interfaces;
using DistancePrinterControl.Database.Logic.ReadServices.Interfaces;
using DistancePrinterControl.Database.Models;

namespace DistancePrinterControl.Database.Logic.ReadServices
{
    public class PrinterReadService : IPrinterReadService
    {
        private readonly IPrinterQueries _queries;
        
        public PrinterReadService(IPrinterQueries queries)
        {
            _queries = queries;
        }
        
        public List<Printer> GetPrinters()
        {
            return _queries.GetPrinters();
        }

        public Printer GetPrinter(int printerId)
        {
            return _queries.GetPrinter(printerId);
        }
    }
}
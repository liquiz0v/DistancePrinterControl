using System.Collections.Generic;
using DistancePrinterControl.Database.Models;

namespace DistancePrinterControl.Database.Logic.Queries.Interfaces
{
    public interface IPrinterQueries
    {
        List<Printer> GetPrinters();
        Printer GetPrinter(int printerId);
    }
}
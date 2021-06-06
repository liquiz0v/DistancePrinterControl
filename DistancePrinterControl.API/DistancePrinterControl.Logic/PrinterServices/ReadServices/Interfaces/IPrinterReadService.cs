using System.Collections.Generic;
using DistancePrinterControl.Database.Models;

namespace DistancePrinterControl.Database.Logic.ReadServices.Interfaces
{
    public interface IPrinterReadService
    {
        List<Printer> GetPrinters();
        Printer GetPrinter(int printerId);
    }
}
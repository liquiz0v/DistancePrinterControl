using System.Collections.Generic;
using System.Threading.Tasks;
using DistancePrinterControl.Database.Models;

namespace DistancePrinterControl.Database.Repository.Interfaces
{
    public interface IPrinterRepository
    {
        Task<Printer> GetPrinterById(int printerId);
        Task<bool> UpdatePrinterById(Printer printer);
        Task<bool> AddPrinterById(Printer printer);
        Task<bool> DeletePrinterById(int printerId);
        Task<List<Printer>> GetPrinters();
    }
}
using System.Threading.Tasks;

namespace DistancePrinterControl.Logic.WriteServices.Interfaces
{
    public interface IPrinterWriteService
    {
        Task<bool> AddPrinter(string printerUrl);
        Task<bool> RemovePrinter(int printerId);
    }
}
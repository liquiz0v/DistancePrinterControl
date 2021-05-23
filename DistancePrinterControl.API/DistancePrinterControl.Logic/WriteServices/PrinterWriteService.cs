using System.Threading.Tasks;
using DistancePrinterControl.Database.Models;
using DistancePrinterControl.Database.Models.Interfaces;
using DistancePrinterControl.Database.Repository.Interfaces;
using DistancePrinterControl.Logic.WriteServices.Interfaces;

namespace DistancePrinterControl.Logic.WriteServices
{
    public class PrinterWriteService : IPrinterWriteService
    {
        private IPrinterRepository _printerRepository;
        public PrinterWriteService(IPrinterRepository printerRepository)
        {
            _printerRepository = printerRepository;
        }
        public Task<bool> AddPrinter(string printerUrl)
        {
            return _printerRepository.AddPrinter(new Printer() {PrinterUrl = printerUrl});
        }

        public Task<bool> RemovePrinter(int printerId)
        {
            return _printerRepository.DeletePrinterById(printerId);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistancePrinterControl.Database.Models;
using DistancePrinterControl.Database.Models.Interfaces;
using DistancePrinterControl.Database.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DistancePrinterControl.Database.Repository
{
    public class PrinterRepository : IPrinterRepository
    {
        private IDistancePrinterControlContext _context;
        
        public PrinterRepository(IDistancePrinterControlContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        
        public async Task<Printer> GetPrinterById(int printerId)
        {
            return await _context.Printers.FirstOrDefaultAsync(i => i.Id == printerId);
        }

        public async Task<bool> UpdatePrinterById(Printer printer)
        {
            var currentEntity = await _context.Printers.FirstOrDefaultAsync(i => i.Id == printer.Id);

            currentEntity.Id = printer.Id;
            currentEntity.PrinterUrl = printer.PrinterUrl;

            _context.Printers.Update(currentEntity);
            return await _context.SaveEntitiesAsync();
        }

        public async Task<bool> AddPrinterById(Printer printer)
        {
            _context.Printers.Add(printer);
            return await _context.SaveEntitiesAsync();
        }

        public async Task<bool> DeletePrinterById(int printerId)
        {
            var entity = await _context.Printers.FirstOrDefaultAsync(i => i.Id == printerId);
            _context.Printers.Remove(entity);
            return await _context.SaveEntitiesAsync();
        }

        public async Task<List<Printer>> GetPrinters()
        {
            return await _context.Printers.ToListAsync();
        }
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DistancePrinterControl.Database.Models.Interfaces
{
    public interface IDistancePrinterControlContext : IDisposable
    {
        DbSet<Printer> Printers { get; set; }
        
        Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);
    }
}
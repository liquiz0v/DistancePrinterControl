using System;
using System.Threading;
using System.Threading.Tasks;
using DistancePrinterControl.Database.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DistancePrinterControl.Database.Models
{
    public class DistancePrinterControlContext : DbContext, IDistancePrinterControlContext
    {
        public DistancePrinterControlContext() 
        {
            
        }

        public DistancePrinterControlContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Printer> Printers { get; set; }
        
        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Printer>().HasData(
                new {Id = 1, PrinterUrl = "http://192.168.1.125:5000"},
                new {Id = 2, PrinterUrl = "http://192.168.1.125:5000"});
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=DistancePrinterControl; Trusted_Connection=True; MultipleActiveResultSets=true");
        }
    }
}
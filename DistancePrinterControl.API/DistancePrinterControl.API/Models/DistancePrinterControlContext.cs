using Microsoft.EntityFrameworkCore;

namespace DistancePrinterControl.API.Models
{
    public class DistancePrinterControlContext : DbContext
    {
        public DistancePrinterControlContext() 
        {
            
        }

        public DistancePrinterControlContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Printer> Printers { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Printer>().HasData(
                new {Id = 1, PrinterUrl = "http://192.168.1.125:5000"},
                new {Id = 2, PrinterUrl = "http://192.168.1.125:5000"});
        }
    }
}
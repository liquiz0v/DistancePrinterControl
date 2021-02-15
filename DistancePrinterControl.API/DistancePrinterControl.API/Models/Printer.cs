using System.ComponentModel.DataAnnotations;

namespace DistancePrinterControl.API.Models
{
    public class Printer
    {
        [Key]
        public int Id { get; set; }
        public string PrinterUrl { get; set; }
    }
}
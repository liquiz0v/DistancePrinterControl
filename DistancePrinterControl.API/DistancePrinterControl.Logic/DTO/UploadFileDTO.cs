using Microsoft.AspNetCore.Http;

namespace DistancePrinterControl.Logic.DTO
{
    public class UploadFileDTO
    {
        public string Name { get; set; }
        public IFormFile File { get; set; }
    }
}
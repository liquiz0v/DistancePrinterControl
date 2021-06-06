using System.Collections.Generic;

namespace DistancePrinterControl.TelegramBot.DTO
{
    public class FilesModel
    {
        public List<FileDTO> Files { get; set; }
        public string Free { get; set; } //Free mem
        public string Total { get; set; } //Total mem
    }
}
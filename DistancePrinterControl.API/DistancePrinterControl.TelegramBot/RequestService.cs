using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DistancePrinterControl.TelegramBot.DTO;
using Newtonsoft.Json;

namespace DistancePrinterControl.TelegramBot
{
    public class RequestService
    {
        private static HttpClient _client;
        public RequestService()
        {
            _client = new HttpClient();
        }

        public async Task<List<PrinterDTO>> GetPrinters()
        {
        
            HttpResponseMessage response = await _client.GetAsync($"http://localhost:5000/Printer");
            string responseBody = await response.Content.ReadAsStringAsync();
            
            var deserializedData = JsonConvert.DeserializeObject<List<PrinterDTO>>(responseBody);

            return deserializedData;
        }

        public async Task<JobDTO> GetCurrentJob(string printerId)
        {
            HttpResponseMessage response = await _client.GetAsync($"http://localhost:5000/Printer/{printerId}/job");
            string responseBody = await response.Content.ReadAsStringAsync();
            
            var deserializedData = JsonConvert.DeserializeObject<JobDTO>(responseBody);
            return deserializedData;
        }

        public async Task<FilesModel> GetFiles(string printerId)
        {
            //TODO: put into method
            HttpResponseMessage response = await _client.GetAsync($"http://localhost:5000/Printer/{printerId}/files");
            string responseBody = await response.Content.ReadAsStringAsync();
            
            var deserializedData = JsonConvert.DeserializeObject<FilesModel>(responseBody);
            return deserializedData;
        }
    }
}
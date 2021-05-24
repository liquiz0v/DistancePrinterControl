using Microsoft.Extensions.Configuration;

namespace DistancePrinterControl.API.Helpers
{
    public interface IUrlHelper
    {
        string ServerUrl { get; set; }
    }
    public class UrlHelper : IUrlHelper
    {
        public string ServerUrl { get; set; }

        public UrlHelper(IConfiguration configuration)
        {
            ServerUrl = configuration[""];
        }
    }
}
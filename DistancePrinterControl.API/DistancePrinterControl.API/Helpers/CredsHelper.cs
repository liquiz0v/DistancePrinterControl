using Microsoft.Extensions.Configuration;

namespace DistancePrinterControl.API.Helpers
{
    public interface ICredsHelper
    {
        public string AuthToken { get; }
    }
    
    public class CredsHelper : ICredsHelper
    {
        public string AuthToken { get; set; }

        public CredsHelper(IConfiguration configuration)
        {
            AuthToken = configuration["AccessToken"];
        }
    }
}
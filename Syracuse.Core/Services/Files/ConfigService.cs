using System.Threading.Tasks;
using Newtonsoft.Json;
using Syracuse.Mobitheque.Core.Models;

namespace Syracuse.Mobitheque.Core.Services.Files
{
    public class ConfigService : FileService, IConfigService
    {
        private Config config;
        private const string configFileName = "config.json";

        public async Task<Config> GetConfig()
        {
            if (config == null)
            {
                var configText = await this.GetRootRessource(configFileName);

                if (configText != string.Empty)
                    this.config = JsonConvert.DeserializeObject<Config>(configText);
            }
            return this.config;
        }
    }
}

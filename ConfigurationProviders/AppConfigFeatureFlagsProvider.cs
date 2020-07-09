using Amazon;
using Amazon.AppConfig;
using Amazon.AppConfig.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace FeatureManagementSandbox.ConfigurationProviders
{
    public class AppConfigFeatureFlagsProvider : ConfigurationProvider
    {
        private static string _awsConfigurationVersion = "1";
        private static string _clientId;
        private const string ClientId = "ClientID";
        private static readonly RegionEndpoint Region = RegionEndpoint.USWest2;
        private const ushort ReloadPeriodMs = 10000;
        private const string Environment = "dev";
        private const string ApplicationName = "appconfig-app-name";
        private const string ConfigurationName = "test";

        private static AmazonAppConfigClient _appConfigClient;

        public AppConfigFeatureFlagsProvider()
        {
            InitializeAppConfig(Region);
            _clientId = System.Environment.GetEnvironmentVariable(ClientId);
            ChangeToken.OnChange(() =>
            {
                var cancellationTokenSource = new CancellationTokenSource(ReloadPeriodMs);
                var cancellationChangeToken = new CancellationChangeToken(cancellationTokenSource.Token);
                return cancellationChangeToken;
            }, async () => { await LoadAsync(); });
        }

        public async Task LoadAsync()
        {
            var allFeatureFlags = await GetAllKeyValuePairs(ApplicationName, _awsConfigurationVersion, _clientId, ConfigurationName, Environment);
            if (allFeatureFlags != null)
            {
                if (!Data.ContentEquals(allFeatureFlags))
                {
                    Data = allFeatureFlags;
                    OnReload();
                }
            };
        }

        [Obsolete("Synchronous Load() is not used. Use LoadAsync() instead.")]
        public override void Load()
        {
            /*
              Use the synchronous configuration loading at your own risk - the method is public and if used in incorrect circumstances, it can deadlock.
              We propose asynchronous execution and health checking in order to determine if the application is ready to serve traffic.
            */
        }

        public static void InitializeAppConfig(RegionEndpoint region)
        {
            if (region == null) throw new ArgumentNullException(nameof(region));

            _appConfigClient = new AmazonAppConfigClient(new AmazonAppConfigConfig
            {
                RegionEndpoint = region
            });
        }

        private static async Task<Dictionary<string, string>> GetAllKeyValuePairs(string appName, string configVersion, string clientId, string configName, string env)
        {
            var request = new GetConfigurationRequest
            {
                Application = appName,
                ClientConfigurationVersion = _awsConfigurationVersion,
                ClientId = clientId,
                Configuration = configName,
                Environment = env

            };

            var configValues =  await _appConfigClient.GetConfigurationAsync(request);
            if (_awsConfigurationVersion == configValues.ConfigurationVersion)
                return null;

            _awsConfigurationVersion = configValues.ConfigurationVersion;
            return DeserializeFromStream(configValues.Content);
        }

        public static Dictionary<string,string> DeserializeFromStream(MemoryStream stream)
        {
            var serializer = new JsonSerializer();
            using var sr = new StreamReader(stream);
            using var jsonTextReader = new JsonTextReader(sr);
            var valuesObject =  serializer.Deserialize<Dictionary<string,string>>(jsonTextReader);
            stream.Dispose();
            return valuesObject;
        }
    }
}

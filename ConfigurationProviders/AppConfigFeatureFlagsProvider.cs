using Amazon;
using Amazon.AppConfig;
using Amazon.AppConfig.Model;
using FeatureManagementSandbox.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FeatureManagementSandbox.ConfigurationProviders
{
    public class AppConfigFeatureFlagsProvider : ConfigurationProvider
    {
        private static string _awsConfigurationVersion = "1";
        private static string _clientId;
        private const string ClientId = "ClientID";
        private static readonly RegionEndpoint Region = RegionEndpoint.USWest2;
        private const int ReloadPeriodMs = 10000;
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
            var newData = await GetConfigurationValues(ApplicationName, _awsConfigurationVersion, _clientId, ConfigurationName, Environment);
            if (newData != null)
            {
                if (!Data.ContentEquals(newData))
                {
                    Data = newData;
                    OnReload();
                }
            }
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

        private async Task<Dictionary<string,string>> GetConfigurationValues(string appName, string configVersion, string clientId, string configName, string env)
        {
            var request = new GetConfigurationRequest
            {
                Application = appName,
                ClientConfigurationVersion = _awsConfigurationVersion,
                ClientId = clientId,
                Configuration = configName,
                Environment = env

            };

            var configValues = await _appConfigClient.GetConfigurationAsync(request);
            if (_awsConfigurationVersion == configValues.ConfigurationVersion || configValues.Content == null ||
                configValues.Content.Length == 0)
                return null;
            
            _awsConfigurationVersion = configValues.ConfigurationVersion;
            var parsedValues =  new Dictionary<string, string>(JsonConfigurationFileParser.Parse(configValues.Content));
            configValues.Content.Dispose();
            return parsedValues;
        }
    }
}

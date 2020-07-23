using Amazon.SimpleSystemsManagement.Model;
using FeatureManagementSandbox.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FeatureManagementSandbox.ConfigurationProviders
{
    public class SsmConfigurationProvider : ConfigurationProvider
    {
        private static ISsmWrapper _client;
        private const int ReloadPeriodMs = 10000;
        private readonly string _featureFlagAwsPath = "/ssm/parameter/path/";


        public SsmConfigurationProvider()
        {
            InitializeSsm();
            ChangeToken.OnChange(() =>
            {
                var cancellationTokenSource = new CancellationTokenSource(ReloadPeriodMs);
                var cancellationChangeToken = new CancellationChangeToken(cancellationTokenSource.Token);
                return cancellationChangeToken;
            }, async () => { await LoadAsync(); });
        }

        public async Task LoadAsync()
        {
            var newData = await GetAllKeyValuePairs();
            if (!Data.ContentEquals(newData))
            {
                Data = newData;
                OnReload();
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

        public static void InitializeSsm(string regionName = null)
        {
            _client = new SsmWrapper(regionName);
        }

        private async Task<Dictionary<string, string>> GetAllKeyValuePairs()
        {
            var featureFlagParameters = await _client.GetParameterStoreParametersByPathAsync(_featureFlagAwsPath);
            return ParseFeatureFlagsFromParameters(featureFlagParameters);
        }

        private static Dictionary<string, string> ParseFeatureFlagsFromParameters(IEnumerable<Parameter> featureFlags)
        {
            var list = new List<JProperty>();
            foreach (var flag in featureFlags)
            {
                FlagTypes flagType;
                var innerProp = new List<JProperty>();
                var flagValueList = flag.Value == "," ? new[] { flag.Value } : flag.Value.Split(',');
                if (flagValueList.Length > 1)
                {
                    foreach (var item in flagValueList)
                    {
                        flagType = FlagTypeHelper.GetFlagType(item);
                        if (flagType == FlagTypes.Invalid)
                            continue;
                        innerProp.Add(ObjectHelper.GetFmFlagProperty(flagType, item));
                    }
                    list.Add(ObjectHelper.GetFmFlagObject(flag.Name, innerProp));
                }

                flagType = FlagTypeHelper.GetFlagType(flag.Value);
                if (flagType == FlagTypes.Invalid)
                    continue;
                list.Add(ObjectHelper.GetFmFlagObject(flag.Name, flag.Value, flagType.ToString()));
            }

            var finalObj = ObjectHelper.ConstructFinalObject(JPropertyComparer.GetUniqueItemList(list));
            var parsedValues = JsonConfigurationFileParser.Parse(finalObj);
            return new Dictionary<string, string>(parsedValues);
        }
    }
}

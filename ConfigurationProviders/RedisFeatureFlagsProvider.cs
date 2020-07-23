using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using StackExchange.Redis;

namespace FeatureManagementSandbox.ConfigurationProviders
{
    public class RedisFeatureFlagsProvider : ConfigurationProvider
    {
        private const string ConnectionString = "localhost";
        private const string SearchPattern = "RedisFeature*";
        private const ushort ReloadPeriodMs = 10000;
        private static ConnectionMultiplexer _redisClient;

        public RedisFeatureFlagsProvider()
        {
            InitializeRedis(ConnectionString);
            ChangeToken.OnChange(() =>
            {
                var cancellationTokenSource = new CancellationTokenSource(ReloadPeriodMs);
                var cancellationChangeToken = new CancellationChangeToken(cancellationTokenSource.Token);
                return cancellationChangeToken;
            }, Load);
        }
        public override void Load()
        {
            var allFeatureFlags = GetAllKeyValuePairs(_redisClient, ConnectionString, SearchPattern);

            if (!Data.ContentEquals(allFeatureFlags))
            {
                Data = allFeatureFlags;
                OnReload();
            }
        }

        public static void InitializeRedis(string server)
        {
            _redisClient = ConnectionMultiplexer.Connect(server);
        }

        private static Dictionary<string, string> GetAllKeyValuePairs(IConnectionMultiplexer host, string hostAddress, string pattern = null)
        {
            if (string.IsNullOrEmpty(hostAddress)) throw new ArgumentNullException(nameof(hostAddress));
            var db = host.GetDatabase();
            var keys = host.GetServer(hostAddress, 6379).Keys(pattern: pattern);
            var keysArr = keys.Select(key => (string)key).ToArray();
            var keyValueMap = keysArr.ToDictionary(key => key, key => db.StringGet(key).ToString());
            return keyValueMap;
        }
    }
}
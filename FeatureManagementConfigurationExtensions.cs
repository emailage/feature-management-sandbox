using System;
using System.Collections.Generic;
using System.Linq;
using FeatureManagementSandbox.ConfigurationSources;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace FeatureManagementSandbox
{
    public static class FeatureManagementConfigurationExtensions
    {
        public static IConfigurationBuilder AddFeatureFlagsConfiguration(this IConfigurationBuilder configuration)
        {
            configuration.Add(new DummyConfigurationSource());
            //configuration.Add(new RedisConfigurationSource());
            //configuration.Add(new AppConfigConfigurationSource());
            return configuration;
        }

        public static string[] ToStringArray(this RedisValue[] values)
        {
            if (values == null) return null;
            if (values.Length == 0) return new string[0];
            return Array.ConvertAll(values, x => (string)x);
        }

        public static bool ContentEquals<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Dictionary<TKey, TValue> otherDictionary)
        {
            return (otherDictionary ?? new Dictionary<TKey, TValue>())
                .OrderBy(kvp => kvp.Key)
                .SequenceEqual((dictionary ?? new Dictionary<TKey, TValue>())
                    .OrderBy(kvp => kvp.Key));
        }
    }
}
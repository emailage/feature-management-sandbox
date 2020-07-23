using FeatureManagementSandbox.ConfigurationSources;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FeatureManagementSandbox
{
    public static class FeatureManagementConfigurationExtensions
    {
        public static IConfigurationBuilder AddFeatureFlagsConfiguration(this IConfigurationBuilder configuration)
        {
            configuration.Add(new DummyConfigurationSource());
            //configuration.Add(new AppConfigConfigurationSource());
            //configuration.Add(new RedisConfigurationSource());
            //configuration.Add(new SsmConfigurationSource());
            return configuration;
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
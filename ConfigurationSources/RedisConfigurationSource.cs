using FeatureManagementSandbox.ConfigurationProviders;
using Microsoft.Extensions.Configuration;

namespace FeatureManagementSandbox.ConfigurationSources
{
    public class RedisConfigurationSource : IConfigurationSource
    {
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new RedisFeatureFlagsProvider();
        }
    }
}

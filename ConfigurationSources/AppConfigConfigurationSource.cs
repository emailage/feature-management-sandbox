using FeatureManagementSandbox.ConfigurationProviders;
using Microsoft.Extensions.Configuration;

namespace FeatureManagementSandbox.ConfigurationSources
{
    public class AppConfigConfigurationSource : IConfigurationSource
    {
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new AppConfigFeatureFlagsProvider();
        }
    }
}

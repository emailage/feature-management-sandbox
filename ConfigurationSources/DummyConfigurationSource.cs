using FeatureManagementSandbox.ConfigurationProviders;
using Microsoft.Extensions.Configuration;

namespace FeatureManagementSandbox.ConfigurationSources
{
    public class DummyConfigurationSource : IConfigurationSource
    {
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new DummyFeatureFlagsProvider();
        }
    }
}
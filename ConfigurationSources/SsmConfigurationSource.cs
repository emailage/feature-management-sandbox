using FeatureManagementSandbox.ConfigurationProviders;
using Microsoft.Extensions.Configuration;

namespace FeatureManagementSandbox.ConfigurationSources
{
    public class SsmConfigurationSource : IConfigurationSource
    {
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new SsmConfigurationProvider();
        }
    }
}

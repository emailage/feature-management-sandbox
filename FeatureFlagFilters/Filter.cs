using FeatureManagementSandbox.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;
using System.Linq;
using System.Threading.Tasks;

namespace FeatureManagementSandbox.FeatureFlagFilters
{
    public class Filter
    {
        [FilterAlias("Filter")]
        public class TestFilter : IFeatureFilter
        {
            private const string LocalHostName = "localhost";
            public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context)
            {
                var settings = context.Parameters.Get<FilterSettings>() ?? new FilterSettings();
                var systemRegion = AwsHelper.GetAwsRegion();
                return Task.FromResult(settings.Settings.Any(x => x.Location == systemRegion || x.Host.ToLower() == LocalHostName));
            }
        }
    }
}

using System.Collections.Generic;

namespace FeatureManagementSandbox.FeatureFlagFilters
{
    public class FilterSettings
    {
        public List<Settings> Settings { get; set; }
    }

    public class Settings
    {
        public string Location { get; set; }
        public string Host { get; set; }
    }
}

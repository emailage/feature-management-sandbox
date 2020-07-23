using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace FeatureManagementSandbox.Utilities
{
    public static class JPropertyComparer
    {
        public static List<JProperty> GetUniqueItemList(List<JProperty> list)
        {
            var unique = new Dictionary<int, JProperty>();
            foreach (var prop in list)
            {
                var key = GetDataHashCode(prop);
                if (!unique.ContainsKey(key))
                    unique.Add(key, prop);
            }

            return unique.Values.ToList();
        }

        private static int GetDataHashCode(JProperty obj)
        {
            return (obj.Name.ToLower().GetHashCode() + obj.Type.GetHashCode()).GetHashCode();
        }
    }
}

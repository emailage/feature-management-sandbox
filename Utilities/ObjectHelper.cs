using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace FeatureManagementSandbox.Utilities
{
    public static class ObjectHelper
    {
        public static JObject ConstructFinalObject(List<JProperty> properties)
        {
            return
                new JObject(
                    new JProperty("FeatureManagement",
                        new JObject(
                            from p in properties
                            select new JProperty(p)
                        )));
        }

        public static JProperty GetFmFlagObject(string key, List<JProperty> properties)
        {
            return
                new JProperty(key,
                    new JObject(
                        new JProperty("EnabledFor",
                            new JArray(
                                new JObject(
                                    new JProperty("Name", "Filter"),
                                    new JProperty("Parameters",
                                        new JObject(
                                            new JProperty("Settings",
                                                new JArray(
                                                    new JObject(from p in properties
                                                        select new JProperty(p)
                                                    ))))))))));
        }

        public static JProperty GetFmFlagObject(string key, string value, string valueType)
        {
            return
                new JProperty(key,
                    new JObject(
                        new JProperty("EnabledFor",
                            new JArray(
                                new JObject(
                                    new JProperty("Name", "Filter"),
                                    new JProperty("Parameters",
                                        new JObject(
                                            new JProperty("Settings",
                                                new JArray(
                                                    new JObject(
                                                        new JProperty(valueType, value)
                                                    ))))))))));
        }

        public static JProperty GetFmFlagProperty(FlagTypes valueType, string value)
        {
            return
                new JProperty(valueType.ToString(), value);
        }
    }
}

using Amazon;
using Amazon.Util;
using System;

namespace FeatureManagementSandbox.Utilities
{
    public class AwsHelper
    {
        public static string GetAwsRegion() =>
            !string.IsNullOrEmpty(EC2InstanceMetadata.InstanceId)
                ? EC2InstanceMetadata.Region.SystemName
                : AWSConfigs.AWSRegion ?? Environment.GetEnvironmentVariable("AWS_DEFAULT_REGION") ?? "us-west2";
    }
}

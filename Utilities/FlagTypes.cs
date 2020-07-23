namespace FeatureManagementSandbox.Utilities
{
    public enum FlagTypes
    {
        Invalid = 0,
        Location = 1,
        Host = 2,
    }

    public class FlagTypeHelper
    {
        public static FlagTypes GetFlagType(string flagValue)
        {
            return flagValue switch
            {
                "us-west-2" => FlagTypes.Location,
                "eu-central-1" => FlagTypes.Location,
                "eu-west-1" => FlagTypes.Location,
                "us-east-1" => FlagTypes.Location,
                "us-west-1" => FlagTypes.Location,
                "ap-southeast-2" => FlagTypes.Location,
                "localhost" => FlagTypes.Host,
                "," => FlagTypes.Invalid,
                _ => FlagTypes.Invalid
            };
        }
    }
}

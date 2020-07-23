using Amazon;
using Amazon.Runtime;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using Amazon.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeatureManagementSandbox.Utilities
{
    public class SsmWrapper : ISsmWrapper
    {
        private readonly AmazonSimpleSystemsManagementClient _client;

        public SsmWrapper(string regionName)
        {
            _client = CreateSsmClient(regionName);
        }

        public async Task<List<Parameter>> GetParameterStoreParametersByPathAsync(string path)
        {
            var parameters = new List<Parameter>();
            string nextToken = null;

            do
            {
                var request = new GetParametersByPathRequest
                {
                    Path = path,
                    NextToken = nextToken,
                    Recursive = true,
                    WithDecryption = true
                };
                var response = await _client.GetParametersByPathAsync(request);
                parameters.AddRange(response.Parameters);
                nextToken = response.NextToken;
            } while (nextToken != null);

            return parameters;
        }

        private static AmazonSimpleSystemsManagementClient CreateSsmClient(string regionName)
        {
            if (string.IsNullOrWhiteSpace(regionName))
            {
                regionName = (EC2InstanceMetadata.Region ?? RegionEndpoint.USWest2).SystemName;
            }

            var region = RegionEndpoint.GetBySystemName(regionName);

            AmazonSimpleSystemsManagementClient client;

            client = TryGetSessionAwsCredentials(out SessionAWSCredentials credentials) ?
                new AmazonSimpleSystemsManagementClient(credentials, region) :
                new AmazonSimpleSystemsManagementClient(region);
            
            return client;
        }

        private static bool TryGetSessionAwsCredentials(out SessionAWSCredentials credentials)
        {
            credentials = null;
#if !DEBUG
            return false;
#endif
            var accessKeyId = GetEnvironmentVariable("aws_access_key_id");
            var secretAccessKey = GetEnvironmentVariable("aws_SECRET_access_key");
            var sessionToken = GetEnvironmentVariable("aws_session_token");
            if (string.IsNullOrWhiteSpace(accessKeyId) || string.IsNullOrWhiteSpace(secretAccessKey) || string.IsNullOrWhiteSpace(sessionToken))
            {
                return false;
            }

            credentials = new SessionAWSCredentials(accessKeyId, secretAccessKey, sessionToken);
            return true;
        }

        private static string GetEnvironmentVariable(string variable)
        {
            return Environment.GetEnvironmentVariable(variable, EnvironmentVariableTarget.User)
                   ?? Environment.GetEnvironmentVariable(variable);
        }
    }

    public interface ISsmWrapper
    {
        Task<List<Parameter>> GetParameterStoreParametersByPathAsync(string path);
    }
}

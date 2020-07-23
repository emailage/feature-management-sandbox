using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using System.Threading.Tasks;

namespace FeatureManagementSandbox.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloWorldController : ControllerBase
    {
        private readonly IFeatureManager _featureManager;
        private readonly IFeatureManagerSnapshot _featureManagerSnapshot;

        public HelloWorldController(
              IFeatureManager featureManager,
              IFeatureManagerSnapshot featureManagerSnapshot)
        {
            _featureManager = featureManager;
            _featureManagerSnapshot = featureManagerSnapshot;
        }

        [HttpGet]
        public async Task<string> Get()
        {
            var redisFeatureA = _featureManagerSnapshot.IsEnabledAsync(nameof(MyFeatureFlags.RedisFeatureA));
            var redisFeatureB = _featureManagerSnapshot.IsEnabledAsync(nameof(MyFeatureFlags.RedisFeatureB));
            var dummyFeatureA = _featureManagerSnapshot.IsEnabledAsync(nameof(MyFeatureFlags.DummyFeatureA));
            var appConfigFeatureA = _featureManagerSnapshot.IsEnabledAsync(nameof(MyFeatureFlags.AppConfigFeatureA));
            var appConfigFeatureB = _featureManagerSnapshot.IsEnabledAsync(nameof(MyFeatureFlags.AppConfigFeatureB));
            var appConfigFeatureC = _featureManagerSnapshot.IsEnabledAsync(nameof(MyFeatureFlags.AppConfigFeatureC));
            var ssmFeatureA = _featureManagerSnapshot.IsEnabledAsync(nameof(MyFeatureFlags.SsmFeatureA));
            var ssmFeatureB = _featureManagerSnapshot.IsEnabledAsync(nameof(MyFeatureFlags.SsmFeatureB));

            return $"RedisFeatureA: {await redisFeatureA},\n " +
                   $"RedisFeatureB: {await redisFeatureB},\n " +
                   $"DummyFeatureA: {await dummyFeatureA},\n " +
                   $"AppConfigFeatureA: {await appConfigFeatureA},\n " +
                   $"AppConfigFeatureB: {await appConfigFeatureB},\n " +
                   $"AppConfigFeatureC: {await appConfigFeatureC},\n " +
                   $"SsmFeatureA: {await ssmFeatureA} \n" +
                   $"SsmFeatureB: {await ssmFeatureB}";
        }
    }
}
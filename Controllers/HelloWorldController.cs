using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;

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

            
            return $"RedisFeatureA: {await redisFeatureA}. RedisFeatureB: {await redisFeatureB}, DummyFeatureA: {await dummyFeatureA}, AppConfigFeatureA: {await appConfigFeatureA}, AppConfigFeatureB: {await appConfigFeatureB}";
        }
    }
}

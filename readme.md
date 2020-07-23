# readme

This repository illustrates how to use:

* [Feature Management](https://www.nuget.org/packages/Microsoft.FeatureManagement)
* with a custom feature flags provider, samples include Redis, AWS App Config and AWS SSM Parameter Store.
* that also automatically refreshes itself

## build & run

```bash
dotnet build
dotnet run
```

The code can be ran both locally and in a remote container.

## usage

```bash
curl http://localhost:5000/helloworld
```

All flags refresh every second, so repeated runs of the command above will give different results.

## Feature Flag Providers

In order to use all the providers in this project (Redis, AWS AppConfig & SSM) you need to have instances of all.
Each is configured in the respective provider.
Demo uses hosted AWS AppConfig & SSM instances and a local Redis instance.

Names of feature flags can be changed in the MyFeatureFlags class to match your own if needed.
Redis Provider uses a pattern to fetch FF. That can be changed in the RedisFeatureFlagsProvider class, SearchPattern variable.
Redis provider is only set up to fetch primitive flags in the format of <key, value>
AppConfig can store more complex, parameterized flag format required for conditional flags and filtering. <key, object>

The format used can be seen in appsettings.json "FeatureManagement" section under "AppSettingsFeature".
The usage is demonstrated in the FeatureFlagFilters.Filter class, TestFilter example.

For SSM in this demo, flags are assumed to be stored as such:
"us-west-2" (or any valid aws region) - flag is enabled
"," - flag is disabled
"localhost" - only enabled for debug purposes.

SSM flags are mapped to JObjects demonstrated in appsettings and parsed by path to Dictionary<string,string>.


By default SSM, Redis & AppConfig providers are comented out in the FeatureManagementConfigurationExtensions class.
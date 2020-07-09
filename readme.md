# readme

This repository illustrates how to use:

* [Feature Management](https://www.nuget.org/packages/Microsoft.FeatureManagement)
* with a custom feature flags provider, samples include Redis and AWS App Config.
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

In order to use all the providers in this project (Redis, AWS AppConfig) you need to have instances of both.
Each is conficured in the respective provider.
We used a hosted AWS AppConfig instance and a local Redis instance.
You can change the names of FF in the MyFeatureFlags class to match your own if needed.
Redis Provider uses a pattern to fetch FF. That can be chnaged in the RedisFeatureFlagsProvider class, SearchPattern variable.

By default, both Redis & AppConfig providers are comented out in the FeatureManagementConfigurationExtensions class.
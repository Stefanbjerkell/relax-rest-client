# Changelog

## V 3.0.0

This major version update is focused on giving more control over the json serialization.
 
### Breaking changes

- Default TimeOutInSeconds value increased from 10 to 30 seconds.
- All HttpRestClient constructors now take a IRestClientSerializer interface instead of JsonSerializerOptions. To give more flexibility.

### New features

- Interface IRestClientSerializer. This gives the posibility to swich out the json serializer used in the HttpRestClient.
- RestClient.Serializers.Newtonsoft package! This contains a IRestClientSerializer using Newtonsoft.
- Static property HttpRestClient.DefaultSerializer. This will be used if no other serializer is set.
- Extension for setting the default serializer in program.cs.

## V 2.0.2

### Breaking changes

- Renamed RestClient to HttpRestClient to fix conflict with namespace.
- Renamed HttpRestClient.WithJsonOptions to HttpRestClient.AddJsonOptions

### New features

- HttpRestClient.AddBasicAuth(username, password)
- HttpRestClient.AddAuthToken(token, schema)

## V 1.0.0

 Initial version of my HttpClient wrapper.

### Features

- RestClient - Package released!
- RestClient.Mock - Package released!
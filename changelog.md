# Changelog

## V 3.0.0

This major version is focused on giving more controll over the JsonSerialization.
 
### Breaking changes

- Default TimeOutInSeconds value increased from 10 to 30 seconds.
- Removed the JsonOptions parameters from all the constructors, due to a rework on how JsonParsing is handled.

### New featurs

- 

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
# Changelog

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
# RestClient

This rest client was created to add some extra features to the standard System.Net.Http.HttpClient.

Features.
- Fluid way of adding headers/query-parameters/url-parameters/body
- Request and response body parsing.
- Error handling with easy to use fluid syntax or possibility to add custom error handlers.
- Mocking tool for easy set up of mocked httpClients for Unit tests.

## Getting started

#### Simple version

```
var client = new RestClient("https://myapi.com);

var request = client.Get("path");

var response = await request.Execute();
```

#### Headers
```
var client = new RestClient("https://myapi.com);

var request = client.Get("path")
	.WithHeader("My-Header-1", "header-value")
	.WithHeader("My-Header-2", "header-value");

var response = await request.Execute();
```

#### Request/Response body
```
var client = new RestClient("https://myapi.com);
var body = new MyBodyObject();

var request = client.Post("path")
	.WithJsonBody(body);

var response = await request.Execute();

var data = response.Content<ResponseContenetType>();
```

Or you can pass the body directly in the Post/Put method.
```
var request = client.Post("path", body)
```
#### Query and route parameters
```
var client = new RestClient("https://myapi.com);
var accountId = "my-account-id";

var request = client.Get("account/{accountId}")
	.WithParameter(accountId)
	.WithQueryParameter("query", "value");

var response = await request.Execute();
```

## Error Handling

To deal with errors you can attach a ErrorHandler to the request. Error handlers can be custom built or you can use 2 of the default ones that comes with this package.

Examples of how to attach the default error handlers.

#### Catch all errors.

```
var client = new RestClient("https://myapi.com);
var accountId = "my-account-id";

var request = client.Get("accounts/{accountId}")
	.OnError()
		.Do(async (request, response) => { // Do something with the response here });

var response = await request.Execute();
```

#### Catch specific HttpStatus.

```
var client = new RestClient("https://myapi.com);
var accountId = "my-account-id";

var request = client.Get("accounts/{accountId}")
	.OnError(HttpStatusCode.NotFound)
		.Throw(new AccountNotFoundException());

var response = await request.Execute();
```

#### Add customer error handler.

```
var client = new RestClient("https://myapi.com);
var accountId = "my-account-id";

var handler = new CustomErrorHandler();

var request = client.Get("accounts/{accountId}")
	.AddErrorHandler(handler);

var response = await request.Execute();
```

Custom error handler must inplement the interface IErrorHandler. It has two methods.c

- bool CanHandle(HttpResponseMessage httpResponse)
- Task Handle(HttpResponseMessage httpResponse, RestClientResponse response)

CanHandle lets the rest client know if the handler can handle the error. If multiple errorHandlers can handle the error the first one added will take priority. So the order in wich you add them will matter if you have more then one.

To handle a error you provide a method that takes the original httpResponse and the expected RestClientResposne as parameters. If you want to you can here modify the RestClientResponse or just do some logging/throwing a exception, its up to you.

## RestClient.Mock

Together with the RestClient package there is also the RestClient.Mock package that has some nice mocking helpers for setting up mocked http-clients.
This can be handy when writing unit tests.

Examples.
```
var body = MyResponseBody();

var httpClient = HttpMock.SetupClient()
            .Get("good-path", HttpStatusCode.OK, body)
			.Post("bad-path", HttpStatusCode.BadRequest)
            .Build();
```

This will set up a httpClient with two mocked path.One that will return OK and another that will return a error.

If you leave out the path parameter it will default to "*" (catching all paths).

You can also choose to set a default response to any request to the client.

```
var delayInMilliseconds = 20000;

var httpClient = HttpMock.SetupClient()
	.WithDefaultResponse(HttpStatusCode.OK, body,delayInMilliseconds)
	.Build();
```

You can provide a delay to any of the mocked responses if you want to.

```
var delayInMilliseconds = 20000;

var httpClient = HttpMock.SetupClient()
	.WithDefaultResponse(HttpStatusCode.OK, body, delayInMilliseconds)
	.Build();
```   

If you want more advanced mocking you can provide the full HttpResponseMessage instead of just HttpStatus and Body.
```
var myResponse = new HttpResponseMessage();

var httpClient = HttpMock.SetupClient()
	.WithDefaultResponse(myResponse)
	.Build();
```
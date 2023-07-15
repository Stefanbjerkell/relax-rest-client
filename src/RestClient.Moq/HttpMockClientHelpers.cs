using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RestClient.Mock;

public static class HttpMockClientHelpers
{
    public static HttpResponseMessage CreateResponseMessage(HttpStatusCode status, object? body = null, JsonSerializerOptions? jsonOptions = null)
    {
        var response = new HttpResponseMessage()
        {
            Content = body is not null ? new StringContent(JsonSerializer.Serialize(body, jsonOptions ?? new JsonSerializerOptions())) : null,
            StatusCode = status
        };

        if (response.Content is not null)
        {
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        }

        return response;
    }
}

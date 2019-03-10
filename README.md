# Overview
[![Build status](https://ci.appveyor.com/api/projects/status/2qa7tli7laa0o4mq/branch/master?svg=true)](https://ci.appveyor.com/project/polewskm/ncode-common-web/branch/master)

Provides a set common classes when implementing WebAPI services.

## NCode.Common.Models
Used by WebAPI clients and servers to provide common definitions for consistent and uniform contracts for all responses in WebAPI applications.

### Common Contracts
```csharp
/// <summary>
/// Represents a common response pattern that can be used in all WebAPI
/// applications. Used for both success and failure responses. For a response
/// to indicate failure, the <c>Error</c> element will contain the error details.
/// For a response to indicate success, the <c>Data</c> element will contain
/// the response details. An optional <c>Diagnostics</c> element can be used
/// to return informational and verbose messages to aide in debugging,
/// instrumentation, etc. Both <c>Error</c> and <c>Data</c> should not
/// exist at the same time, but if so, the <c>Error</c> element should take
/// precedence and indicate a failed response.
/// </summary>
public interface IResponse
{
    /// <summary>
    /// Gets a value indicating whether the response was successful. This
    /// element is not returned in the JSON response and is only provided
    /// as a convenience for developers to check if the <c>Error</c> element
    /// is null.
    /// </summary>
    [JsonIgnore]
    bool Success { get; }

    /// <summary>
    /// Gets the error details for failed a response.
    /// </summary>
    [JsonProperty(Order = 1, NullValueHandling = NullValueHandling.Ignore)]
    ErrorData Error { get; }

    /// <summary>
    /// Gets or sets a dictionary used to store diagnostic data such as
    /// informational and verbose messages to aide in debugging and instrumentation.
    /// </summary>
    [JsonProperty(Order = 3, NullValueHandling = NullValueHandling.Ignore)]
    IDictionary<string, object> Diagnostics { get; set; }
}

/// <summary>
/// Represents a common response pattern that can be used in all WebAPI
/// applications. Used for both success and failure responses. For a response
/// to indicate failure, the <c>Error</c> element will contain the error details.
/// For a response to indicate success, the <c>Data</c> element will contain
/// the response details. An optional <c>Diagnostics</c> element can be used
/// to return informational and verbose messages to aide in debugging,
/// instrumentation, etc. Both <c>Error</c> and <c>Data</c> should not
/// exist at the same time, but if so, the <c>Error</c> element should take
/// precedence and indicate a failed response.
/// </summary>
/// <typeparam name="TData">The data type for the details in a successful response.</typeparam>
public interface IResponse<out TData> : IResponse
{
    /// <summary>
    /// Gets the details for a successful response.
    /// </summary>
    [JsonProperty(Order = 2, NullValueHandling = NullValueHandling.Ignore)]
    TData Data { get; }
}

/// <summary>
/// Contains the data for all failed responses. At a minimum contains an
/// error code and an error message. Optionally can include additional
/// error details.
/// </summary>
public class ErrorData
{
    /// <summary>
    /// Gets or sets a numeric <c>code</c> used to identify the error.
    /// This value is also returned as the HTTP Status Code for REST responses.
    /// </summary>
    [JsonProperty(Order = 1)]
    public int Code { get; set; }

    /// <summary>
    /// Gets or sets a <c>message</c> used to describe the error.
    /// </summary>
    [JsonProperty(Order = 2)]
    public string Message { get; set; }

    /// <summary>
    /// Gets or sets a dictionary containing the details for the failure.
    /// </summary>
    [JsonProperty(Order = 3)]
    public IDictionary<string, object> Details { get; set; }
}
```

### Example JSON Response
```json
{
    // the "error" element will be omitted if null
    "error": {
        "code": 500,
        "message": "An unhandled exception occured",
        "details": {
            "Exception": {
                // the exception details...
            }
        }
    },

    // the "data" element will be omitted if null
    "data": {
        "widgetId": 6767,
        "steps": [
            "Collect underpants",
            "...",
            "Profit"
        ]
    },

    // the "diagnostics" element will be omitted if null
    "diagnostics": {
        "timer": "00:00:02.654",
        "serverName": "AWS-I-1234"
    }
}
```

## NCode.Common.Responses
Used by WebAPI applications to create responses by using the factory selector pattern. Additional responses can be provided by developers by creating their own extension methods.

### Definition
```csharp
public interface IResponseFactory
{
    // nothing
}

public class ResponseFactory : IResponseFactory
{
    // nothing
}

public static class ResponseCreator
{
    public static IResponseFactory Factory { get; } = new ResponseFactory();
}

public static class ResponseFactoryExtensions
{
    public static IResponse Success(this IResponseFactory factory)
    {
        return new ErrorResponse();
    }

    public static IResponse<TData> Success<TData>(this IResponseFactory factory, TData data)
    {
        return new Response<TData>
        {
            Data = data,
        };
    }

    public static IResponse Error(this IResponseFactory factory, int code, string message)
    {
        return new ErrorResponse
        {
            Error = new ErrorData
            {
                Code = code,
                Message = message,
            },
        };
    }

    // more extension methods exist...
}
```

### Example Usage

```csharp
private static void Main()
{
    var errorNotFound = ResponseCreator.Factory.Error(404, "The resource was not found");
    var successCreateId = ResponseCreator.Factory.Success(new { Id = 6767 } );
    // ...
}
```

## NCode.Common.AspNetCore
Provides common middleware and utilities for ASP.NET Core WebAPI services.

```csharp
public class WidgetController : Controller
{
    public IActionResult GetWidget(int id)
    {
        if (!ModelState.IsValid)
            // return BadRequest for any model state validation errors
            return ModelState.AsErrorResult();

        var widget = new Widget { /* ... */ };

        // return the widget response as a MVC OkObjectResult
        var response = ResponseCreator.Factory.Success(widget);
        var result = response.AsActionResult();

        return result;
    }
}

public class Startup
{
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        // return JSON for any unhandled exceptions
        app.UseResponseExceptionHandler();

        // return JSON for any response that has an HTTP status code between 400-599 and doesn't already have a body
        app.UseResponseErrorHandler();

        // ...
    }
}
```

# Release Notes
* v1.0.0 - Initial release

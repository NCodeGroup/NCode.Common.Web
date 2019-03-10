#region Copyright Preamble
// 
//    Copyright @ 2019 NCode Group
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
#endregion

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NCode.Common.Models;
using NCode.Common.Responses;
using Newtonsoft.Json;

namespace NCode.Common.AspNetCore.Middleware
{
    /// <summary>
    /// Captures synchronous and asynchronous <see cref="Exception"/> instances
    /// from the ASP.NET Core pipeline and generates REST error responses.
    /// </summary>
    public class ResponseExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JsonSerializerSettings _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseExceptionHandlerMiddleware"/> class.
        /// </summary>
        /// <param name="next"><see cref="RequestDelegate"/></param>
        /// <param name="settings"><see cref="JsonSerializerSettings"/></param>
        public ResponseExceptionHandlerMiddleware(RequestDelegate next, JsonSerializerSettings settings)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _settings = settings; // nullable
        }

        /// <summary>
        /// Process an individual request.
        /// </summary>
        /// <param name="httpContext"><see cref="HttpContext"/></param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public virtual async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext).ConfigureAwait(false);
            }
            catch (UnauthorizedAccessException exception)
            {
                // unauthorized exceptions return the 403 HTTP status code
                if (!await HandleExceptionAsync(httpContext, exception, CommonStatusCodes.Forbidden, exception.Message).ConfigureAwait(false))
                    throw;
            }
            catch (OperationCanceledException exception)
            {
                // canceled operations return the 499 HTTP status code
                if (!await HandleExceptionAsync(httpContext, exception, CommonStatusCodes.Canceled, exception.Message).ConfigureAwait(false))
                    throw;
            }
            catch (NotImplementedException exception)
            {
                // not implemented operations return the 501 HTTP status code
                if (!await HandleExceptionAsync(httpContext, exception, CommonStatusCodes.NotImplemented, exception.Message).ConfigureAwait(false))
                    throw;
            }
            catch (Exception exception)
            {
                // all other unhandled exceptions return the 500 HTTP status code
                if (!await HandleExceptionAsync(httpContext, exception, CommonStatusCodes.Unhandled, "Unhandled Error").ConfigureAwait(false))
                    throw;
            }
        }

        private async Task<bool> HandleExceptionAsync(HttpContext context, Exception exception, int code, string message)
        {
            // We can't do anything if the response has already started, just abort.
            if (context.Response.HasStarted)
                return false;

            context.Response.StatusCode = code;
            context.Response.ContentType = "application/json";

            var response = ResponseCreator.Factory.Error(code, message, exception);
            var json = JsonConvert.SerializeObject(response, _settings);

            await context.Response.WriteAsync(json, context.RequestAborted).ConfigureAwait(false);

            return true;
        }

    }
}
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
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using NCode.Common.Responses;
using Newtonsoft.Json;

namespace NCode.Common.AspNetCore.Middleware
{
    /// <summary>
    /// <see cref="IApplicationBuilder"/> extension methods for the <see cref="ResponseExceptionHandlerMiddleware"/>.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Captures synchronous and asynchronous <see cref="Exception"/>
        /// instances from the ASP.NET Core pipeline pipeline and generates
        /// REST error responses.
        /// </summary>
        /// <param name="app"><see cref="IApplicationBuilder"/></param>
        /// <param name="settings"><see cref="JsonSerializerSettings"/></param>
        /// <returns><see cref="IApplicationBuilder"/></returns>
        public static IApplicationBuilder UseResponseExceptionHandler(this IApplicationBuilder app, JsonSerializerSettings settings = null)
        {
            return app.UseMiddleware<ResponseExceptionHandlerMiddleware>(settings);
        }

        /// <summary>
        /// Adds a middleware that checks for responses with status codes
        /// between 400 and 599 that do not have a body and if so, returns
        /// a JSON error response.
        /// </summary>
        /// <param name="app"><see cref="IApplicationBuilder"/></param>
        /// <param name="settings"><see cref="JsonSerializerSettings"/></param>
        /// <returns><see cref="IApplicationBuilder"/></returns>
        public static IApplicationBuilder UseResponseErrorHandler(this IApplicationBuilder app, JsonSerializerSettings settings = null)
        {
            return app.UseStatusCodePages(async statusCodeContext =>
            {
                // format the JSON response
                var httpContext = statusCodeContext.HttpContext;
                var httpStatusCode = httpContext.Response.StatusCode;
                var reasonPhrase = ReasonPhrases.GetReasonPhrase(httpStatusCode);
                var response = ResponseCreator.Factory.Error(httpStatusCode, reasonPhrase);
                var json = JsonConvert.SerializeObject(response, settings);

                httpContext.Response.ContentType = "application/json";
                await httpContext.Response.WriteAsync(json, httpContext.RequestAborted).ConfigureAwait(false);
            });
        }

    }
}
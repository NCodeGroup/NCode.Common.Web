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
using System.Net;
using Microsoft.AspNetCore.Mvc;
using NCode.Common.Models;

namespace NCode.Common.AspNetCore.Responses
{
    /// <summary>
    /// Provides extension methods for <see cref="IResponse"/>.
    /// </summary>
    public static class ResponseExtensions
    {
        /// <summary>
        /// Creates a new <see cref="IActionResult"/> that contains the
        /// corresponding <see cref="ObjectResult"/> for the given <paramref name="response"/>.
        /// If the response has an <c>error</c> number, then that is used to
        /// return the proper result such as <see cref="NotFoundObjectResult"/>
        /// or <see cref="BadRequestObjectResult"/>, otherwise <see cref="ObjectResult"/>
        /// is returned with the <c>200</c> HTTP status code.
        /// </summary>
        /// <param name="response">The response to return.</param>
        /// <returns><see cref="IActionResult"/></returns>
        public static IActionResult AsActionResult(this IResponse response)
        {
            return AsActionResult(response, CommonStatusCodes.Success);
        }

        /// <summary>
        /// Creates a new <see cref="IActionResult"/> that contains the
        /// corresponding <see cref="ObjectResult"/> for the given <paramref name="response"/>.
        /// If the response has an <c>error</c> number, then that is used to
        /// return the proper result such as <see cref="NotFoundObjectResult"/>
        /// or <see cref="BadRequestObjectResult"/>, otherwise <see cref="ObjectResult"/>
        /// is returned with the specified <paramref name="statusCode"/>.
        /// </summary>
        /// <param name="response">The response to return.</param>
        /// <param name="statusCode">The default HTTP status code to return if the response doesn't have an error code.</param>
        /// <returns><see cref="IActionResult"/></returns>
        public static IActionResult AsActionResult(this IResponse response, int statusCode)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));

            var code = response.Error?.Code ?? statusCode;
            switch ((HttpStatusCode)code)
            {
                case HttpStatusCode.OK:
                    return new OkObjectResult(response);

                case HttpStatusCode.NotFound:
                    return new NotFoundObjectResult(response);

                case HttpStatusCode.BadRequest:
                    return new BadRequestObjectResult(response);
            }

            return new ObjectResult(response)
            {
                StatusCode = code,
            };
        }

    }
}
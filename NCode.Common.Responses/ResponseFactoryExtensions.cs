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
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NCode.Common.Models;

namespace NCode.Common.Responses
{
    /// <summary>
    /// Provides extension methods for <see cref="IResponseFactory"/>.
    /// </summary>
    public static class ResponseFactoryExtensions
    {
        /// <summary>
        /// Creates a new <see cref="IResponse"/> instance that represents a
        /// successful response with no data and no errors.
        /// </summary>
        /// <param name="factory"><see cref="IResponseFactory"/></param>
        /// <returns><see cref="IResponse"/></returns>
        public static IResponse Success(this IResponseFactory factory)
        {
            return new Response();
        }

        /// <summary>
        /// Creates a new <see cref="IResponse{TData}"/> instance that represents
        /// a successful response with the specified <paramref name="data"/>
        /// for it's details.
        /// </summary>
        /// <typeparam name="TData">The data type for the details in a successful response.</typeparam>
        /// <param name="factory"><see cref="IResponseFactory"/></param>
        /// <param name="data">The details to assign in the successful response.</param>
        /// <returns><see cref="IResponse{TData}"/></returns>
        public static IResponse<TData> Success<TData>(this IResponseFactory factory, TData data)
        {
            return new Response<TData>
            {
                Data = data,
            };
        }

        /// <summary>
        /// Creates a new <see cref="IResponse"/> instance that represents a
        /// failed response with the specified error details.
        /// </summary>
        /// <param name="factory"><see cref="IResponseFactory"/></param>
        /// <param name="code">A numeric <c>code</c> used to identify the error.</param>
        /// <param name="message">A <c>message</c> used to describe the error.</param>
        /// <returns><see cref="IResponse"/></returns>
        public static IResponse Error(this IResponseFactory factory, int code, string message)
        {
            return new Response
            {
                Error = new ErrorData
                {
                    Code = code,
                    Message = message,
                },
            };
        }

        /// <summary>
        /// Creates a new <see cref="IResponse"/> instance that represents a
        /// failed response with the specified error details.
        /// </summary>
        /// <param name="factory"><see cref="IResponseFactory"/></param>
        /// <param name="code">A numeric <c>code</c> used to identify the error.</param>
        /// <param name="message">A <c>message</c> used to describe the error.</param>
        /// <param name="exception">The <see cref="Exception"/> which caused the error.</param>
        /// <returns><see cref="IResponse"/></returns>
        public static IResponse Error(this IResponseFactory factory, int code, string message, Exception exception)
        {
            var details = new Dictionary<string, object>
            {
                ["Exception"] = exception,
            };

            return Error(factory, code, message, details);
        }

        /// <summary>
        /// Creates a new <see cref="IResponse"/> instance that represents a
        /// failed response with the specified error details.
        /// </summary>
        /// <param name="factory"><see cref="IResponseFactory"/></param>
        /// <param name="code">A numeric <c>code</c> used to identify the error.</param>
        /// <param name="message">A <c>message</c> used to describe the error.</param>
        /// <param name="details">Optionally contains any details for the error.</param>
        /// <returns><see cref="IResponse"/></returns>
        public static IResponse Error(this IResponseFactory factory, int code, string message, IEnumerable<KeyValuePair<string, string>> details)
        {
            var detailsDictionary = details?.ToDictionary(pair => pair.Key, pair => (object)pair.Value);

            return Error(factory, code, message, detailsDictionary);
        }

        /// <summary>
        /// Creates a new <see cref="IResponse"/> instance that represents a
        /// failed response with the specified error details.
        /// </summary>
        /// <param name="factory"><see cref="IResponseFactory"/></param>
        /// <param name="code">A numeric <c>code</c> used to identify the error.</param>
        /// <param name="message">A <c>message</c> used to describe the error.</param>
        /// <param name="details">Optionally contains any details for the error.</param>
        /// <returns><see cref="IResponse"/></returns>
        public static IResponse Error(this IResponseFactory factory, int code, string message, IEnumerable<KeyValuePair<string, object>> details)
        {
            var detailsDictionary = details as IDictionary<string, object>
                ?? details?.ToDictionary(pair => pair.Key, pair => pair.Value);

            return new Response
            {
                Error = new ErrorData
                {
                    Code = code,
                    Message = message,
                    Details = detailsDictionary,
                },
            };
        }

        /// <summary>
        /// Creates a new <see cref="IResponse"/> instance that represents a failed response for a missing resource.
        /// </summary>
        /// <param name="factory"><see cref="IResponseFactory"/></param>
        /// <param name="idName">Contains the name of the Key or Id that wasn't found.</param>
        /// <param name="idValue">Contains the value of the Key or Id that wasn't found.</param>
        /// <returns><see cref="IResponse"/></returns>
        public static IResponse NotFound(this IResponseFactory factory, string idName, object idValue)
        {
            var details = new Dictionary<string, object>
            {
                // dictionary keys cannot be null
                [idName ?? "Id"] = idValue,
            };

            return NotFound(factory, details);
        }

        /// <summary>
        /// Creates a new <see cref="IResponse"/> instance that represents a failed response for a missing resource.
        /// </summary>
        /// <param name="factory"><see cref="IResponseFactory"/></param>
        /// <param name="details">Contains key-value-pairs with the values of the keys/ids and their corresponding values that returned no results.</param>
        /// <returns><see cref="IResponse"/></returns>
        public static IResponse NotFound(this IResponseFactory factory, IEnumerable<KeyValuePair<string, object>> details)
        {
            return Error(factory, CommonStatusCodes.NotFound, "The specified resource was not found.", details);
        }

        /// <summary>
        /// Creates a new <see cref="IResponse"/> instance that represents a
        /// failed response for a bad request.
        /// </summary>
        /// <param name="factory"><see cref="IResponseFactory"/></param>
        /// <param name="errors">A dictionary of errors for the bad request containing property names and the problems for each property.</param>
        /// <returns><see cref="IResponse"/></returns>
        public static IResponse BadRequest(this IResponseFactory factory, IDictionary<string, string[]> errors)
        {
            var details = errors?.ToDictionary(pair => pair.Key, pair => (object)pair.Value);

            return Error(factory, CommonStatusCodes.BadRequest, "Request validation failed.", details);
        }

        /// <summary>
        /// Creates a new <see cref="IResponse"/> instance that represents a
        /// failed response for a resource that is protected and cannot be modified.
        /// </summary>
        /// <param name="factory"><see cref="IResponseFactory"/></param>
        /// <returns><see cref="IResponse"/></returns>
        public static IResponse Protected(this IResponseFactory factory)
        {
            return Error(factory, CommonStatusCodes.Locked, "The resource is protected and cannot be modified.");
        }

        /// <summary>
        /// Creates a new <see cref="IResponse"/> instance that represents a
        /// failed response for a resource that is currently locked for editing by another user.
        /// </summary>
        /// <param name="factory"><see cref="IResponseFactory"/></param>
        /// <param name="exception">A <see cref="DBConcurrencyException"/> that contains the details of who has the resource currently locked.</param>
        /// <returns><see cref="IResponse"/></returns>
        public static IResponse Locked(this IResponseFactory factory, DBConcurrencyException exception)
        {
            var details = new Dictionary<string, object>
            {
                ["UserId"] = exception?.Data["UserId"] as int?,
                ["DisplayName"] = exception?.Data["DisplayName"] as string,
            };

            return Error(factory, CommonStatusCodes.Locked, "The resource is currently locked for editing by another user.", details);
        }

        /// <summary>
        /// Creates a new <see cref="IResponse"/> instance that represents a failed response with a list of problems describing a conflict.
        /// </summary>
        /// <param name="factory"><see cref="IResponseFactory"/></param>
        /// <param name="message">Contains the conflict message.</param>
        /// <param name="problems">A list of problems describing the conflict.</param>
        /// <returns><see cref="IResponse"/></returns>
        public static IResponse Conflict(this IResponseFactory factory, string message, IEnumerable<string> problems)
        {
            // force enumeration of the problems
            var collection = problems as IReadOnlyCollection<string> ?? problems?.ToArray();
            var details = new Dictionary<string, object> { ["Problems"] = collection };

            return Error(factory, CommonStatusCodes.Conflict, message, details);
        }

        /// <summary>
        /// Creates a new <see cref="IResponse"/> instance that represents a failed response with a list of problems describing a conflict.
        /// </summary>
        /// <param name="factory"><see cref="IResponseFactory"/></param>
        /// <param name="message">Contains the conflict message.</param>
        /// <param name="problems">A list of problems describing the conflict.</param>
        /// <returns><see cref="IResponse"/></returns>
        public static IResponse Conflict(this IResponseFactory factory, string message, params string[] problems)
        {
            return Conflict(factory, message, problems?.AsEnumerable());
        }

        /// <summary>
        /// Creates a new <see cref="IResponse"/> instance that represents a
        /// failed response for violating a unique constraint.
        /// </summary>
        /// <param name="factory"><see cref="IResponseFactory"/></param>
        /// <param name="problems">A list of problems describing the unique violation.</param>
        /// <returns><see cref="IResponse"/></returns>
        public static IResponse Duplicate(this IResponseFactory factory, IEnumerable<string> problems)
        {
            return Conflict(factory, "Violation of unique constraints.", problems);
        }

        /// <summary>
        /// Creates a new <see cref="IResponse"/> instance that represents a
        /// failed response for violating a unique constraint.
        /// </summary>
        /// <param name="factory"><see cref="IResponseFactory"/></param>
        /// <param name="problems">A list of problems describing the unique violation.</param>
        /// <returns><see cref="IResponse"/></returns>
        public static IResponse Duplicate(this IResponseFactory factory, params string[] problems)
        {
            return Duplicate(factory, problems?.AsEnumerable());
        }

        /// <summary>
        /// Creates a new <see cref="IResponse"/> instance that represents a failed response for violating business rules.
        /// </summary>
        /// <param name="factory"><see cref="IResponseFactory"/></param>
        /// <param name="problems">A list of problems describing the business rule violations.</param>
        /// <returns><see cref="IResponse"/></returns>
        public static IResponse BusinessRules(this IResponseFactory factory, IEnumerable<string> problems)
        {
            return Conflict(factory, "Violation of business rules.", problems);
        }

    }
}
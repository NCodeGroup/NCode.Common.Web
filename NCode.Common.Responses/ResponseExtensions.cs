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
using NCode.Common.Models;

namespace NCode.Common.Responses
{
    /// <summary>
    /// Provides extension methods for the <see cref="IResponse"/> interface.
    /// </summary>
    public static class ResponseExtensions
    {
        /// <summary>
        /// Given a non-generic response, constructs and returns a generic
        /// response using the specified <typeparamref name="TData"/>. This
        /// method is only valid for failed responses because it does not
        /// preserve the actual data for a given response and the <c>Data</c>
        /// member for the newly returned response will always be null.
        /// </summary>
        /// <typeparam name="TData">The data type for the response.</typeparam>
        /// <param name="response">The response to convert.</param>
        /// <returns>A newly allocated generic <see cref="IResponse{TData}"/> using the specified <typeparamref name="TData"/>.</returns>
        public static IResponse<TData> As<TData>(this IResponse response)
            where TData : new()
        {
            return As<TData>(response, null);
        }

        /// <summary>
        /// Given a non-generic response, constructs and returns a generic
        /// response using the specified <typeparamref name="TData"/>. This
        /// method is only valid for failed responses because it does not
        /// preserve the actual data for a given response and the <c>Data</c>
        /// member for the newly returned response will always be null.
        /// </summary>
        /// <typeparam name="TData">The data type for the response.</typeparam>
        /// <param name="response">The response to convert.</param>
        /// <param name="diagnostics">An optional diagnostics dictionary to assign to the response. If null, the existing diagnostics value will remain.</param>
        /// <returns>A newly allocated generic <see cref="IResponse{TData}"/> using the specified <typeparamref name="TData"/>.</returns>
        public static IResponse<TData> As<TData>(this IResponse response, IDictionary<string, object> diagnostics)
            where TData : new()
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));

            if (response.Success)
                throw new InvalidOperationException();

            return new Response<TData>
            {
                Error = response.Error,
                Diagnostics = diagnostics ?? response.Diagnostics,
            };
        }

        /// <summary>
        /// Updates the <c>Diagnostics</c> member of a response by using the specified callback.
        /// </summary>
        /// <param name="response">The response instance.</param>
        /// <param name="configurator">A user provided callback to update the diagnostics dictionary.</param>
        /// <returns>The fluent response instance.</returns>
        public static T WithDiagnostics<T>(this T response, Action<IDictionary<string, object>> configurator)
            where T : IResponse
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));
            if (configurator == null)
                throw new ArgumentNullException(nameof(configurator));

            var diagnostics = response.Diagnostics;
            if (diagnostics == null)
            {
                diagnostics = new Dictionary<string, object>();
                response.Diagnostics = diagnostics;
            }

            configurator(diagnostics);

            return response;
        }

        /// <summary>
        /// Sets the <c>Diagnostics</c> member of a response to the specified value.
        /// </summary>
        /// <param name="response">The response instance.</param>
        /// <param name="diagnostics">The dictionary to assign to the response.</param>
        /// <returns>The fluent response instance.</returns>
        public static T WithDiagnostics<T>(this T response, IDictionary<string, object> diagnostics)
            where T : IResponse
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));

            response.Diagnostics = diagnostics;

            return response;
        }

    }
}
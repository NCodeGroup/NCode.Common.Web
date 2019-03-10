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

using System.Collections.Generic;
using Newtonsoft.Json;

#pragma warning disable CA1716 // Identifiers should not match keywords
#pragma warning disable CA2227 // Collection properties should be read only

namespace NCode.Common.Models
{
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
    /// Provides a default implementation for the <see cref="IResponse"/>
    /// interface.
    /// </summary>
    public class Response : IResponse
    {
        /// <inheritdoc />
        [JsonIgnore]
        public bool Success => Error == null;

        /// <summary>
        /// Gets or sets the error details for failed a response.
        /// </summary>
        [JsonProperty(Order = 1, NullValueHandling = NullValueHandling.Ignore)]
        public ErrorData Error { get; set; }

        /// <inheritdoc />
        [JsonProperty(Order = 3, NullValueHandling = NullValueHandling.Ignore)]
        public IDictionary<string, object> Diagnostics { get; set; }
    }

    /// <summary>
    /// Provides a default implementation for the <see cref="IResponse{TData}"/>
    /// interface.
    /// </summary>
    /// <typeparam name="TData">The data type for the details in a successful response.</typeparam>
    public class Response<TData> : Response, IResponse<TData>
    {
        /// <summary>
        /// Gets or sets the details for a successful response.
        /// </summary>
        [JsonProperty(Order = 2, NullValueHandling = NullValueHandling.Ignore)]
        public TData Data { get; set; }
    }

}
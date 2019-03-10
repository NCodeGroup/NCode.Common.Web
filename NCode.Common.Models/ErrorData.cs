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

#pragma warning disable CA2227 // Collection properties should be read only

namespace NCode.Common.Models
{
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
}
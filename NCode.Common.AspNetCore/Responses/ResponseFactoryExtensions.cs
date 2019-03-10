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
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NCode.Common.Models;
using NCode.Common.Responses;

namespace NCode.Common.AspNetCore.Responses
{
    /// <summary>
    /// Provides extensions methods for <see cref="IResponseFactory"/>.
    /// </summary>
    public static class ResponseFactoryExtensions
    {
        /// <summary>
        /// Creates a new <see cref="IResponse"/> that represents a failed
        /// response for a bad request due to model state validation errors.
        /// </summary>
        /// <param name="factory"><see cref="IResponseFactory"/></param>
        /// <param name="modelState">The <see cref="ModelStateDictionary"/> which contains the model validation errors.</param>
        /// <returns><see cref="IResponse"/></returns>
        public static IResponse BadRequest(this IResponseFactory factory, ModelStateDictionary modelState)
        {
            if (modelState == null)
                throw new ArgumentNullException(nameof(modelState));
            if (modelState.IsValid)
                throw new InvalidOperationException();

            // the SerializableError for a ModelStateDictionary is really a IDictionary<string, string[]>
            var errors = new SerializableError(modelState)
                .ToDictionary(pair => pair.Key, pair => (string[])pair.Value);

            return factory.BadRequest(errors);
        }

    }
}
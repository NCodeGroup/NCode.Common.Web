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

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NCode.Common.Responses;

namespace NCode.Common.AspNetCore.Responses
{
    /// <summary>
    /// Provides extension methods for <see cref="ModelStateDictionary"/>.
    /// </summary>
    public static class ModelStateDictionaryExtensions
    {
        /// <summary>
        /// Creates a new <see cref="IActionResult"/> that contains a <see cref="BadRequestObjectResult"/>
        /// from the model validation errors in <paramref name="modelState"/>.
        /// </summary>
        /// <param name="modelState">The <see cref="ModelStateDictionary"/> which contains the model validation errors.</param>
        /// <returns><see cref="IActionResult"/></returns>
        public static IActionResult AsErrorResult(this ModelStateDictionary modelState)
        {
            var response = ResponseCreator.Factory.BadRequest(modelState);
            var result = new BadRequestObjectResult(response);

            return result;
        }

    }
}
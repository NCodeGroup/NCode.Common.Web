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
using System.ComponentModel;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NCode.Common.AspNetCore.Swagger
{
    /// <summary>
    /// Provides an implementation of <see cref="IOperationFilter"/> that can
    /// be used by <c>Swagger</c> to correctly specify the default value for
    /// parameters on request classes.
    /// </summary>
    public class AddDefaultValueOperationFilter : IOperationFilter
    {
        /// <inheritdoc />
        public virtual void Apply(Operation operation, OperationFilterContext context)
        {
            // check for null or empty
            if ((operation?.Parameters?.Count ?? 0) == 0)
                return;

            // Join all non-body operation parameters which don't have a
            // default value to their corresponding action parameter correlating
            // by the parameter's name case-insensitive.
            // ReSharper disable once PossibleNullReferenceException
            var join = operation.Parameters.OfType<NonBodyParameter>()
                .Where(operationParameter => operationParameter.Default == null)
                .Join(
                    context.ApiDescription.ParameterDescriptions,
                    operationParameter => operationParameter.Name,
                    actionParameter => actionParameter.Name,
                    Tuple.Create,
                    StringComparer.OrdinalIgnoreCase);

            foreach (var (operationParameter, actionParameter) in join)
            {
                // anything and everything can be null...
                var modelMetadata = actionParameter.ModelMetadata as DefaultModelMetadata;
                var defaultValueAttribute = modelMetadata?.Attributes?.PropertyAttributes?.OfType<DefaultValueAttribute>()?.FirstOrDefault();

                operationParameter.Default = defaultValueAttribute?.Value;
            }
        }

    }
}
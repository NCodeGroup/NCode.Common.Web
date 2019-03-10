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

using NCode.Common.Models;

#pragma warning disable CA1040 // Avoid empty interfaces

namespace NCode.Common.Responses
{
    /// <summary>
    /// Provides factory methods to create instances of <see cref="IResponse"/>.
    /// See the <see cref="ResponseCreator"/> static singleton and <see cref="ResponseFactoryExtensions"/>
    /// for more details and the purpose of this pattern.
    /// </summary>
    public interface IResponseFactory
    {
        // nothing
    }

    /// <inheritdoc />
    /// <summary>
    /// Provides the default implementation for the <see cref="IResponseFactory" /> class.
    /// </summary>
    public class ResponseFactory : IResponseFactory
    {
        // nothing
    }
}
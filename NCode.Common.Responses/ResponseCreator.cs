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

namespace NCode.Common.Responses
{
    /// <summary>
    /// Provides the <see cref="IResponseFactory"/> singleton for extension methods
    /// that can be used to create instances of <see cref="IResponse"/>.
    /// </summary>
    /// <remarks>
    /// This is called the factory selector pattern and is identical to the one
    /// used in MassTransit which allows developers to create extension methods
    /// that customize how additional response instances are returned.
    /// </remarks>
    public static class ResponseCreator
    {
        /// <summary>
        /// Gets the <see cref="IResponseFactory"/> singleton which can be used
        /// to create instances of <see cref="IResponse"/>.
        /// </summary>
        public static IResponseFactory Factory { get; } = new ResponseFactory();
    }
}
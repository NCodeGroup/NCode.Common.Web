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
using System.Diagnostics;
using NCode.Common.Models;
using Newtonsoft.Json;

namespace NCode.Common.AspNetCore.Converters
{
    /// <summary>
    /// Provides the implementation for a JSON converter that serializes a
    /// <see cref="Stopwatch"/> to JSON by outputting it's elapsed time. This
    /// allows for a running stopwatch to be inserted into the diagnostics
    /// for a <see cref="IResponse"/> and the elapsed time for the stopwatch
    /// will be correctly serialized to JSON.
    /// </summary>
    public class StopwatchConverter : JsonConverter<Stopwatch>
    {
        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, Stopwatch value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.Elapsed);
        }

        /// <inheritdoc />
        public override Stopwatch ReadJson(JsonReader reader, Type objectType, Stopwatch existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            // we never deserialize a stopwatch and it cannot be constructed manually, so return any gibberish
            return Stopwatch.StartNew();
        }

    }
}
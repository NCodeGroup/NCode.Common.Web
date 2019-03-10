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

namespace NCode.Common.Models
{
    /// <summary>
    /// Contains common HTTP status codes used for various REST responses.
    /// </summary>
    public static class CommonStatusCodes
    {
        /// <summary />
        public const int Success = 200;

        /// <summary />
        public const int Created = 201;

        /// <summary />
        public const int BadRequest = 400;

        /// <summary />
        public const int Forbidden = 403;

        /// <summary />
        public const int NotFound = 404;

        /// <summary />
        public const int Conflict = 409;

        /// <summary />
        public const int Locked = 423;

        /// <summary />
        public const int Canceled = 499;

        /// <summary />
        public const int Unhandled = 500;

        /// <summary />
        public const int NotImplemented = 501;
    }
}
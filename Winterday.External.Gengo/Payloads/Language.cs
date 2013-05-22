//
// Language.cs
//
// Author:
//       Jarl Erik Schmidt <github@jarlerik.com>
//
// Copyright (c) 2013 Jarl Erik Schmidt
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

namespace Winterday.External.Gengo.Payloads
{
    using System;

    using Newtonsoft.Json.Linq;

    /// <summary>
    /// A language supported by the Gengo service
    /// </summary>
    public class Language
    {
        /// <summary>
        /// English name of language
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Native name of language
        /// </summary>
        public string LocalizedName { get; private set; }


        /// <summary>
        /// Language code (used in job submissions)
        /// </summary>
        public string Code { get; private set; }

        /// <summary>
        /// The pricing unit for the language
        /// </summary>
        public string UnitType { get; private set; }

        internal Language(JObject obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            Name = obj.Value<string>("language");
            LocalizedName = obj.Value<string>("localized_name");
            Code = obj.Value<string>("lc");
            UnitType = obj.Value<string>("unit_type");
        }
    }
}


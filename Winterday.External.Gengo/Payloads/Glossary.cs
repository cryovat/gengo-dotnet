//
// Glossary.cs
//
// Author:
//       Jarl Erik Schmidt <github@jarlerik.com>
//
// Copyright (c) 2014 Jarl Erik Schmidt
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
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Represents the tuple of a language's numeric ID and code
    /// </summary>
    public struct LanguageIdAndCodePair
    {
        /// <summary>
        /// Numeric language Id
        /// </summary>
        public readonly int Id;
        
        /// <summary>
        /// Language code
        /// </summary>
        public readonly string Code;

        internal LanguageIdAndCodePair(int id, string code)
        {
            Id = id;
            Code = code;
        }
    }

    /// <summary>
    /// Represents an user glossary
    /// </summary>
    public class Glossary
    {
        /// <summary>
        /// The glossary Id
        /// </summary>
        public int GlossaryId {get; set;}

        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// The customer user ID
        /// </summary>
        public int CustomerUserId { get; private set; }

        /// <summary>
        /// The source language ID
        /// </summary>
        public LanguageIdAndCodePair SourceLanguage { get; private set; }

        /// <summary>
        /// A list of IDs and codes for target languages
        /// </summary>
        public IReadOnlyCollection<LanguageIdAndCodePair> TargetLanguages { get; private set; }

        /// <summary>
        /// The unit count
        /// </summary>
        public int UnitCount { get; private set; }

        /// <summary>
        /// If the glossary is public
        /// </summary>
        public bool IsPublic { get; private set; }

        /// <summary>
        /// The glossary status
        /// </summary>
        public int Status { get; private set; }

        /// <summary>
        /// The glossary title
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// The glossary description
        /// </summary>
        public string Description { get; private set; }

        internal Glossary(JObject json)
        {
            GlossaryId = json.Value<int>("id");

            CustomerUserId = json.Value<int>("customer_user_id");
            CreatedTime = DateTime.ParseExact(json.Value<string>("ctime"), "yyyy-MM-dd hh:mm:ss.ffffff", CultureInfo.InvariantCulture);

            SourceLanguage =
                new LanguageIdAndCodePair(
                    json.Value<int>("source_language_id"),
                    json.Value<string>("source_language_code"));

            TargetLanguages =
                json.Value<JArray>("target_languages")
                    .Values<JArray>().Select(arr => new LanguageIdAndCodePair(Convert.ToInt32(arr[0]), Convert.ToString(arr[1])))
                    .ToList().AsReadOnly();

            UnitCount = json.Value<int>("unit_count");
            IsPublic = json.Value<bool>("is_public");

            Status = json.Value<int>("status");

            Title = json.Value<string>("title");
            Description = json.Value<string>("description");
        }
    }
}

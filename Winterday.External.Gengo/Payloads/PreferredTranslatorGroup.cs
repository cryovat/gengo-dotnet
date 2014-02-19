//
// PreferredTranslatorGroup.cs
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
    using System.Linq;

    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Represents a group of preferred translators within a source/target/tier combination
    /// </summary>
    public class PreferredTranslatorGroup
    {
        /// <summary>
        /// Language code of source language
        /// </summary>
        public string FromLanguage { get; private set; }

        /// <summary>
        /// Language code for target language
        /// </summary>
        public string ToLanguage { get; private set; }

        /// <summary>
        /// The translation quality tier
        /// </summary>
        public TranslationTier Tier { get; private set; }

        /// <summary>
        /// The translators within the group
        /// </summary>
        public IReadOnlyCollection<PreferredTranslator> Translators { get; set; }

        internal PreferredTranslatorGroup(JObject json)
        {
            FromLanguage = json.Value<string>("lc_src");
            ToLanguage = json.Value<string>("lc_tgt");
            Tier = json.Value<string>("tier").ToTranslationTier();

            var rawTranslators = json.Value<JArray>("translators").Values<JObject>();

            Translators = rawTranslators.Select(pt => new PreferredTranslator(pt)).ToList().AsReadOnly();
        }
    }
}

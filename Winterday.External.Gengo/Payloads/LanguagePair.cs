//
// LanguagePair.cs
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
    /// A possible combination of languages and translation quality offered
    /// by the service, along with pricing information
    /// </summary>
    public class LanguagePair
    {
        /// <summary>
        /// Language code of source language
        /// </summary>
        public string FromLanguage  { get; private set; }

        /// <summary>
        /// Language code for target language
        /// </summary>
        public string ToLanguage  { get; private set; }

        /// <summary>
        /// The translation quality tier
        /// </summary>
        public TranslationTier Tier { get; private set; }

        /// <summary>
        /// The currency code for unit price
        /// </summary>
        public string Currency { get; private set; }

        /// <summary>
        /// The price per translation unit
        /// </summary>
        public decimal UnitPrice { get; private set; }

        internal LanguagePair(JObject obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            FromLanguage = obj.Value<string>("lc_src");
            ToLanguage = obj.Value<string>("lc_tgt");
            Tier = obj.Value<string>("tier").ToTranslationTier();
            Currency = obj.Value<string>("currency");
            UnitPrice = obj.DecValueStrict("unit_price");
        }

        public override string ToString()
        {
            return String.Format("FromLanguage: {0}, ToLanguage: {1}, Tier: {2}, UnitPrice: {3}", FromLanguage, ToLanguage, Tier, UnitPrice);
        }
    }
}


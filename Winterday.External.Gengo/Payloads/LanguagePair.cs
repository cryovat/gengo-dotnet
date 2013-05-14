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

    public class LanguagePair
    {
        readonly string _fromLanguage;
        readonly string _toLanguage;

        readonly string _tier;

        readonly decimal _unitPrice;

        public string FromLanguage
        {
            get
            {
                return _fromLanguage;
            }
        }

        public string ToLanguage
        {
            get
            {
                return _toLanguage;
            }
        }

        public string Tier
        {
            get
            {
                return _tier;
            }
        }

        public decimal UnitPrice
        {
            get
            {
                return _unitPrice;
            }
        }

        LanguagePair(string fromLanguage, string toLanguage, string tier, decimal unitPrice)
        {
            if (string.IsNullOrWhiteSpace(fromLanguage))
                throw new ArgumentException("From Language not provided", "fromLanguage");

            if (string.IsNullOrWhiteSpace(toLanguage))
                throw new ArgumentException("To Language not provided", "toLanguage");

            if (string.IsNullOrWhiteSpace(tier))
                throw new ArgumentException("Tier not provided", "tier");

            _fromLanguage = fromLanguage;
            _toLanguage = toLanguage;
            _tier = tier;

            _unitPrice = unitPrice;
        }

        public override string ToString()
        {
            return String.Format("FromLanguage: {0}, ToLanguage: {1}, Tier: {2}, UnitPrice: {3}", _fromLanguage, _toLanguage, _tier, _unitPrice);
        }



        internal static LanguagePair FromJObject(JObject o)
        {

            return new LanguagePair(
                o.Value<string>("lc_src"),
                o.Value<string>("lc_tgt"),
                o.Value<string>("tier"),
                o.Value<decimal>("unit_price")
                );

        }
    }
}


//
// Quote.cs
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
    using System.Globalization;

    using Newtonsoft.Json.Linq;

    public class FileQuote : Quote
    {
        readonly string _body;
        readonly string _identifier;
        readonly string _sourceLang;
        readonly string _title;

        public string Body
        {
            get { return _body; }
        }

        public string Identifier
        {
            get { return _identifier; }
        }

        public string SourceLanguage
        {
            get { return _sourceLang; }
        }

        public string Title
        {
            get { return _title; }
        }

        internal FileQuote(JObject obj) : base(obj)
        {
            _body = obj.Value<string>("body");
            _identifier = obj.Value<string>("identifier");
            _sourceLang = obj.Value<string>("lc_src");
            _title = obj.Value<string>("title");
        }
    }

    public class Quote
    {

        readonly int _unitCount;

        readonly decimal _credits;
        readonly string _currency;
        readonly string _identifier;
        readonly string _sourceLang;
        
        readonly TimeSpan _eta;
        readonly JobType _type;

        public int UnitCount
        {
            get { return _unitCount; }
        }

        public decimal Credits
        {
            get { return _credits; }
        }

        public string Currency
        {
            get { return _currency; }
        }

        public string Identifier
        {
            get { return _identifier; }
        }

        public string SourceLanguage
        {
            get { return _sourceLang; }
        }

        public TimeSpan Eta
        {
            get { return _eta; }
        }

        public JobType JobType
        {
            get { return _type; }
        }

        internal Quote(JObject obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");

            _unitCount = obj.Value<int>("unit_count");

            Decimal.TryParse(obj.Value<string>("credits"),
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out _credits);

            _currency = obj.Value<string>("currency");
            _identifier = obj.Value<string>("identifier");
            _sourceLang = obj.Value<string>("lc_src");

            _eta = TimeSpan.FromSeconds(obj.Value<int>("eta"));
            _type = obj.Value<string>("type").ToJobType();
        }
    }
}

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

    /// <summary>
    /// Quote for a file-based job
    /// </summary>
    public class FileQuote : Quote
    {
        /// <summary>
        /// Text body extracted from file
        /// </summary>
        public string Body { get; private set; }

        /// <summary>
        /// The title of the submitted job (undocumented,
        /// appears to be file name)
        /// </summary>
        public string Title  { get; private set; }

        internal FileQuote(JObject obj) : base(obj)
        {
            Body = obj.Value<string>("body");
            Title = obj.Value<string>("title");
        }
    }

    /// <summary>
    /// Quote for a text-based job
    /// </summary>
    public class Quote
    {
        /// <summary>
        /// The number of translation units within the job
        /// </summary>
        public int UnitCount { get; private set; }

        /// <summary>
        /// The projected cost of the job
        /// </summary>
        public decimal Credits  { get; private set; }

        /// <summary>
        /// The currency code for the cost
        /// </summary>
        public string Currency  { get; private set; }

        /// <summary>
        /// The identifier of the source file (if applicable)
        /// </summary>
        public string Identifier  { get; private set; }

        /// <summary>
        /// The source language of the job
        /// </summary>
        public string SourceLanguage  { get; private set; }

        /// <summary>
        /// Projected time until job completion
        /// </summary>
        public TimeSpan Eta { get; private set; }

        /// <summary>
        /// Wether the job was submitted as a text or file job
        /// </summary>
        public JobType JobType  { get; private set; }

        internal Quote(JObject obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");

            UnitCount = obj.IntValueStrict("unit_count");
            Credits = obj.DecValueStrict("credits");

            Currency = obj.Value<string>("currency");
            Identifier = obj.Value<string>("identifier");
            SourceLanguage = obj.Value<string>("lc_src");

            Eta = obj.TsValueStrict("eta");
            JobType = obj.Value<string>("type").ToJobType();
        }
    }
}

//
// Confirmation.cs
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
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Newtonsoft.Json.Linq;

    public class Confirmation
    {
        int _groupId;
        int _jobCount;
        int _orderId;

        decimal _creditsUsed;

        string _currency;

        readonly List<DuplicateSubmission> _dupes;
        readonly IReadOnlyList<DuplicateSubmission> _dupesRo;

        public int GroupId
        {
            get { return _groupId; }
        }

        public int JobCount
        {
            get { return _jobCount; }
        }

        public int OrderId
        {
            get { return _orderId; }
        }

        public decimal CreditsUsed
        {
            get { return _creditsUsed; }
        }

        public string Currency
        {
            get { return _currency; }
        }

        public bool HasGroupId
        {
            get { return _groupId != 0; }
        }

        public IReadOnlyList<DuplicateSubmission> Duplicates
        {
            get { return _dupesRo; }
        }

        internal Confirmation(Dictionary<string, Job> submitted, JObject result)
        {
            if (submitted == null)
                throw new ArgumentNullException("submitted");
            
            if (result == null)
                throw new ArgumentNullException("result");

            Int32.TryParse(
                result.Value<string>("group_id"),
                NumberStyles.Integer,
                CultureInfo.InvariantCulture,
                out _groupId);

            Int32.TryParse(
                result.Value<string>("job_count"),
                NumberStyles.Integer,
                CultureInfo.InvariantCulture,
                out _jobCount);

            Int32.TryParse(
                result.Value<string>("order_id"),
                NumberStyles.Integer,
                CultureInfo.InvariantCulture,
                out _orderId);

            Decimal.TryParse(
                result.Value<string>("credits_used"),
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out _creditsUsed);

            _currency = result.Value<string>("currency");

            _dupes = new List<DuplicateSubmission>();
            _dupesRo = _dupes.AsReadOnly();
            
            var dupesObj = result["jobs"] as JObject;

            if (dupesObj != null)
            {
                foreach (var pair in dupesObj)
                {
                    Job dupe = null;
                    var raw = pair.Value as JArray;

                    if (raw != null && submitted.TryGetValue(pair.Key, out dupe))
                    {
                        _dupes.Add(
                            new DuplicateSubmission(
                                dupe,
                                raw.Values<JObject>().Select((o) => 
                                    new SubmittedJob(o))));
                    }
                }
            }
        }


    }
}

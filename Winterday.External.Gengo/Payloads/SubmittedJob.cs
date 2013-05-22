//
// SubmittedJob.cs
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

    public class SubmittedJob : Job
    {
        DateTime _created;

        decimal _credits;

        int _jobId;
        int _unitCount;

        string _currency;
        string _bodyTranslated;

        TimeSpan? _eta;

        TranslationStatus _status;

        Uri _captchaUrl;

        public DateTime Created
        {
            get { return _created; }
        }

        public decimal Credits
        {
            get { return _credits; }
        }

        public int Id
        {
            get { return _jobId; }
        }

        public int UnitCount
        {
            get { return _unitCount; }
        }

        public string Currency
        {
            get { return _currency; }
        }

        public string BodyTranslated
        {
            get { return _bodyTranslated; }
        }

        public TimeSpan? EstimatedTimeToCompletion
        {
            get { return _eta; }
        }

        public TranslationStatus Status
        {
            get { return _status; }
        }

        public Uri CaptchaUrl
        {
            get { return _captchaUrl; }
        }

        internal SubmittedJob(JObject json) : base(json)
        {
            long createdRaw;

            if (Int64.TryParse(json.Value<string>("ctime"), out createdRaw))
            {
                _created = createdRaw.ToDateFromTimestamp();
            }

            Decimal.TryParse(
                json.Value<string>("credits"),
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out _credits);

            Int32.TryParse(json.Value<string>("job_id"), out _jobId);
            Int32.TryParse(json.Value<string>("unit_count"), out _unitCount);

            _currency = json.Value<string>("currency");
            _bodyTranslated = json.Value<string>("body_tgt");

            double eta = -1;

            if (Double.TryParse(
                json.Value<string>("eta"),
                NumberStyles.Integer,
                CultureInfo.InvariantCulture,
                out eta) && eta > -1)
            {
                _eta = TimeSpan.FromSeconds(eta);
            }

            _status = json.Value<string>("status").ToTranslationStatus();

            var captchaUrl = json.Value<string>("captcha_url");

            Uri.TryCreate(captchaUrl, UriKind.Absolute, out _captchaUrl);
        }
    }
}

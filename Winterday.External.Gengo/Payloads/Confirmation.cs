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
    using System.Globalization;

    using Newtonsoft.Json.Linq;

    public class Confirmation
    {
        int _groupId;
        int _jobCount;
        int _orderId;

        decimal _creditsUsed;

        string _currency;

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

        internal Confirmation(JObject obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            Int32.TryParse(
                obj.Value<string>("group_id"),
                NumberStyles.Integer,
                CultureInfo.InvariantCulture,
                out _groupId);

            Int32.TryParse(
                obj.Value<string>("job_count"),
                NumberStyles.Integer,
                CultureInfo.InvariantCulture,
                out _jobCount);

            Int32.TryParse(
                obj.Value<string>("order_id"),
                NumberStyles.Integer,
                CultureInfo.InvariantCulture,
                out _orderId);

            Decimal.TryParse(
                obj.Value<string>("credits_used"),
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out _creditsUsed);

            _currency = obj.Value<string>("currency");
        }


    }
}

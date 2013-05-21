//
// Order.cs
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

    using Newtonsoft.Json.Linq;

    public class Order
    {
        public int OrderId { get; private set; }
        public string Currency { get; private set; }

        public decimal TotalCredits { get; private set; }
        public int TotalUnits { get; private set; }

        public bool UseSameTranslator { get; private set; }

        public int QueuedJobs { get; private set; }
        public int TotalJobs { get; private set; }

        public IReadOnlyCollection<int> AvailableJobs { get; private set; }
        public IReadOnlyCollection<int> PendingJobs { get; private set; }
        public IReadOnlyCollection<int> ReviewableJobs { get; private set; }
        public IReadOnlyCollection<int> ApprovedJobs { get; private set; }

        internal Order(JObject obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            OrderId = obj.IntValueStrict("order_id");
            Currency = obj.Value<string>("currency");

            TotalCredits = obj.DecValueStrict("total_credits");
            TotalUnits = obj.IntValueStrict("total_units");

            UseSameTranslator = obj.BoolValueStrict("as_group");

            QueuedJobs = obj.IntValueStrict("jobs_queued");
            TotalJobs = obj.IntValueStrict("total_jobs");

            AvailableJobs = obj.ReadIntArrayAsRoList("jobs_available");
            PendingJobs = obj.ReadIntArrayAsRoList("jobs_pending");
            ReviewableJobs = obj.ReadIntArrayAsRoList("jobs_reviewable");
            ApprovedJobs = obj.ReadIntArrayAsRoList("jobs_approved");
        }
    }
}

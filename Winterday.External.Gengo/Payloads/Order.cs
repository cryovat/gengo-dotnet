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

    /// <summary>
    /// A submitted and confirmed order
    /// </summary>
    public class Order
    {
        /// <summary>
        /// The order ID
        /// </summary>
        public int OrderId { get; private set; }

        /// <summary>
        /// The currency code for order price
        /// </summary>
        public string Currency { get; private set; }

        /// <summary>
        /// Total cost of order in credits
        /// </summary>
        public decimal TotalCredits { get; private set; }

        /// <summary>
        /// The total number of translation units in order
        /// </summary>
        public int TotalUnits { get; private set; }

        /// <summary>
        /// If the order was submitted with a requirement to use the same
        /// translator for all jobs
        /// </summary>
        [Obsolete("This property appears to have been removed from the API.", true)]
        public bool UseSameTranslator { get; private set; }

        /// <summary>
        /// Count of queued jobs
        /// </summary>
        public int QueuedJobs { get; private set; }

        /// <summary>
        /// Count of all jobs in order
        /// </summary>
        public int TotalJobs { get; private set; }

        /// <summary>
        /// List of IDs of jobs in the 'Available' state
        /// </summary>
        public IReadOnlyList<int> AvailableJobs { get; private set; }

        /// <summary>
        /// List of IDs of jobs in the 'Pending' state
        /// </summary>
        public IReadOnlyList<int> PendingJobs { get; private set; }

        /// <summary>
        /// List of IDs of jobs in the 'Reviewable' state
        /// </summary>
        public IReadOnlyList<int> ReviewableJobs { get; private set; }

        /// <summary>
        /// List of IDs of jobs in the 'Approved' state
        /// </summary>
        public IReadOnlyList<int> ApprovedJobs { get; private set; }

        /// <summary>
        /// List of IDs of jobs in the 'Revising' state
        /// </summary>
        public IReadOnlyList<int> RevisingJobs { get; private set; }

        internal Order(JObject obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            OrderId = obj.IntValueStrict("order_id");
            Currency = obj.Value<string>("currency");

            TotalCredits = obj.DecValueStrict("total_credits");
            TotalUnits = obj.IntValueStrict("total_units");

            QueuedJobs = obj.IntValueStrict("jobs_queued");
            TotalJobs = obj.IntValueStrict("total_jobs");

            AvailableJobs = obj.ReadIntArrayAsRoList("jobs_available");
            PendingJobs = obj.ReadIntArrayAsRoList("jobs_pending");
            ReviewableJobs = obj.ReadIntArrayAsRoList("jobs_reviewable");
            ApprovedJobs = obj.ReadIntArrayAsRoList("jobs_approved");
            RevisingJobs = obj.ReadIntArrayAsRoList("jobs_revising");
        }
    }
}

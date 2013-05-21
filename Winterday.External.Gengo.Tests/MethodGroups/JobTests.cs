//
// JobTests.cs
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

namespace Winterday.External.Gengo.Tests.MethodGroups
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Winterday.External.Gengo.Payloads;

    [TestClass]
    public class JobTests
    {
        GengoClient _client;

        [TestInitialize]
        public void SetUpAttribute()
        {
            _client = new GengoClient(TestKeys.PrivateKey, TestKeys.PublicKey, ClientMode.Sandbox);
        }

        [TestMethod]
        public async Task TestGetJob()
        {
            var availJob = await GetJobWithStatus(TranslationStatus.Available);

            var job = await _client.Job.Get(availJob.Id, true);

            Assert.IsNotNull(job);
        }


        [TestMethod]
        public async Task TestDeleteJob()
        {
            var availJob = await GetJobWithStatus(TranslationStatus.Available);

            await _client.Job.Delete(availJob.Id);

            var job = await _client.Job.Get(availJob.Id, true);

            Assert.AreEqual(TranslationStatus.Cancelled, job.Status);
        }

        [TestMethod]
        public async Task TestGetAndPostComments()
        {
            var now = DateTime.Now;
            var comment = "Posted " + now.ToString();

            var availJob = await GetJobWithStatus(TranslationStatus.Available);

            await _client.Job.PostComment(availJob.Id, comment);

            var comments = await _client.Job.GetComments(availJob.Id);

            Assert.IsNotNull(comments.Where(c => c.Body == comment).FirstOrDefault(),
                "Could not find posted comment");
        }

        [TestMethod]
        public async Task TestReturnForRevision()
        {
            var availJob = await GetJobWithStatus(TranslationStatus.Reviewable);

            await _client.Job.ReturnForRevision(availJob.Id, "Not really good");
        }

        [TestMethod]
        public async Task TestGetFeedback()
        {
            var availJob = await GetJobWithStatus(TranslationStatus.Approved);

            var feedback = await _client.Job.GetFeedback(availJob.Id);

            Assert.IsNotNull(feedback);
        }

        private async Task<TimestampedId> GetJobWithStatus(TranslationStatus status)
        {
            var items = await _client.Jobs.GetRecentJobs(status: status);

            if (items.Length == 0)
            {
                Assert.Inconclusive("Could not find job with status " + status);
            }

            return items[0];
        }
    }
}

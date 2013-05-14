//
// JobsTests.cs
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

namespace Winterday.External.Gengo.Tests.Endpoints
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Winterday.External.Gengo.Payloads;

    [TestClass]
    public class JobsTests
    {
        GengoClient client;

        [TestInitialize]
        public void SetUpAttribute()
        {
            client = new GengoClient(TestKeys.PrivateKey, TestKeys.PublicKey, ClientMode.Sandbox);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task TestThrowsOnNoJobs()
        {
            var res = await client.Jobs.Submit(true, true);

            Assert.Fail("Exception was not thrown");
        }

        [TestMethod]
        public async Task TestSubmitTwoJobs()
        {
            var job1Dt = DateTime.Now;

            var job1 = new Job
            {
                Slug = "job 1 - " + job1Dt.ToTimeStamp(),
                Body = job1Dt.ToString(),
                SourceLanguage = "ja",
                TargetLanguage = "en",
            };

            var job2Dt = DateTime.Now;

            var job2 = new Job
            {
                Slug = "job 2 - " + job1Dt.ToTimeStamp(),
                Body = job2Dt.ToString(),
                SourceLanguage = "en",
                TargetLanguage = "ja",
                CallbackUrl = new Uri("http://www.zombo.com")
            };

            var res = await client.Jobs.Submit(false, true, job1, job2);

            Assert.AreEqual(2, res.JobCount);
        }
    }
}

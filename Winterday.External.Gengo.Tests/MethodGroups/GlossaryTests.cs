//
// GlossaryTests.cs
//
// Author:
//       Jarl Erik Schmidt <github@jarlerik.com>
//
// Copyright (c) 2014 Jarl Erik Schmidt
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

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Winterday.External.Gengo.Tests.MethodGroups
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Winterday.External.Gengo.MethodGroups;
    using Winterday.External.Gengo.Payloads;
    using Winterday.External.Gengo.Tests.Mocks;

    [TestClass]
    public class GlossaryTests
    {
        GengoClient _client;

        [TestInitialize]
        public void SetUpAttribute()
        {
            _client = new GengoClient(TestKeys.PrivateKey, TestKeys.PublicKey, ClientMode.Sandbox);
        }

        [TestMethod]
        public void TestGroupExists()
        {
            Assert.IsNotNull(_client.Glossary);
        }

        [TestMethod]
        public async Task TestGetAllMocked()
        {
            var mockedClient = new MockedGengoClient();
            var group = new GlossaryMethodGroup(mockedClient);

            mockedClient.Json[GlossaryMethodGroup.UriPartGlossary] = @"
[
    {
      ""customer_user_id"": 50110,
      ""source_language_id"": 8,
      ""target_languages"": [
        [
          14,
          ""ja""
        ]
      ],
      ""id"": 115,
      ""is_public"": false,
      ""unit_count"": 2,
      ""description"": null,
      ""source_language_code"": ""en-US"",
      ""ctime"": ""2012-07-19 02:57:10.526565"",
      ""title"": ""1342666627_50110_en_ja_glossary.csv"",
      ""status"": 1
    }
]
";
            var list = await group.GetAll();

            Assert.AreEqual(1, list.Length, "Length does not match");
            Assert.AreEqual(115, list[0].GlossaryId, "Ids don't match");
            Assert.AreEqual(50110, list[0].CustomerUserId, "Customer Ids don't match");
            Assert.AreEqual("1342666627_50110_en_ja_glossary.csv", list[0].Title, "Titles don't match");

            var dt = list[0].CreatedTime;
            Assert.AreEqual(new DateTime(2012, 07, 19, 2, 57, 10), new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second), "Dates don't match");
        }

        [TestMethod]
        public async Task TestGetOneMocked()
        {
            var mockedClient = new MockedGengoClient();
            var group = new GlossaryMethodGroup(mockedClient);

            mockedClient.Json[GlossaryMethodGroup.UriPartGlossary + 115] = @"
{
    ""customer_user_id"": 50110,
    ""source_language_id"": 8,
    ""target_languages"": [
    [
        14,
        ""ja""
    ]
    ],
    ""id"": 115,
    ""is_public"": false,
    ""unit_count"": 2,
    ""description"": null,
    ""source_language_code"": ""en-US"",
    ""ctime"": ""2012-07-19 02:57:10.526565"",
    ""title"": ""1342666627_50110_en_ja_glossary.csv"",
    ""status"": 1
}
";
            var glossary = await group.Get(115);

            Assert.IsNotNull(glossary);
            Assert.AreEqual(115, glossary.GlossaryId, "Ids don't match");
            Assert.AreEqual(50110, glossary.CustomerUserId, "Customer Ids don't match");
            Assert.AreEqual("1342666627_50110_en_ja_glossary.csv", glossary.Title, "Titles don't match");

            var dt = glossary.CreatedTime;
            Assert.AreEqual(new DateTime(2012, 07, 19, 2, 57, 10), new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second), "Dates don't match");
        }

        [TestMethod]
        public async Task TestGetAllLive()
        {
            var list = await _client.Glossary.GetAll();

            Assert.IsNotNull(list);
        }

        [TestMethod]
        public async Task TestGetOneLive()
        {
            var list = await _client.Glossary.GetAll();

            if (list.Length == 0)
            {
                Assert.Inconclusive("This test requires at least one glossary");
            }

            var single = await _client.Glossary.Get(list[0].GlossaryId);

            Assert.AreEqual(list[0].GlossaryId, single.GlossaryId, "Ids don't match");
            Assert.AreEqual(list[0].Title, single.Title, "Titles don't match");
            Assert.AreEqual(list[0].CreatedTime, single.CreatedTime, "Dates don't match");
        }
    }
}

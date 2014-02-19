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

    using Winterday.External.Gengo.Payloads;

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
        public async Task TestGetAll()
        {
            var list = await _client.Glossary.GetAll();

            Assert.IsNotNull(list);
        }

        [TestMethod]
        public async Task TestGetOne()
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

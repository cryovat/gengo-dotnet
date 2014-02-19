//
// AccountTests.cs
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

namespace Winterday.External.Gengo.Tests.MethodGroups
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Winterday.External.Gengo.MethodGroups;
    using Winterday.External.Gengo.Tests.Mocks;

    [TestClass]
    public class AccountTests
    {
        GengoClient client;

        [TestInitialize]
        public void SetUpAttribute()
        {
            client = new GengoClient(TestKeys.PrivateKey, TestKeys.PublicKey, ClientMode.Sandbox);
        }

        [TestMethod]
        public async Task TestGetStats()
        {
            var stats = await client.Account.GetStats();

            Assert.IsNotNull(stats);
            Assert.AreNotEqual(new DateTime(), stats.UserSince);
            Assert.IsTrue(0 != stats.CreditsSpent);
        }

        [TestMethod]
        public async Task TestGetBalance()
        {
            var balance = await client.Account.GetBalance();

            Assert.IsNotNull(balance);
            Assert.IsTrue(balance.Credits > 0);
            Assert.IsNotNull(balance.Currency);
        }

        [TestMethod]
        public async Task TestGetPreferredTranslators()
        {
            var mockedClient = new MockedGengoClient();
            var group = new AccountMethodGroup(mockedClient);

            mockedClient.Json["account/preferred_translators"] = @"
[
    {
      ""translators"": [
        {
          ""last_login"": 1375824155,
          ""number_of_jobs"": 5,
          ""id"": 8596
        },
        {
          ""last_login"": 1372822132,
          ""number_of_jobs"": 2,
          ""id"": 24123
        }
      ],
      ""lc_tgt"": ""ja"",
      ""lc_src"": ""en"",
      ""tier"": ""standard""
    },
    {
      ""translators"": [
        {
          ""last_login"": 1375825234,
          ""number_of_jobs"": 10,
          ""id"": 14765
        },
        {
          ""last_login"": 1372822132,
          ""number_of_jobs"": 1,
          ""id"": 3627
        }
      ],
      ""lc_tgt"": ""en"",
      ""lc_src"": ""ja"",
      ""tier"": ""pro""
    }
]
";

            var preferences = await group.GetPreferredTranslators();

            Assert.IsNotNull(preferences);
            Assert.AreEqual(2, preferences.Length, "Lengths does not match");

            Assert.AreEqual(new DateTime(2013, 7, 3), preferences[0].Translators[1].LastLogin.Date, "Login dates don't match");
            Assert.AreEqual(TranslationTier.Standard, preferences[0].Tier, "First tier don't match");
            Assert.AreEqual(TranslationTier.Pro, preferences[1].Tier, "Second tier don't match");
            Assert.AreEqual(2, preferences[0].Translators[1].NumberOfJobs, "Number of jobs don't match");
            Assert.AreEqual(3627, preferences[1].Translators[1].Id, "Translator Ids don't match");
        }

    }
}


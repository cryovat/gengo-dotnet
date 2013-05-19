//
// ServiceTests.cs
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
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Winterday.External.Gengo.Payloads;

    [TestClass]
    public class ServiceTests
    {
        GengoClient client;

        [TestInitialize]
        public void SetUpAttribute()
        {
            client = new GengoClient(TestKeys.PrivateKey, TestKeys.PublicKey, ClientMode.Sandbox);
        }

        [TestMethod]
        public async Task TestGetLanguagePairs()
        {
            var pairs = await client.Service.GetLanguagePairs();

            Assert.IsTrue(pairs.Length > 0);
        }

        [TestMethod]
        public async Task TestGetLanguages()
        {
            var langs = await client.Service.GetLanguages();

            Assert.IsTrue(langs.Length > 0);
        }

        [TestMethod]
        public async Task TestGetQuotes()
        {
            var job1 = new Job
            {
                Slug = "quote job 1",
                Body = "Never gonna give you up",
                SourceLanguage = "en",
                TargetLanguage = "ja"
            };

            var job2 = new Job
            {
                Slug = "quote job 2",
                Body = "Never gonna let you down",
                SourceLanguage = "en",
                TargetLanguage = "ja",
                TranslationTier = TranslationTier.Ultra
            };

            var job3 = new Job
            {
                Slug = "quote job 3",
                Body = "Never gonna run around and desert you",
                SourceLanguage = "en",
                TargetLanguage = "ja",
                TranslationTier = TranslationTier.Pro
            };

            var quotes = await client.Service.GetQuote(true, job1, job2, job3);

            Assert.AreEqual(3, quotes.Length);
        }

        [TestMethod]
        public async Task TestGetFileQuote()
        {
            var text = "Hang down your head, Tom Dooley\nHang down your head and cry";
            var raw = Encoding.UTF8.GetBytes(text);

            var file = new FileJob("lyrics.txt", raw)
            {
                Slug = "test file",
                SourceLanguage = "en",
                TargetLanguage = "ja",
                TranslationTier = TranslationTier.Standard
            };

            var quote = await client.Service.GetQuoteForFiles(file);

            Assert.AreEqual(1, quote.Length);
            Assert.AreEqual(text, quote[0].Body);
        }
    }
}

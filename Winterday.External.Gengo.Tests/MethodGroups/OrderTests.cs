//
// OrderTests.cs
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
    public class OrderTests
    {
        GengoClient _client;

        [TestInitialize]
        public void SetUpAttribute()
        {
            _client = new GengoClient(TestKeys.PrivateKey, TestKeys.PublicKey, ClientMode.Sandbox);
        }

        [TestMethod]
        public async Task TestCreateGetAndDeleteOrder()
        {
            var now = DateTime.Now;

            var confirmation = await _client.Jobs.Submit(true, false,
                new Job()
                {
                    Slug = "Test " + now.ToShortDateString(),
                    Body = "Test " + now.ToString(),
                    SourceLanguage = "en",
                    TargetLanguage = "ja",
                });

            var order = await _client.Order.Get(confirmation.OrderId);

            await _client.Order.Delete(order.OrderId);

            Thread.Sleep(TimeSpan.FromSeconds(5));

            try
            {
                order = await _client.Order.Get(order.OrderId);

                Assert.Fail("Failed to delete order?");
            }
            catch (GengoException)
            {
                Assert.IsTrue(true);
            }
        }
    }
}

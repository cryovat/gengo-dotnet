//
// GengoClientTests.cs
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

namespace Winterday.External.Gengo.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class GengoClientTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestConstructorFailsOnRelativeUri()
        {
            var client = new GengoClient("foo", "bar", "baz");
            Assert.Fail("Constructor did not throw exception");
        }

        [TestMethod]
        public void TestConstructorValidatesCustomUri()
        {
            var client = new GengoClient("foo", "bar", "http://www.example.com");
            Assert.IsTrue(true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestBuildUriThrowsOnNullArgument()
        {
            var client = new GengoClient("foo", "bar");
            client.BuildUri(null, false);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestBuildUriThrowsOnAbsoluteUri()
        {
            var client = new GengoClient("foo", "bar");
            client.BuildUri("http://absolute.uri/", false);
        }

        [TestMethod]
        public void TestBuildUriMinimalConstructor()
        {
            String expected = GengoClient.ProductionBaseUri + "bazinga?api_key=bar";

            var prodClient = new GengoClient("foo", "bar");
            var uri = prodClient.BuildUri("bazinga", false);

            Assert.AreEqual(expected, uri.ToString());
        }

        [TestMethod]
        public void TestBuildUriProductionConstructor()
        {
            String expected = GengoClient.ProductionBaseUri + "bazinga?api_key=bar";

            var prodClient = new GengoClient("foo", "bar", ClientMode.Production);
            var uri = prodClient.BuildUri("bazinga", false);

            Assert.AreEqual(expected, uri.ToString());
        }

        [TestMethod]
        public void TestBuildUriSandboxConstructor()
        {
            String expected = GengoClient.SandboxBaseUri + "bazinga?api_key=bar";

            var prodClient = new GengoClient("foo", "bar", ClientMode.Sandbox);
            var uri = prodClient.BuildUri("bazinga", false);

            Assert.AreEqual(expected, uri.ToString());
        }

        [TestMethod]
        public void TestBuildUriCustomUriConstructor()
        {
            String customUri = "http://www.example.com/";
            String expected = customUri + "bazinga?api_key=bar";

            var prodClient = new GengoClient("foo", "bar", customUri);
            var uri = prodClient.BuildUri("bazinga", false);

            Assert.AreEqual(expected, uri.ToString());
        }

    }
}


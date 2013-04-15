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
	using NUnit.Framework;

	[TestFixture()]
	public class GengoClientTests
	{
		[Test()]
		public void TestConstructorValidatesCustomUri () {

			Assert.Throws<ArgumentException> (() => new GengoClient("foo", "bar", "baz"));
			Assert.DoesNotThrow (() => new GengoClient("foo", "bar", "http://www.example.com"));
		}

		[Test()]
		public void TestBuildUriValidesParameters ()
		{
			var client = new GengoClient ("foo", "bar");

			Assert.Throws <ArgumentException> (() => client.BuildUri (null));
			Assert.Throws <ArgumentException> (() => client.BuildUri ("http://absolute.uri/"));
		}

		[Test()]
		public void TestBuildUriMinimalConstructor ()
		{
			String expected = GengoClient.ProductionBaseUri + "bazinga";
			
			var prodClient = new GengoClient ("foo", "bar");
			var uri = prodClient.BuildUri ("bazinga");
			
			Assert.AreEqual (expected, uri.ToString ());
		}

		[Test()]
		public void TestBuildUriProductionConstructor ()
		{
			String expected = GengoClient.ProductionBaseUri + "bazinga";
			
			var prodClient = new GengoClient ("foo", "bar", ClientMode.Production);
			var uri = prodClient.BuildUri ("bazinga");
			
			Assert.AreEqual (expected, uri.ToString ());
		}

		[Test()]
		public void TestBuildUriSandboxConstructor ()
		{
			String expected = GengoClient.SandboxBaseUri + "bazinga";
			
			var prodClient = new GengoClient ("foo", "bar", ClientMode.Sandbox);
			var uri = prodClient.BuildUri ("bazinga");
			
			Assert.AreEqual (expected, uri.ToString ());
		}

		[Test()]
		public void TestBuildUriCustomUriConstructor ()
		{
			String customUri = "http://www.example.com/";
			String expected = customUri + "bazinga";
			
			var prodClient = new GengoClient ("foo", "bar", customUri);
			var uri = prodClient.BuildUri ("bazinga");
			
			Assert.AreEqual (expected, uri.ToString ());
		}

		[Test()]
		public void TestBuildRequestSetsModes ()
		{
			var client = new GengoClient ("foo", "bar");
			var request = client.BuildRequest ("hello", HttpMethod.Post);

			Assert.IsTrue (String.Equals (request.Accept, "application/xml", StringComparison.OrdinalIgnoreCase),
			               "Accept should be application/xml, got: " + request.Accept);

			Assert.IsTrue (String.Equals (request.Method, "post", StringComparison.OrdinalIgnoreCase),
			               "Method should be POST, got: " + request.Method);
		}
	}
}


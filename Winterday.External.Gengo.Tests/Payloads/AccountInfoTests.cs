//
// AccountStatsTests.cs
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
using System;
using NUnit.Framework;

namespace Winterday.External.Gengo.Tests.Payloads
{
	[TestFixture()]
	public class AccountInfoTest
	{
		GengoClient client;
		
		[TestFixtureSetUp]
		public void SetUpAttribute()
		{
			client = new GengoClient (TestKeys.PrivateKey, TestKeys.PublicKey, ClientMode.Sandbox);
		}
		
		[Test()]
		public void TestGetStats ()
		{
			var stats = client.GetStats ();

			Assert.IsNotNull (stats);
			Assert.AreNotEqual (new DateTime (), stats.UserSince);
			Assert.GreaterOrEqual (0, stats.CreditsSpent);
		}

		[Test()]
		public void TestGetBalance ()
		{
			Assert.DoesNotThrow (() => client.GetBalance ());
		}
	}
}


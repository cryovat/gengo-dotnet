//
// ExtensionsTests.cs
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
	using System.Collections.Generic;

	using NUnit.Framework;

	using Winterday.External.Gengo;

	[TestFixture()]
	public class ExtensionsTests
	{
		[Test()]
		public void ToQueryStringNullDict ()
		{
			Dictionary<string, string> nothing;

			Assert.IsTrue (String.IsNullOrWhiteSpace (nothing.ToQueryString()),
			               "Null dict should yield empty string");
		}

		[Test()]
		public void ToQueryStringOneValue ()
		{
			var dict = new Dictionary<string, string>();
			dict ["foo"] = "bar";

			Assert.AreEqual ("?foo=bar",
			                 dict.ToQueryString ());
		}

		[Test()]
		public void ToQueryStringTwoValues ()
		{
			var dict = new Dictionary<string, string>();
			dict ["foo"] = "bar";
			dict ["ba"] = "zinga";
			
			Assert.AreEqual ("?foo=bar&ba=zinga",
			                 dict.ToQueryString ());
		}

		[Test()]
		public void ToQueryStringUnsafeValues ()
		{
			var dict = new Dictionary<string, string>();
			dict ["foo"] = "#?&%=";

			Assert.AreEqual ("?foo=%23%3F%26%25%3D",
			                 dict.ToQueryString ());
		}

		[Test()]
		public void TranslationStatusConversions()
		{
			Assert.AreEqual ("approved", TranslationStatus.Approved.ToStatusString ());
			Assert.AreEqual ("available", TranslationStatus.Available.ToStatusString ());
			Assert.AreEqual ("cancelled", TranslationStatus.Cancelled.ToStatusString ());
			Assert.AreEqual ("pending", TranslationStatus.Pending.ToStatusString ());
			Assert.AreEqual ("reviewable", TranslationStatus.Reviewable.ToStatusString ());
			Assert.AreEqual ("revising", TranslationStatus.Revising.ToStatusString ());

			Assert.AreEqual (TranslationStatus.Approved, "ApPrOvEd".ToTranslationStatus ());
			Assert.AreEqual (TranslationStatus.Available, "AVAILable".ToTranslationStatus ());
			Assert.AreEqual (TranslationStatus.Cancelled, "cancelLED".ToTranslationStatus ());
			Assert.AreEqual (TranslationStatus.Pending, "pendinG".ToTranslationStatus());
			Assert.AreEqual (TranslationStatus.Reviewable, "reviEWable".ToTranslationStatus ());
			Assert.AreEqual (TranslationStatus.Revising, "reviSING".ToTranslationStatus ());

			Assert.AreEqual (TranslationStatus.Unknown, ((string)null).ToTranslationStatus ());
			Assert.AreEqual (TranslationStatus.Unknown, "bazinga".ToTranslationStatus ());
		}

		[Test()]
		public void TranslationTierConversions()
		{
			Assert.AreEqual ("ultra", TranslationTier.Ultra.ToTierString ());
			Assert.AreEqual ("machine", TranslationTier.Machine.ToTierString ());
			Assert.AreEqual ("standard", TranslationTier.Standard.ToTierString ());
			Assert.AreEqual ("pro", TranslationTier.Pro.ToTierString ());

			Assert.AreEqual (TranslationTier.Machine, "MACHINE".ToTranslationTier ());
			Assert.AreEqual (TranslationTier.Pro, "PrO".ToTranslationTier ());
			Assert.AreEqual (TranslationTier.Standard, "StandarD".ToTranslationTier ());
			Assert.AreEqual (TranslationTier.Ultra, "UlTrA".ToTranslationTier ());
			
			Assert.AreEqual (TranslationTier.Unknown, ((string)null).ToTranslationTier ());
			Assert.AreEqual (TranslationTier.Unknown, "bazinga".ToTranslationTier ());
		}

		[Test()]
		public void JobTypeConversions() {

			Assert.AreEqual ("text", JobType.Text.ToTypeString ());
			Assert.AreEqual ("file", JobType.File.ToTypeString ());

			Assert.AreEqual (JobType.File, "fiLE".ToJobType ());
			Assert.AreEqual (JobType.Text, "TeXT".ToJobType ());

			Assert.AreEqual (JobType.Text, "bazinga".ToJobType ());
		}

		[Test()]
		public void TestUnixTimeStamp()
		{
			var utcNow = DateTime.UtcNow;

			// Shave off milliseconds:
			utcNow = new DateTime (utcNow.Year, utcNow.Month, utcNow.Day,
			                       utcNow.Hour, utcNow.Minute, utcNow.Second, 0,
			                       DateTimeKind.Utc);

			Assert.AreEqual (utcNow, utcNow.ToTimeStamp ().ToDateFromTimestamp ());
		}
	}
}


//
// Extensions.cs
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
namespace Winterday.External.Gengo
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.IO;
	using System.Linq;
	using System.Net;
	using System.Security.Cryptography;
	using System.Text;

	public static class Extensions
	{
		public static decimal ToDecimal (this string value)
		{
			if (string.IsNullOrWhiteSpace (value)) {
				return 0;
			}

			decimal d;
			Decimal.TryParse (value, NumberStyles.Number, CultureInfo.InvariantCulture, out d);

			return d;
		}

		public static string ToQueryString (this Dictionary<string, string> dict)
		{
			if (dict == null) {
				return string.Empty;
			}

			var sb = new StringBuilder ();

			foreach (var pair in dict) {
				if (!string.IsNullOrWhiteSpace (pair.Key)) {
					if (sb.Length == 0) {
						sb.Append ("?");
					} else {
						sb.Append ("&");
					}

					sb.Append (pair.Key.HexEscape ());
					sb.Append ("=");
					sb.Append (pair.Value.HexEscape ());
				}
			}

			return sb.ToString ();
		}

		static string HexEscape (this string value)
		{
			return String.Join (String.Empty, value.ToCharArray ().Select (c => c.HexEscape ()));
		}

		static string HexEscape (this char c)
		{
			if ((c >= 48 && c <= 57) || (c >= 65 && c <= 90) || (c >= 97 && c <= 122)) {
				return c.ToString ();
			}

			return Uri.HexEscape (c);
		}

		public static string ToTierString (this TranslationTier tier)
		{
			return tier.ToString ().ToLowerInvariant ();
		}

		public static TranslationTier ToTranslationTier (this string value)
		{
			TranslationTier tier;

			if (Enum.TryParse<TranslationTier> (value, true, out tier)) {
				return tier;
			}

			return TranslationTier.Unknown;
		}

		public static string ToStatusString (this TranslationStatus status)
		{
			return status.ToString ().ToLowerInvariant ();
		}

		public static TranslationStatus ToTranslationStatus(this string value)
		{
			TranslationStatus status;

			if (Enum.TryParse <TranslationStatus> (value, true, out status)) {
				return status;
			}

			return TranslationStatus.Unknown;
		}

		public static string ToMethodString (this HttpMethod method)
		{
			return method.ToString ().ToUpperInvariant ();
		}

		public static String SHA1Hash (String privateKey, String value)
		{
			var utf8 = Encoding.UTF8;
			var keybytes = utf8.GetBytes (privateKey);
			var valuebytes = utf8.GetBytes (value);

			var algo = new HMACSHA1 (keybytes);

			using (var ms = new MemoryStream(valuebytes)) {
				return Convert.ToBase64String (algo.ComputeHash (ms));
			}
		}

		static readonly DateTime unixEpoch = new DateTime (1970, 1, 1, 0, 0, 0);

		public static int ToTimeStamp (this DateTime time)
		{			
			return (int)(time - unixEpoch).TotalSeconds;
		}
	}
}


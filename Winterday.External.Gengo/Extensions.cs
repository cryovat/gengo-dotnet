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
    using System.Threading.Tasks;

    using Newtonsoft.Json.Linq;

    using Winterday.External.Gengo.Payloads;
    using Winterday.External.Gengo.Properties;
    
    /// <summary>
    /// Contains extension methods used internally to ease working with Json
    /// content and enumerations.
    /// </summary>
    internal static class Extensions
    {
        internal static decimal ToDecimal(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return 0;
            }

            decimal d;
            Decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out d);

            return d;
        }

        const NumberStyles UIntStyle = NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

        internal static long ToLong(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }

            long i;

            Int64.TryParse(value, UIntStyle, CultureInfo.InvariantCulture, out i);

            return i;
        }

        internal static string ToQueryString(this Dictionary<string, string> dict)
        {
            if (dict == null)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();

            foreach (var pair in dict)
            {
                if (!string.IsNullOrWhiteSpace(pair.Key))
                {
                    if (sb.Length == 0)
                    {
                        sb.Append("?");
                    }
                    else
                    {
                        sb.Append("&");
                    }

                    sb.Append(pair.Key.HexEscape());
                    sb.Append("=");
                    sb.Append(pair.Value.HexEscape());
                }
            }

            return sb.ToString();
        }

        static string HexEscape(this string value)
        {
            return String.Join(String.Empty, value.ToCharArray().Select(c => c.HexEscape()));
        }

        static string HexEscape(this char c)
        {
            if ((c >= 48 && c <= 57) || (c >= 65 && c <= 90) || (c >= 97 && c <= 122))
            {
                return c.ToString();
            }

            return Uri.HexEscape(c);
        }

        internal static string ToTypeString(this JobType type)
        {
            return type.ToString().ToLowerInvariant();
        }

        internal static EnumT TryParseEnum<EnumT>(this string value, EnumT defaultValue, bool removeSpaces) where EnumT : struct
        {
            EnumT result = defaultValue;
            
            if (removeSpaces)
            {
                value = (value ?? String.Empty).Replace(" ", "");
            }

            if (Enum.TryParse<EnumT>(value, true, out result))
            {
                return result;
            }

            return defaultValue;
        }

        internal static AuthorType ToAuthorType(this string value)
        {
            return value.TryParseEnum(AuthorType.Unknown, true); 
        }

        internal static string ToAuthorString(this AuthorType type)
        {
            if (type == AuthorType.SeniorTranslator)
            {
                return "senior translator";
            }
            else
            {
                return type.ToString().ToLowerInvariant();
            }
        }

        internal static JobType ToJobType(this string value)
        {
            return value.TryParseEnum(JobType.Text, false);
        }

        internal static string ToReasonString(this RejectionReason reason)
        {
            return reason.ToString().ToLowerInvariant();
        }

        internal static string ToTierString(this TranslationTier tier)
        {
            return tier.ToString().ToLowerInvariant();
        }

        internal static TranslationTier ToTranslationTier(this string value)
        {
            return value.TryParseEnum(TranslationTier.Unknown, false);
        }

        internal static string ToStatusString(this TranslationStatus status)
        {
            return status.ToString().ToLowerInvariant();
        }

        internal static TranslationStatus ToTranslationStatus(this string value)
        {
            return value.TryParseEnum(TranslationStatus.Unknown, false);
        }

        internal static byte[] ToUTF8Bytes(this string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }

        internal static String SHA1Hash(this String privateKey, String value)
        {
            var keybytes = privateKey.ToUTF8Bytes();
            var valuebytes = value.ToUTF8Bytes();

            var algo = new HMACSHA1(keybytes);
            var hash = algo.ComputeHash(valuebytes);

            var sb = new StringBuilder(2 * hash.Length);

            foreach (var b in hash)
            {
                sb.AppendFormat("{0:x2}", b);
            }

            return sb.ToString();
        }

        static readonly DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        internal static long ToTimeStamp(this DateTime time)
        {
            return (long)(time - unixEpoch).TotalSeconds;
        }

        internal static DateTime ToDateFromTimestamp(this long timestamp)
        {
            return unixEpoch.AddSeconds(timestamp);
        }

        internal static JArray ToJsonJobsArray(this IEnumerable<Job> jobs)
        {
            if (jobs == null) throw new ArgumentNullException("jobs");

            var arr = new JArray();

            foreach (var job in jobs)
            {
                arr.Add(job.ToJObject());
            }

            return arr;
        }

        internal static Tuple<JObject, Dictionary<string, Job>> ToJsonJobsList(this IEnumerable<Job> jobs)
        {
            if (jobs == null) throw new ArgumentNullException("jobs");

            var count = 1;

            var jobsObj = new JObject();
            var mapping = new Dictionary<string, Job>();

            foreach (var item in jobs)
            {
                if (item is SubmittedJob)
                    throw new ArgumentException(
                        Resources.JobIsAlreadySubmitted);

                var key = "job_" + count;

                mapping[key] = item;
                jobsObj[key] = item.ToJObject();

                count += 1;
            }

            return Tuple.Create(jobsObj, mapping);
        }

        internal static IReadOnlyCollection<int>
            ReadIntArrayAsRoList(this JObject obj, string propName)
        {
            if (string.IsNullOrWhiteSpace(propName))
                throw new ArgumentException(Resources.PropertyNameNotProvided);

            var list = new List<int>();

            obj.ReadIntArrayIntoList(propName, list);

            return list;
        }

        internal static void ReadIntArrayIntoList(this JObject obj, string propName, IList<int> ints)
        {
            if (string.IsNullOrWhiteSpace(propName))
                throw new ArgumentException(Resources.PropertyNameNotProvided);
            
            if (ints == null)
                throw new ArgumentNullException("ints");

            if (ints.IsReadOnly)
                throw new ArgumentException(Resources.ListIsReadOnly, "ints");

            if (obj == null)
                return;

            var arr = obj.Value<JArray>(propName);

            if (arr == null)
                return;

            foreach (var item in arr)
            {
                if (item == null) continue;

                int i;

                if (Int32.TryParse(
                    item.ToString(),
                    NumberStyles.Integer,
                    CultureInfo.InvariantCulture,
                    out i))
                {
                    ints.Add(i);
                }
            }
        }

        internal static bool BoolValueStrict(this JObject json, string propName)
        {
            return (json.IntValueStrict(propName) == 1);
        }


        internal static DateTime DateValueStrict(this JObject json, string propName)
        {
            return (json.LongValueStrict(propName).ToDateFromTimestamp());
        }


        internal static decimal DecValueStrict(this JObject json, string propName)
        {
            if (json == null)
                throw new ArgumentNullException("json");

            if (string.IsNullOrWhiteSpace(propName))
                throw new ArgumentException(Resources.PropertyNameNotProvided);

            decimal i;

            if (Decimal.TryParse(
                json.Value<string>(propName),
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out i))
            {
                return i;
            }
            else
            {
                var err = string.Format
                    (Resources.NamedPropertyNotFound,
                    propName);

                throw new ArgumentException(err, "json");
            }

        }

        internal static int IntValueStrict(this JObject json, string propName)
        {
            if (json == null)
                throw new ArgumentNullException("json");

            if (string.IsNullOrWhiteSpace(propName))
                throw new ArgumentException(Resources.PropertyNameNotProvided);

            int i;

            if (Int32.TryParse(
                json.Value<string>(propName),
                NumberStyles.Integer,
                CultureInfo.InvariantCulture,
                out i))
            {
                return i;
            }
            else {
                var err = string.Format
                    (Resources.NamedPropertyNotFound,
                    propName);

                throw new ArgumentException(err, "json");
            }

        }

        internal static long LongValueStrict(this JObject json, string propName)
        {
            if (json == null)
                throw new ArgumentNullException("json");

            if (string.IsNullOrWhiteSpace(propName))
                throw new ArgumentException(Resources.PropertyNameNotProvided);

            long i;

            if (Int64.TryParse(
                json.Value<string>(propName),
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out i))
            {
                return i;
            }
            else
            {
                var err = string.Format
                    (Resources.NamedPropertyNotFound,
                    propName);

                throw new ArgumentException(err, "json");
            }

        }

        internal static TimeSpan TsValueStrict(this JObject json, string propName)
        {
            return TimeSpan.FromSeconds(json.IntValueStrict(propName));
        }

        internal static async Task<JsonT> GetJsonPropertyAsync<JsonT>(
            this IGengoClient client, string propName, String uriPart, bool authenticated
            ) where JsonT : JToken
        {
            var data = await client.GetJsonAsync<JObject>(uriPart, authenticated);

            if (data == null) throw new Exception(
                "Remote service did not return object.");

            var result = data[propName] as JsonT;

            if (result == null) throw new Exception(
                "Object returned by remote service did not have property " + propName + " of type " + typeof(JsonT).Name);
            return result;
        }

        internal static IEnumerable<JObject> Objects(this JArray arr)
        {
            return arr == null ?
                Enumerable.Empty<JObject>() :
                arr.OfType<JObject>();
        }

        internal static IEnumerable<T> SelectFromObjects<T>(
            this JArray arr, Func<JObject, T> selector)
        {
            if (selector == null) throw new ArgumentNullException("func");

            return arr.Objects().Select(selector);
        }
        
    }
}


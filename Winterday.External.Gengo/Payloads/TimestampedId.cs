//
// TimestampedId.cs
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

namespace Winterday.External.Gengo.Payloads
{
    using System;
    using System.Globalization;

    using Newtonsoft.Json.Linq;

    public class TimestampedId
    {
        private readonly int _id;
        private readonly DateTime _created;

        public int Id
        {
            get { return _id; }
        }

        public DateTime Created
        {
            get { return _created; }
        }

        internal TimestampedId(JObject obj, string idProp, string createdProp)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            
            if (idProp == null)
                throw new ArgumentNullException("idProp");
            
            if (createdProp == null)
                throw new ArgumentNullException("createdProp");

            Int32.TryParse(
                obj.Value<string>(idProp), NumberStyles.Integer,
                CultureInfo.InvariantCulture, out _id);

            long ts;

            Int64.TryParse(
                obj.Value<string>(createdProp), NumberStyles.Integer,
                CultureInfo.InvariantCulture, out ts);

            if (ts > 0)
            {
                _created = ts.ToDateFromTimestamp();
            }
        }


    }
}

//
// LanguagePair.cs
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
    using System.Linq;

    using Newtonsoft.Json.Linq;

    public class Comment
    {
        readonly int _jobID;
        readonly string _body;
        readonly AuthorType _author;
        readonly DateTime _created;

        public int JobID
        {
            get
            {
                return _jobID;
            }
        }

        public string Body
        {
            get
            {
                return _body;
            }
        }

        public AuthorType Author
        {
            get
            {
                return _author;
            }
        }

        public DateTime Created
        {
            get
            {
                return _created;
            }
        }

        Comment(int jobId, string body, AuthorType author, DateTime created)
        {
            _jobID = jobId;
            _body = body;
            _author = author;
            _created = created;
        }

        internal static Comment FromJObject(int jobID, JObject c)
        {
            var created = long.Parse(c.Value<string>("ctime"));

            return new Comment(
                jobID,
                c.Value<string>("body"),
                c.Value<string>("author").ToAuthorType(),
                created.ToDateFromTimestamp()
                );

        }
    }
}

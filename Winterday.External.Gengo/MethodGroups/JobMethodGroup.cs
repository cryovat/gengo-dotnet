//
// JobMethodGroup.cs
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

namespace Winterday.External.Gengo.MethodGroups
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Newtonsoft.Json.Linq;

    using Winterday.External.Gengo.Payloads;
    using Winterday.External.Gengo.Properties;

    public class JobMethodGroup
    {
        internal const string UriPartJob = "translate/job/";

        internal const string UriPartComment = "translate/job/{0}/comment";
        internal const string UriPartComments = "translate/job/{0}/comments";

        readonly IGengoClient _client;

        internal JobMethodGroup(IGengoClient client)
        {
            if (client == null)
                throw new ArgumentNullException("client");

            _client = client;
        }

        public async Task Approve(int jobId, Stars stars,
            string commentForTranslator,
            string commentForGengo,
            bool gengoCommentIsPublic)
        {
            throw new NotImplementedException();
        }

        public async Task<SubmittedJob> Get(int jobId,
            bool includeMachineTranslation)
        {
            var uri = UriPartJob + jobId;

            var data = new Dictionary<string, string>();
            data["pre_mt"] =
                Convert.ToInt32(includeMachineTranslation).ToString();

            var obj = await _client.GetJsonAsync<JObject>(uri, data, true);

            return new SubmittedJob(obj["job"] as JObject);
        }

        public async Task<Feedback> GetFeedback(int jobId)
        {
            var uri = UriPartJob + jobId;

            var obj = await _client.GetJsonAsync<JObject>(uri, true);

            return new Feedback(obj["feedback"] as JObject);
        }

        public async Task<byte[]> GetPreviewImage(int jobId)
        {
            throw new NotImplementedException();
        }

        public async Task Reject(int jobId, RejectionReason reason,
            string comment, string captcha, bool requeueJob)
        {
            throw new NotImplementedException();
        }

        public async Task ReturnForRevision(int jobId, string comment)
        {
            var uri = UriPartJob + jobId;

            if (string.IsNullOrWhiteSpace(comment))
                throw new ArgumentException(
                    Resources.RevisionCommentMandatory);

            var data = new JObject();
            data["action"] = "revise";
            data["comment"] = comment;

            await _client.PutJsonAsync<JObject>(uri, data);
        }

        public async Task Delete(int jobId)
        {
            var uri = UriPartJob + jobId;
            
            await _client.DeleteAsync<JObject>(uri);
        }

        public async Task<Comment[]> GetComments(int jobID)
        {
            var json = await _client.GetJsonAsync<JObject>(string.Format(UriPartComments, jobID), true);

            var thread = json.Value<JArray>("thread");

            return thread.Values<JObject>().Select(
                o => new Comment(o)).ToArray();
        }

        public async Task PostComment(int jobID, string body)
        {
            if (String.IsNullOrWhiteSpace(body)) throw new ArgumentException("Comment body not provided", "body");

            var json = new JObject(new JProperty("body", body));

            await _client.PostJsonAsync<JToken>(string.Format(UriPartComment, jobID), json);
        }

    }
}

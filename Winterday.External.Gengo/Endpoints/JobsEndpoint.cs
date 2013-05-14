//
// Jobs.cs
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


namespace Winterday.External.Gengo.Endpoints
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Newtonsoft.Json.Linq;

    using Winterday.External.Gengo.Payloads;
    using Winterday.External.Gengo.Properties;

    public class JobsEndpoint
    {
        internal const string UriPartJobsEndpoint = "translate/jobs";

        readonly GengoClient _client;

        internal JobsEndpoint(GengoClient client)
        {
            if (client == null)
                throw new ArgumentNullException("client");

            _client = client;
        }

        public Task<Confirmation> Submit(
            bool requireSameTranslator,
            bool allowTranslatorChange,
            params Job[] jobs)
        {
            if (jobs == null) throw new ArgumentNullException("jobs");

            return Submit(
                requireSameTranslator,
                allowTranslatorChange,
                (IEnumerable<Job>)jobs);
        }

        public async Task<Confirmation> Submit(
            bool requireSameTranslator,
            bool allowTranslatorChange,
            IEnumerable<Job> jobs)
        {
            if (jobs == null) throw new ArgumentNullException("jobs");

            var jobsObj = new JObject();

            var count = 1;

            foreach (var item in jobs)
            {
                jobsObj["job_" + count] = item.ToJObject();

                count += 1;
            }

            if (!jobsObj.HasValues)
                throw new ArgumentException(
                    Resources.AtLeastOneJobRequired);

            var payload = new JObject();
            payload["jobs"] = jobsObj;
            payload["as_group"] = Convert.ToInt32(requireSameTranslator);
            payload["allow_fork"] = Convert.ToInt32(allowTranslatorChange);

            var response =
                await _client.PostJsonAsync<JObject>(
                UriPartJobsEndpoint,
                payload);

            return new Confirmation(response);
        }
    }
}

//
// ServiceMethodGroup.cs
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

    /// <summary>
    /// Provides access to methods in the
    /// <a href="http://developers.gengo.com/v2/service/">Service</a>
    /// method group.
    /// </summary>
    public class ServiceMethodGroup
    {
        internal const string UriPartLanguages = "translate/service/languages";
        internal const string UriPartLanguagePairs = "translate/service/language_pairs";

        internal const string UriPartQuote = "translate/service/quote";
        internal const string UriPartQuoteFiles = "translate/service/quote/file";

        readonly IGengoClient _client;

        internal ServiceMethodGroup(IGengoClient client)
        {
            if (client == null)
                throw new ArgumentNullException("client");

            _client = client;
        }

        /// <summary>
        /// Gets the languages currently supported by Gengo
        /// </summary>
        /// <returns>Task yielding array of languages</returns>
        public async Task<Language[]> GetLanguages()
        {
            var json = await _client.GetJsonAsync<JArray>(UriPartLanguages, false);

            return json.Values<JObject>().Select(e => new Language(e)).ToArray();
        }

        /// <summary>
        /// Gets a list of source and target languages supported
        /// by Gengo
        /// </summary>
        /// <returns>Task yielding array of language pairs</returns>
        public async Task<LanguagePair[]> GetLanguagePairs()
        {
            var json = await _client.GetJsonAsync<JArray>(UriPartLanguagePairs, false);

            return json.Values<JObject>().Select(
                e => new LanguagePair(e)).ToArray();
        }

        /// <summary>
        /// Gets a price quote for a sequence of jobs
        /// </summary>
        /// <param name="requireSameTranslator">
        /// Wether the jobs may be split among several translators
        /// </param>
        /// <param name="jobs">Jobs to get quote for</param>
        /// <returns>Task yielding job quotes</returns>
        public Task<Quote[]> GetQuote(
            bool requireSameTranslator, params Job[] jobs)
        {
            if (jobs == null) throw new ArgumentNullException("jobs");

            return GetQuote(requireSameTranslator, (IEnumerable<Job>)jobs);
        }

        /// <summary>
        /// Gets a price quote for a sequence of jobs
        /// </summary>
        /// <param name="requireSameTranslator">
        /// Wether the jobs may be split among several translators
        /// </param>
        /// <param name="jobs">Jobs to get quote for</param>
        /// <returns>Task yielding job quotes</returns>
        public async Task<Quote[]> GetQuote(
                bool requireSameTranslator, IEnumerable<Job> jobs)
        {
            if (jobs == null) throw new ArgumentNullException("jobs");
            
            var payload = new JObject();

            payload["jobs"] = jobs.ToJsonJobsArray();
            payload["as_group"] = Convert.ToInt32(requireSameTranslator);

            var response = await _client.PostJsonAsync<JObject>(
                UriPartQuote, payload);

            var quotes = response.Value<JArray>("jobs");

            return quotes.Select(o => new Quote((JObject)o)).ToArray();
        }

        /// <summary>
        /// Gets a price quote for a sequence of jobs where the source
        /// text is contained in files
        /// </summary>
        /// <param name="jobs">File jobs to get quote for</param>
        /// <returns>Task yielding file job quotes</returns>
        public Task<FileQuote[]> GetQuoteForFiles(
            params FileJob[] jobs)
        {
            if (jobs == null)
                throw new ArgumentNullException("jobs");

            return GetQuoteForFiles((IEnumerable<FileJob>)jobs);
        }

        /// <summary>
        /// Gets a price quote for a sequence of jobs where the source
        /// text is contained in files
        /// </summary>
        /// <param name="jobs">File jobs to get quote for</param>
        /// <returns>Task yielding file job quotes</returns>
        public async Task<FileQuote[]> GetQuoteForFiles(
            IEnumerable<FileJob> jobs)
        {
            if (jobs == null)
                throw new ArgumentNullException("jobs");

            var payload = new JObject();

            payload["jobs"] = jobs.ToJsonJobsArray();

            var response = await _client.PostJsonAsync<JObject>(
                UriPartQuoteFiles, payload, jobs);

            var quotes = response.Value<JArray>("jobs");

            return quotes.Select(o => new FileQuote((JObject)o)).ToArray();
        }
    }
}

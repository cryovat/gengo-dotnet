//
// GlossaryMethodGroup.cs
//
// Author:
//       Jarl Erik Schmidt <github@jarlerik.com>
//
// Copyright (c) 2014 Jarl Erik Schmidt
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
    using System.Linq;
    using System.Threading.Tasks;

    using Newtonsoft.Json.Linq;

    using Winterday.External.Gengo.Payloads;

    /// <summary>
    /// Provides access to methods in the
    /// <a href="http://developers.gengo.com/v2/api_methods/glossary/">Glossary</a>
    /// method group.
    /// </summary>
    public class GlossaryMethodGroup
    {
        internal const string UriPartGlossary = "translate/glossary/";

        readonly IGengoClient _client;

        internal GlossaryMethodGroup(IGengoClient client)
        {
            if (client == null)
                throw new ArgumentNullException("client");

            _client = client;
        }

        /// <summary>
        /// Gets all glossaries belonging to the authenticated user
        /// </summary>
        /// <returns>Task yielding glossaries</returns>
        public async Task<Glossary[]> GetAll()
        {
            var arr = await _client.GetJsonAsync<JArray>(UriPartGlossary, true);

            return arr.Values<JObject>().Select(obj => new Glossary(obj)).ToArray();
        }

        /// <summary>
        /// Gets a specific glossary
        /// </summary>
        /// <param name="glossaryId">The glossary ID</param>
        /// <returns>Task yielding glossary</returns>
        public async Task<Glossary> Get(int glossaryId)
        {
            var uri = UriPartGlossary + glossaryId;

            var obj = await _client.GetJsonAsync<JObject>(uri, true);

            return new Glossary(obj);
        }
    }
}

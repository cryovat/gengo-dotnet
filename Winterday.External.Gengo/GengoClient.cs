//
// GengoClient.cs
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
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Net.Mime;
    using System.Text;
    using System.Threading.Tasks;

    using Newtonsoft.Json.Linq;

    using Winterday.External.Gengo.Endpoints;
    using Winterday.External.Gengo.Payloads;
    using Winterday.External.Gengo.Properties;

    public class GengoClient : IDisposable
    {
        internal const string ProductionBaseUri = "http://api.gengo.com/v2/";
        internal const string SandboxBaseUri = "http://api.sandbox.gengo.com/v2/";

        internal const string UriPartComment = "translate/job/{0}/comment";
        internal const string UriPartComments = "translate/job/{0}/comments";

        internal const string MimeTypeApplicationXml = "application/xml";
        internal const string MimeTypeApplicationJson = "application/json";

        readonly string _privateKey;
        readonly string _publicKey;

        readonly Uri _baseUri;
        readonly HttpClient _client = new HttpClient();

        AccountEndpoint _account;
        JobsEndpoint _jobs;
        ServiceEndpoint _service;

        bool _disposed;

        public bool IsDisposed
        {
            get
            {
                return _disposed;
            }
        }

        public AccountEndpoint Account
        {
            get { return _account; }
        }

        public JobsEndpoint Jobs
        {
            get { return _jobs; }
        }

        public ServiceEndpoint Service
        {
            get { return _service; }
        }

        public GengoClient(string privateKey, string publicKey)
        {
            if (string.IsNullOrWhiteSpace(privateKey))
                throw new ArgumentException("Private key not specified", "privateKey");

            if (string.IsNullOrWhiteSpace(publicKey))
                throw new ArgumentException("Public key not specified", "publicKey");

            _privateKey = privateKey;
            _publicKey = publicKey;

            _baseUri = new Uri(ProductionBaseUri);

            initClient();
        }

        public GengoClient(string privateKey, string publicKey, ClientMode mode)
        {
            if (string.IsNullOrWhiteSpace(privateKey))
                throw new ArgumentException("Private key not specified", "privateKey");

            if (string.IsNullOrWhiteSpace(publicKey))
                throw new ArgumentException("Public key not specified", "publicKey");

            _privateKey = privateKey;
            _publicKey = publicKey;

            var uri = mode == ClientMode.Production ? ProductionBaseUri : SandboxBaseUri;

            _baseUri = new Uri(uri);

            initClient();
        }

        public GengoClient(string privateKey, string publicKey, String baseUri)
        {
            if (string.IsNullOrWhiteSpace(privateKey))
                throw new ArgumentException("Private key not specified", "privateKey");

            if (string.IsNullOrWhiteSpace(publicKey))
                throw new ArgumentException("Public key not specified", "publicKey");

            if (string.IsNullOrWhiteSpace(baseUri))
                throw new ArgumentException("Base uri not specified", "baseUri");

            if (!Uri.IsWellFormedUriString(baseUri, UriKind.Absolute))
                throw new ArgumentException("Base uri is not an absolute uri", "baseUri");

            _privateKey = privateKey;
            _publicKey = publicKey;

            _baseUri = new Uri(baseUri);

            initClient();
        }

        private void initClient()
        {
            _account = new AccountEndpoint(this);
            _jobs = new JobsEndpoint(this);
            _service = new ServiceEndpoint(this);

            var assemblyName = GetType().Assembly.GetName();
            var headers = _client.DefaultRequestHeaders;

            headers.UserAgent.Add(new ProductInfoHeaderValue(assemblyName.Name, assemblyName.Version.ToString()));
            headers.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue(MimeTypeApplicationJson));
        }

        // TODO: Implement tests
        public async Task<Comment[]> GetComments(int jobID)
        {
            var xml = await GetJsonAsync<JObject>(string.Format(UriPartComments, jobID), true);
            throw new NotImplementedException();
            //return xml.Element("thread").Elements().Select(e => Comment.FromXContainer(jobID, e)).ToArray();
        }

        // TODO: Implement tests
        public async Task PostComment(int jobID, string body)
        {
            if (String.IsNullOrWhiteSpace(body)) throw new ArgumentException("Comment body not provided", "body");

            var json = new JObject(new JProperty("body", body));

            await PostJsonAsync<JToken>(string.Format(UriPartComment, jobID), json);
        }

        internal void AddAuthData(Dictionary<string, string> dict)
        {
            AddAuthData(dict, true);
        }

        internal void AddAuthData(Dictionary<string, string> dict, bool authenticated)
        {
            if (dict == null) throw new ArgumentNullException("dict");

            dict["api_key"] = _publicKey;

            if (authenticated)
            {
                var ts = DateTime.UtcNow.ToTimeStamp().ToString();
                var hash = _privateKey.SHA1Hash(ts);

                dict["ts"] = ts;
                dict["api_sig"] = hash;

            }
        }

        internal Uri BuildUri(String uriPart, bool authenticated)
        {
            return BuildUri(uriPart, null, authenticated);
        }

        internal Uri BuildUri(String uriPart, Dictionary<string, string> query, bool authenticated)
        {
            if (String.IsNullOrWhiteSpace("uriPart"))
                throw new ArgumentException("Uri part not provided", "baseUri");

            if (!Uri.IsWellFormedUriString(uriPart, UriKind.Relative))
                throw new ArgumentException("Uri part not valid relative uri", "baseUri");

            query = query ?? new Dictionary<string, string>();
            
            AddAuthData(query, authenticated);

            return new Uri(_baseUri, uriPart + query.ToQueryString());
        }

        internal async Task<JsonT> DeleteAsync<JsonT>(String uripart) where JsonT : JToken
        {
            var response = await  _client.DeleteAsync(BuildUri(uripart, true));
            var responseStr = await response.Content.ReadAsStringAsync();

            return UnpackJson<JsonT>(responseStr);
        }

        internal Task<string> GetStringAsync(String uriPart, bool authenticated)
        {
            return _client.GetStringAsync(BuildUri(uriPart, authenticated));
        }

        internal async Task<JsonT> GetJsonAsync<JsonT>(String uriPart, bool authenticated) where JsonT : JToken
        {
            var rawJson = await GetStringAsync(uriPart, authenticated);

            return UnpackJson<JsonT>(rawJson);
        }

        internal async Task<JsonT> PostFormAsync<JsonT>(String uriPart, Dictionary<string, string> values) where JsonT : JToken
        {
            if (values == null) throw new ArgumentNullException("values");

            var auth = new Dictionary<string, string>();

            AddAuthData(auth);

            var data = new FormUrlEncodedContent(values);

            var response = await _client.PostAsync(new Uri(_baseUri, uriPart), data);
            var responseStr = await response.Content.ReadAsStringAsync();

            return UnpackJson<JsonT>(responseStr);
        }

        internal async Task<JsonT> PostJsonAsync<JsonT>(String uriPart, JToken json) where JsonT : JToken
        {
            if (json == null) throw new ArgumentNullException("json");

            var auth = new Dictionary<string, string>();

            AddAuthData(auth);
            
            var multi = new MultipartFormDataContent();

            foreach (var pair  in auth)
            {
                multi.Add(new StringContent(pair.Value, Encoding.UTF8, MediaTypeNames.Text.Plain), pair.Key);
            }

            var data = new StringContent(json.ToString(), Encoding.UTF8, MimeTypeApplicationJson);

            multi.Add(data, "data");

            var response = await _client.PostAsync(new Uri(_baseUri, uriPart), multi);
            var responseStr = await response.Content.ReadAsStringAsync();

            return UnpackJson<JsonT>(responseStr);
        }

        internal JsonT UnpackJson<JsonT>(String rawJson) where JsonT : JToken
        {
            var json = JObject.Parse(rawJson);

            var opstat = json.Value<string>("opstat");
            var errObj = json.Value<JObject>("err");

            if (errObj == null) {
                return json["response"] as JsonT;
            } else 
            {
                string message = errObj.Value<string>("msg");;
                string code = errObj.Value<string>("code");;

                throw new GengoException(message, opstat, code);
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _client.Dispose();
                _disposed = true;
            }
        }
    }
}


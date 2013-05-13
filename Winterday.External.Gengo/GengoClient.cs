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
using System.Collections.Generic;


namespace Winterday.External.Gengo
{
	using System;
    using System.Linq;
	using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
	using System.Xml.Linq;

	using Winterday.External.Gengo.Payloads;

	public class GengoClient : IDisposable
	{
		internal const string ProductionBaseUri = "http://api.gengo.com/v2/";
		internal const string SandboxBaseUri = "http://api.sandbox.gengo.com/v2/";

		internal const string UriPartLanguages = "translate/service/languages";
		internal const string UriPartLanguagePairs = "translate/service/language_pairs";

		internal const string UriPartStats = "account/stats";
		internal const string UriPartBalance = "account/balance";

		internal const string MimeTypeApplicationXml = "application/xml";

		readonly string _privateKey;
		readonly string _publicKey;

		readonly Uri _baseUri;
        readonly HttpClient _client = new HttpClient();

        bool _disposed;

        public bool IsDisposed
        {
            get
            {
                return _disposed;
            }
        }

		public GengoClient (string privateKey, string publicKey)
		{
			if (string.IsNullOrWhiteSpace (privateKey))
				throw new ArgumentException ("Private key not specified", "privateKey");

			if (string.IsNullOrWhiteSpace (publicKey))
				throw new ArgumentException ("Public key not specified", "publicKey");

			_privateKey = privateKey;
			_publicKey = publicKey;

			_baseUri = new Uri(ProductionBaseUri);

            initClient();
		}

		public GengoClient (string privateKey, string publicKey, ClientMode mode)
		{
			if (string.IsNullOrWhiteSpace (privateKey))
				throw new ArgumentException ("Private key not specified", "privateKey");
			
			if (string.IsNullOrWhiteSpace (publicKey))
				throw new ArgumentException ("Public key not specified", "publicKey");
			
			_privateKey = privateKey;
			_publicKey = publicKey;
			
			var uri = mode == ClientMode.Production ? ProductionBaseUri : SandboxBaseUri;
			
			_baseUri = new Uri (uri);

            initClient();
		}

		public GengoClient (string privateKey, string publicKey, String baseUri)
		{
			if (string.IsNullOrWhiteSpace (privateKey))
				throw new ArgumentException ("Private key not specified", "privateKey");
			
			if (string.IsNullOrWhiteSpace (publicKey))
				throw new ArgumentException ("Public key not specified", "publicKey");

			if (string.IsNullOrWhiteSpace (baseUri))
				throw new ArgumentException ("Base uri not specified", "baseUri");
		
			if (!Uri.IsWellFormedUriString (baseUri, UriKind.Absolute))
				throw new ArgumentException ("Base uri is not an absolute uri", "baseUri");

			_privateKey = privateKey;
			_publicKey = publicKey;
			
			_baseUri = new Uri (baseUri);

            initClient();
		}

        private void initClient()
        {
            var assemblyName = GetType().Assembly.GetName();
            var headers = _client.DefaultRequestHeaders;

            headers.UserAgent.Add(new ProductInfoHeaderValue(assemblyName.Name, assemblyName.Version.ToString()));
            headers.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));
            headers.Accept.Add(new MediaTypeWithQualityHeaderValue(MimeTypeApplicationXml));
        }

		public async Task<Language[]> GetLanguages()
		{
			var xml = await GetXmlAsync (UriPartLanguages, false);
			
            return xml.Elements().Select(e => Language.FromXContainer(e)).ToArray();
		}

		public async Task<LanguagePair[]> GetLanguagePairs()
		{
			var xml = await GetXmlAsync (UriPartLanguagePairs, false);
			
            return xml.Elements().Select(e => LanguagePair.FromXContainer(e)).ToArray();
		}

		public async Task<AccountStats> GetStats() {

            var xml = await GetXmlAsync(UriPartStats, true);

			return AccountStats.FromXContainer (xml);
		}

		public async Task<decimal> GetBalance() {
			
			var xml = await GetXmlAsync (UriPartBalance, true);
			
			return xml.Element ("credits").Value.ToDecimal ();
		}

		internal Uri BuildUri(String uriPart, bool authenticated)
		{
			return BuildUri (uriPart, null, authenticated);
		}

        internal Uri BuildUri(String uriPart, Dictionary<string, string> query, bool authenticated)
        {
            if (String.IsNullOrWhiteSpace("uriPart"))
                throw new ArgumentException("Uri part not provided", "baseUri");

            if (!Uri.IsWellFormedUriString(uriPart, UriKind.Relative))
                throw new ArgumentException("Uri part not valid relative uri", "baseUri");

            query = query ?? new Dictionary<string, string>();
            query["api_key"] = _publicKey;

            if (authenticated)
            {
                var ts = DateTime.UtcNow.ToTimeStamp().ToString();
                var hash = _privateKey.SHA1Hash(ts);

                query["ts"] = ts;
                query["api_sig"] = hash;

            }
            
            return new Uri(_baseUri, uriPart + query.ToQueryString());
        }

        internal Task<string> GetStringAsync(String uriPart, bool authenticated)
        {
            return _client.GetStringAsync(BuildUri(uriPart, authenticated));
        }

        internal async Task<XElement> GetXmlAsync(String uriPart, bool authenticated)
        {
            var rawXml = await GetStringAsync(uriPart, authenticated);

            return UnpackXml(rawXml);
        }

        internal XElement UnpackXml(String rawXml)
        {
            var root = XDocument.Parse(rawXml).Root;
            var opstatElm = root.Element("opstat");

            if (opstatElm == null)
                throw new InvalidOperationException("Response XML did not contain opstat");

            var opstat = opstatElm.Value.ToLower();

            var errElm = root.Element("err");

            var responseElm = root.Element("response");

            if (opstat == "error" && errElm == null)
            {
                throw new GengoException(null, opstat, null);
            }
            if (opstat == "error" && errElm != null)
            {
                var msgElm = errElm.Element("msg");
                var codeElm = errElm.Element("code");

                string message = null;
                string code = null;

                if (msgElm != null)
                    message = msgElm.Value;

                if (codeElm != null)
                    code = codeElm.Value;

                throw new GengoException(message, opstat, code);
            }
            if (responseElm != null)
            {
                return responseElm;
            }

            return new XElement("response");
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


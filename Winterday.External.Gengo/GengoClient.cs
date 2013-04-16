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
	using System.Net;
	using System.Xml.Linq;

	using Winterday.External.Gengo.Payloads;

	public class GengoClient
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

		public GengoClient (string privateKey, string publicKey)
		{
			if (string.IsNullOrWhiteSpace (privateKey))
				throw new ArgumentException ("Private key not specified", "privateKey");

			if (string.IsNullOrWhiteSpace (publicKey))
				throw new ArgumentException ("Public key not specified", "publicKey");

			_privateKey = privateKey;
			_publicKey = publicKey;

			_baseUri = new Uri(ProductionBaseUri);
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
		}

		public IEnumerable<Language> GetLanguages()
		{
			var xml = GetXmlResponse (UriPartLanguages, HttpMethod.Get, null);
			
			foreach (var e in xml.Elements ()) {
				yield return Language.FromXContainer (e);
			}
		}

		public IEnumerable<LanguagePair> GetLanguagePairs()
		{
			var xml = GetXmlResponse (UriPartLanguagePairs, HttpMethod.Get, null);
			
			foreach (var e in xml.Elements ()) {
				yield return LanguagePair.FromXContainer(e);
			}
		}

		public AccountStats GetStats() {

			var xml = GetXmlResponse (UriPartStats, HttpMethod.Get, null, true);

			return AccountStats.FromXContainer (xml);
		}

		public decimal GetBalance() {
			
			var xml = GetXmlResponse (UriPartBalance, HttpMethod.Get, null, true);
			
			return xml.Element ("credits").Value.ToDecimal ();
		}

		internal Uri BuildUri(String uriPart)
		{
			return BuildUri (uriPart, null);
		}

		internal Uri BuildUri(String uriPart, Dictionary<string, string> query) {
			if (String.IsNullOrWhiteSpace ("uriPart"))
				throw new ArgumentException ("Uri part not provided", "baseUri");

			if (!Uri.IsWellFormedUriString (uriPart, UriKind.Relative))
				throw new ArgumentException ("Uri part not valid relative uri", "baseUri");

			return new Uri (_baseUri, uriPart + query.ToQueryString());
		}

		internal HttpWebRequest BuildRequest(String uriPart, HttpMethod method) {
			return BuildRequest (uriPart, method, null, false);
		}

		internal HttpWebRequest BuildRequest(String uriPart, HttpMethod method, Dictionary<string, string> query) {
			return BuildRequest (uriPart, method, query, false);
		}

		internal HttpWebRequest BuildRequest(String uriPart, HttpMethod method, Dictionary<string, string> query,
		                                     bool authenticated) {

			query = query ?? new Dictionary<string, string> ();
			query ["api_key"] = _publicKey;
			
			if (authenticated) {
				var ts = DateTime.UtcNow.ToTimeStamp ().ToString ();
				var hash = _privateKey.SHA1Hash (ts);

				query["ts"] = ts;
				query["api_sig"] = hash;

			}

			var requestUri = BuildUri (uriPart, query);
			var request = WebRequest.Create (requestUri) as HttpWebRequest;

			if (request == null)
				throw new InvalidOperationException ("Invalid uri scheme: " + requestUri.Scheme);

			request.UserAgent = typeof(GengoClient).Assembly.FullName;
			request.Method = method.ToMethodString ();
			request.Accept = MimeTypeApplicationXml;

			return request;
		}

		internal XElement GetXmlResponse(String uriPart, HttpMethod method, Dictionary<string, string> query) {
			return GetXmlResponse (uriPart, method, query, false);
		}

		internal XElement GetXmlResponse(String uriPart, HttpMethod method, Dictionary<string, string> query,
		                                 bool authenticated) {

			var request = BuildRequest (uriPart, method, query, authenticated);

			var response = request.GetResponse ();

			var root = XDocument.Load (response.GetResponseStream ()).Root;
			var opstatElm = root.Element ("opstat");

			if (opstatElm == null)
				throw new InvalidOperationException ("Response XML did not contain opstat");

			var opstat = opstatElm.Value.ToLower ();

			var errElm = root.Element ("err");

			var responseElm = root.Element ("response");

			if (opstat == "error" && errElm == null) {
				throw new GengoException (null, opstat, null);
			}
			if (opstat == "error" && errElm != null) {
				var msgElm = errElm.Element ("msg");
				var codeElm = errElm.Element ("code");

				string message = null;
				string code = null;

				if (msgElm != null)
					message = msgElm.Value;

				if (codeElm != null)
					code = codeElm.Value;

				throw new GengoException (message, opstat, code);
			}
			if (responseElm != null) {
				return responseElm;
			}

			return new XElement("response");
		}
	}
}


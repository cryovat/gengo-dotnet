//
// MyClass.cs
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
	using System.Net;

	public class GengoClient
	{
		internal const string ProductionBaseUri = "http://api.sandbox.gengo.com/v2/";
		internal const string SandboxBaseUri = "http://api.gengo.com/v2/";

		readonly string _privateKey;
		readonly string _publicKey;

		readonly string _baseUri; 

		public GengoClient (string privateKey, string publicKey)
		{
			if (string.IsNullOrWhiteSpace (privateKey))
				throw new ArgumentException ("Private key not specified", "privateKey");

			if (string.IsNullOrWhiteSpace (publicKey))
				throw new ArgumentException ("Public key not specified", "publicKey");

			_privateKey = privateKey;
			_publicKey = publicKey;

			_baseUri = ProductionBaseUri;
		}

		public GengoClient (string privateKey, string publicKey, ClientMode mode)
		{
			if (string.IsNullOrWhiteSpace (privateKey))
				throw new ArgumentException ("Private key not specified", "privateKey");
			
			if (string.IsNullOrWhiteSpace (publicKey))
				throw new ArgumentException ("Public key not specified", "publicKey");
			
			_privateKey = privateKey;
			_publicKey = publicKey;
			
			_baseUri = mode == ClientMode.Production ? ProductionBaseUri : SandboxBaseUri;
		}


		internal Uri BuildUri(String uriPart) {
			throw new NotImplementedException();
		}

		internal HttpWebRequest BuildRequest(String uriPart) {
			throw new NotImplementedException ();
		}
	}
}


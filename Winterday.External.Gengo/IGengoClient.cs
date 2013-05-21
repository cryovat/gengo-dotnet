//
// IGengoClient.cs
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
    using System.Threading.Tasks;

    using Newtonsoft.Json.Linq;

    internal interface IGengoClient
    {
        bool IsDisposed { get; }

        Task<JsonT> DeleteAsync<JsonT>(string uripart) where JsonT : JToken;

        Task<string> GetStringAsync(string uriPart, bool authenticated);

        Task<string> GetStringAsync(string uriPart, Dictionary<string, string> values, bool authenticated);

        Task<JsonT> GetJsonAsync<JsonT>(string uriPart, bool authenticated) where JsonT : JToken;

        Task<JsonT> GetJsonAsync<JsonT>(string uriPart, Dictionary<string, string> values, bool authenticated) where JsonT : JToken;

        Task<JsonT> PostFormAsync<JsonT>(String uriPart, Dictionary<string, string> values) where JsonT : JToken;

        Task<JsonT> PostJsonAsync<JsonT>(String uriPart, JToken json) where JsonT : JToken;

        Task<JsonT> PostJsonAsync<JsonT>(
            String uriPart, JToken json, IEnumerable<IPostableFile> files
            ) where JsonT : JToken;

        Task<JsonT> PutJsonAsync<JsonT>(String uriPart, JToken json) where JsonT : JToken;
    }
}

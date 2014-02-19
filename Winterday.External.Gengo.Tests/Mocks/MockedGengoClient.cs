//
// MockedGengoClient.cs
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


namespace Winterday.External.Gengo.Tests.Mocks
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Newtonsoft.Json.Linq;

    public class MockedGengoClient : IGengoClient
    {
       public IDictionary<string, string> Json { get; private set; }
        public IDictionary<string, byte[]> Images { get; private set; }

        public MockedGengoClient()
        {
            Json = new Dictionary<string, string>();
            Images = new Dictionary<string, byte[]>();
        }

        bool IGengoClient.IsDisposed
        {
            get { return false; }
        }

        Task<JsonT> IGengoClient.DeleteAsync<JsonT>(string uripart)
        {
            return Task.FromResult((JsonT)JToken.Parse(Json[uripart]));
        }

        Task<byte[]> IGengoClient.GetByteArrayAsync(string uriPart, bool authenticated)
        {
            return Task.FromResult(Images[uriPart]);
        }

        Task<string> IGengoClient.GetStringAsync(string uriPart, bool authenticated)
        {
            return Task.FromResult(Json[uriPart]);
        }

        Task<string> IGengoClient.GetStringAsync(string uriPart, Dictionary<string, string> values, bool authenticated)
        {
            return Task.FromResult(Json[uriPart]);
        }

        Task<JsonT> IGengoClient.GetJsonAsync<JsonT>(string uriPart, bool authenticated)
        {
            return Task.FromResult((JsonT)JToken.Parse(Json[uriPart]));
        }

        Task<JsonT> IGengoClient.GetJsonAsync<JsonT>(string uriPart, Dictionary<string, string> values, bool authenticated)
        {
            return Task.FromResult((JsonT)JToken.Parse(Json[uriPart]));
        }

        Task<JsonT> IGengoClient.PostFormAsync<JsonT>(string uriPart, Dictionary<string, string> values)
        {
            return Task.FromResult((JsonT)JToken.Parse(Json[uriPart]));
        }

        Task<JsonT> IGengoClient.PostJsonAsync<JsonT>(string uriPart, Newtonsoft.Json.Linq.JToken json)
        {
            return Task.FromResult((JsonT)JToken.Parse(Json[uriPart]));
        }

        Task<JsonT> IGengoClient.PostJsonAsync<JsonT>(string uriPart, Newtonsoft.Json.Linq.JToken json, IEnumerable<IPostableFile> files)
        {
            return Task.FromResult((JsonT)JToken.Parse(Json[uriPart]));
        }

        Task<JsonT> IGengoClient.PutJsonAsync<JsonT>(string uriPart, Newtonsoft.Json.Linq.JToken json)
        {
            return Task.FromResult((JsonT)JToken.Parse(Json[uriPart]));
        }
    }
}

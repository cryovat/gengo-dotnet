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
    }
}

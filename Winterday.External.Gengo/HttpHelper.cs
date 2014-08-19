using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Winterday.External.Gengo
{
    class HttpHelper
    {
        private static readonly int LARGE_DATA_LIMIT = 2000;

        public static HttpContent GetFormUrlEncodedContent(Dictionary<string, string> data)
        {
            var builder = new StringBuilder();
            foreach (var dataPart in data)
            {
                builder.Append(dataPart.Key + "=");
                builder.Append(EncodeLargeData(dataPart.Value));
                builder.Append("&");
            }

            builder.Remove(builder.Length - 1, 1);

            var content = new StringContent(builder.ToString(), Encoding.UTF8);
            content.Headers.ContentType = 
                new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            return content;
        }

        private static string EncodeLargeData(string data)
        {
            var builder = new StringBuilder();
            var loopCount = data.Length/LARGE_DATA_LIMIT;

            for (var i = 0; i <= loopCount; i++)
            {
                if (i < loopCount)
                {
                    builder.Append(Uri.EscapeDataString(data.Substring(LARGE_DATA_LIMIT*i, LARGE_DATA_LIMIT)));
                }
                else
                {
                    builder.Append(Uri.EscapeDataString(data.Substring(LARGE_DATA_LIMIT * i)));
                }
            }

            return builder.ToString();
        }
    }
}

//
// FileJob.cs
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

namespace Winterday.External.Gengo.Payloads
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Web;

    using Newtonsoft.Json.Linq;

    using Winterday.External.Gengo.Properties;

    public class FileJob : Job, IDisposable, IPostableFile
    {
        readonly Lazy<HttpContent> _content;

        bool disposed;

        public string FileKey { get; private set; }
        public string FileName { get; private set; }

        HttpContent IPostableFile.Content
        {
            get {
                if (disposed)
                {
                    throw new ObjectDisposedException("FileJob");
                }
                else
                {
                    return _content.Value;
                }            
            }
        }

        public override JobType JobType
        {
            get
            {
                return JobType.File;
            }
            set
            {
                throw new InvalidOperationException(
                    Resources.JobTypeMayNotBeChanged);
            }
        }

        public FileJob(string filePath, string fileKey = null)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException();

            FileKey = fileKey ?? Guid.NewGuid().ToString();
            FileName = Path.GetFileName(FileName);

            _content = LazyStream(FileName, () => File.OpenRead(filePath));
        }

        public FileJob(string filePath, Stream stream, string fileKey = null)
        {
            if (String.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException(
                    Resources.FileNameNotProvided,
                    "filePath");

            if (stream == null)
                throw new ArgumentNullException("stream");

            FileKey = fileKey ?? Guid.NewGuid().ToString();
            FileName = Path.GetFileName(filePath);

            _content = LazyStream(FileName, () => stream);
        }

        public FileJob(string fileName, byte[] rawData, string fileKey = null)
        {
            if (String.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException(
                    Resources.FileNameNotProvided,
                    "fileName");

            if (rawData == null)
                throw new ArgumentNullException("rawData");

            FileKey = fileKey ?? Guid.NewGuid().ToString();
            FileName = fileName;

            _content = LazyByteArray(FileName, () => rawData);
        }

        internal override JObject ToJObject()
        {
            var obj = base.ToJObject();
            obj["file_key"] = FileKey;
            return obj;
        }

        private static Lazy<HttpContent> LazyStream(String name, Func<Stream> f)
        {
            return new Lazy<HttpContent>(() => {
                var c = new StreamContent(f());
                c.Headers.ContentType =
                    new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(name));

                return c;
            });
        }

        private Lazy<HttpContent> LazyByteArray(String name, Func<byte[]> f)
        {
            return new Lazy<HttpContent>(() =>
            {
                var c = new ByteArrayContent(f());
                c.Headers.ContentType =
                    new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(name));

                return c;
            });
        }

        public void Dispose()
        {
            disposed = true;

            if (_content.IsValueCreated)
            {
                _content.Value.Dispose();
            }
        }
    }
}

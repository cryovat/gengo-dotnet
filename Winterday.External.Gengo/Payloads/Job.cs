//
// Job.cs
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

    using Newtonsoft.Json.Linq;

    using Winterday.External.Gengo;
    using Winterday.External.Gengo.Properties;

    /// <summary>
    /// Represents a submittable job.
    /// </summary>
    public class Job
    {
        bool _autoApprove;
        bool _force;
        bool _readOnly;

        string _body;
        string _comment;
        string _customData;
        string _fileId;
        string _slug;
        string _sourceLang;
        string _targetLang;

        Uri _callbackUrl;
        Uri _fileUrl;

        JobType _type;
        TranslationTier _tier;

        /// <summary>
        /// If the job should be automatically approved upon translation.
        /// Defaults to false
        /// </summary>
        public bool AutoApprove
        {
            get { return _autoApprove; }
            set
            {
                if (_readOnly)
                {
                    throw new InvalidOperationException(Resources.JobIsReadOnly);
                }
                else if (_autoApprove != value)
                {
                    _autoApprove = value;
                }
            }
        }

        /// <summary>
        /// If the job should skip duplicate checks. Defaults to false
        /// </summary>
        public bool Force
        {
            get { return _force; }
            set
            {
                if (_readOnly)
                {
                    throw new InvalidOperationException(Resources.JobIsReadOnly);
                }
                else if (_force != value)
                {
                    _force = value;
                }
            }
        }

        /// <summary>
        /// If the curent object is in a read-only state
        /// </summary>
        public bool IsReadOnly
        {
            get { return _readOnly; }
        }

        /// <summary>
        /// The untranslated text body of the job (mandatory for Text
        /// type job submissions)
        /// </summary>
        public string Body
        {
            get { return _body; }
            set
            {
                if (_readOnly)
                {
                    throw new InvalidOperationException(Resources.JobIsReadOnly);
                }
                else if (_body != value)
                {
                    _body = value;
                }
            }
        }

        /// <summary>
        /// Comment for translator (only used by during submissions)
        /// </summary>
        public string Comment
        {
            get { return _comment; }
            set
            {
                if (_readOnly)
                {
                    throw new InvalidOperationException(Resources.JobIsReadOnly);
                }
                else if (_comment != value)
                {
                    _comment = value;
                }
            }
        }

        /// <summary>
        /// Custom data associated with the submitted job
        /// </summary>
        public string CustomData
        {
            get { return _customData; }
            set
            {
                if (_readOnly)
                {
                    throw new InvalidOperationException(Resources.JobIsReadOnly);
                }
                else if (_customData != value)
                {
                    _customData = value;
                }
            }
        }

        /// <summary>
        /// ID of file previously submitted for quote that serves as source for the
        /// body text of the job
        /// </summary>
        public string FileId
        {
            get { return _fileId; }
            set
            {
                if (_readOnly)
                {
                    throw new InvalidOperationException(Resources.JobIsReadOnly);
                }
                else if (_fileId != value)
                {
                    _fileId = value;
                }
            }
        }

        /// <summary>
        /// The (mandatory) title of the job
        /// </summary>
        public string Slug
        {
            get { return _slug; }
            set
            {
                if (_readOnly)
                {
                    throw new InvalidOperationException(Resources.JobIsReadOnly);
                }
                else if (_slug != value)
                {
                    _slug = value;
                }
            }
        }

        /// <summary>
        /// Code for source language
        /// </summary>
        public string SourceLanguage
        {
            get { return _sourceLang; }
            set
            {
                if (_readOnly)
                {
                    throw new InvalidOperationException(Resources.JobIsReadOnly);
                }
                else if (_sourceLang != value)
                {
                    _sourceLang = value;
                }
            }
        }

        /// <summary>
        /// Code for target language
        /// </summary>
        public string TargetLanguage
        {
            get { return _targetLang; }
            set
            {
                if (_readOnly)
                {
                    throw new InvalidOperationException(Resources.JobIsReadOnly);
                }
                else if (_targetLang != value)
                {
                    _targetLang = value;
                }
            }
        }

        /// <summary>
        /// Callback url that Gengo will use to notify about status changes
        /// </summary>
        public Uri CallbackUrl
        {
            get { return _callbackUrl; }
            set
            {
                if (_readOnly)
                {
                    throw new InvalidOperationException(Resources.JobIsReadOnly);
                }
                else if (_callbackUrl != value)
                {
                    if (_callbackUrl != null && !_callbackUrl.IsAbsoluteUri)
                    {
                        throw new ArgumentException(Resources.CallbackUrlMustBeAbsolute, "value");
                    }

                    _callbackUrl = value;
                }
            }
        }

        /// <summary>
        /// The job type
        /// </summary>
        public virtual JobType JobType
        {
            get { return _type; }
            set
            {
                if (_readOnly)
                {
                    throw new InvalidOperationException(Resources.JobIsReadOnly);
                }
                else if (_type != value)
                {
                    _type = value;
                }
            }
        }

        /// <summary>
        /// The pricing tier of the job
        /// </summary>
        public TranslationTier TranslationTier
        {
            get { return _tier; }
            set
            {
                if (_readOnly)
                {
                    throw new InvalidOperationException(Resources.JobIsReadOnly);
                }
                else if (_tier != value)
                {
                    _tier = value;
                }
            }
        }

        public Uri FileUrl { get { return _fileUrl; }}

        public Job()
        {
            _readOnly = false;
        }

        protected Job(bool readOnly)
        {
            _readOnly = readOnly;
        }

        protected Job(JObject json)
        {
            if (json == null) throw new ArgumentNullException("json");

            _readOnly = true;

            _autoApprove = json.Value<string>("auto_approve") == "1";
            _force = json.Value<string>("force") == "1";

            _body = json.Value<string>("body_src");
            _comment = json.Value<string>("comment");
            _customData = json.Value<string>("custom_data");

            _slug = json.Value<string>("slug");
            _sourceLang = json.Value<string>("lc_src");
            _targetLang = json.Value<string>("lc_tgt");

            _type = json.Value<string>("type").ToJobType();
            _tier = json.Value<string>("tier").ToTranslationTier();

            var callback = json.Value<string>("callback_url");

            Uri.TryCreate(callback, UriKind.Absolute, out _callbackUrl);

            var fileUrl = json.Value<string>("tgt_file_link");
            Uri.TryCreate(fileUrl, UriKind.Absolute, out _fileUrl);
        }

        internal virtual JObject ToJObject()
        {
            if (_tier == TranslationTier.Unknown)
                throw new InvalidOperationException(Resources.InvalidTier);

            if (_type == JobType.File && string.IsNullOrWhiteSpace(_fileId))
                throw new InvalidOperationException(Resources.FileButIdNotSpecified);

            if (_type == JobType.Text && !string.IsNullOrWhiteSpace(_fileId))
                throw new InvalidOperationException(Resources.TextButIdSpecified);

            if (JobType == Gengo.JobType.Text && String.IsNullOrWhiteSpace(_body))
                throw new InvalidOperationException(Resources.MissingBody);

            if (String.IsNullOrWhiteSpace(_slug))
                throw new InvalidOperationException(Resources.MissingSlug);

            if (String.IsNullOrWhiteSpace(_sourceLang))
                throw new InvalidOperationException(Resources.MissingSourceLang);

            if (String.IsNullOrWhiteSpace(_targetLang))
                throw new InvalidOperationException(Resources.MissingTargetLang);

            var obj = new JObject();

            obj["auto_approve"] = Convert.ToInt32(_autoApprove);
            obj["force"] = Convert.ToInt32(_force);

            obj["slug"] = _slug;
            obj["body_src"] = _body;

            if (_comment != null)
                obj["comment"] = _comment;

            if (_customData != null)
                obj["custom_data"] = _customData;
            
            if (_fileId != null)
                obj["identifier"] = _fileId;

            obj["lc_src"] = _sourceLang;
            obj["lc_tgt"] = _targetLang;

            obj["type"] = JobType.ToTypeString();
            obj["tier"] = _tier.ToTierString();

            if (_callbackUrl != null)
            {
                obj["callback_url"] = _callbackUrl.ToString();
            }

            return obj;
        }
    }
}

//
// Enumerations.cs
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
    /// <summary>
    /// The author of a comment
    /// </summary>
    public enum AuthorType
    {
        /// <summary>
        /// This value is the generic "undefined" one and should never be
        /// returned by the service.
        /// </summary>
        Unknown,
        Translator,
        SeniorTranslator,
        Customer
    }

    /// <summary>
    /// The type of job submission
    /// </summary>
    public enum JobType
    {
        /// <summary>
        /// The text to be translated is/was submitted along with the job
        /// description
        /// </summary>
        Text,
        /// <summary>
        /// The text to be translated is/was submitted for a quote before
        /// actual submission
        /// </summary>
        File
    }

    /// <summary>
    /// The mode for the Gengo API client
    /// </summary>
    public enum ClientMode
    {
        /// <summary>
        /// The client invokes API methods in the (for-money)
        /// production service
        /// </summary>
        Production,
        /// <summary>
        /// The client invokes API methods in the (free testing) Sandbox
        /// </summary>
        Sandbox
    }

    /// <summary>
    /// Reason for rejecting a translation
    /// </summary>
    public enum RejectionReason
    {
        Quality,
        Incomplete,
        Other
    }

    /// <summary>
    /// Rating for a reviewable translation. Increasing scale
    /// </summary>
    public enum Stars : int
    {
        Unspecified = 0,
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5
    }

    /// <summary>
    /// The translation tier for a job. Decides price/time
    /// </summary>
    public enum TranslationTier
    {
        Standard,
        Machine,
        Pro,
        Ultra,
        Unknown
    }

    /// <summary>
    /// The status of a given job. See the API documentation for
    /// <a href="http://developers.gengo.com/overview/#job-statuses">
    /// status meanings</a>
    /// </summary>
    public enum TranslationStatus
    {
        Available,
        Pending,
        Reviewable,
        Revising,
        Approved,
        Cancelled,
        Unknown
    }
}


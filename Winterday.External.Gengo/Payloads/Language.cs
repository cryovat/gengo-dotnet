//
// Language.cs
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
    using System.Xml.Linq;

    public class Language
    {
        readonly string _name;
        readonly string _localizedName;
        readonly string _code;
        readonly string _unitType;

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public string LocalizedName
        {
            get
            {
                return _localizedName;
            }
        }

        public string Code
        {
            get
            {
                return _code;
            }
        }

        public string UnitType
        {
            get
            {
                return _unitType;
            }
        }

        public Language(string name, string localizedName, string code, string unitType)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Language name not provided", "name");

            if (string.IsNullOrWhiteSpace(localizedName))
                throw new ArgumentException("Localized language name not provided", "localizedName");

            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException("Language code not provided", "code");
            }

            if (string.IsNullOrWhiteSpace(unitType))
            {
                throw new ArgumentException("Language unit type not provided", "unitType");
            }

            _name = name;
            _localizedName = localizedName;
            _code = code;
            _unitType = unitType;
        }

        internal static Language FromXContainer(XContainer c)
        {

            return new Language(
                c.Element("language").Value,
                c.Element("localized_name").Value,
                c.Element("lc").Value,
                c.Element("unit_type").Value
                );

        }
    }
}


using System;
using System.Linq;

namespace SQLFormatter.Base
{
    public class Clause
    {

        private static string _WHITE_SPACES_ = new string(' ', 2);

        public Token _token { get; set; }
        public int _indent { get; set; }
        public string[] _content { get; set; }

        public Clause(Token _token, int _indent, string[] _content)
        {
            this._token = _token ?? throw new ArgumentNullException(nameof(_token));
            this._indent = _indent;
            this._content = _content;
        }

        public string Format()
        {
            string s = "";

            s = s + "\n"
                  + string.Concat(Enumerable.Repeat(_WHITE_SPACES_, _indent))
                  + _token._title
                  + (_token._newline ? "\n" : " ");

            bool first = true;
            foreach (string el in _content)
            {
                if ("SELECT".Equals(_token._title, StringComparison.CurrentCultureIgnoreCase))
                {
                    // need to add/display comma before elements
                    if (!",".Equals(el))
                    {
                        s = s + string.Concat(Enumerable.Repeat(_WHITE_SPACES_, _indent + 1))
                              + (first ? "" : ", ") + el + "\n";
                    }
                }
                else if ("AND".Equals(el) || "OR".Equals(el))
                {
                    // change line before logic operator
                    s = s + "\n" + string.Concat(Enumerable.Repeat(_WHITE_SPACES_, _indent + 1))
                          + el + " ";
                }
                else
                {
                    s = s + (first && _token._newline ? string.Concat(Enumerable.Repeat(_WHITE_SPACES_, _indent + 1)) : "")
                          + el + " ";
                }
                first = false;
            }

            s = s.Substring(0, s.Length - 1);

            return s;
        }

    }
}

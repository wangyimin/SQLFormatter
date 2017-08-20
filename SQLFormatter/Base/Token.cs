using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLFormatter.Base
{
    public class Token
    {
        public string _title { get; }
        public bool _newline { get; }

        public Token(string _title, bool _newline)
        {
            this._title = _title ?? throw new ArgumentNullException(nameof(_title));
            this._newline = _newline;
        }
    }
}

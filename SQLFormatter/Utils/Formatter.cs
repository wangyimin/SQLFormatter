using SQLFormatter.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace SQLFormatter.Utils
{
    class Formatter
    {
        private List<Token> _tokens = new List<Token> {
            new Token("SELECT", true),
            new Token("FROM", true),
            new Token("WHERE", true),
            new Token("(", true),
            new Token(")", false)
        };
   
        public void Parse(string s, int indent, Clause clause, List<Clause> lst)
        {
            if (string.IsNullOrEmpty(s) || lst == null)
            {
                throw new ArgumentNullException("Null parameter.");
            }

            int i;

            //Trace.WriteLine(s);
            string[] parts = Regex.Split(s, 
                    @"((?<=[,\\(\\)\\+\\*\/\\=\\'\s-])|(?=[,\\(\\)\\+\\*\/\\=\\'\s-]))")
                .Where(el => el.Trim() != String.Empty).ToArray();

            for (i = 0; i < parts.Length; i++)
            {
                string part = parts[i];
                if (IsToken(part))
                {
                    if (clause != null)
                    {
                        // store value to previous clause
                        Array.Copy(parts, clause._content = new string[i], i);
                    }

                    if ("(".Equals(part))
                    {
                        // indent on
                        Clause c = new Clause(Get(part), indent + 1, null);
                        lst.Add(c);

                        int pos = GetMatchParenthesisPostion(parts, i+1);

                        // process the part included by parenthesis firstly
                        Parse(Join(parts, i + 1, pos - 1), indent + 1,  c, lst);
                        
                        // process the other parts excluded by parenthesis secondly
                        Parse(Join(parts, pos, parts.Length - 1), indent + 1, null, lst);
                        
                        return;
                    }
                    else if (")".Equals(part))
                    {
                        // indend off
                        Clause c = new Clause(Get(part), indent, null);
                        lst.Add(c);
                        if (parts.Length == 1)
                        {
                            // the last part
                            c._content = new string[0];
                            return;
                        }
                        Parse(Join(parts, i + 1, parts.Length - 1), indent - 1, c, lst);

                        return;
                    }
                    else
                    {
                        // other
                        Clause c = new Clause(Get(part), indent, null);
                        lst.Add(c);
                        Parse(Join(parts, i + 1, parts.Length - 1), indent, c, lst);

                        return;
                    }
                }
            }

            if (i == parts.Length)
            {
                Array.Copy(parts, clause._content = new string[parts.Length], parts.Length);
            }
        }
        
        public bool IsToken(string s)
        {
            return _tokens.Exists(token => token._title.Equals(s, StringComparison.CurrentCultureIgnoreCase));
        }

        public Token Get(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                throw new ArgumentNullException(nameof(s));
            }

            return _tokens.Find(token => token._title.Equals(s, StringComparison.CurrentCultureIgnoreCase));
        }

        public string Join(string[] s, int n, int m)
        {   
            
            if (s == null || m < n || n < 0 || m < 0)
            {
                throw new InvalidOperationException("Wrong parameter.");
            }
            return string.Join(" ",s.Skip(n).Take(m - n + 1).ToArray());           
        }

        public int GetMatchParenthesisPostion(string[] s, int n)
        {
            Stack<string> stack = new Stack<string>();
            stack.Push("(");

            int r = n;

            foreach(string el in s.Skip(n).ToArray())
            {
                if (")".Equals(el))
                {
                    stack.Pop();
                    if (stack.Count == 0)
                    {
                        return r;
                    }
                }
                else if ("(".Equals(el))
                {
                    stack.Push("(");
                }
                else
                {
                    //none
                }
                r++;
            }

            throw new InvalidOperationException("The number of left/right parenthesis is unmatched.");
        }
    }
}

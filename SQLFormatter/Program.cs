using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Diagnostics;
using SQLFormatter.Base;
using SQLFormatter.Utils;

namespace SQLFormatter
{
    class Program
    {
        static void Main(string[] args)
        {

            string s = "SELECT a, b, c FROM (SELECT * FROM (SELECT * FROM t1)t2) V WHERE a = 19 AND (b=20 OR c=1)";

            Formatter f = new Formatter();
            List<Clause> lst = new List<Clause>();
            f.Parse(s, 0, null, lst);

            string sql = "";
            foreach (Clause el in lst)
            {
                sql = sql + el.Format();
            }
            Trace.WriteLine(sql);
        }
    }
}

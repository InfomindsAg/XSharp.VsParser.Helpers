using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XSharp.Parser.Helpers.Listeners
{
    public struct ParserContext
    {
        public string MethodName { get; set; }
        public string ClassName { get; set; }
        public string InheritsClassName { get; set; }

        public void Clear()
        {
            MethodName = null;
            ClassName = null;
            InheritsClassName = null;
        }
    }
}

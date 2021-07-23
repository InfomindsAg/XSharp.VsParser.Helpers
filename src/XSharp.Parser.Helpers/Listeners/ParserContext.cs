using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XSharp.Parser.Helpers.Listeners
{
    public class ParserContext
    {
        public class ClassContext
        {
            public string Name { get; internal set; }
            public string Inherits { get; internal set; }
        }

        public class MethodContext
        {
            public string Name { get; internal set; }
        }


        public ClassContext Class { get; internal set; }
        public MethodContext Method { get; internal set; }
        
        public void Clear()
        {
            Class = null;
            Method = null;
        }
    }
}

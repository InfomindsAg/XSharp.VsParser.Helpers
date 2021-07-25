using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.VsParser.Helpers.Parser
{
    public class Result
    {
        public class Item
        {
            public string Message { get; set; }
            public int Line { get; set; }

            public override string ToString()
                => $"{Message} - Line: {Line}";
        }

        public List<Item> Errors { get; } = new();
        public List<Item> Warnings { get; } = new();

        public bool OK => Errors.Count == 0;

        public void Clear()
        {
            Errors.Clear();
            Warnings.Clear();
        }
    }
}

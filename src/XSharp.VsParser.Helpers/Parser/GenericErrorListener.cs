using LanguageService.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;

namespace XSharp.VsParser.Helpers.Parser
{

    class GenericErrorListener : XSharp.Parser.VsParser.IErrorListener
    {
        public Result Result = new();

        public void ReportError(string fileName, LinePositionSpan span, string errorCode, string message, object[] args)
            => Result.Errors.Add(new Result.Item { Message = $"{errorCode} - {message}", Line = span.Line });

        public void ReportWarning(string fileName, LinePositionSpan span, string errorCode, string message, object[] args)
            => Result.Warnings.Add(new Result.Item { Message = $"{errorCode} - {message}", Line = span.Line });

        public void Clear()
            => Result = new();
    }
}

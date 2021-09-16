using LanguageService.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;

namespace XSharp.VsParser.Helpers.Parser
{

    class GenericErrorListener : XSharp.Parser.VsParser.IErrorListener
    {
        string BuildMessage(string message, object[] args)
        {
            if (args?.Length > 0)
                return string.Join(", ", args);
            return message;
        }

        public Result Result = new();

        public void ReportError(string fileName, LinePositionSpan span, string errorCode, string message, object[] args)
            => Result.Errors.Add(new Result.Item { Message = $"{errorCode} - {BuildMessage(message, args)}", Line = span.Line, Position = span.Column });

        public void ReportWarning(string fileName, LinePositionSpan span, string errorCode, string message, object[] args)
            => Result.Warnings.Add(new Result.Item { Message = $"{errorCode} - {BuildMessage(message, args)}", Line = span.Line, Position = span.Column });

        public void Clear()
            => Result = new();
    }
}

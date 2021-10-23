# XSharp.Parser.Helpers

## Introduction

XSharp.VsParser.Helpers contains some helper classes, that simplify the parsing and rewriting of XSharp code files. It uses the VsParser.Dll of the XSharp project.

## Requirements

In order to use this Nuget, you must install a version of the XSharp Compiler and accept the terms and conditions of the XSharp project.

## Classes

### ProjectHelper

* Get the Options for the parser initialisiation
* Get a list of all the files in the project

### ParserHelper

* Parses a xsharp file and exposes it as tree
* Tree
  * Can be enumerated
  * Exposes a Rewriter to change the source code
  * Can dump the tree as Xml or Yaml
* Comments

### CacheHelper

* Caches serializable data objects for source files
* Is helpful to store calculated data to reduce the amount of files to parse

### FileExtensionHelper

* Detects the most likely encoding of a file

### Extension Methods

A series of IParseTree extension methods like

* WhereType or WhereTypeWithChildren to filter the tree
* FirstParentOrDefault to get a specific parent matching the type
* ToValues Methods for Class, Method, ... to give easy access to important values

Check the IParseTreeExtensions, ToIndexExtensions and ToValueExtensions files in the Parser folder for the complete list.

### RewriteFor extension methods

A series of helper methods for code rewriting. Check the Rewriter folder for the complete list.

# Examples

This is a simple example on how to use the ParserHelper to parse source code and extract information from the tree.

```csharp
string sampleSourceCode = @"
// Example Class
class Example inherit BaseExample
    
    // Dummy Method
    method Dummy(value)
        System.Console.WriteLine(value)
        return
end class
";

// For this example, we initialize the parser with the default options. Normally, the options should be loaded usind the ProjectHelper
var parser = ParserHelper.BuildWithVoDefaultOptions();

// To get access to the tree, we have to parse the source code first
var result = parser.ParseText(sampleSourceCode, "dummy.prg");
if (!result.OK)
    throw new Exception("File can not be parsed");

// Now we can access the tree. The first step is normaly to dump the tree to yaml to get a better understanding of the IParseTree items, that were created based on our source code
var dump = parser.Tree.DumpYaml();
Console.WriteLine(dump);

// in the dump we noticed, that the parser creates a class_context for every class. We can use that to find the first class_context.
var classContext = parser.Tree.FirstOrDefaultType<Class_Context>();

// from the class context we can extract the class name using the ToValue extension method
var className = classContext?.ToValues().Name;
Console.WriteLine(className);

// to get all the child nodes of the classContext, whe use AsEnumerable 
var classChilds = classContext.AsEnumerable();

// now we want to count methods of the class. we use whereType and Count
var methodsCount = classChilds.WhereType<MethodContext>().Count();
Console.WriteLine(methodsCount);

```


Check the XSharp.VsParser.Helpers.Tests project for additional samples on how to use the parser helper

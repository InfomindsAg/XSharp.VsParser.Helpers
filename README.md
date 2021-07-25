# XSharp.Parser.Helpers

## Introduction

XSharp.VsParser.Helpers contains some helper classes, that simplify the parsing and rewriting of XSharp code files.

## Requirements

In order to use this Nuget, you must install a version of the XSharp Compiler and accept the terms and conditions of the XSharp project.

## Classes

### ProjectHelper

* Get the Options for the parser initialisiation
* Get a list of all the files in the project

### ParserHelper

* Parses a x# file and exposes is SyntaxTree
* SyntaxTree 
  * Can be enumerated
  * Exposes a Rewriter to change sourcecode

### Extension Methods

* WhereType Filter for SyntaxTree
* FirstParentOrDefault to get a specific parent matching the type
* ToValues Methods for Class, Method, ... to give easy access to important values
* Various Replace... Methods to simplify source rewriting

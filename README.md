# XSharp.Parser.Helpers

## Introduction

XSharp.Parser.Helpers contains some helper classes, that simplify the parsing of XSharp code files.

## Requirements

In order to use this Nuget, you must install a version of the XSharp Compiler.

## Classes

### ProjectHelper

* Get the Options for the parser initialisiation
* Get a list of all the files in the project

### ParserHelper

* Parses a x# file
* Executes XSharpBaseListeners on the parsed source
* Executes XSharpBaseRewriters on the parsed source

## ExtendedXSharpBaseListener

* Extends the XSharpBaseListener with a Current property, that gives easy access to the current classname, methodname, ...

## XSharpBaseRewriter

* Adds a series of methods to simplify source code rewriting
﻿using System;
using System.Collections.Generic;
using System.Linq;
using XSharp.VsParser.Helpers.Parser.Values;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;

namespace XSharp.VsParser.Helpers.Parser
{
    /// <summary>
    /// ToValue Extensions
    /// </summary>
    public static partial class ToValuesExtensions
    {

        #region ClassContext

        /// <summary>
        /// Converts a Class_Context instance to a ClassContextValues instance
        /// </summary>
        /// <param name="context">A Class_Context instance</param>
        /// <returns>A ClassContextValues instance</returns>
        public static ClassContextValues ToValues(this Class_Context context) => ClassContextValues.Build(context);

        /// <summary>
        /// Converts a sequence of Class_Context instances to a sequence of ClassContextValues instances
        /// </summary>
        /// <param name="enumerable">A sequence of Class_Context instances</param>
        /// <returns>A sequence of ClassContextValues instances</returns>
        public static IEnumerable<ClassContextValues> ToValues(this IEnumerable<Class_Context> enumerable) => enumerable.Select(ClassContextValues.Build);

        #endregion

        #region MethodContext

        /// <summary>
        /// Converts a MethodContext instance to a MethodContextValues instance
        /// </summary>
        /// <param name="context">A MethodContext instance</param>
        /// <returns>A MethodContextValues instance</returns>
        public static MethodContextValues ToValues(this MethodContext context) => MethodContextValues.Build(context);

        /// <summary>
        /// Converts a sequence of MethodContext instances to a sequence of MethodContextValues instances
        /// </summary>
        /// <param name="enumerable">A sequence of MethodContext instances</param>
        /// <returns>A sequence of MethodContextValues instances</returns>
        public static IEnumerable<MethodContextValues> ToValues(this IEnumerable<MethodContext> enumerable) => enumerable.Select(MethodContextValues.Build);

        #endregion

        #region SignatureContext

        /// <summary>
        /// Converts a SignatureContext instance to a SignatureContextValues instance
        /// </summary>
        /// <param name="context">A SignatureContext instance</param>
        /// <returns>A SignatureContextValues instance</returns>
        public static SignatureContextValues ToValues(this SignatureContext context) => SignatureContextValues.Build(context);

        /// <summary>
        /// Converts a sequence of SignatureContext instances to a sequence of SignatureContextValues instances
        /// </summary>
        /// <param name="enumerable">A sequence of SignatureContext instances</param>
        /// <returns>A sequence of SignatureContextValues instances</returns>
        public static IEnumerable<SignatureContextValues> ToValues(this IEnumerable<SignatureContext> enumerable) => enumerable.Select(SignatureContextValues.Build);

        #endregion

        #region ConstructorContext

        /// <summary>
        /// Converts a ConstructorContext instance to a ConstructorContextValues instance
        /// </summary>
        /// <param name="context">A ConstructorContext instance</param>
        /// <returns>A ConstructorContextValues instance</returns>
        public static ConstructorContextValues ToValues(this ConstructorContext context) => ConstructorContextValues.Build(context);

        /// <summary>
        /// Converts a sequence of ConstructorContext instances to a sequence of ConstructorContextValues instances
        /// </summary>
        /// <param name="enumerable">A sequence of ConstructorContext instances</param>
        /// <returns>A sequence of ConstructorContextValues instances</returns>
        public static IEnumerable<ConstructorContextValues> ToValues(this IEnumerable<ConstructorContext> enumerable) => enumerable.Select(ConstructorContextValues.Build);

        #endregion

        #region ParameterContext

        /// <summary>
        /// Converts a ParameterContext instance to a ParameterContextValues instance
        /// </summary>
        /// <param name="context">A ParameterContext instance</param>
        /// <returns>A ParameterContextValues instance</returns>
        public static ParameterContextValues ToValues(this ParameterContext context) => ParameterContextValues.Build(context);

        /// <summary>
        /// Converts a sequence of ParameterContext instances to a sequence of ParameterContextValues instances
        /// </summary>
        /// <param name="enumerable">A sequence of ParameterContext instances</param>
        /// <returns>A sequence of ParameterContextValues instances</returns>
        public static IEnumerable<ParameterContextValues> ToValues(this IEnumerable<ParameterContext> enumerable) => enumerable.Select(ParameterContextValues.Build);

        #endregion

        #region SuperExpressionContext

        /// <summary>
        /// Converts a SuperExpressionContext instance to a SuperExpressionContextValues instance
        /// </summary>
        /// <param name="context">A SuperExpressionContext instance</param>
        /// <returns>A SuperExpressionContextValues instance</returns>
        public static SuperExpressionContextValues ToValues(this SuperExpressionContext context) => SuperExpressionContextValues.Build(context);

        /// <summary>
        /// Converts a sequence of SuperExpressionContext instances to a sequence of SuperExpressionContextValues instances
        /// </summary>
        /// <param name="enumerable">A sequence of SuperExpressionContext instances</param>
        /// <returns>A sequence of SuperExpressionContextValues instances</returns>
        public static IEnumerable<SuperExpressionContextValues> ToValues(this IEnumerable<SuperExpressionContext> enumerable) => enumerable.Select(SuperExpressionContextValues.Build);

        #endregion

        #region ReturnStmt

        /// <summary>
        /// Converts a ReturnStmtContext instance to a ReturnStmtContextValues instance
        /// </summary>
        /// <param name="context">A ReturnStmtContext instance</param>
        /// <returns>A ReturnStmtContextValues instance</returns>
        public static ReturnStmtContextValues ToValues(this ReturnStmtContext context) => ReturnStmtContextValues.Build(context);

        /// <summary>
        /// Converts a sequence of ReturnStmtContext instances to a sequence of ReturnStmtContextValues instances
        /// </summary>
        /// <param name="enumerable">A sequence of ReturnStmtContext instances</param>
        /// <returns>A sequence of ReturnStmtContextValues instances</returns>
        public static IEnumerable<ReturnStmtContextValues> ToValues(this IEnumerable<ReturnStmtContext> enumerable) => enumerable.Select(ReturnStmtContextValues.Build);

        #endregion

        #region MethodContext

        /// <summary>
        /// Converts a FuncprocContext instance to a FuncprocContextValues instance
        /// </summary>
        /// <param name="context">A FuncprocContext instance</param>
        /// <returns>A FuncprocContextValues instance</returns>
        public static FuncprocContextValues ToValues(this FuncprocContext context) => FuncprocContextValues.Build(context);

        /// <summary>
        /// Converts a sequence of FuncprocContext instances to a sequence of FuncprocContextValues instances
        /// </summary>
        /// <param name="enumerable">A sequence of FuncprocContext instances</param>
        /// <returns>A sequence of FuncprocContextValues instances</returns>
        public static IEnumerable<FuncprocContextValues> ToValues(this IEnumerable<FuncprocContext> enumerable) => enumerable.Select(FuncprocContextValues.Build);

        #endregion


        
        // Todo: Add Conversions for Properties, Access/Assign, ...


    }


}

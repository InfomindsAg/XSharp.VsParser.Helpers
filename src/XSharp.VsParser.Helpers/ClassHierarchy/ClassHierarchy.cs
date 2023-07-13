using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using XSharp.VsParser.Helpers.Cache;
using XSharp.VsParser.Helpers.Parser;
using XSharp.VsParser.Helpers.Project;
using XSharp.VsParser.Helpers.Utilities;
using XSharp.VsParser.Helpers.Extensions;
using static LanguageService.CodeAnalysis.XSharp.SyntaxParser.XSharpParser;
using LiteDB;

namespace XSharp.VsParser.Helpers.ClassHierarchy
{
    /// <summary>
    /// Delegate for a preprocessing function
    /// </summary>
    /// <param name="fileName">Filename of the code file</param>
    /// <param name="sourceCode">The sourcecode</param>
    /// <returns>The preprocessed sourcecde</returns>
    public delegate string SourceFilePreprocessor(string fileName, string sourceCode);

    /// <summary>
    /// Builds an index of classes and there base classes
    /// </summary>
    public class ClassHierarchy
    {
        const string CacheVersionNumber = "3";
        readonly ConcurrentQueue<CacheData> CacheDataQueue = new();
        readonly string CacheFileName;

        bool IndexInitialized = false;
        IReadOnlyDictionary<string, string> ClassBaseClassIndex;
        IReadOnlyDictionary<string, NameHashset> ClassInterfacesIndex;
        IReadOnlyDictionary<string, string> ClassProjectFileName;

        class CacheData
        {
            public CacheDataItem[] Classes { get; set; }
        }

        class CacheDataItem
        {
            public string Name { get; set; }
            public string BaseClassName { get; set; }
            public string[] Implements { get; set; }
            public string ProjectFileName { get; set; }
        }

        CacheHelper _Cache;
        List<string> _ParseOptions;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cacheFileName">Filename for the cache file. If emtpy, no cache will be created.</param>
        public ClassHierarchy(string cacheFileName = null)
        {
            CacheFileName = cacheFileName;
        }

        void ExecuteFile(string fileName, string projectName, SourceFilePreprocessor SourceFilePreprocessor)
        {
            var sourceCode = File.ReadAllText(fileName);
            if (SourceFilePreprocessor != null)
                sourceCode = SourceFilePreprocessor.Invoke(fileName, sourceCode);

            if (!(_Cache?.TryGetValue(fileName, sourceCode, out CacheData currentCache) ?? false))
            {
                currentCache = new();
                var parser = ParserHelper.BuildWithOptionsList(_ParseOptions);
                if (!parser.ParseText(sourceCode, fileName).OK)
                    return;

                currentCache.Classes = parser.Tree
                                            .WhereType<Class_Context>()
                                            .ToValues()
                                            .Select(q => new CacheDataItem
                                            {
                                                Name = q.Name,
                                                BaseClassName = q.Inherits,
                                                Implements = q.Implements,
                                                ProjectFileName = projectName
                                            })
                                            .ToArray();
                _Cache?.Add(fileName, sourceCode, currentCache);
            }

            CacheDataQueue.Enqueue(currentCache);
        }


        void CreateIndex()
        {
            if (IndexInitialized)
                return;

            var tempClassBaseClass = new MappingDictionary();
            var tempClassInterfaces = new MappingDictionary<NameHashset>();
            var tempClassProjectFileNames = new MappingDictionary();

            foreach (var cacheData in CacheDataQueue)
            {
                foreach (CacheDataItem item in cacheData.Classes)
                {
                    if (!string.IsNullOrEmpty(item.BaseClassName))
                        tempClassBaseClass[item.Name] = item.BaseClassName;

                    if (item.Implements?.Length > 0)
                        tempClassInterfaces[item.Name] = new(item.Implements);

                    if (!string.IsNullOrEmpty(item.ProjectFileName))
                        tempClassProjectFileNames[item.Name] = item.ProjectFileName;
                }
            }

            ClassBaseClassIndex = tempClassBaseClass;
            ClassInterfacesIndex = tempClassInterfaces;
            ClassProjectFileName = tempClassProjectFileNames;

            IndexInitialized = true;
        }

        /// <summary>
        /// Analyzes 
        /// </summary>
        /// <param name="projectFilePath">Path for the projectfile</param>
        /// <param name="SourceFilePreprocessor">A function with the signature (string fileName, string sourceCode) can be passed for preprocessing files before analysis</param>
        public void AnalyzeProject(string projectFilePath, SourceFilePreprocessor SourceFilePreprocessor = null)
        {
            IndexInitialized = false;

            try
            {
                if (!string.IsNullOrEmpty(CacheFileName))
                    _Cache = new CacheHelper(CacheFileName, CacheVersionNumber);

                var project = new ProjectHelper(projectFilePath);
                _ParseOptions = project.GetOptions();

                var fileNames = project.GetSourceFileInfos().OrderByLargestFiles().Select(q => q.FullName).ToArray();

                var projectFileName = Path.GetFileName(projectFilePath);
                var maxProcessorCount = Environment.ProcessorCount;
                if (maxProcessorCount > 10)
                    maxProcessorCount -= 2;

                Parallel.ForEach(fileNames, new() { MaxDegreeOfParallelism = maxProcessorCount }, fileName => ExecuteFile(fileName, projectFileName, SourceFilePreprocessor));
            }
            finally
            {
                _Cache?.Dispose();
                _Cache = null;
            }
        }

        /// <summary>
        /// Returns the class hierarchy for a className as strings
        /// </summary>
        /// <param name="className">the class name</param>
        /// <returns>the class hierarchy</returns>
        public IEnumerable<string> GetClassHierarchy(string className)
        {
            CreateIndex();

            var baseClassName = className;
            var found = true;
            while (!string.IsNullOrEmpty(baseClassName) && found)
            {
                yield return baseClassName;
                found = ClassBaseClassIndex.TryGetValue(baseClassName, out baseClassName);
            }
        }

        /// <summary>
        /// Checks, if the one of the baseClassNames is a base-class for the class
        /// </summary>
        /// <param name="className">The class-name for the class</param>
        /// <param name="baseClassNames">The list of the base-class-names</param>
        /// <returns>True, is one of the base-class-names is a base-class for the class</returns>
        public bool IsBaseClass(string className, NameHashset baseClassNames)
        {
            CreateIndex();

            if (string.IsNullOrEmpty(className) || !ClassBaseClassIndex.ContainsKey(className) || string.IsNullOrEmpty(ClassBaseClassIndex[className]))
                return false;

            foreach (string baseClassName in GetClassHierarchy(className))
            {
                if (baseClassNames.Contains(baseClassName))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Checks, if the base-class-name is a base-class for the class
        /// </summary>
        /// <param name="className">The class-name for the class</param>
        /// <param name="baseClassName">The base-class-name</param>
        /// <returns>True, is one of the base-class-names is a base-class for the class</returns>
        public bool IsBaseClass(string className, string baseClassName)
            => IsBaseClass(className, new NameHashset() { baseClassName });

        /// <summary>
        /// Returns the ProjectFileName (without the path), in which the class is defined
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public string GetProjectFileName(string className)
        {
            CreateIndex();

            if (ClassProjectFileName.ContainsKey(className))
                return ClassProjectFileName[className];

            return null;
        }

        /// <summary>
        /// Checks if class or one of its ancesters implements the interface(s)
        /// </summary>
        /// <param name="className">The class name</param>
        /// <param name="interface">The interface name(s)</param>
        /// <returns></returns>
        public bool ImplementsInterface(string className, params string[] @interface)
        {
            CreateIndex();

            if (string.IsNullOrEmpty(className) || !ClassBaseClassIndex.ContainsKey(className) || @interface?.Length == 0 || string.IsNullOrEmpty(@interface[0]))
                return false;

            foreach (string baseClassName in GetClassHierarchy(className))
            {
                if (ClassInterfacesIndex.TryGetValue(baseClassName, out var classInterfaces) && @interface.Any(q => classInterfaces.Contains(q)))
                    return true;
            }

            return false;
        }
    }
}

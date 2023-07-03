using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace XSharp.VsParser.Helpers.Project
{
    /// <summary>
    /// The ProjectHelper Class
    /// </summary>
    public class ProjectHelper
    {
        private readonly string _FilePath;
        private readonly XDocument _ProjectXml;
        private readonly XNamespace _Ns;

        /// <summary>
        /// Creates a new instance of the ProjectHelper class for a XSharp project file
        /// </summary>
        /// <param name="filePath">The fileName of an XSharp project file</param>
        public ProjectHelper(string filePath)
        {
            _FilePath = filePath;
            _ProjectXml = XDocument.Load(filePath);
            _Ns = _ProjectXml.Root.Name.Namespace;
        }

        /// <summary>
        /// Reads the list of files from the project and returns it.
        /// </summary>
        /// <returns>
        /// A list of source files.
        /// </returns>
        public List<string> GetSourceFiles(bool fullPath = false)
        {
            var root = _ProjectXml.Root;
            var result = new List<string>();

            var files = root.Descendants(_Ns + "Compile")
                .Select(q => q.Attribute("Include")?.Value)
                .Where(q => !string.IsNullOrEmpty(q));

            if (fullPath)
            {
                var basePath = Path.GetDirectoryName(_FilePath);
                if (!string.IsNullOrEmpty(basePath))
                    files = files.Select(q => Path.Combine(basePath, q));
            }

            return files.ToList();
        }

        /// <summary>
        /// Reads the list of fileinfos from the project and returns it.
        /// </summary>
        /// <returns>
        /// A list of source files.
        /// </returns>
        public IEnumerable<FileInfo> GetSourceFileInfos()
            => GetSourceFiles(true)
                .Select(q => new FileInfo(q));

        /// <summary>
        /// Reads the list of options from the project and returns it.
        /// </summary>
        /// <returns>
        /// A list of found options.
        /// </returns>
        public List<string> GetOptions()
        {
            var root = _ProjectXml.Root;
            var options = new List<string>();

            void Add(string key, string value)
            {
                if (!string.IsNullOrEmpty(value))
                    options.Add($"{key}:{value}");
            }

            Add("dialect", GetProjectProperty("Dialect")?.ToLower());
            options.AddRange(GetReferences().Select(x => $"r:{x}"));
            Add("ns", GetProjectProperty("RootNamespace"));
            options.AddRange(GetFlags());

            Add("stddefs", GetProjectProperty("StandardDefs")?.Trim());
            options.Add($"nostddefs{(GetProjectProperty("NoStandardDefs")?.ToLower() == "true" ? "+" : "-")}");

            return options;
        }


        private string GetProjectProperty(string propertyName)
            => _ProjectXml.Root.Element(_Ns + "PropertyGroup").Element(_Ns + propertyName)?.Value;

        private List<string> GetReferences()
        {
            var root = _ProjectXml.Root;
            var tagNames = new string[] { "Reference", "ProjectReference" };

            return root.Descendants()
                .Where(p => tagNames.Contains(p.Name.LocalName))
                .Select(q => q.Attribute("Include")?.Value)
                .Where(q => !string.IsNullOrEmpty(q))
                .ToList();
        }

        private List<string> GetFlags()
        {
            var root = _ProjectXml.Root;
            var flags = new string[] {"vo1", "vo2" , "vo3" , "vo4" , "vo5" , "vo6" , "vo7" , "vo8" , "vo9" ,
                "vo10" , "vo11" , "vo12", "vo13", "vo14", "vo15","vo16",
                "cs", "az","ins", "lb","memvar","namedargs","undeclared","unsafe","xpp1","xpp2","fox1", "allowdot",
                "ovf", "ns"};
            var result = new List<string>();

            foreach (var node in root.Element(_Ns + "PropertyGroup").Elements())
            {
                string localName = node.Name.LocalName.ToLower();

                if (!flags.Contains(localName) ||
                    !bool.TryParse(node.Value, out bool value))
                {
                    continue;
                }

                result.Add($"{localName}{(value ? "+" : "-")}");
            }

            return result;
        }

    }
}

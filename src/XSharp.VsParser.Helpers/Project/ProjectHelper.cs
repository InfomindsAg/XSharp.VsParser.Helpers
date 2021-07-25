using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace XSharp.VsParser.Helpers.Project
{
    public class ProjectHelper
    {
        private readonly string _FilePath;
        private readonly XDocument _ProjectXml;

        public ProjectHelper(string filePath)
        {
            _FilePath = filePath;
            _ProjectXml = XDocument.Load(filePath);
        }

        public List<string> GetSourceFiles()
        {
            var result = new List<string>();

            // TODO: Read list of files from Projects
            throw new NotImplementedException();
        }

        public List<string> GetOptions()
        {
            var result = new List<string>();

            // TODO: Read options from Projects
            throw new NotImplementedException();
        }

    }
}

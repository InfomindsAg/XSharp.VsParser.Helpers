using Extensions.Data;
using LiteDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace XSharp.VsParser.Helpers.Cache
{
    /// <summary>
    /// The CacheHelper class can be used to cache data generated based on parsed source code. The cache returns cached data, as long as the hash for the source code stays the same. A LiteDB Database is used for Storage
    /// </summary>
    public class CacheHelper : IDisposable
    {
        class Version
        {
            public static string DefaultID = "Version";
            public string Id { get; set; } = DefaultID;
            public int ApplicationVersion { get; set; } = 0;
        }

        class CacheItem<T>
        {
            public string Id { get; set; }
            public uint Hash { get; set; }
            public T Data { get; set; }
        }

        readonly string _CacheFileName;

        LiteDatabase DB;
        readonly int _ApplicationVersion;


        void OpenDb()
        {
            try
            {
                DB = new LiteDatabase(_CacheFileName);
            }
            catch
            {
                var logFileName = Path.Combine(Path.GetDirectoryName(_CacheFileName), Path.GetFileNameWithoutExtension(_CacheFileName) + "-log" + Path.GetExtension(_CacheFileName));
                if (File.Exists(logFileName))
                    File.Delete(logFileName);
                DB = new LiteDatabase(_CacheFileName);
            }

            if (DB.GetCollection<Version>().Count() == 0)
                DB.GetCollection<Version>().Insert(new Version() { ApplicationVersion = _ApplicationVersion });
        }

        BsonValue GetKeyValue(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException($"{fileName} can not be emtpy");
            return new BsonValue(fileName.ToLower());
        }

        ILiteCollection<CacheItem<T>> GetCollection<T>() where T : class
        {
            return DB.GetCollection<CacheItem<T>>(typeof(T).Name);
        }

        static uint GetHash(string sourceCode) => XXHash.XXH32(Encoding.UTF8.GetBytes(sourceCode));

        /// <summary>
        /// Creates a new CacheHelper instance
        /// </summary>
        /// <param name="cacheFileName">The fileName for the cache file</param>
        /// <param name="applicationVersion">The applicationVersion is used to version the cache-content. If the version stored in the cacheFile is different, the cache will be automatically deleted.</param>
        public CacheHelper(string cacheFileName, int applicationVersion = 0)
        {
            BsonMapper.Global.TrimWhitespace = false;
            BsonMapper.Global.EmptyStringToNull = false;
            BsonMapper.Global.SerializeNullValues = true;

            if (string.IsNullOrEmpty(cacheFileName))
                throw new ArgumentException($"{cacheFileName} can not be emtpy");

            _CacheFileName = cacheFileName;
            _ApplicationVersion = applicationVersion;

            OpenDb();

            if (DB.GetCollection<Version>().FindById(Version.DefaultID)?.ApplicationVersion != applicationVersion)
                Drop();
        }

        /// <summary>
        /// Deletes the cache file
        /// </summary>
        public void Drop()
        {
            DB.Dispose();
            File.Delete(_CacheFileName);
            OpenDb();
        }

        /// <summary>
        /// Tries to get the cached data for a fileName/souceCode. The data is only returned, if the sourceCode was not changed.
        /// </summary>
        /// <typeparam name="T">The type of the cache data. Use only serializable data types!</typeparam>
        /// <param name="fileName">The fileName of the source file</param>
        /// <param name="sourceCode">The file content (to chech if the source file changed)</param>
        /// <param name="result">The cached data</param>
        /// <returns>True, if the cache contained data for the fileName and the source code was not changed. Otherwise false.</returns>
        public bool TryGetValue<T>(string fileName, string sourceCode, out T result) where T : class
        {
            result = default;

            var id = GetKeyValue(fileName);
            var collection = GetCollection<T>();
            var item = collection.FindById(id);
            if (item == null)
                return false;
            uint hash = GetHash(sourceCode);
            if (item.Hash != hash)
            {
                collection.Delete(id);
                return false;
            }
            result = item.Data;
            return true;
        }

        /// <summary>
        /// Tries to get the cached data for a fileName/souceCode. The data is only returned, if the sourceCode was not changed.
        /// </summary>
        /// <typeparam name="T">The type of the cache data. Use only serializable data types!</typeparam>
        /// <param name="fileName">The fileName of the source file</param>
        /// <param name="result">The cached data</param>
        /// <returns>True, if the cache contained data for the fileName and the source code was not changed. Otherwise false.</returns>
        public bool TryGetValue<T>(string fileName, out T result) where T : class
        {
            result = null;
            if (File.Exists(fileName))
                return false;

            return TryGetValue(fileName, File.ReadAllText(fileName), out result);
        }

        /// <summary>
        /// Adds cached data for a fileName/souceCode to the cache.
        /// </summary>
        /// <typeparam name="T">The type of the cache data. Use only serializable data types!</typeparam>
        /// <param name="fileName">The fileName of the source file</param>
        /// <param name="sourceCode">The file content (to chech if the source file changed)</param>
        /// <param name="data">The cached data</param>
        public void Add<T>(string fileName, string sourceCode, T data) where T : class
        {
            var id = GetKeyValue(fileName);
            var hash = GetHash(sourceCode);
            var collection = GetCollection<T>();
            var item = collection.FindById(id);

            if (item == null)
            {
                collection.Insert(new CacheItem<T> { Id = id.AsString, Hash = hash, Data = data });
            }
            else
            {
                item.Hash = hash;
                item.Data = data;
                collection.Update(item);
            }
        }

        /// <summary>
        /// Disposes the object
        /// </summary>
        public void Dispose()
        {
            if (DB != null)
                DB.Dispose();
        }
    }
}

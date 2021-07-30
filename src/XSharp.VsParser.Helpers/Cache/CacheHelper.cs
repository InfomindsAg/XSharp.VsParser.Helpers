using Extensions.Data;
using LiteDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace XSharp.VsParser.Helpers.Cache
{
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
            DB = new LiteDatabase(_CacheFileName);
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

        public void Drop()
        {
            DB.Dispose();
            File.Delete(_CacheFileName);
            OpenDb();
        }

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

        public bool TryGetValue<T>(string fileName, out T result) where T : class
        {
            result = null;
            if (File.Exists(fileName))
                return false;

            return TryGetValue(fileName, File.ReadAllText(fileName), out result);
        }

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

        public void Dispose()
        {
            if (DB != null)
                DB.Dispose();
        }
    }
}

using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading.Tasks;
using ProtoBuf;

namespace InterProcData
{
    public class DataMap<T> : IDisposable
    {
        protected T SharedData;
        protected MemoryMappedFile MemoryMap;

        public DataMap(T sharedData)
        {
            SharedData = sharedData;
        }

        public void StartHostAsync(string name)
        {
            Task.Factory.StartNew(() => StartHost(name));
        }

        public void StartHost(string name)
        {
            var ms = new MemoryStream();
            Serializer.SerializeWithLengthPrefix(ms, SharedData, PrefixStyle.Fixed32);
            var buffer = ms.GetBuffer();

            MemoryMap = MemoryMappedFile.CreateNew(name, ms.Length);

            var stream = MemoryMap.CreateViewStream();
            stream.Write(buffer, 0, (int)ms.Length);
        }

        public static T GetHostedData(string name)
        {
            var memoryMap = MemoryMappedFile.OpenExisting(name);

            var stream = memoryMap.CreateViewStream();
            stream.Seek(0, SeekOrigin.Begin);

            return Serializer.DeserializeWithLengthPrefix<T>(stream, PrefixStyle.Fixed32);
        }

        public void Dispose()
        {
            MemoryMap.Dispose();
        }
    }
}

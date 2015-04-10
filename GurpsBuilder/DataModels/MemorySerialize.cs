using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace GurpsBuilder.DataModels
{
    public static class MemorySerialize
    {
        public static MemoryStream Serialize(object o)
        {
            MemoryStream stream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, o);
            return stream;
        }

        public static MemoryStream Serialize<T>(T o)
        {
            return Serialize(o);
        }

        public static object Deserialize(MemoryStream stream)
        {
            IFormatter formatter = new BinaryFormatter();
            stream.Seek(0, SeekOrigin.Begin);
            object o = formatter.Deserialize(stream);
            return o;
        }

        public static T Deserialize<T>(MemoryStream stream)
        {
            return (T)Deserialize(stream);
        }
    }
}

using System;
using System.IO;
using System.Xml.Serialization;

namespace NSWallet.Shared
{
    public static class XMLManager<T>
    {
        public static T GetIconSet(string path)
        {
            T obj = default(T);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            var reader = System.Xml.XmlReader.Create(path);
			obj = (T)(serializer.Deserialize(reader));
            return obj;
        }
    }
}
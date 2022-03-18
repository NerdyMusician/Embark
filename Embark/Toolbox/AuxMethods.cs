using Embark.Windows;
using System;
using System.IO;
using System.Xml.Serialization;

namespace Embark.Toolbox
{
    public static class AuxMethods
    {
        public static void WriteToLogFile(string message, bool notifyUser = false)
        {
            if (notifyUser) { NotifyUser(message); }
            File.AppendAllText("log.txt", DateTime.Now + ": " + message + "\n");
        }
        public static void NotifyUser(string message)
        {
            new NotificationDialog(message).ShowDialog();
        }
        public static T DeepClone<T>(this T obj)
        {
            using MemoryStream ms = new();
            XmlSerializer xmlSerializer = new(typeof(T));
            xmlSerializer.Serialize(ms, obj);
            ms.Position = 0; // Fixes "Root element is missing" issue https://stackoverflow.com/questions/30698349/xml-serializing-and-deserializing-with-memory-stream 
            return (T)xmlSerializer.Deserialize(ms);
        }

    }
}

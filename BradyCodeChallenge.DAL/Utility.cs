using System.Xml.Serialization;

namespace BradyCodeChallenge.DAL
{
    // Utility class providing XML serialization and deserialization methods
    public static class Utility
    {
        // Method to deserialize XML data from a file to an object of type T
        public static T DeserializeXmlToObject<T>(string filePath)
        {
            // Create an XmlSerializer for the specified type T
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            // Open the file stream at the specified path
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                // Deserialize the XML data from the file stream and cast it to type T
                return (T)serializer.Deserialize(fileStream);
            }
        }

        // Method to serialize an object of type T to XML and save it to a file
        public static void SerializeObjectToXml<T>(T obj, string filePath)
        {
            // Create an XmlSerializer for the specified type T
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            // Open a text writer to the specified file path
            using (TextWriter writer = new StreamWriter(filePath))
            {
                // Serialize the object to XML and write it to the file
                serializer.Serialize(writer, obj);
            }
        }
    }
}

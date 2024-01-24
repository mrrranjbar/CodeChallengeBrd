using System.Xml.Serialization;

namespace BradyCodeChallenge.DAL.Model.Output
{
    public class Generator
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Total")]
        public decimal Total { get; set; }
    }
}

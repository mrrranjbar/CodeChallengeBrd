using System.Xml.Serialization;

namespace BradyCodeChallenge.DAL.Model.Inputs
{
    public class WindGenerator : Generator
    {
        [XmlElement("Location")]
        public string Location { get; set; }
    }
}

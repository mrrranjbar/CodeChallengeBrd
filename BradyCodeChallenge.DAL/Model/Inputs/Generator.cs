using System.Xml.Serialization;

namespace BradyCodeChallenge.DAL.Model.Inputs
{
    public class Generator
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Generation")]
        public List<Generation> Generations { get; set; }
    }
}

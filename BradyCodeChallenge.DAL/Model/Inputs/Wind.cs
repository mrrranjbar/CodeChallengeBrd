using System.Xml.Serialization;

namespace BradyCodeChallenge.DAL.Model.Inputs
{
    public class Wind
    {
        [XmlElement("WindGenerator")]
        public List<WindGenerator> WindGenerators { get; set; }
    }
}

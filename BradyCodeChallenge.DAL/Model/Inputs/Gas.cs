using System.Xml.Serialization;

namespace BradyCodeChallenge.DAL.Model.Inputs
{
    public class Gas
    {
        [XmlElement("GasGenerator")]    
        public List<GasGenerator> GasGenerator { get; set; }
    }
}

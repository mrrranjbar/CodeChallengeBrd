using System.Xml.Serialization;

namespace BradyCodeChallenge.DAL.Model.Inputs
{
    public class Coal
    {
        [XmlElement("CoalGenerator")]
        public List<CoalGenerator> CoalGenerator { get; set; }
    }
}

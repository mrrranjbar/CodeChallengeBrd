using System.Xml.Serialization;

namespace BradyCodeChallenge.DAL.Model.Output
{
    public class MaxEmissionGenerators
    {
        [XmlElement("Day")]
        public List<Day> Days { get; set; }
    }
}

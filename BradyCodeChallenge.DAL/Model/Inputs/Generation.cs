using System.Xml.Serialization;

namespace BradyCodeChallenge.DAL.Model.Inputs
{
    public class Generation
    {
        [XmlElement("Day")]
        public List<Day> Days { get; set; }
    }
}

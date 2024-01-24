using BradyCodeChallenge.DAL.Model.Enums;
using System.Xml.Serialization;

namespace BradyCodeChallenge.DAL.Model.Static
{ 
    public class GeneratorInfo
    {
        [XmlIgnore]
        public int GeneratorInfoId { get; set; }
        public GeneratorType GeneratorType { get; set; }
        public Enums.ValueFactor ValueFactor { get; set; }
        public EmissionFactor EmissionFactor { get; set; }
    }
}

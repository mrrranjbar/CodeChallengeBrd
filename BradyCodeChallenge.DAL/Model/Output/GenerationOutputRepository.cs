
namespace BradyCodeChallenge.DAL.Model.Output
{
    // Repository class for handling GenerationOutput data
    public class GenerationOutputRepository : IGenerationOutputRepository
    {
        // Method to create an XML file containing GenerationOutput data
        public void Create(GenerationOutput entity, string filePath, string preFileNameWithoutExtension)
        {
            // Generate the full path for the output XML file and Serialize the GenerationOutput object to XML and save it to the specified file path
            Utility.SerializeObjectToXml(entity, filePath + "\\" + preFileNameWithoutExtension + "-Result.xml");
        }
    }
}

using BradyCodeChallenge.DAL.Model.Error;

namespace BradyCodeChallenge.DAL.Model.Inputs
{
    // Repository class for handling GenerationReport data
    public class GenerationReportRepository : IGenerationReportRepository
    {
        // Method to retrieve a GenerationReport object from a specified file path
        public GenerationReport? Get(string filePath)
        {
            // Check if the file exists at the specified path
            if (!File.Exists(filePath))
            {
                // Print an error message and return null if the file does not exist
                ErrorContent.PrintError(new ErrorContent() { Message = $"There is no file in this address: {filePath}!" });
                return null;
            }

            // Deserialize the XML file at the specified path to a GenerationReport object
            return Utility.DeserializeXmlToObject<GenerationReport>(filePath);
        }
    }
}

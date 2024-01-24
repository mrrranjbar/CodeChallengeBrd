
namespace BradyCodeChallenge.DAL.Model.Error
{
    // Class representing error content with a message property
    public class ErrorContent
    {
        // Property to store the error message
        public string Message { get; set; }

        // Static method to print the error message to the console
        public static void PrintError(ErrorContent error)
        {
            // Output the error message to the console
            Console.WriteLine(error.Message);
        }
    }
}

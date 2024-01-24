using BradyCodeChallenge.DAL.Model.Error;
using BradyCodeChallenge.DAL.Model.Inputs;
using BradyCodeChallenge.DAL.Model.Output;
using BradyCodeChallenge.Services.Core;
using Microsoft.Extensions.Configuration;

namespace BradyCodeChallenge.Services
{
    // Watcher class implementing IWatcher interface
    public class Watcher : IWatcher
    {
        // Constructor to initialize required dependencies
        private readonly IConfiguration _configuration;
        private readonly IGenerationReportRepository _generationReportRepository;
        private readonly IGenerationOutputRepository _outputRepository;
        private readonly ICalculation _calculation;
        public Watcher(IConfiguration configuration,
            IGenerationReportRepository generationReportRepository,
            IGenerationOutputRepository outputRepository,
            ICalculation calculation) 
        { 
            _configuration = configuration;
            _generationReportRepository = generationReportRepository;
            _outputRepository = outputRepository;
            _calculation = calculation;
        }

        // Method to start watching the input folder for new files
        public void StartWatching() 
        {
            // Get the input folder path from configuration
            var inputFolderPath = _configuration.GetRequiredSection("InputFolderPath");
            
            // Check if the input folder path is correct
            if (inputFolderPath.Value == null)
            {
                // Print an error message and return if the path is incorrect
                ErrorContent.PrintError(new ErrorContent() { Message = $"Input Folder Path is NOT correct! Method name is {nameof(StartWatching)}" });
                return;
            }

            // Create a FileSystemWatcher to monitor the input folder for file creations
            FileSystemWatcher watcher = new FileSystemWatcher(inputFolderPath.Value);
            watcher.Created += OnFileCreated;
            watcher.EnableRaisingEvents = true;

            // Return after setting up the watcher
            return;
        }

        // Event handler for file creation in the input folder
        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            // Retrieve the generation report from the repository based on the new file
            GenerationReport? generationReport = _generationReportRepository.Get(e.FullPath);

            // Get the output folder path from configuration
            var outputFolderPath = _configuration.GetRequiredSection("OutputFolderPath");

            // Check if the generation report or output folder path is incorrect
            if (generationReport == null || outputFolderPath.Value == null)
            {
                // Print an error message and return if either is incorrect
                ErrorContent.PrintError(new ErrorContent() { Message = $"Input and/or Output folder paths are not correct! Method name is {nameof(OnFileCreated)}" });
                return;
            }

            // Calculate the generation output based on the generation report
            GenerationOutput? res = _calculation.Calculate(generationReport);

            // Check if the generation output is null
            if (res == null) 
            {
                // Print an error message and return if the generation output is null
                ErrorContent.PrintError(new ErrorContent() { Message = $"Generation Output is null! Method name is {nameof(OnFileCreated)}" });
                return;
            }

            // Create the output file using the calculated generation output
            _outputRepository.Create(res, outputFolderPath.Value, Path.GetFileNameWithoutExtension(e.FullPath));

            // Return after processing the created file
            return;
        }
    }
}

using System;
using System.IO;

namespace DataProcessor
{
    internal class FileProcessor
    {
        private string filePath;
        private static readonly string BackupDirectoryName = "backup";
        private static readonly string InProgressDirectoryName = "processing";
        private static readonly string CompletedDirectoryName = "complete";
        public string inputFilePath { get; }

        public FileProcessor(string filePath)
        {
            inputFilePath = filePath;
        }

        internal void Process()
        {
            Console.WriteLine($"Begin process of {inputFilePath}");

            // Check if file exists
            if (!File.Exists(inputFilePath))
            {
                Console.WriteLine($"ERROR: file {inputFilePath} does not exist.");
                return;
            }

            string rootDirectoryPath = new DirectoryInfo(inputFilePath).Parent.Parent.FullName;
             Console.WriteLine($"Root data path is {rootDirectoryPath}");
           
            // Check if backup directory exists
            string inputFileDirectoryPath = Path.GetDirectoryName(inputFilePath);
            string backupDirectoryPath = Path.Combine(rootDirectoryPath, BackupDirectoryName);
            //if (!Directory.Exists(backupDirectoryPath))
            //
            //    Console.WriteLine($"Creating {backupDirectoryPath}");

            //The method does not throw error if the directory already exists , no need to check 
             Directory.CreateDirectory(backupDirectoryPath);

            // copy file to backup directory
            string inputFileName = Path.GetFileName(inputFilePath);
            string backupFilePath = Path.Combine(backupDirectoryPath, inputFileName);
            Console.WriteLine($"Copying {inputFilePath} to {backupFilePath}");
            File.Copy(inputFilePath, backupFilePath, true);

            // Move to in progress dir
            Directory.CreateDirectory(Path.Combine(rootDirectoryPath, InProgressDirectoryName));
            string inProgressFilePath = Path.Combine(rootDirectoryPath, InProgressDirectoryName, inputFileName);

            if (File.Exists(inProgressFilePath))
            {
                Console.WriteLine($"ERROR: a file name {inProgressFilePath} is already being processed.");
                return;
            }

            Console.WriteLine($"Moving {inputFilePath} to {inProgressFilePath}");
            File.Move(inputFilePath, inProgressFilePath);

            // Determine type of file
            string extension = Path.GetExtension(inputFilePath);

            string completedDirectoryPath = Path.Combine(rootDirectoryPath, CompletedDirectoryName);
            Directory.CreateDirectory(completedDirectoryPath);

            var completedFileName = $"{Path.GetFileNameWithoutExtension(inputFilePath)}--{Guid.NewGuid()}-{extension}";


            var completedFilePath = Path.Combine(completedDirectoryPath, completedFileName);

            switch (extension)
            {
                case ".txt":
                    //ProcessTxtFile(inProgressFilePath);
                    var textProcessor = new TextFileProcessor(inProgressFilePath, completedFilePath);
                    textProcessor.Process();
                    break;
                case ".data":
                    var binaryProcessor = new BinaryFileProcessor(inProgressFilePath, completedFilePath);
                    binaryProcessor.Process();
                    break;
                case ".csv":
                    var csvProcessor = new CsvFileProcessor(inProgressFilePath, completedFilePath);
                    csvProcessor.Process();
                    break;
                default:
                    Console.WriteLine($"{extension} is an unsupported file type.");
                    break;
            }

            

            //File.Move(inProgressFilePath, completedFilePath);
            Console.WriteLine($"Completed processing of {inProgressFilePath}");
            Console.WriteLine($"Deleting {inProgressFilePath}");
            File.Delete(inProgressFilePath);

        }

        //private void ProcessTxtFile(string inProgressFilePath)
        //{
        //    Console.WriteLine($"Processing text file {inProgressFilePath}");
        //}
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            var command = args[0];

            if(command == "--file")
            {
                var filePath = args[1];
                Console.WriteLine($"Single file {filePath} selected");
                ProcessSingleFile(filePath);
            }
            else if(command == "--dir")
            {
                var directoryPath = args[1];
                var fileType = args[2];
                Console.WriteLine($"Directory {directoryPath} select for {fileType} files");
                ProcessDirectory(directoryPath, fileType);
            }
            else
            {
                Console.WriteLine("Invalid command line options");
            }

            Console.WriteLine("Press enter to quit");
        }

        private static void ProcessDirectory(string directoryPath, string fileType)
        {
            // Get list of all files in directory
            //var allFiles = Directory.GetFiles(directoryPath);

            switch (fileType)
            {
                case "TEXT":
                    string[] textFiles = Directory.GetFiles(directoryPath, "*.txt");
                    foreach (var textFilePath in textFiles)
                    {
                        var fileProcessor = new FileProcessor(textFilePath);
                        fileProcessor.Process();
                    }
                    break;
                default:
                    Console.WriteLine($"ERROR: {fileType} is not supported");
                    return;
            }
        }

        private static void ProcessSingleFile(string filePath)
        {
            var fileProcessor = new FileProcessor(filePath);
            fileProcessor.Process();
        }

        
    }
}

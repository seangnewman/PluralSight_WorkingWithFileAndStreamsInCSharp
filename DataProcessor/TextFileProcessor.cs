using System.IO;
using System.Text;
using System;
namespace DataProcessor
  
{
    class TextFileProcessor
    {
        public string InputFilePath { get; }
        public string OutputFilePath { get; }

        public TextFileProcessor(string inputFilePath, string outputFilePath)
        {
            InputFilePath = inputFilePath;
            OutputFilePath = outputFilePath;
        }

        public void Process()
        {
            // Using read all text
            //string originalText = File.ReadAllText(InputFilePath);
            //string processedTest = originalText.ToUpperInvariant();
            //File.WriteAllText(OutputFilePath, processedTest);

            // Using read all lines
            //string[] lines = File.ReadAllLines(InputFilePath);
            //lines[1] = lines[1].ToUpperInvariant();
            //File.WriteAllLines(OutputFilePath, lines);

            // using (var inputFileStream = new FileStream(InputFilePath, FileMode.Open))
            //    using(var inputStreamReader = new StreamReader(inputFileStream))
            //    using(var outputFileStream = new FileStream(OutputFilePath, FileMode.Create))
            //    using (var outputStreamWriter = new StreamWriter(outputFileStream))
            //{
            //    while (!inputStreamReader.EndOfStream)
            //    {
            //        string line = inputStreamReader.ReadLine();  // Only one line in memory as processing
            //        string processedLine = line.ToUpperInvariant();
            //        outputStreamWriter.WriteLine(processedLine);

            //    }
            //}

            //using (var inputStreamReader = new StreamReader(InputFilePath)) 
            //  using (var outputStreamWriter = new StreamWriter(OutputFilePath))
            //  {
            //    while (!inputStreamReader.EndOfStream)
            //    {
            //        string line = inputStreamReader.ReadLine();  // Only one line in memory as processing
            //        string processedLine = line.ToUpperInvariant();

            //        bool isLastLine = inputStreamReader.EndOfStream;

            //        if (isLastLine)
            //        {
            //            outputStreamWriter.Write(processedLine);
            //        }
            //        else
            //        {
            //            outputStreamWriter.WriteLine(processedLine);
            //        }

            //    }


                using (var inputStreamReader = new StreamReader(InputFilePath))
                using (var outputStreamWriter = new StreamWriter(OutputFilePath))
                {
                   var currentLineNumber = 1;

                while (!inputStreamReader.EndOfStream)
                {
                    string line = inputStreamReader.ReadLine();
                    if (currentLineNumber == 2)
                    {
                        Write(line.ToUpperInvariant());

                    }
                    else
                    {
                        Write(line);
                    }



                    currentLineNumber++;
                }
                   void Write(string content)
                   {

                    bool isLastLine = inputStreamReader.EndOfStream;
                    if (isLastLine)
                    {
                        outputStreamWriter.Write(content);
                    }
                    else
                    {
                        outputStreamWriter.WriteLine(content);
                    }

                }
                }





        }
    }
}

using System.IO;
using System.Text;
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
            string[] lines = File.ReadAllLines(InputFilePath);
            lines[1] = lines[1].ToUpperInvariant();
            File.WriteAllLines(OutputFilePath, lines);



        }
    }
}

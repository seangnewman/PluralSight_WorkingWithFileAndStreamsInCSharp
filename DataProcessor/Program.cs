using System;
using System.IO;
using System.Runtime.Caching;
using System.Threading;


namespace DataProcessor
{
    class Program
    {

        //private static ConcurrentDictionary<string, string> FilesToProcess = new ConcurrentDictionary<string, string>();
        private static MemoryCache FilesToProcess = MemoryCache.Default;
        static void Main(string[] args)
        {
            #region Refactoring t user FileSystemWatcher
            //var command = args[0];

            //if(command == "--file")
            //{
            //    var filePath = args[1];
            //    Console.WriteLine($"Single file {filePath} selected");
            //    ProcessSingleFile(filePath);
            //}
            //else if(command == "--dir")
            //{
            //    var directoryPath = args[1];
            //    var fileType = args[2];
            //    Console.WriteLine($"Directory {directoryPath} select for {fileType} files");
            //    ProcessDirectory(directoryPath, fileType);
            //}
            //else
            //{
            //    Console.WriteLine("Invalid command line options");
            //}
            #endregion
            Console.WriteLine("Parsing command line options");
            var directoryToWatch = args[0];

            if (!Directory.Exists(directoryToWatch))
            {
                Console.WriteLine($"ERROR: {directoryToWatch} does not exist");
            }
            else
            {
                Console.WriteLine($"Watching directory {directoryToWatch} for changes");
            }

            ProcessExistingFiles(directoryToWatch);

            using (var inputFileWatcher = new FileSystemWatcher(directoryToWatch))
           // using (var timer = new Timer(ProcessFiles, null, 0, 10000))
            {
                inputFileWatcher.IncludeSubdirectories = false;
                inputFileWatcher.InternalBufferSize = 32726; // 32KB
                inputFileWatcher.Filter = "*.*";

                // Notify only if lastwrite has been changed
                inputFileWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;

                inputFileWatcher.Created += FileCreated;
                inputFileWatcher.Changed += FileChanged;
                inputFileWatcher.Deleted += FileDeleted;
                inputFileWatcher.Renamed += FileRenamed;
                inputFileWatcher.Error += WatcherError;

                //By default EnableRaisingEvents is set to false
                inputFileWatcher.EnableRaisingEvents = true;

                Console.WriteLine("Press enter to quit");
                Console.ReadLine();
            }


        }

        private static void ProcessExistingFiles(string inputDirectory)
        {
            Console.WriteLine($"* Checking {inputDirectory} for existing files");
            foreach (var filePath in Directory.EnumerateFiles(inputDirectory))
            {
                Console.WriteLine($" - Found{filePath}");
                AddToCache(filePath);
            }

        }

        //private static void ProcessFiles(Object stateInfo)
        //{
        //    foreach (var fileName in FilesToProcess.k)
        //    {
        //        if (FilesToProcess.TryRemove(fileName, out _))
        //        {
        //            // The _ indicates we do not care about output parameter
        //            var fileProcessor = new FileProcessor(fileName);
        //            fileProcessor.Process();
        //        }
        //    }
        //}

        private static void WatcherError(object sender, ErrorEventArgs e)
        {
            Console.WriteLine($"ERROR: file system wtching may no longer be active: {e.GetException()}");
        }

        private static void FileRenamed(object sender, RenamedEventArgs e)
        {
            Console.WriteLine($"* File renamed: {e.OldName} to {e.Name} - type: {e.ChangeType}");
        }

        private static void FileDeleted(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"* File deleted: {e.Name} - type: {e.ChangeType}");
        }

        private static void FileChanged(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"* File changed: {e.Name} - type: {e.ChangeType}");
            //var fileProcessor = new FileProcessor(e.FullPath);
            //fileProcessor.Process();
            //If filepath exists it will not add
            //FilesToProcess.TryAdd(e.FullPath, e.FullPath);
            AddToCache(e.FullPath);

        }

        private static void AddToCache(string fullPath)
        {
            var item = new CacheItem(fullPath, fullPath);

            var policy = new CacheItemPolicy
            {
                RemovedCallback = ProcessFile,
                SlidingExpiration = TimeSpan.FromSeconds(2)   // Must be greater than 1 sec
            };

            FilesToProcess.Add(item, policy);
        }

        private static void ProcessFile(CacheEntryRemovedArguments args)
        {
            Console.WriteLine($"* Cache item removed {args.CacheItem.Key} because {args.RemovedReason}");

            if (args.RemovedReason == CacheEntryRemovedReason.Expired)
            {
                var fileProcessor = new FileProcessor(args.CacheItem.Key);
                fileProcessor.Process();
            }
            else
            {
                Console.WriteLine($"WARNING!: {args.CacheItem.Key} was removed unexpectedly  ");
            }
        }

        private static void FileCreated(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"* File created: {e.Name} - type: {e.ChangeType}");
            //var fileProcessor = new FileProcessor(e.FullPath);
            //fileProcessor.Process();

            //If filepath exists it will not add
            //FilesToProcess.TryAdd(e.FullPath, e.FullPath);
            AddToCache(e.FullPath);
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

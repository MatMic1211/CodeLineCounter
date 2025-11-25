using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Please provide a path to the text file.");
            return;
        }

        string configFilePath = args[0];

        if (!File.Exists(configFilePath))
        {
            Console.WriteLine("The provided text file does not exist: " + configFilePath);
            return;
        }

        var directoryPaths = File.ReadAllLines(configFilePath)
            .Select(line => line
                .Replace("\ufeff", "")
                .Trim()
                .Trim('"')
                .Trim())
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .ToList();

        if (directoryPaths.Count == 0)
        {
            Console.WriteLine("No valid directory paths found in the file.");
            return;
        }

        string[] allowedExtensions = { ".html", ".cs", ".less", ".js" };

        int grandTotal = 0;

        foreach (var directoryPath in directoryPaths)
        {
            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine("The provided directory does not exist: " + directoryPath);
                continue;
            }

            var files = Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories)
                .Where(file => allowedExtensions.Contains(Path.GetExtension(file).ToLower()))
                .ToList();

            int directoryTotal = 0;

            foreach (var file in files)
            {
                try
                {
                    var lines = File.ReadAllLines(file);
                    int nonEmptyLineCount = lines.Count(line => !string.IsNullOrWhiteSpace(line));
                    directoryTotal += nonEmptyLineCount;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading file {file}: {ex.Message}");
                }
            }

            grandTotal += directoryTotal;

            Console.WriteLine("========================================");
            Console.WriteLine($"DIRECTORY: {directoryPath}");
            Console.WriteLine($"TOTAL NON-EMPTY LINES: {directoryTotal}");
            Console.WriteLine("========================================\n");
        }

        Console.WriteLine("########################################");
        Console.WriteLine($"GLOBAL TOTAL NON-EMPTY LINES: {grandTotal}");
        Console.WriteLine("########################################");
    }
}

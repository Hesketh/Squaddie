using System;
using Squaddie.Serialization;
using Squaddie;
using Squaddie.Serialization;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.IO;

namespace Squaddie.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            ICharacterPoolFileBuilder writer;
            ICharacterPoolFileBuilder reader;

            bool toJson = args.Contains("-j") || args.Contains("--toJson");
            bool toBinary = args.Contains("-b") || args.Contains("--toBinary");
            bool merge = args.Contains("-m") || args.Contains("--merge");

            if (merge && toBinary)
            {
                writer = new CharacterPoolBinaryBuilder();
                reader = new CharacterPoolBinaryBuilder();
            }
            else if (merge && toJson)
            {
                bool pretty = args.Contains("-p") || args.Contains("-pretty");

                writer = new CharacterPoolJsonBuilder(pretty);
                reader = new CharacterPoolJsonBuilder(pretty);
            }
            else if (toJson) 
            {
                bool pretty = args.Contains("-p") || args.Contains("-pretty");
                
                writer = new CharacterPoolJsonBuilder(pretty);
                reader = new CharacterPoolBinaryBuilder();
            }
            else if (toBinary)
            {
                writer = new CharacterPoolBinaryBuilder();
                reader = new CharacterPoolJsonBuilder();
            }
            else // Display help if we don't have any conversion instruction
            {
                Console.WriteLine("Squaddie - XCOM 2 War of the Chosen Character Pool conversion utility, written by Alex Hesketh.\n" +
                    "\n\t-h\n\t--help\t\t: List commands and usage instructions.\n" +
                    "\n\t-j\n\t--toJson\t: Convert from binary input to json output.\n" +
                    "\n\t-p\n\t--pretty\t: Whether to out JSON as pretty print.\n" +
                    "\n\t-b\n\t--toBinary\t: Convert from json input to binary output.\n" +
                    "\n\t-m\n\t--merge\t\t: Merge multiple pools of the same kind into a single pool. You should also specify the type with -b or -j\n" + 
                    "\n\t-i\n\t--input\t\t: Provide the input file filepath. Should be a directory path when using merge option.\n" +
                    "\n\t-o\n\t--output\t\t: Provide the output file filepath (Will overwrite any existing file).");
                return;
            }

            Dictionary<string, string> switchMappings = new Dictionary<string, string>()
             {
                 { "-i", "input" },
                 { "-o", "output" },
                 { "--input", "input" },
                 { "--output", "output" },
             };

            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddCommandLine(args, switchMappings);
            
            IConfigurationRoot config = builder.Build();

            string inputPath = config["input"];
            string outputPath = config["output"];

            if (string.IsNullOrWhiteSpace(inputPath))
            {
                if (merge && !Directory.Exists(inputPath))
                {
                    throw new ArgumentException("-i, --input must be provided to a valid directory when used with --merge option");
                }
                else if (!File.Exists(inputPath))
                {
                    throw new ArgumentException("-i, --input must be provided to a valid file to convert");
                }
            }

            if (string.IsNullOrWhiteSpace(outputPath))
            {
                throw new ArgumentException("-o, --output must be provided to give destination for output");
            }

            CharacterPool pool = null;
            if (!merge)
            {
                pool = reader.LoadFromFile(inputPath);
            }
            else if (merge)
            {
                string pattern = toBinary ? "*.bin" : "*.json";
                pool = MergePoolsInDirectory(inputPath, pattern, reader);
            }

            if (pool != null)
            {
                writer.SaveToFile(outputPath, pool);
            } 
            else
            {
                throw new ArgumentNullException("Can't write character pool to file. Character pool is empty!");
            }
        }

        public static CharacterPool MergePoolsInDirectory(string directoryPath, string pattern, ICharacterPoolFileBuilder reader)
        {
            CharacterPool mergedPool = new CharacterPool();
            int success = 0, failed = 0;
            foreach (string poolFile in Directory.EnumerateFiles(directoryPath, pattern))
            {
                try
                {
                    Console.WriteLine(poolFile);
                    CharacterPool pool = reader.LoadFromFile(poolFile);
                    mergedPool.Characters.AddRange(pool.Characters);
                    success++;
                } catch (Exception e)
                {
                    Console.WriteLine("Failed merging "+ poolFile + ": " + e.Message);
                    failed++;
                }
            }
            Console.WriteLine("Merged {0:D} files with {1:D} characters in total. Failed merging {2:D} files.", success, mergedPool.Characters.Count, failed);
            return mergedPool;
        }
    }
}

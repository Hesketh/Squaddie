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

            if (args.Contains("-j") || args.Contains("--toJson")) 
            {
                bool pretty = args.Contains("-p") || args.Contains("-pretty");
                
                writer = new CharacterPoolJsonBuilder(pretty);
                reader = new CharacterPoolBinaryBuilder();
            }
            else if (args.Contains("-b") || args.Contains("--toBinary"))
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
                    "\n\t-i\n\t--input\t\t: Provide the input file filepath.\n" +
                    "\n\t-o\n\t--ouput\t\t: Provide the output file filepath (Will overwrite any existing file).");
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

            if (string.IsNullOrWhiteSpace(inputPath) || !File.Exists(inputPath))
            {
                throw new ArgumentException("-i, --input must be provided to a valid file to convert");
            }

            if (string.IsNullOrWhiteSpace(outputPath))
            {
                throw new ArgumentException("-i, --input must be provided to give destination for output");
            }

            CharacterPool pool = reader.LoadFromFile(inputPath);
            writer.SaveToFile(outputPath, pool);
        }
    }
}

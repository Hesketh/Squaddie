# Squaddie

A huge selling point for XCOM 2 was their Character Pool system. Using it, characters I had created could naturally appear as if one of their randomly generated characters throughout a campaign. I found great joy in filling a character pool full of people I knew, so that I could be rescuing my brother who is a scientist in one campaign, to seeing myself turn up as a captured prisoner to rescue. This system is not without its flaws however, the UI to create a character for the character pool is full of animations, transitions and other UI features that just slow everything down enough to be a chore for mass character creation. That is where Squaddie comes in.

Squaddie is a library to load the native Character Pool binary format into a C# structure for editing and to compliment this I also added the ability to re-serialize the structure as a JSON file.

Then for ease of use by non-developers, I have provided a simple CLI windows application that can be used to convert from a binary to json, or a json to binary.

  
# Squaddie CLI
A simple command line windows application that can be used to convert from a binary to json, or a json to binary.

### Commands
|Command| Function |
|--|--|
|-h, --help| List commands and usage instructions. |
|-j, --toJson| Convert from binary input to json output. |
|-b, --toBinary| Convert from json input to binary output. |
|-m, --merge| Whether to out JSON as pretty print. |
|-p, --pretty| Whether to out JSON as pretty print. |
|-i, --input| Provide the input file filepath. |
|-o, --output| Provide the output file filepath (Will overwrite any existing file). |

### Examples

Converting from a binary to a JSON file. Also using -p to provide us pretty print (line breaks that aid human readability)

    ./SquaddieCLI.exe -j -p -i myCoolPool.bin -o myCoolPool.json

Converting from a JSON file to a binary file

    ./SquaddieCLI.exe -b -i myCoolerPool.json -o woahThisLoadsIntoXCOM.bin

Merge multiple character pool files in a directory into a single character pool (In binary format)

    ./SquaddieCLI.exe -m -i .\Importable -o Merged.bin -b

Merge multiple character pool files in a directory into a single character pool (In json format)

    ./SquaddieCLI.exe -m -i .\Importable -o Merged.json -j -p


# Squaddie C# Library
*ICharacterPoolFileBuilder* provides the interface for all Character Pool file loaders. The ones included within the library are *CharacterPoolJsonBuilder* for converting to/from JSON and *CharacterPoolBinaryBuilder*  for converting to/from XCOM 2 Character Pool Binary files.

A *CharacterPool* represents the collection of *Character*s and the name of the pool. From this you can access the *Characters* property to access the collection.

A *Character* is pretty much a collection of *IProperty*. These can be viewed as a simple struct contain a Key and a Value. 

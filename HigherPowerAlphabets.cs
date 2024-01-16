using System;
using System.Collections.Generic; 
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace MarkovCompression
{
  public delegate string CompressionDelegate(string text, Dictionary<string, int> dictionary);
  public delegate string DecompressionDelegate(string compressedText, Dictionary<string, int> dictionary);

  interface ICompressor {
    string Compress(string text, Dictionary<string, int> dictionary);
  }
    
  interface IDecompressor {
    string Decompress(string compressedText, Dictionary<string, int> dictionary);
  }
  
  public class Compressor : ICompressor
  {
    public string Compress(string text, Dictionary<string, int> dictionary)
    {
       StringBuilder compressed = new StringBuilder();
      string currentString = "";

      foreach(char c in text)
      {
        string currentChar = c.ToString();
        
        if(dictionary.ContainsKey(currentString + currentChar))
        {
          currentString += currentChar;
        }
        else {
          compressed.Append(dictionary[currentString] + " ");
          
          dictionary.Add(currentString + currentChar, dictionary.Count);
          currentString = currentChar;
        }
      }

      if(currentString != "") {
        compressed.Append(dictionary[currentString]);
      }

      return compressed.ToString(); 
    }
  }
  
  public class Decompressor : IDecompressor 
  {
    public string Decompress(string compressedText, Dictionary<string, int> dictionary)
    {
      StringBuilder decompressed = new StringBuilder();
      List<string> reverseLookup = new List<string>(dictionary.Keys);
      
      string[] entries = compressedText.Split(" ");

      string currentEntry = reverseLookup[int.Parse(entries[0])];
      decompressed.Append(currentEntry);

      foreach(string entry in entries.Skip(1))
      {
        int currentCode = int.Parse(entry);
        string newEntry;
        
        if(dictionary.ContainsKey(currentEntry + currentCode)) {
          newEntry = reverseLookup[dictionary[currentEntry + currentCode]];
        }
        else {
          newEntry = reverseLookup[currentCode];
          
          int newIndex = dictionary.Count;
          dictionary.Add(currentEntry + currentCode, newIndex);
          reverseLookup.Insert(newIndex, currentEntry + newEntry[0]);
        }
        
        decompressed.Append(newEntry);
        currentEntry = entry;
      }

      return decompressed.ToString();
    }
  }
  
  public abstract class Program
  {
    public static async Task Main(string[] args)
    {
      CompressionDelegate compressionDelegate = CompressText;
      DecompressionDelegate decompressionDelegate = DecompressText;
      
      ICompressor compressor = new Compressor();
      IDecompressor decompressor = new Decompressor();
      
      while (true)
      {
        Console.WriteLine("Enter the original text to compress (type 'exit' to end):");
        string text = Console.ReadLine() ?? string.Empty;

        if (text.ToLower() == "exit")
        {
          break;
        }

        if (true)
        {
          Dictionary<string, int> dictionary = BuildDictionary(text);

          string compressed = await Task.Run(() => compressor.Compress(text, dictionary));

          Console.WriteLine("\nCompressed text: " + compressed);

          Console.WriteLine("\nDo you want to decompress the text? (y/n)");
          if(Console.ReadLine() == "y")
          {
            string decompressed = await Task.Run(() => decompressor.Decompress(compressed, dictionary));
            Console.WriteLine("\nDecompressed text: " + decompressed + "\n");
          }
        }
      }
    }

    static Dictionary<string, int> BuildDictionary(string text)
    {
      Dictionary<string, int> dictionary = new Dictionary<string, int>();
      int index = 0;

      foreach(char c in text)
      {
        string currentChar = c.ToString();
        if(!dictionary.ContainsKey(currentChar))
        {
          dictionary.Add(currentChar, index++);  
        }
      }

      return dictionary;
    }
    
    static string CompressText(string text, Dictionary<string, int> dictionary)
    {
      ICompressor compressor = new Compressor();
      return compressor.Compress(text, dictionary);
    }

    // Delegate for decompression operation
    static string DecompressText(string compressedText, Dictionary<string, int> dictionary)
    {
      IDecompressor decompressor = new Decompressor();
      return decompressor.Decompress(compressedText, dictionary);
    }
    
    // static string Compress(string text, Dictionary<string, int> dictionary) 
    // {
    //   I moved the Compression logic from here to use in Interfaces
    // }

    // static string Decompress(string compressedText, Dictionary<string, int> dictionary)
    // {
    //   I moved the Decompression logic from here to use in Interfaces
    // }
  }
}
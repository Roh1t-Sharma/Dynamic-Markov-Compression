// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Text;
// using System.Threading.Tasks;
// using System.Linq;
//
// interface ICompression
// {
//     string Compress(string text, Dictionary<string, int> dictionary);
//     string Decompress(string compressedText, Dictionary<string, int> dictionary);
// }
//
// class MarkovCompression : ICompression
// {
//     public Dictionary<string, int> BuildDictionary(string text)
//     {
//         Dictionary<string, int> dictionary = new Dictionary<string, int>();
//         int index = 0;
//
//         foreach (char c in text)
//         {
//             string currentChar = c.ToString();
//             if (!dictionary.ContainsKey(currentChar))
//             {
//                 dictionary.Add(currentChar, index++);
//             }
//         }
//
//         return dictionary;
//     }
//
//     public string Compress(string text, Dictionary<string, int> dictionary)
//     {
//         StringBuilder compressed = new StringBuilder();
//         string currentString = "";
//
//         foreach (char c in text)
//         {
//             string currentChar = c.ToString();
//
//             if (dictionary.ContainsKey(currentString + currentChar))
//             {
//                 currentString += currentChar;
//             }
//             else
//             {
//                 compressed.Append(dictionary[currentString] + " ");
//
//                 dictionary.Add(currentString + currentChar, dictionary.Count);
//                 currentString = currentChar;
//             }
//         }
//
//         if (currentString != "")
//         {
//             compressed.Append(dictionary[currentString]);
//         }
//
//         return compressed.ToString();
//     }
//
//     public string Decompress(string compressedText, Dictionary<string, int> dictionary)
//     {
//         StringBuilder decompressed = new StringBuilder();
//         List<string> reverseLookup = new List<string>(dictionary.Keys);
//
//         string[] entries = compressedText.Split(" ");
//
//         string currentEntry = reverseLookup[int.Parse(entries[0])];
//         decompressed.Append(currentEntry);
//
//         foreach (string entry in entries.Skip(1))
//         {
//             int currentCode = int.Parse(entry);
//             string newEntry;
//
//             if (dictionary.ContainsKey(currentEntry + currentCode))
//             {
//                 newEntry = reverseLookup[dictionary[currentEntry + currentCode]];
//             }
//             else
//             {
//                 newEntry = reverseLookup[currentCode];
//
//                 int newIndex = dictionary.Count;
//                 dictionary.Add(currentEntry + currentCode, newIndex);
//                 reverseLookup.Insert(newIndex, currentEntry + newEntry[0]);
//             }
//
//             decompressed.Append(newEntry);
//             currentEntry = newEntry;
//         }
//
//         return decompressed.ToString();
//     }
// }
//
// class Program
// {
//     static async Task Main(string[] args)
//     {
//         string originalText = GetInputText("Enter text to compress: ");
//
//         Dictionary<string, int>? dictionary = null;
//
//         try
//         {
//             dictionary = await Task.Run(() => new MarkovCompression().BuildDictionary(originalText));
//         }
//         catch (Exception ex)
//         {
//             Console.WriteLine($"Error building dictionary: {ex.Message}");
//             return;
//         }
//
//         string compressedText = new MarkovCompression().Compress(originalText, dictionary);
//
//         Console.WriteLine("Compressed text: " + compressedText);
//
//         double compressionRate = (double)compressedText.Length / originalText.Length;
//         double compressionRatio = (double)originalText.Length / compressedText.Length;
//
//         Console.WriteLine($"Compression Rate: {compressionRate:P2}");
//         Console.WriteLine($"Compression Ratio: {compressionRatio:F2}");
//         Console.WriteLine($"Size of Compressed Text: {GetByteSize(compressedText)} bytes");
//
//         await SaveToFileAsync(compressedText, "compressed.txt");
//
//         while (true)
//         {
//             Console.WriteLine("\nDo you want to : ");
//             Console.WriteLine("\n1: compress another text? ");
//             Console.WriteLine("\n2: decompress?)");
//             Console.WriteLine("\n3: exit ");
//             string choice = Console.ReadLine().ToLower();
//
//             if (choice == "1")
//             {
//                 Console.WriteLine("\nEnter text to compress (or type 'exit' to go back): ");
//                 string newText = Console.ReadLine();
//
//                 if (newText.ToLower() == "exit")
//                     break;
//
//                 try
//                 {
//                     dictionary = await Task.Run(() => new MarkovCompression().BuildDictionary(newText));
//                 }
//                 catch (Exception ex)
//                 {
//                     Console.WriteLine($"Error building dictionary: {ex.Message}");
//                     return;
//                 }
//
//                 string newCompressedText = new MarkovCompression().Compress(newText, dictionary);
//
//                 double newCompressionRate = (double)newCompressedText.Length / newText.Length;
//                 double newCompressionRatio = (double)newText.Length / newCompressedText.Length;
//
//                 Console.WriteLine($"Compressed text: {newCompressedText}");
//                 Console.WriteLine($"Compression Rate: {newCompressionRate:P2}");
//                 Console.WriteLine($"Compression Ratio: {newCompressionRatio:F2}");
//                 Console.WriteLine($"Size of Compressed Text: {GetByteSize(newCompressedText)} bytes");
//
//                 await SaveToFileAsync(newCompressedText, "compressed_new.txt");
//             }
//             else if (choice == "2")
//             {
//                 string decompressedText = new MarkovCompression().Decompress(compressedText, dictionary);
//                 Console.WriteLine("Decompressed text: " + decompressedText);
//                 Console.WriteLine($"Size of Decompressed Text: {GetByteSize(decompressedText)} bytes");
//
//                 await SaveToFileAsync(decompressedText, "decompressed.txt");
//             }
//             else if (choice == "3")
//             {
//                 break; // Exit the loop
//             }
//             else
//             {
//                 Console.WriteLine("Invalid choice. Please enter '1' to compress, '2' to decompress, or '3' to exit.");
//             }
//         }
//     }
//
//     static string GetInputText(string prompt)
//     {
//         Console.Write(prompt);
//         return Console.ReadLine();
//     }
//
//     static int GetByteSize(string text)
//     {
//         // Convert the string to bytes using UTF-8 encoding
//         byte[] bytes = Encoding.UTF8.GetBytes(text);
//         return bytes.Length;
//     }
//
//     static async Task SaveToFileAsync(string data, string fileName)
//     {
//         try
//         {
//             await File.WriteAllTextAsync(fileName, data);
//             Console.WriteLine($"Data saved to {fileName}");
//         }
//         catch (Exception ex)
//         {
//             Console.WriteLine($"Error saving data to {fileName}: {ex.Message}");
//         }
//     }
// }

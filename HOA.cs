// using System;
// using System.IO;
// using System.Text;
// using System.Collections.Generic;
// using System.Threading.Tasks; 
// using System.Linq;
//
// namespace MarkovCompression
// {
//     //  delegate for compression and decompression methods
//     delegate string CompressionDelegate(string text, Dictionary<string, int> dictionary);
//
//     class Program
//     {
//         static async Task Main(string[] args) //threads
//         {
//             Console.Write("Enter text to compress: ");
//             string text = Console.ReadLine() ?? throw new InvalidOperationException();
//
//             // Save the original text to a file
//             await SaveToFileAsync("original.txt", text); //threads
//
//             // Measure the size of the original text
//             long originalSize = new FileInfo("original.txt").Length;
//             Console.WriteLine($"Original Size: {originalSize} bytes");
//
//             Dictionary<string, int> dictionary = BuildDictionary(text);
//
//             // Using a compression delegate for flexibility
//             CompressionDelegate compressMethod = Compress;
//             string compressed = await CompressAsync(text, dictionary, compressMethod);
//
//             // Save the compressed text to a file
//             await SaveToFileAsync("compressed.txt", compressed); //threads
//
//             // Measure the size of the compressed text
//             long compressedSize = new FileInfo("compressed.txt").Length;
//             Console.WriteLine($"Compressed Size: {compressedSize} bytes");
//
//             // Calculate Compression Ratio
//             double compressionRatio = (double)originalSize / compressedSize;
//             Console.WriteLine($"Compression Ratio: {compressionRatio}");
//
//             // Calculate Compression Rate
//             double compressionRate = 100 * (1 - compressionRatio);
//             Console.WriteLine($"Compression Rate: {compressionRate}%");
//
//             Console.Write("Decompress? (y/n) ");
//             string? choice = Console.ReadLine()?.ToLower() ?? string.Empty;
//             if (choice == "y")
//             {
//                 // Using a compression delegate for flexibility
//                 CompressionDelegate decompressMethod = Decompress;
//                 string decompressed = await DecompressAsync(compressed, dictionary, decompressMethod); //threads
//                 Console.WriteLine("Decompressed text: " + decompressed);
//
//                 // Save the decompressed text to a file
//                 await SaveToFileAsync("decompressed.txt", decompressed);
//             }
//
//             Console.Write("Compress another text? (y/n) ");
//             choice = Console.ReadLine()?.ToLower();
//             if (choice == "y")
//             {
//                 await Main(args);
//             }
//         }
//
//         // Added an asynchronous version of SaveToFile method
//         static async Task SaveToFileAsync(string fileName, string content)
//         {
//             using (StreamWriter writer = new StreamWriter(fileName))
//             {
//                 await writer.WriteAsync(content);
//             }
//             Console.WriteLine($"Text saved to {fileName}");
//         }
//
//         // Added an asynchronous version of Compress method
//         static async Task<string> CompressAsync(string text, Dictionary<string, int> dictionary, CompressionDelegate compressMethod)
//         {
//             return await Task.Run(() => compressMethod(text, dictionary));
//         }
//
//         // Added an asynchronous version of Decompress method
//         static async Task<string> DecompressAsync(string compressed, Dictionary<string, int> dictionary, CompressionDelegate decompressMethod)
//         {
//             return await Task.Run(() => decompressMethod(compressed, dictionary));
//         }
//
//         static void SaveToFile(string fileName, string content)
//         {
//             using (StreamWriter writer = new StreamWriter(fileName))
//             {
//                 writer.Write(content);
//             }
//             Console.WriteLine($"Text saved to {fileName}");
//         }
//
//         static Dictionary<string, int> BuildDictionary(string text)
//         {
//             Dictionary<string, int> dictionary = new Dictionary<string, int>();
//             int index = 0;
//             foreach (char c in text)
//             {
//                 string currentChar = c.ToString();
//                 if (!dictionary.ContainsKey(currentChar))
//                 {
//                     dictionary.Add(currentChar, index++);
//                 }
//             }
//
//             return dictionary;
//         }
//
//         static string Compress(string text, Dictionary<string, int> dictionary)
//         {
//             StringBuilder compressed = new StringBuilder();
//             string currentSubstring = "";
//
//             foreach (char c in text)
//             {
//                 string currentChar = c.ToString();
//                 string newSubstring = currentSubstring + currentChar;
//
//                 if (dictionary.ContainsKey(newSubstring))
//                 {
//                     currentSubstring = newSubstring;
//                 }
//                 else
//                 {
//                     compressed.Append(dictionary[currentSubstring]);
//                     dictionary.Add(newSubstring, dictionary.Count);
//                     currentSubstring = currentChar;
//                 }
//             }
//
//             if (!string.IsNullOrEmpty(currentSubstring))
//             {
//                 compressed.Append(dictionary[currentSubstring]);
//             }
//
//             return compressed.ToString();
//         }
//
//         static string Decompress(string compressed, Dictionary<string, int> dictionary)
//         {
//             StringBuilder decompressed = new StringBuilder();
//             
//             // using LINQ to convert the keys of the dictionary into a list using LINQ.
//             List<string> reverseDictionary = new List<string>(dictionary.Keys); 
//             
//
//             int currentCode = int.Parse(compressed[0].ToString());
//             string currentSubstring = reverseDictionary[currentCode];
//
//             decompressed.Append(currentSubstring);
//
//             for (int i = 1; i < compressed.Length; i++)
//             {
//                 var nextCode = int.Parse(compressed[i].ToString());
//
//                 if (dictionary.TryGetValue(currentCode.ToString() + nextCode.ToString(), out int value))
//                 {
//                     currentSubstring = reverseDictionary[value];
//                 }
//                 else
//                 {
//                     currentSubstring = reverseDictionary[nextCode];
//                 }
//
//                 decompressed.Append(currentSubstring);
//
//                 // Handle the "An item with the same key has already been added" error
//                 if (!dictionary.ContainsKey(currentCode.ToString() + currentSubstring[0]))
//                 {
//                     dictionary.Add(currentCode.ToString() + currentSubstring[0], dictionary.Count);
//                 }
//
//                 currentCode = nextCode;
//             }
//
//             return decompressed.ToString();
//         }
//     }
// }

//
// using System;
// using System.IO;
// using System.Text;
// using System.Collections.Generic;
// using System.Diagnostics;
// using System.Threading.Tasks;
// using System.Linq;
//
// namespace BinaryCompression
// {
//
//     // Define an interface for compression and decompression
//     public interface ICompressor
//     {
//         string? Compress(string? text, Dictionary<char, string> dict);
//         string? Decompress(string? compressed, Dictionary<char, string> dict);
//         Dictionary<char, string> BuildDictionary(string? originalText);
//     }
//
//     // Implement the interface in a class
//     public class BinaryCompressor : ICompressor
//     {
//
//         public Dictionary<char, string> BuildDictionary(string? text)
//         {
//             Dictionary<char, string> dict = new Dictionary<char, string>();
//
//             if (text != null)
//                 foreach (char c in text)
//                 {
//                     if (!dict.ContainsKey(c))
//                     {
//                         dict.Add(c, Convert.ToString(dict.Count, 2).PadLeft(5, '0'));
//                     }
//                 }
//
//             return dict;
//         }
//
//         public string Compress(string? text, Dictionary<char, string> dict)
//         {
//             StringBuilder compressed = new StringBuilder();
//
//             if (text != null)
//                 foreach (char c in text)
//                 {
//                     compressed.Append(dict[c]);
//                 }
//
//             return compressed.ToString();
//         }
//
//         public string Decompress(string? compressed, Dictionary<char, string> dict)
//         {
//             StringBuilder decompressed = new StringBuilder();
//
//             if (compressed != null)
//                 for (int i = 0; i < compressed.Length; i += 5)
//                 {
//                     string code = compressed.Substring(i, 5);
//                     int index = Convert.ToInt32(code, 2);
//                     decompressed.Append(dict.ElementAt(index).Key);
//                 }
//
//             return decompressed.ToString();
//         }
//     }
//
//     class Program
//     {
//
//         static async Task Main(string[] args)
//         {
//             try
//             {
//                 Console.Write("Enter text to compress: ");
//                 string? originalText = Console.ReadLine();
//
//                 Stopwatch stopwatch = new Stopwatch();
//
//                 // Compression
//                 stopwatch.Start();
//                 ICompressor compressor = new BinaryCompressor();
//                 Dictionary<char, string> dict = compressor.BuildDictionary(originalText);
//                 string? compressedText = compressor.Compress(originalText, dict);
//                 stopwatch.Stop();
//
//                 double compressionRatio = CalculateCompressionRatio(originalText, compressedText);
//                 if (originalText != null)
//                 {
//                     double compressionSpeed = CalculateCompressionSpeed(stopwatch.ElapsedMilliseconds, originalText.Length);
//
//                     Console.WriteLine("Compressed text: " + compressedText);
//                     Console.WriteLine($"Compression Ratio: {compressionRatio:P2}");
//                     Console.WriteLine($"Compression Speed: {compressionSpeed} characters/ms");
//                 }
//
//                 await SaveToFileAsync("original.txt", originalText);
//                 await SaveToFileAsync("compressed.txt", compressedText);
//
//                 Console.Write("Decompress? (y/n) ");
//                 string? choice = Console.ReadLine()?.ToLower();
//
//                 if (choice == "y")
//                 {
//                     stopwatch.Restart();
//
//                     // Decompression
//                     string? decompressedText = compressor.Decompress(compressedText, dict);
//
//                     stopwatch.Stop();
//
//                     Console.WriteLine("Decompressed text: " + decompressedText);
//                     if (compressedText != null)
//                         Console.WriteLine(
//                             $"Decompression Speed: {CalculateCompressionSpeed(stopwatch.ElapsedMilliseconds, compressedText.Length)} characters/ms");
//
//                     await SaveToFileAsync("decompressed.txt", decompressedText);
//                 }
//
//                 Console.Write("Compress another text? (y/n) ");
//                 choice = Console.ReadLine()?.ToLower();
//
//                 if (choice == "y")
//                 {
//                     await Main(args);
//                 }
//             }
//             catch (Exception ex)
//             {
//                 Console.WriteLine($"An error occurred: {ex.Message}");
//             }
//         }
//
//         static double CalculateCompressionRatio(string? original, string? compressed)
//         {
//             Debug.Assert(original != null, nameof(original) + " != null");
//             double originalSize = Encoding.UTF8.GetByteCount(original);
//             Debug.Assert(compressed != null, nameof(compressed) + " != null");
//             double compressedSize = compressed.Length / 8.0; // Assuming 8 bits per character
//
//             return compressedSize / originalSize;
//         }
//
//         static double CalculateCompressionSpeed(long elapsedTime, int processedSize)
//         {
//             return processedSize / (double)elapsedTime;
//         }
//
//         static async Task SaveToFileAsync(string fileName, string? content)
//         {
//             using (FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
//             {
//                 using (StreamWriter writer = new StreamWriter(fileStream))
//                 {
//                     await writer.WriteAsync(content);
//                 }
//             }
//
//             if (content != null)
//                 Console.WriteLine($"Saved {fileName} with size: {Encoding.UTF8.GetByteCount(content)} bytes");
//         }
//     }
// }

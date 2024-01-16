// //     This one is WORKING, using LZW algorithm method.
//
// namespace DMC;
//
// class DmcBinaryAlphabets
// {
//     static void Main()
//     {
//         while (true)
//         {
//             Console.WriteLine("Enter the original text to compress (type 'exit' to end):");
//             string? originalText = Console.ReadLine();
//             if (originalText?.ToLower() == "exit")
//             {
//                 break;
//             }
//         
//             Console.WriteLine($"Original Text: {originalText}");
//
//             // for compression
//             if (originalText != null)
//             {
//                 string compressedText = Compress(originalText);
//                 Console.WriteLine($"Compressed Text: {compressedText}");
//
//                 // for decompression
//                 string decompressedText = Decompress(compressedText);
//                 Console.WriteLine($"Decompressed Text: {decompressedText}\n");
//             }   
//         }
//     }
//
//     static string Compress(string? input)
//     {
//         Dictionary<string, int> dictionary = new Dictionary<string, int>();
//         for (int i = 0; i < 256; i++)
//         {
//             dictionary.Add(((char)i).ToString(), i);
//         }
//
//         string current = "";
//         List<string> result = new List<string>();
//         if (input != null)
//             foreach (char c in input)
//             {
//                 string combined = current + c;
//                 if (dictionary.ContainsKey(combined))
//                 {
//                     current = combined;
//                 }
//                 else
//                 {
//                     result.Add(Convert.ToString(dictionary[current], 2).PadLeft(8, '0'));
//                     dictionary.Add(combined, dictionary.Count);
//                     current = c.ToString();
//                 }
//             }
//
//         if (!string.IsNullOrEmpty(current))
//         {
//             result.Add(Convert.ToString(dictionary[current], 2).PadLeft(8, '0'));
//         }
//
//         return string.Join("", result);
//     }
//
//     static string Decompress(string input)
//     {
//         Dictionary<int, string> dictionary = new Dictionary<int, string>();
//         for (int i = 0; i < 256; i++)
//         {
//             dictionary.Add(i, ((char)i).ToString());
//         }
//
//         List<string> result = new List<string>();
//
//         if (input.Length >= 8) // Check if there are at least 8 characters in the input string
//         {
//             int currentCode = Convert.ToInt32(input.Substring(0, 8), 2);
//             result.Add(dictionary[currentCode]);
//
//             for (int i = 8; i < input.Length; i += 8)
//             {
//                 if (i + 8 <= input.Length) // Check if there are at least 8 characters remaining in the input string
//                 {
//                     int code = Convert.ToInt32(input.Substring(i, 8), 2);
//
//                     if (dictionary.ContainsKey(code))
//                     {
//                         result.Add(dictionary[code]);
//                         dictionary.Add(dictionary.Count, dictionary[currentCode] + dictionary[code][0]);
//                     }
//                     else
//                     {
//                         string entry = dictionary[currentCode] + dictionary[currentCode][0];
//                         result.Add(entry);
//                         dictionary.Add(code, entry);
//                     }
//
//                     currentCode = code;
//                 }
//                 else
//                 {
//                     // Handle the case where there are less than 8 characters remaining in the input string
//                     // You may choose to throw an exception or handle it accordingly
//                     Console.WriteLine("Invalid input length for decompression.");
//                     break;
//                 }
//             }
//         }
//
//         return string.Join("", result);
//     }
// }
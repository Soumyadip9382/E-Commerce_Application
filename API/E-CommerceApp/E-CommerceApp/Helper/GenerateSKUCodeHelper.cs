using E_CommerceApp.Domain.DTOs;
using E_CommerceApp.Domain.Models;
using static E_CommerceApp.Domain.DTOs.ProductDTO;
using static E_CommerceApp.Domain.DTOs.VariantDTO;

namespace E_CommerceApp.Helper
{
    public class GenerateSKUCodeHelper
    {
        public static class SkuGenerator 
        {
            private static string GetLastCategoryCode(string categoryPath)
            {
                var last = categoryPath
                    .Split('>', StringSplitOptions.RemoveEmptyEntries)
                    .Last()
                    .Trim();

                return GetCode(last, 4);
            }

            // 🔹 Generic code generator (letters + numbers)
            private static string GetCode(string input, int length)
            {
                if (string.IsNullOrWhiteSpace(input)) return "NA";

                var clean = new string(input
                    .Where(char.IsLetterOrDigit)
                    .ToArray())
                    .ToUpper();

                return clean.Length <= length
                    ? clean
                    : clean.Substring(0, length);
            }

            // 🔹 Product code (e.g. iPhone 15 → IP15)
            private static string GenerateProductCode(string name)
            {
                var words = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                string code = "";

                foreach (var word in words)
                {
                    if (char.IsLetter(word[0]))
                        code += char.ToUpper(word[0]);

                    if (word.Any(char.IsDigit))
                        code += new string(word.Where(char.IsDigit).ToArray());
                }

                return code.Length > 10 ? code.Substring(0, 10) : code;
            }

            // 🔹 Normalize size/storage
            private static string NormalizeSize(string size)
            {
                if (string.IsNullOrWhiteSpace(size)) return "NA";

                size = size.ToUpper().Trim();

                return size switch
                {
                    "SMALL" => "S",
                    "MEDIUM" => "M",
                    "LARGE" => "L",
                    _ => size.Replace("GB", "G").Replace(" ", "")
                };
            }




            public static string GenerateSKU(
        string productName,
        string brand,
        string categoryPath,
        string color,
        string size)
            {
                string categoryCode = GetLastCategoryCode(categoryPath);
                string brandCode = GetCode(brand, 3);
                string productCode = GenerateProductCode(productName);

                string colorCode = $"C{GetCode(color, 3)}";
                string sizeCode = $"S{NormalizeSize(size)}";
                string unique = GenerateHash($"{productName}-{color}-{size}");


                return $"{categoryCode}-{brandCode}-{productCode}-{colorCode}-{sizeCode}-{unique}";
            }

            private static string GenerateHash(string input)
            {
                using var md5 = System.Security.Cryptography.MD5.Create();
                var bytes = System.Text.Encoding.UTF8.GetBytes(input);
                var hash = md5.ComputeHash(bytes);

                return BitConverter.ToString(hash)
                    .Replace("-", "")
                    .Substring(0, 6); // short & readable
            }
        }
    }
}

using System.Security.Cryptography;
using System.Text;

namespace ShopperAPi.Helpers
{
    public static class StringExtentions
    {
        public static string hashStrings(this string firstString, string secondString)
        {
            var input = firstString + secondString;
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] data = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                var sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string.
                return sBuilder.ToString();
            }
        }

    }
}

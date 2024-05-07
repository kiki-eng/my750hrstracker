using System.Security.Cryptography;
using System.Text;
using BC = BCrypt.Net.BCrypt;
namespace _750HrsTracker.Helpers
{
    public class Encryption
    {
        public static string HashPassword(string toHash)
        {

            return BC.HashPassword(toHash);
        }

        public static bool CompareHashedPassword(string toCompare, string hashed)
        {

            bool verified = BC.Verify(toCompare, hashed);
            //string newHash = hashPassword(toCompare);
            if (verified)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string SHA256(string value)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = System.Security.Cryptography.SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }

        public static string EncryptDecryptAES(string data, string encryptionKey, string encryptionIv, string action = "encrypt")
        {
            try
            {

                byte[] encryptionKeyByte = Encoding.UTF8.GetBytes(encryptionKey);
                byte[] encryptionIvByte = Encoding.UTF8.GetBytes(encryptionIv);

                string result = "";

                // Create Aes that generates a new key and initialization vector (IV).    
                // Same key must be used in encryption and decryption    
                using (Aes aes = Aes.Create())
                {
                    aes.Key = encryptionKeyByte;
                    aes.IV = encryptionIvByte;

                    if (action.Equals("encrypt"))
                    {
                        byte[] array;

                        ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                                {
                                    streamWriter.Write(data);
                                }

                                array = memoryStream.ToArray();
                            }
                        }

                        result = Convert.ToBase64String(array);
                    }
                    else if (action == "decrypt")
                    {
                        byte[] iv = new byte[16];
                        byte[] buffer = Convert.FromBase64String(data);

                        ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                        using (MemoryStream memoryStream = new MemoryStream(buffer))
                        {
                            using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                                {
                                    result = streamReader.ReadToEnd();
                                }
                            }
                        }

                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public static string? Base64EncodeDecode(string data, string action = "encode")
        {
            string? encodedDecoded = null;
            if (action.Equals("encode"))
            {
                var plainTextBytes = Encoding.UTF8.GetBytes(data);
                encodedDecoded = Convert.ToBase64String(plainTextBytes);
            }
            else if (action.Equals("decode"))
            {
                var base64EncodedBytes = Convert.FromBase64String(data);
                encodedDecoded = Encoding.UTF8.GetString(base64EncodedBytes);
            }



            return encodedDecoded;
        }
    }
}

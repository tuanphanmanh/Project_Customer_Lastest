using esign.Authorization.Users;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace esign.Security
{
    public class Cryptography
    {
        private static byte[] CombineKey(byte[] key)
        {
            var bytes = Encoding.UTF8.GetBytes("d$24^8SX?9dW/£Jw");
            var byteIndexs = new int[] { 2, 6, 0, 7, 2, 0, 1, 4, 0, 8, 0, 9, 2, 0, 2, 1 };
            byte[] combinationKey = new byte[32];
            for (int i = 0; i < byteIndexs.Length; i++)
            {
                var index = byteIndexs[i];
                combinationKey[i * 2] = key?.Length > index ? key[index] : bytes[index];
                combinationKey[i * 2 + 1] = bytes[index];
            }
            return combinationKey;
        }
        public static Encryption EncryptStringToBytes(string str)
        {
            using (var aes = Aes.Create())
            {
                byte[] encrypted;
                // Setting a key size disposes the previously-set key. 
                // Setting a key size is redundant if a key going to be set after this statement. 
                // According to https://en.wikipedia.org/wiki/Advanced_Encryption_Standard, Supported key sizes are 128, 192 and 256
                aes.KeySize = 128;
                // aes.BlockSize = 128; // According to https://en.wikipedia.org/wiki/Advanced_Encryption_Standard: Block size for AES is always 128
                var key = aes.Key;
                //
                aes.Key = CombineKey(key);
                aes.GenerateIV(); // The get method of the 'IV' property of the 'SymmetricAlgorithm' automatically generates an IV if it is has not been generate before. 
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                using (var msEncrypt = new MemoryStream())
                {
                    msEncrypt.Write(aes.IV, 0, aes.IV.Length);
                    var encoder = aes.CreateEncryptor();
                    using (var csEncrypt = new CryptoStream(msEncrypt, encoder, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(str);
                        }
                    }
                    encrypted = msEncrypt.ToArray();
                }
                return new Encryption()
                {
                    plain = str,
                    key = key,
                    encrypted = encrypted
                };
            }
        }
        public static string DecryptStringFromBytes(Encryption encryption)
        {
            string decrypted;
            using (var aes = Aes.Create())
            {
                // Setting a key size disposes the previously-set key. 
                // Setting a key size will generate a new key. 
                // Setting a key size is redundant if a key going to be set after this statement. 
                // aes.KeySize = 128;                
                aes.Key = CombineKey(encryption.key);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                using (var msDecryptor = new MemoryStream(encryption.encrypted))
                {
                    byte[] readIV = new byte[16];
                    msDecryptor.Read(readIV, 0, 16);
                    aes.IV = readIV;
                    var decoder = aes.CreateDecryptor();
                    using (var csDecryptor = new CryptoStream(msDecryptor, decoder, CryptoStreamMode.Read))
                    {
                        using (var srReader = new StreamReader(csDecryptor))
                        {
                            decrypted = srReader.ReadToEnd();
                        }
                    }
                }
            }
            return decrypted;
        }
        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.ASCII.GetBytes(input);
                var hashBytes = md5.ComputeHash(inputBytes);
                return Convert.ToHexString(hashBytes);
            }
        }
        //        
        public static string GeneratePassword(int length, int numberOfNonAlphanumericCharacters)
        {
            var punctuations = "!@#$%^&*()_-+=[{]};:>|./?".ToCharArray();
            //
            if (length < 1 || length > 128)
            {
                throw new ArgumentException(nameof(length));
            }

            if (numberOfNonAlphanumericCharacters > length || numberOfNonAlphanumericCharacters < 0)
            {
                throw new ArgumentException(nameof(numberOfNonAlphanumericCharacters));
            }

            using (var rng = RandomNumberGenerator.Create())
            {
                var byteBuffer = new byte[length];

                rng.GetBytes(byteBuffer);

                var count = 0;
                var characterBuffer = new char[length];

                for (var iter = 0; iter < length; iter++)
                {
                    var i = byteBuffer[iter] % 87;

                    if (i < 10)
                    {
                        characterBuffer[iter] = (char)('0' + i);
                    }
                    else if (i < 36)
                    {
                        characterBuffer[iter] = (char)('A' + i - 10);
                    }
                    else if (i < 62)
                    {
                        characterBuffer[iter] = (char)('a' + i - 36);
                    }
                    else
                    {
                        characterBuffer[iter] = punctuations[i - 62];
                        count++;
                    }
                }

                if (count >= numberOfNonAlphanumericCharacters)
                {
                    return new string(characterBuffer);
                }

                int j;
                var rand = new Random();

                for (j = 0; j < numberOfNonAlphanumericCharacters - count; j++)
                {
                    int k;
                    do
                    {
                        k = rand.Next(0, length);
                    }
                    while (!char.IsLetterOrDigit(characterBuffer[k]));

                    characterBuffer[k] = punctuations[rand.Next(0, punctuations.Length)];
                }

                return new string(characterBuffer);
            }
        }
        //
        public static Spliterator SplitFromBytes(byte[] bytes)
        {
            var spliterator = new Spliterator();
            if (bytes?.Length > 1)
            {
                var smallSize = 1;
                if (bytes.Length < 3000)
                {
                    smallSize = (int)Math.Round(bytes.Length / 3.0, 0);
                }
                else
                {
                    smallSize = 1024;
                }
                //
                spliterator.small = bytes.Take(smallSize).ToArray();
                spliterator.big = bytes.Skip(smallSize).ToArray();                                
            }
            return spliterator;
        }        
    }

    public class Encryption
    {
        public string plain;
        public byte[] key;
        public byte[] encrypted;
    }
    public class Spliterator
    {
        public byte[] small;
        public byte[] big;
    }
}

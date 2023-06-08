using IPMO.IServices;
using System.Security.Cryptography;
using System.Text;

namespace IPMO.Services
{
    public  class Encrypted : IEncrypted
    {
        private  readonly byte[] EncryptionKey;
        public Encrypted()
        {
            EncryptionKey = GenerateEncryptionKey();
        }


        public string Encrypt(string clearText)
        {
            string encryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                encryptor.Padding = PaddingMode.ISO10126;
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }

            return clearText;
        }


        public string Decrypt(string cipherText)
        {
            string encryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                encryptor.Padding = PaddingMode.ISO10126;
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }

            return cipherText;
        }

        public byte[] EncryptPassword(byte[] clearBytes)
        {
            string encryptionKey = "MAKV2SPBNI99212";
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                encryptor.Padding = PaddingMode.ISO10126;
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    return ms.ToArray();
                }
            }
        }

        public byte[] DecryptPassword(byte[] encryptedBytes)
        {
            string encryptionKey = "MAKV2SPBNI99212";
            byte[] decryptedBytes;

            using (Aes decryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                decryptor.Key = pdb.GetBytes(32);
                decryptor.IV = pdb.GetBytes(16);
                decryptor.Padding = PaddingMode.ISO10126;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(encryptedBytes, 0, encryptedBytes.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }


        /*   public string Decrypt(string encryptedText)
           {
               byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
               string decryptedText;

               using (Aes aes = Aes.Create())
               {
                   aes.Key = EncryptionKey;
                   aes.IV = FixedIV();
                   aes.Padding = PaddingMode.PKCS7;

                   ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                   using (MemoryStream memoryStream = new MemoryStream(encryptedBytes))
                   using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                   using (StreamReader reader = new StreamReader(cryptoStream))
                   {
                       decryptedText = reader.ReadToEnd();
                   }
               }

               return decryptedText;
           }

           public string Encrypt(string plainText)
           {
               byte[] encryptedBytes;

               using (Aes aes = Aes.Create())
               {
                   aes.Key = EncryptionKey;
                   aes.IV = FixedIV();
                   aes.Padding = PaddingMode.PKCS7;

                   ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                   byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

                   using (MemoryStream memoryStream = new MemoryStream())
                   using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                   {
                       cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                       cryptoStream.FlushFinalBlock();

                       encryptedBytes = memoryStream.ToArray();
                   }
               }

               return Convert.ToBase64String(encryptedBytes);
           }
        */

        public byte[] GenerateEncryptionKey()
          {
              byte[] key = new byte[32]; // 256 bits
              using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
              {
                  rng.GetBytes(key);
              }
              return key;
          }

          public byte[] FixedIV()
          {
              // Use a fixed IV. Make sure it's the same for encryption and decryption.
              byte[] iv = new byte[16] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10 };
              return iv;
          }  
      
    }
}
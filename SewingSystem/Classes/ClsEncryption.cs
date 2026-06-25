using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace SewingSystem.Classes
{
    public class ClsEncryption
    {
        //Refrence https://www.csharpstar.com/csharp-encrypt-and-decrypt-data-using-symmetric-key/
        private string passPhrase = "BTechPassphrase";
        private string saltValue = "BTechSaltValue";
        private string hashAlgorithm = "SHA256";
        private string initVector = "!1A3g2D4s9K556g7";
        private int keySize = 256;

        public string DecryptString(string text)
        {
            string plainText = Decrypt(text);
            return plainText;
        }

        public string EncryptString(string text)
        {
            string cipherText = Encrypt(text);
            return cipherText;
        }

        private string Encrypt(string plainText)
        {
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(this.initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(this.saltValue);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            PasswordDeriveBytes password = new PasswordDeriveBytes(this.passPhrase, saltValueBytes, this.hashAlgorithm, 2);
            byte[] keyBytes = password.GetBytes(this.keySize / 8);

            RijndaelManaged symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC };

            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();

            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();

            byte[] cipherTextBytes = memoryStream.ToArray();

            memoryStream.Close();
            cryptoStream.Close();

            return Convert.ToBase64String(cipherTextBytes);
        }

        private string Decrypt(string cipherText)
        {
            string text = null;
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(this.initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(this.saltValue);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

            PasswordDeriveBytes password = new PasswordDeriveBytes(this.passPhrase, saltValueBytes, this.hashAlgorithm, 2);
            byte[] keyBytes = password.GetBytes(this.keySize / 8);

            RijndaelManaged symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC };
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);

            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

            try
            {
                byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                text = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
            }
            catch (Exception ex)
            {
                ClsXtraMssgBox.ShowWarning("للمتابعة يرجى شراء النسخه الأصليه! \n للتواصل الإتصال على الأرقام التالية: 966500876876");
                Application.Exit();
            }
            memoryStream.Close();
            return text;
        }
    }
}
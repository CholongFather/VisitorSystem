using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace VisitorSystem.Util
{
    public static class Cipher
    {

        /// <summary>
        /// AES256 Key
        /// </summary>
        public static string key = "abcdefghijklmnopqrstuvwxyz123456";

        /// <summary>
        /// AES256 암호화
        /// </summary>
        /// <param name="Input">평문</param>
        /// <returns></returns>
        public static string AES_encrypt(string Input)
        {
            RijndaelManaged aes = new RijndaelManaged();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] xBuff = null;
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
                {
                    byte[] xXml = Encoding.UTF8.GetBytes(Input);
                    cs.Write(xXml, 0, xXml.Length);
                }

                xBuff = ms.ToArray();
            }

            string Output = Convert.ToBase64String(xBuff);
            return Output;
        }

        /// <summary>
        /// AES 256 복호화
        /// </summary>
        /// <param name="Input">암호화된 Text</param>
        /// <returns></returns>
        public static string AES_decrypt(string Input)
        {
            RijndaelManaged aes = new RijndaelManaged();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            var decrypt = aes.CreateDecryptor();
            byte[] xBuff = null;
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
                {
                    byte[] xXml = Convert.FromBase64String(Input);
                    cs.Write(xXml, 0, xXml.Length);
                }

                xBuff = ms.ToArray();
            }

            string Output = Encoding.UTF8.GetString(xBuff);
            return Output;
        }

        /// <summary>
        /// 단방향 해시 SHA256 암호화
        /// </summary>
        /// <param name="phrase">평문</param>
        /// <returns></returns>
        public static string EncryptSHA256(string phrase)
        {
            Encoding encoder = Encoding.GetEncoding("utf-8");

            SHA256CryptoServiceProvider sha256hasher = new SHA256CryptoServiceProvider();
            byte[] hashedDataBytes = sha256hasher.ComputeHash(encoder.GetBytes(phrase));

            string hashString = string.Empty;

            foreach (byte x in hashedDataBytes)
            {
                hashString += String.Format("{0:x2}", x);
            }


            return Convert.ToBase64String(encoder.GetBytes(hashString));
        }
    }
}
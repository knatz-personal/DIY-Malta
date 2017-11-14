using System;
using System.IO;
using System.Security.Cryptography;

namespace Common.Utilities
{
    public class Encryption
    {
        /// <summary>
        ///     The inverted Value
        /// </summary>
        private static readonly byte[] IV_192 =
        {
            1, 2, 3, 4, 5, 12, 13, 14,
            13, 14, 15, 13, 17, 21, 22, 23,
            24, 25, 121, 122, 122, 123, 124, 124
        };

        /// <summary>
        ///     The encrption key
        /// </summary>
        private static readonly byte[] KEY_192 =
        {
            111, 21, 12, 65, 21, 12, 2, 1,
            5, 30, 34, 78, 98, 1, 32, 122,
            123, 124, 125, 126, 212, 212, 213, 214
        };

        /// <summary>
        ///     Decrypts triple DES.
        /// </summary>
        /// <param name="encryptedString">The encrypted String.</param>
        /// <returns></returns>
        public static String DecryptTripleDES(String encryptedString)
        {
            //encrypt
            if (encryptedString != "")
            {
                //if
                var cryptoprovider = new TripleDESCryptoServiceProvider();
                Byte[] buffer = Convert.FromBase64String(encryptedString);
                var ms = new MemoryStream(buffer);
                var cs = new CryptoStream(ms, cryptoprovider.CreateDecryptor(KEY_192, IV_192), CryptoStreamMode.Read);
                var sw = new StreamReader(cs);
                return sw.ReadToEnd();
            } //if
            return "";
        } //decrypt

        /// <summary>
        ///     Encrypts using triple DES.
        /// </summary>
        /// <param name="unEncryptedString">The unencrypted String.</param>
        /// <returns></returns>
        public static String EncryptTripleDES(String unEncryptedString)
        {
            //encrypt
            if (unEncryptedString != "")
            {
                //if
                var cryptoprovider = new TripleDESCryptoServiceProvider();
                var ms = new MemoryStream();
                var cs = new CryptoStream(ms, cryptoprovider.CreateEncryptor(KEY_192, IV_192), CryptoStreamMode.Write);
                var sw = new StreamWriter(cs);
                sw.Write(unEncryptedString);
                sw.Flush();
                cs.FlushFinalBlock();
                ms.Flush();
                return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
            } //if
            return "";
        } //encrypt
    }
}
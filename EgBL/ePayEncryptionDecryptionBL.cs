using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Data;

namespace EgBL
{
    public class ePayEncryptionDecryptionBL
    {

        //public static void Main(string[] args)
        //{

        //    string str1 = "mypassword";

        //    Console.WriteLine("Original Text: {0}", str1);

        //    int i = 128;

        //    string str2 = GenerateKey(i);

        //    Console.WriteLine("Using complete key \'{0}\'", str2);

        //    string str3 = Encrypt(str1, str2, i);

        //    Console.WriteLine("Encrypted Text: {0}", str3);

        //    string str4 = Decrypt(str3, str2, i);

        //    Console.WriteLine("Decrypted Text: {0}", str4);

        //    Console.Read();

        //}



        public  string GenerateKey(int keySize)
        {

            RijndaelManaged rijndaelManaged = new RijndaelManaged();

            rijndaelManaged.KeySize = keySize;

            rijndaelManaged.BlockSize = 128;

            rijndaelManaged.Mode = CipherMode.ECB;

            rijndaelManaged.Padding = PaddingMode.PKCS7;

            rijndaelManaged.GenerateKey();

            string str2 = Convert.ToBase64String(rijndaelManaged.Key);

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(str2));

        }



        public  string Encrypt(string plainStr, string completeEncodedKey, int keySize)
        {

            RijndaelManaged rijndaelManaged = new RijndaelManaged();

            rijndaelManaged.KeySize = keySize;

            rijndaelManaged.BlockSize = 128;

            rijndaelManaged.Mode = CipherMode.ECB;

            rijndaelManaged.Padding = PaddingMode.PKCS7;

            rijndaelManaged.Key = Convert.FromBase64String(completeEncodedKey.Replace(" ", "+"));

            byte[] bs1 = Encoding.UTF8.GetBytes(plainStr);

            return Convert.ToBase64String(rijndaelManaged.CreateEncryptor().TransformFinalBlock(bs1, 0, (int)bs1.Length));

        }



        public  string Decrypt(string encryptedText, string completeEncodedKey, int keySize)
        {

            RijndaelManaged rijndaelManaged = new RijndaelManaged();

            rijndaelManaged.KeySize = keySize;

            rijndaelManaged.BlockSize = 128;

            rijndaelManaged.Mode = CipherMode.ECB;

            rijndaelManaged.Padding = PaddingMode.PKCS7;

            rijndaelManaged.Key = Convert.FromBase64String(completeEncodedKey.Replace(" ", "+"));

            ICryptoTransform iCryptoTransform = rijndaelManaged.CreateDecryptor();

            byte[] bs = Convert.FromBase64CharArray(encryptedText.Replace(" ", "+").ToCharArray(), 0, encryptedText.Length);

            return Encoding.UTF8.GetString(iCryptoTransform.TransformFinalBlock(bs, 0, (int)bs.Length));

        }

    }

}

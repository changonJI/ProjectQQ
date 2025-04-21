using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace QQ
{
    public static class TableData
    {
        private static string key = "ProjectQ";
        private static string fixedKey = key.PadLeft(16,'0');
        private static string iv = "Projectq";
        private static string fixedIv = iv.PadLeft(16, '0');

        public static string path = StringBuilderPool.Get(Application.dataPath, "/Resources/Table/");
        public static void SaveData(string saveFileName, string str_Data)
        {
            string _path = StringBuilderPool.Get(path, saveFileName,".csv");

            File.WriteAllText(_path, Encrypt(str_Data));

#if (UNITY_EDITOR)
            Debug.Log($"CSV File Saved at : {_path}");

            AssetDatabase.Refresh();
#endif
        }

        public static string LoadData(string loadFileName)
        {
            string _path = StringBuilderPool.Get(path, loadFileName, ".csv");

            string csvData = File.ReadAllText(_path);

#if (UNITY_EDITOR)
            Debug.Log($"CSV File load at : {_path}");
#endif
            return Decrypt(csvData);
        }

        public static string Encrypt(string data)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(fixedKey.Substring(0,16));
            byte[] ivBytes = Encoding.UTF8.GetBytes(fixedIv.Substring(0,16));

            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.IV = ivBytes;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                byte[] encrypted = encryptor.TransformFinalBlock(Encoding.UTF8.GetBytes(data), 0, data.Length);

                return System.Convert.ToBase64String(encrypted);
            }
        }

        public static string Decrypt(string data)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(fixedKey.Substring(0, 16));
            byte[] ivBytes = Encoding.UTF8.GetBytes(fixedIv.Substring(0, 16));

            byte[] buffer = System.Convert.FromBase64String(data);

            using(Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.IV = ivBytes;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                byte[] decrypted = decryptor.TransformFinalBlock(buffer, 0, buffer.Length);

                return Encoding.UTF8.GetString(decrypted);
            }
        }
    }
}
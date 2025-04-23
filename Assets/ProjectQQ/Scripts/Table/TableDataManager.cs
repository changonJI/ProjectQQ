using Cysharp.Threading.Tasks;
using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace QQ
{
    public static class TableDataManager
    {
        private static string key = "ProjectQ";
        private static string fixedKey = key.PadLeft(16,'0');
        private static string iv = "Projectq";
        private static string fixedIv = iv.PadLeft(16, '0');

        public static string path = StringBuilderPool.Get(Application.dataPath, "/Resources/Table/");

        /// <summary>
        /// CSV 파일을 암호화 하여 저장합니다.
        /// </summary>
        /// <param name="saveFileName">nameof(ClassName)</param>
        /// <param name="str_Data">구글시트에서 받아온 데이터 string 값</param>
        public static void SaveData(string saveFileName, string str_Data)
        {
            string _path = StringBuilderPool.Get(path, saveFileName,".csv");

            File.WriteAllText(_path, Encrypt(str_Data));

#if (UNITY_EDITOR)
            Debug.Log($"CSV File Saved at : {_path}");

            AssetDatabase.Refresh();
#endif
        }

        /// <summary>
        /// CSV 파일을 복호화 하여 불러옵니다.
        /// </summary>
        /// <param name="loadFileName">nameof(ClassName)</param>
        /// <returns></returns>
        public static string LoadData(string loadFileName)
        {
            string _path = StringBuilderPool.Get(path, loadFileName, ".csv");

            string csvData = File.ReadAllText(_path);

#if (UNITY_EDITOR)
            Debug.Log($"CSV File load at : {_path}");
#endif
            return Decrypt(csvData);
        }

        public static async UniTaskVoid LoadTableData()
        {
            var localTable = Assembly.GetAssembly(typeof(IDataManager));

            foreach (var type in localTable.GetTypes())
            {
                if (type.IsClass && type.GetInterface("IDataManager") != null)
                {
                    Type baseType = type.BaseType;

                    if (baseType != null)
                    {
                        var property = baseType.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static);
                        if (property != null)
                        {
                            IDataManager instance = (IDataManager)property.GetValue(null);
                            instance.LoadData();
                        }
                    }
                }
            }

            await UniTask.Yield();
        }

        /// <summary>
        /// 복사해온 URL 에서 edit?usp=sharing 제거후 제거된 부분에 export?format=tsv&range=시트 범위&gid=시트ID값
        /// </summary>
        /// <param name="address">https:// ~ /</param>
        /// <param name="range">구글 시트 Range</param>
        /// <param name="sheetID">시트 고유 ID</param>
        /// <returns></returns>
        public static string GetGoogleSheetAddress(string address, string range, string sheetID)
        {
            if (address.Contains("edit?usp=sharing"))
            {
                return address.Replace("edit?usp=sharing", $"export?format=tsv&range={range}&gid={sheetID}");
            }
            else
                return StringBuilderPool.Get(address, "/export?format=tsv&range={range}&gid={sheetID}");
        }

        #region Encrypt / Decrypt
        private static string Encrypt(string data)
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

        private static string Decrypt(string data)
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
        #endregion
    }
}
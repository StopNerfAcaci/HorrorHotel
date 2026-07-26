using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
#endif

namespace _Scripts
{
#if UNITY_EDITOR
    public class LevelLoaderTest : IPreprocessBuildWithReport
#else
    public class LevelLoaderTest
#endif
    {
        // Key và IV phải đúng 16 ký tự (AES-128)
        private const string Key = "HorrorHotelKey16";
        private const string IV  = "HorrorHotelIV128";

        // ─── Mã hóa ───────────────────────────────────────────────────────────

        /// <summary>Mã hóa chuỗi JSON, trả về chuỗi Base64</summary>
        public static string Encrypt(string plainText)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(Key);
            byte[] ivBytes  = Encoding.UTF8.GetBytes(IV);

            using var aes       = Aes.Create();
            aes.Key = keyBytes;
            aes.IV  = ivBytes;

            using var encryptor = aes.CreateEncryptor();
            using var ms        = new MemoryStream();
            using var cs        = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
            using var sw        = new StreamWriter(cs);

            sw.Write(plainText);
            sw.Close();

            return Convert.ToBase64String(ms.ToArray());
        }

        // ─── Giải mã ──────────────────────────────────────────────────────────

        /// <summary>Giải mã chuỗi Base64 về JSON gốc</summary>
        public static string Decrypt(string cipherText)
        {
            byte[] keyBytes    = Encoding.UTF8.GetBytes(Key);
            byte[] ivBytes     = Encoding.UTF8.GetBytes(IV);
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            using var aes       = Aes.Create();
            aes.Key = keyBytes;
            aes.IV  = ivBytes;

            using var decryptor = aes.CreateDecryptor();
            using var ms        = new MemoryStream(cipherBytes);
            using var cs        = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr        = new StreamReader(cs);

            return sr.ReadToEnd();
        }

        // ─── File I/O ─────────────────────────────────────────────────────────

        /// <summary>Đọc file đã mã hóa, trả về chuỗi JSON gốc</summary>
        public static string LoadEncryptedJson(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Debug.LogWarning($"[LevelLoaderTest] File không tồn tại: {filePath}");
                return null;
            }

            string cipher = File.ReadAllText(filePath);
            return Decrypt(cipher);
        }

        /// <summary>Lưu chuỗi JSON vào file dưới dạng đã mã hóa</summary>
        public static void SaveEncryptedJson(string filePath, string jsonString)
        {
            string cipher = Encrypt(jsonString);
            File.WriteAllText(filePath, cipher);
            Debug.Log($"[LevelLoaderTest] Đã lưu file mã hóa: {filePath}");
        }

        // ─── Load Level ───────────────────────────────────────────────────────

        /// <summary>Load level từ file JSON mã hóa</summary>
        public void LoadLevel(string levelFilePath = null)
        {
            string path = levelFilePath ?? Path.Combine(Application.persistentDataPath, "level.dat");

#if UNITY_EDITOR
            Debug.Log($"[LevelLoaderTest] Đang load level từ: {path}");
#endif

            string json = LoadEncryptedJson(path);
            if (json == null) return;

            Debug.Log($"[LevelLoaderTest] JSON đã giải mã: {json}");
            // TODO: JsonUtility.FromJson<LevelData>(json) khi có LevelData class
        }

        // ─── Build Callback ───────────────────────────────────────────────────

#if UNITY_EDITOR
        public int callbackOrder { get; }

        public void OnPreprocessBuild(BuildReport report)
        {
            // Có thể dùng để convert/mã hóa asset trước khi build
            Debug.Log("[LevelLoaderTest] OnPreprocessBuild: sẵn sàng mã hóa level data.");
        }
#endif
    }
}

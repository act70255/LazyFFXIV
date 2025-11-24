using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using LazyTFFXIV.Interfaces;

namespace LazyTFFXIV.Services
{
    /// <summary>
    /// 使用 DPAPI 加密儲存敏感資料
    /// </summary>
    public class DpapiSecretStorage : ISecretStorage
    {
        private readonly string _basePath;
        private readonly string _secretKeyPath;
        private readonly string _passwordPath;

        /// <summary>
        /// 初始化 DpapiSecretStorage
        /// </summary>
        /// <param name="basePath">資料儲存目錄路徑</param>
        public DpapiSecretStorage(string basePath)
        {
            _basePath = basePath;
            _secretKeyPath = Path.Combine(basePath, "secretkey.dat");
            _passwordPath = Path.Combine(basePath, "password.dat");

            // 確保目錄存在
            EnsureDirectoryExists();
        }

        /// <summary>
        /// 確保儲存目錄存在
        /// </summary>
        private void EnsureDirectoryExists()
        {
            if (!Directory.Exists(_basePath))
            {
                Directory.CreateDirectory(_basePath);
            }
        }

        /// <summary>
        /// 使用 DPAPI 加密並儲存資料到檔案
        /// </summary>
        private void SaveEncrypted(string filePath, string data)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            byte[] encryptedBytes = ProtectedData.Protect(
                dataBytes,
                null,
                DataProtectionScope.CurrentUser);

            File.WriteAllBytes(filePath, encryptedBytes);
        }

        /// <summary>
        /// 從檔案讀取並使用 DPAPI 解密資料
        /// </summary>
        private string LoadDecrypted(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            byte[] encryptedBytes = File.ReadAllBytes(filePath);
            byte[] decryptedBytes = ProtectedData.Unprotect(
                encryptedBytes,
                null,
                DataProtectionScope.CurrentUser);

            return Encoding.UTF8.GetString(decryptedBytes);
        }

        public void SaveSecretKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("密鑰不可為空", nameof(key));
            }

            SaveEncrypted(_secretKeyPath, key);
        }

        public string LoadSecretKey()
        {
            return LoadDecrypted(_secretKeyPath);
        }

        public bool SecretKeyExists()
        {
            return File.Exists(_secretKeyPath);
        }

        public void SavePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("密碼不可為空", nameof(password));
            }

            SaveEncrypted(_passwordPath, password);
        }

        public string LoadPassword()
        {
            return LoadDecrypted(_passwordPath);
        }

        public bool PasswordExists()
        {
            return File.Exists(_passwordPath);
        }
    }
}

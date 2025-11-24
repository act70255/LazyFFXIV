namespace LazyTFFXIV.Interfaces
{
    /// <summary>
    /// 負責安全的讀寫敏感資料（使用 DPAPI 加密）
    /// </summary>
    public interface ISecretStorage
    {
        /// <summary>
        /// 儲存 TOTP 密鑰
        /// </summary>
        /// <param name="key">Base32 格式的密鑰</param>
        void SaveSecretKey(string key);

        /// <summary>
        /// 讀取 TOTP 密鑰
        /// </summary>
        /// <returns>解密後的密鑰</returns>
        string LoadSecretKey();

        /// <summary>
        /// 檢查密鑰是否存在
        /// </summary>
        /// <returns>true 表示已設定密鑰</returns>
        bool SecretKeyExists();

        /// <summary>
        /// 儲存固定密碼
        /// </summary>
        /// <param name="password">密碼明文</param>
        void SavePassword(string password);

        /// <summary>
        /// 讀取固定密碼
        /// </summary>
        /// <returns>解密後的密碼</returns>
        string LoadPassword();

        /// <summary>
        /// 檢查密碼是否存在
        /// </summary>
        /// <returns>true 表示已設定密碼</returns>
        bool PasswordExists();
    }
}

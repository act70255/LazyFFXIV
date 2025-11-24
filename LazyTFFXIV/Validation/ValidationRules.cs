using System.Text.RegularExpressions;

namespace LazyTFFXIV.Validation
{
    /// <summary>
    /// 輸入驗證常數與方法
    /// </summary>
    public static class ValidationRules
    {
        // SendKeys 延遲時間限制
        public const int MinDelayMs = 50;
        public const int MaxDelayMs = 2000;
        public const int DefaultDelayMs = 300;
        public const int DelayIncrement = 50;

        // Secret Key 長度限制
        public const int MinSecretKeyLength = 16;
        public const int MaxSecretKeyLength = 50;

        // Base32 字元集
        private static readonly Regex Base32Regex = new Regex("^[A-Z2-7]+$", RegexOptions.Compiled);

        /// <summary>
        /// 驗證延遲時間是否在有效範圍內
        /// </summary>
        /// <param name="delayMs">延遲毫秒數</param>
        /// <returns>true 表示有效</returns>
        public static bool IsValidDelay(int delayMs)
        {
            return delayMs >= MinDelayMs && delayMs <= MaxDelayMs;
        }

        /// <summary>
        /// 驗證 Secret Key 格式是否正確（Base32）
        /// </summary>
        /// <param name="secretKey">待驗證的密鑰</param>
        /// <returns>true 表示格式正確</returns>
        public static bool IsValidSecretKey(string secretKey)
        {
            if (string.IsNullOrWhiteSpace(secretKey))
                return false;

            // 移除空格並轉大寫
            string cleaned = secretKey.Replace(" ", "").ToUpperInvariant();

            // 檢查長度
            if (cleaned.Length < MinSecretKeyLength || cleaned.Length > MaxSecretKeyLength)
                return false;

            // 檢查是否為有效的 Base32 字元
            return Base32Regex.IsMatch(cleaned);
        }

        /// <summary>
        /// 驗證應用程式路徑是否有效
        /// </summary>
        /// <param name="path">檔案路徑</param>
        /// <returns>true 表示路徑有效且檔案存在</returns>
        public static bool IsValidAppPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return false;

            // 檢查副檔名
            if (!path.EndsWith(".exe", System.StringComparison.OrdinalIgnoreCase))
                return false;

            // 檢查檔案是否存在
            return System.IO.File.Exists(path);
        }
    }
}

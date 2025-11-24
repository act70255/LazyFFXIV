namespace LazyTFFXIV.Interfaces
{
    /// <summary>
    /// 負責產生驗證碼（TOTP）
    /// </summary>
    public interface IOtpGenerator
    {
        /// <summary>
        /// 根據密鑰產生 TOTP 驗證碼
        /// </summary>
        /// <param name="secretKey">Base32 格式的密鑰</param>
        /// <returns>6 位數的驗證碼</returns>
        string Generate(string secretKey);
    }
}

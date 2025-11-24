using System;
using LazyTFFXIV.Interfaces;
using OtpNet;

namespace LazyTFFXIV.Services
{
    /// <summary>
    /// 使用 Otp.NET 套件產生 TOTP 驗證碼
    /// </summary>
    public class TotpOtpGenerator : IOtpGenerator
    {
        /// <summary>
        /// 根據密鑰產生 6 位數 TOTP 驗證碼
        /// </summary>
        /// <param name="secretKey">Base32 格式的密鑰</param>
        /// <returns>6 位數驗證碼</returns>
        public string Generate(string secretKey)
        {
            if (string.IsNullOrWhiteSpace(secretKey))
            {
                throw new ArgumentException("密鑰不可為空", nameof(secretKey));
            }

            // 清理密鑰（移除空格並轉大寫）
            string cleanedKey = secretKey.Replace(" ", "").ToUpperInvariant();

            // 將 Base32 密鑰轉換為位元組陣列
            byte[] secretBytes = Base32Encoding.ToBytes(cleanedKey);

            // 建立 TOTP 產生器 (SHA-1, 6 位數, 30 秒週期)
            var totp = new Totp(secretBytes, step: 30, mode: OtpHashMode.Sha1, totpSize: 6);

            // 產生驗證碼
            return totp.ComputeTotp();
        }
    }
}

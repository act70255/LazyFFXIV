namespace LazyTFFXIV.Interfaces
{
    /// <summary>
    /// 負責控制外部程式與模擬輸入
    /// </summary>
    public interface IAppAutomator
    {
        /// <summary>
        /// 啟動應用程式並執行自動登入
        /// </summary>
        /// <param name="appPath">應用程式路徑</param>
        /// <param name="password">固定密碼</param>
        /// <param name="otp">一次性驗證碼</param>
        /// <param name="delayMs">每個按鍵之間的延遲毫秒數</param>
        void RunAndLogin(string appPath, string password, string otp, int delayMs);
    }
}

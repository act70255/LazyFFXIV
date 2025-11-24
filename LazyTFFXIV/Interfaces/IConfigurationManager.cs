namespace LazyTFFXIV.Interfaces
{
    /// <summary>
    /// 負責管理非敏感設定資料（明碼儲存）
    /// </summary>
    public interface IConfigurationManager
    {
        /// <summary>
        /// 儲存應用程式路徑
        /// </summary>
        /// <param name="path">目標應用程式的完整路徑</param>
        void SaveAppPath(string path);

        /// <summary>
        /// 讀取應用程式路徑
        /// </summary>
        /// <returns>已儲存的路徑，若未設定則回傳 null 或空字串</returns>
        string LoadAppPath();

        /// <summary>
        /// 檢查路徑是否已設定
        /// </summary>
        /// <returns>true 表示已設定路徑</returns>
        bool AppPathExists();

        /// <summary>
        /// 儲存 SendKeys 延遲時間
        /// </summary>
        /// <param name="delayMs">延遲毫秒數</param>
        void SaveSendKeysDelay(int delayMs);

        /// <summary>
        /// 讀取 SendKeys 延遲時間
        /// </summary>
        /// <returns>延遲毫秒數，預設 300ms</returns>
        int LoadSendKeysDelay();
    }
}

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using LazyTFFXIV.Interfaces;

namespace LazyTFFXIV.Services
{
    /// <summary>
    /// 使用 SendKeys 模擬鍵盤輸入的自動化服務
    /// </summary>
    public class SendKeysAutomator : IAppAutomator
    {
        /// <summary>
        /// 啟動應用程式並執行自動登入
        /// 若應用程式目錄存在 account.txt，則逐行讀取帳號：[帳號] -> [TAB] -> [OTP] -> [ENTER]
        /// 若不存在，則直接輸入：[OTP] -> [ENTER]
        /// </summary>
        /// <param name="appPath">應用程式路徑</param>
        /// <param name="otp">一次性驗證碼</param>
        /// <param name="delayMs">每個按鍵之間的延遲毫秒數</param>
        public void RunAndLogin(string appPath, string otp, int delayMs)
        {
            if (string.IsNullOrWhiteSpace(appPath))
            {
                throw new ArgumentException("應用程式路徑不可為空", nameof(appPath));
            }

            if (string.IsNullOrWhiteSpace(otp))
            {
                throw new ArgumentException("OTP 不可為空", nameof(otp));
            }

            // 啟動目標應用程式
            Process process = Process.Start(appPath);

            if (process == null)
            {
                throw new InvalidOperationException("無法啟動應用程式");
            }

            // 等待應用程式進入可輸入狀態
            process.WaitForInputIdle();

            // 額外等待確保視窗完全載入
            Thread.Sleep(2000 + delayMs);

            // 取得應用程式目錄
            string appDirectory = Path.GetDirectoryName(appPath);
            string currentDirectory = Directory.GetCurrentDirectory();
            string accountFilePath = Path.Combine(currentDirectory, "account.txt");

            // 讀取帳號列表
            string[] inputs = File.Exists(accountFilePath)
                ? File.ReadAllLines(accountFilePath)
                : new string[] { };
            Debug.WriteLine($"讀取到 {inputs.Length} 組資料");
            // 執行多帳號登入
            foreach (string line in inputs)
            {
                Debug.WriteLine(line);
                string account = line.Trim();
                if (!string.IsNullOrEmpty(account))
                {
                    // 輸入帳號
                    OutputTexts(account, delayMs);

                    // 按下 TAB 切換到 OTP 欄位
                    SendKeys.SendWait("{TAB}");
                    Thread.Sleep(delayMs);
                }
            }
            // 執行單一登入（只輸入 OTP）
            OutputTexts(otp, delayMs);

            // 按下 ENTER 送出登入
            SendKeys.SendWait("{ENTER}");
        }

        /// <summary>
        /// 將文字複製到剪貼板並模擬 Ctrl+V 貼上
        /// </summary>
        /// <param name="text">要貼上的文字</param>
        /// <param name="delayMs">延遲毫秒數</param>
        private void OutputTexts(string text, int delayMs)
        {
            Clipboard.SetText(text);
            Thread.Sleep(delayMs);
            SendKeys.SendWait("^v");
            Thread.Sleep(delayMs);
        }
    }
}

using System;
using System.Diagnostics;
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
        /// 輸入序列：[密碼] -> [TAB] -> [OTP] -> [ENTER]
        /// </summary>
        /// <param name="appPath">應用程式路徑</param>
        /// <param name="password">固定密碼</param>
        /// <param name="otp">一次性驗證碼</param>
        /// <param name="delayMs">每個按鍵之間的延遲毫秒數</param>
        public void RunAndLogin(string appPath, string password, string otp, int delayMs)
        {
            if (string.IsNullOrWhiteSpace(appPath))
            {
                throw new ArgumentException("應用程式路徑不可為空", nameof(appPath));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("密碼不可為空", nameof(password));
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

            // 輸入密碼（使用 SendWait 確保輸入完成）
            SendKeys.SendWait(EscapeSpecialCharacters(password));
            Thread.Sleep(delayMs);

            // 按下 TAB 切換到 OTP 欄位
            SendKeys.SendWait("{TAB}");
            Thread.Sleep(delayMs);

            // 輸入 OTP
            SendKeys.SendWait(otp);
            Thread.Sleep(delayMs);

            // 按下 ENTER 送出登入
            SendKeys.SendWait("{ENTER}");
        }

        /// <summary>
        /// 轉義 SendKeys 的特殊字元
        /// SendKeys 中的特殊字元需要用大括號包起來
        /// </summary>
        /// <param name="input">原始輸入字串</param>
        /// <returns>轉義後的字串</returns>
        private string EscapeSpecialCharacters(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            // SendKeys 特殊字元：+ ^ % ~ ( ) { } [ ]
            // 需要用大括號轉義
            var result = new System.Text.StringBuilder();
            foreach (char c in input)
            {
                switch (c)
                {
                    case '+':
                        result.Append("{+}");
                        break;
                    case '^':
                        result.Append("{^}");
                        break;
                    case '%':
                        result.Append("{%}");
                        break;
                    case '~':
                        result.Append("{~}");
                        break;
                    case '(':
                        result.Append("{(}");
                        break;
                    case ')':
                        result.Append("{)}");
                        break;
                    case '{':
                        result.Append("{{}");
                        break;
                    case '}':
                        result.Append("{}}");
                        break;
                    case '[':
                        result.Append("{[}");
                        break;
                    case ']':
                        result.Append("{]}");
                        break;
                    default:
                        result.Append(c);
                        break;
                }
            }
            return result.ToString();
        }
    }
}

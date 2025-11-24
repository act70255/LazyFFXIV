using System;
using System.IO;
using System.Windows.Forms;
using LazyTFFXIV.Forms;
using LazyTFFXIV.Interfaces;
using LazyTFFXIV.Services;

namespace LazyTFFXIV
{
    /// <summary>
    /// 程式進入點 (Composition Root)
    /// 在這裡組裝所有模組，實現依賴注入
    /// </summary>
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 設定檔案路徑 (存放於 %AppData%\LazyTFFXIV\)
            string appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "LazyTFFXIV");

            // 建立服務實例 (Pure DI - 方便未來替換，符合 Open/Closed Principle)
            ISecretStorage secretStorage = new DpapiSecretStorage(appDataPath);
            IConfigurationManager configManager = new FileConfigurationManager(appDataPath);
            IOtpGenerator otpGenerator = new TotpOtpGenerator();
            IAppAutomator automator = new SendKeysAutomator();

            // 建立主視窗並注入依賴
            Application.Run(new MainForm(secretStorage, configManager, otpGenerator, automator));
        }
    }
}

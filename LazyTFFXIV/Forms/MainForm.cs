using System;
using System.Drawing;
using System.Windows.Forms;
using LazyTFFXIV.Interfaces;
using LazyTFFXIV.Theme;
using LazyTFFXIV.Validation;

namespace LazyTFFXIV.Forms
{
    /// <summary>
    /// 主視窗表單
    /// </summary>
    public partial class MainForm : Form
    {
        private readonly ISecretStorage _secretStorage;
        private readonly IConfigurationManager _configManager;
        private readonly IOtpGenerator _otpGenerator;
        private readonly IAppAutomator _automator;

        /// <summary>
        /// 建構子注入所有依賴
        /// </summary>
        public MainForm(
            ISecretStorage secretStorage,
            IConfigurationManager configManager,
            IOtpGenerator otpGenerator,
            IAppAutomator automator)
        {
            InitializeComponent();

            _secretStorage = secretStorage ?? throw new ArgumentNullException(nameof(secretStorage));
            _configManager = configManager ?? throw new ArgumentNullException(nameof(configManager));
            _otpGenerator = otpGenerator ?? throw new ArgumentNullException(nameof(otpGenerator));
            _automator = automator ?? throw new ArgumentNullException(nameof(automator));

            // 初始化主題樣式
            ApplyTheme();

            // 初始化延遲設定
            InitializeDelayControl();

            // 隱藏密碼相關 UI
            HidePasswordControls();

            // 更新 UI 狀態
            UpdateUIStatus();
        }

        /// <summary>
        /// 套用主題顏色
        /// </summary>
        private void ApplyTheme()
        {
            this.BackColor = ThemeColors.FormBackground;

            // 標題樣式
            lblTitle.ForeColor = ThemeColors.TextPrimary;

            // 狀態標籤樣式
            lblSecretKeyStatus.ForeColor = ThemeColors.TextSecondary;
            lblPasswordStatus.ForeColor = ThemeColors.TextSecondary;
            lblAppPathStatus.ForeColor = ThemeColors.TextSecondary;
            lblDelayStatus.ForeColor = ThemeColors.TextSecondary;

            // 按鈕樣式
            ApplyButtonStyle(btnSetSecretKey);
            ApplyButtonStyle(btnSetPassword);
            ApplyButtonStyle(btnSelectApp);

            // 執行按鈕使用主要顏色
            ApplyPrimaryButtonStyle(btnRun);

            // NumericUpDown 樣式
            nudDelay.BackColor = ThemeColors.InputBackground;
            nudDelay.ForeColor = ThemeColors.TextPrimary;
        }

        /// <summary>
        /// 隱藏密碼相關控制項
        /// </summary>
        private void HidePasswordControls()
        {
            lblPasswordStatus.Visible = false;
            pnlPasswordIndicator.Visible = false;
            btnSetPassword.Visible = false;
        }

        /// <summary>
        /// 套用一般按鈕樣式
        /// </summary>
        private void ApplyButtonStyle(Button btn)
        {
            btn.BackColor = ThemeColors.ButtonBackground;
            btn.ForeColor = ThemeColors.ButtonForeground;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.Hand;

            btn.MouseEnter += (s, e) => btn.BackColor = ThemeColors.ButtonBackgroundHover;
            btn.MouseLeave += (s, e) => btn.BackColor = ThemeColors.ButtonBackground;
        }

        /// <summary>
        /// 套用主要按鈕樣式
        /// </summary>
        private void ApplyPrimaryButtonStyle(Button btn)
        {
            btn.BackColor = ThemeColors.PrimaryButtonBackground;
            btn.ForeColor = ThemeColors.ButtonForeground;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.Hand;

            btn.MouseEnter += (s, e) =>
            {
                if (btn.Enabled)
                    btn.BackColor = ThemeColors.PrimaryButtonHover;
            };
            btn.MouseLeave += (s, e) =>
            {
                if (btn.Enabled)
                    btn.BackColor = ThemeColors.PrimaryButtonBackground;
            };
        }

        /// <summary>
        /// 初始化延遲控制項
        /// </summary>
        private void InitializeDelayControl()
        {
            nudDelay.Minimum = ValidationRules.MinDelayMs;
            nudDelay.Maximum = ValidationRules.MaxDelayMs;
            nudDelay.Increment = ValidationRules.DelayIncrement;
            nudDelay.Value = _configManager.LoadSendKeysDelay();
        }

        /// <summary>
        /// 更新畫面狀態指示
        /// </summary>
        private void UpdateUIStatus()
        {
            bool hasKey = _secretStorage.SecretKeyExists();
            bool hasPassword = _secretStorage.PasswordExists();
            bool hasAppPath = _configManager.AppPathExists();

            // 更新狀態指示燈
            UpdateStatusIndicator(pnlSecretKeyIndicator, lblSecretKeyStatus, hasKey, "TOTP 密鑰");

            // 應用程式路徑顯示完整路徑
            if (hasAppPath)
            {
                string path = _configManager.LoadAppPath();
                pnlAppPathIndicator.BackColor = ThemeColors.StatusReady;
                lblAppPathStatus.Text = TruncatePath(path, 35);
            }
            else
            {
                pnlAppPathIndicator.BackColor = ThemeColors.StatusNotSet;
                lblAppPathStatus.Text = "應用程式路徑 - 未設定";
            }

            // 更新執行按鈕狀態
            btnRun.Enabled = hasKey && hasAppPath;
            if (!btnRun.Enabled)
            {
                btnRun.BackColor = ThemeColors.ButtonDisabled;
            }
            else
            {
                btnRun.BackColor = ThemeColors.PrimaryButtonBackground;
            }
        }

        /// <summary>
        /// 更新狀態指示器
        /// </summary>
        private void UpdateStatusIndicator(Panel indicator, Label statusLabel, bool isSet, string itemName)
        {
            if (isSet)
            {
                indicator.BackColor = ThemeColors.StatusReady;
                statusLabel.Text = $"{itemName} - 已設定";
            }
            else
            {
                indicator.BackColor = ThemeColors.StatusNotSet;
                statusLabel.Text = $"{itemName} - 未設定";
            }
        }

        /// <summary>
        /// 截斷過長的路徑顯示
        /// </summary>
        private string TruncatePath(string path, int maxLength)
        {
            if (string.IsNullOrEmpty(path) || path.Length <= maxLength)
                return path;

            return "..." + path.Substring(path.Length - maxLength + 3);
        }

        /// <summary>
        /// 設定密鑰按鈕點擊事件
        /// </summary>
        private void btnSetSecretKey_Click(object sender, EventArgs e)
        {
            using (var dialog = new InputDialog("設定 TOTP 密鑰", "請輸入 Base32 格式的密鑰：", false))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string key = dialog.InputValue.Replace(" ", "").ToUpperInvariant();

                    if (!ValidationRules.IsValidSecretKey(key))
                    {
                        MessageBox.Show(
                            "密鑰格式不正確！\n\n請確認：\n• 使用 Base32 字元 (A-Z, 2-7)\n• 長度為 16-32 字元",
                            "格式錯誤",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        return;
                    }

                    try
                    {
                        _secretStorage.SaveSecretKey(key);
                        UpdateUIStatus();
                        MessageBox.Show("密鑰已儲存！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"儲存失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// 設定密碼按鈕點擊事件
        /// </summary>
        private void btnSetPassword_Click(object sender, EventArgs e)
        {
            using (var dialog = new InputDialog("設定登入密碼", "請輸入固定密碼：", true))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string password = dialog.InputValue;

                    if (string.IsNullOrWhiteSpace(password))
                    {
                        MessageBox.Show("密碼不可為空！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    try
                    {
                        _secretStorage.SavePassword(password);
                        UpdateUIStatus();
                        MessageBox.Show("密碼已儲存！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"儲存失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// 選擇應用程式按鈕點擊事件
        /// </summary>
        private void btnSelectApp_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Title = "選擇目標應用程式";
                dialog.Filter = "執行檔 (*.exe)|*.exe";
                dialog.CheckFileExists = true;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        _configManager.SaveAppPath(dialog.FileName);
                        UpdateUIStatus();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"儲存失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// 延遲值變更事件
        /// </summary>
        private void nudDelay_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                _configManager.SaveSendKeysDelay((int)nudDelay.Value);
            }
            catch
            {
                // 靜默處理儲存錯誤
            }
        }

        /// <summary>
        /// 執行登入按鈕點擊事件
        /// </summary>
        private void btnRun_Click(object sender, EventArgs e)
        {
            try
            {
                // 取得所有必要資料
                string secretKey = _secretStorage.LoadSecretKey();
                string password = _secretStorage.PasswordExists() ? _secretStorage.LoadPassword() : string.Empty;
                string appPath = _configManager.LoadAppPath();
                int delayMs = _configManager.LoadSendKeysDelay();

                // 驗證應用程式路徑
                if (!ValidationRules.IsValidAppPath(appPath))
                {
                    MessageBox.Show(
                        "應用程式路徑無效或檔案不存在！\n請重新選擇應用程式。",
                        "路徑錯誤",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                // 產生 OTP
                string otp = _otpGenerator.Generate(secretKey);

                // 最小化視窗
                this.WindowState = FormWindowState.Minimized;

                // 執行自動登入
                _automator.RunAndLogin(appPath, password, otp, delayMs);
            }
            catch (Exception ex)
            {
                this.WindowState = FormWindowState.Normal;
                MessageBox.Show($"執行失敗：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    /// <summary>
    /// 簡單的輸入對話框
    /// </summary>
    public class InputDialog : Form
    {
        private TextBox txtInput;
        private Button btnOK;
        private Button btnCancel;

        /// <summary>
        /// 取得輸入值
        /// </summary>
        public string InputValue => txtInput.Text;

        public InputDialog(string title, string prompt, bool isPassword)
        {
            this.Text = title;
            this.Size = new Size(400, 150);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = ThemeColors.FormBackground;

            var lblPrompt = new Label
            {
                Text = prompt,
                Location = new Point(15, 15),
                Size = new Size(360, 20),
                ForeColor = ThemeColors.TextPrimary
            };

            txtInput = new TextBox
            {
                Location = new Point(15, 40),
                Size = new Size(355, 25),
                UseSystemPasswordChar = isPassword,
                BackColor = ThemeColors.InputBackground,
                ForeColor = ThemeColors.TextPrimary
            };

            btnOK = new Button
            {
                Text = "確定",
                Location = new Point(210, 75),
                Size = new Size(75, 28),
                DialogResult = DialogResult.OK,
                BackColor = ThemeColors.PrimaryButtonBackground,
                ForeColor = ThemeColors.ButtonForeground,
                FlatStyle = FlatStyle.Flat
            };
            btnOK.FlatAppearance.BorderSize = 0;

            btnCancel = new Button
            {
                Text = "取消",
                Location = new Point(295, 75),
                Size = new Size(75, 28),
                DialogResult = DialogResult.Cancel,
                BackColor = ThemeColors.ButtonBackground,
                ForeColor = ThemeColors.ButtonForeground,
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.FlatAppearance.BorderSize = 0;

            this.Controls.AddRange(new Control[] { lblPrompt, txtInput, btnOK, btnCancel });
            this.AcceptButton = btnOK;
            this.CancelButton = btnCancel;
        }
    }
}

namespace LazyTFFXIV.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // UI 控制項
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlSecretKeyIndicator;
        private System.Windows.Forms.Label lblSecretKeyStatus;
        private System.Windows.Forms.Button btnSetSecretKey;
        private System.Windows.Forms.Panel pnlPasswordIndicator;
        private System.Windows.Forms.Label lblPasswordStatus;
        private System.Windows.Forms.Button btnSetPassword;
        private System.Windows.Forms.Panel pnlAppPathIndicator;
        private System.Windows.Forms.Label lblAppPathStatus;
        private System.Windows.Forms.Button btnSelectApp;
        private System.Windows.Forms.Label lblDelayStatus;
        private System.Windows.Forms.NumericUpDown nudDelay;
        private System.Windows.Forms.Label lblDelayUnit;
        private System.Windows.Forms.Button btnRun;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlSecretKeyIndicator = new System.Windows.Forms.Panel();
            this.lblSecretKeyStatus = new System.Windows.Forms.Label();
            this.btnSetSecretKey = new System.Windows.Forms.Button();
            this.pnlPasswordIndicator = new System.Windows.Forms.Panel();
            this.lblPasswordStatus = new System.Windows.Forms.Label();
            this.btnSetPassword = new System.Windows.Forms.Button();
            this.pnlAppPathIndicator = new System.Windows.Forms.Panel();
            this.lblAppPathStatus = new System.Windows.Forms.Label();
            this.btnSelectApp = new System.Windows.Forms.Button();
            this.lblDelayStatus = new System.Windows.Forms.Label();
            this.nudDelay = new System.Windows.Forms.NumericUpDown();
            this.lblDelayUnit = new System.Windows.Forms.Label();
            this.btnRun = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudDelay)).BeginInit();
            this.SuspendLayout();
            //
            // lblTitle
            //
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(15, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(380, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "LazyTFFXIV 自動化登入";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // pnlSecretKeyIndicator
            //
            this.pnlSecretKeyIndicator.Location = new System.Drawing.Point(20, 60);
            this.pnlSecretKeyIndicator.Name = "pnlSecretKeyIndicator";
            this.pnlSecretKeyIndicator.Size = new System.Drawing.Size(12, 12);
            this.pnlSecretKeyIndicator.TabIndex = 1;
            //
            // lblSecretKeyStatus
            //
            this.lblSecretKeyStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSecretKeyStatus.Location = new System.Drawing.Point(40, 57);
            this.lblSecretKeyStatus.Name = "lblSecretKeyStatus";
            this.lblSecretKeyStatus.Size = new System.Drawing.Size(200, 20);
            this.lblSecretKeyStatus.TabIndex = 2;
            this.lblSecretKeyStatus.Text = "TOTP 密鑰 - 未設定";
            //
            // btnSetSecretKey
            //
            this.btnSetSecretKey.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnSetSecretKey.Location = new System.Drawing.Point(300, 52);
            this.btnSetSecretKey.Name = "btnSetSecretKey";
            this.btnSetSecretKey.Size = new System.Drawing.Size(100, 30);
            this.btnSetSecretKey.TabIndex = 3;
            this.btnSetSecretKey.Text = "設定密鑰";
            this.btnSetSecretKey.Click += new System.EventHandler(this.btnSetSecretKey_Click);
            //
            // pnlPasswordIndicator
            //
            this.pnlPasswordIndicator.Location = new System.Drawing.Point(20, 100);
            this.pnlPasswordIndicator.Name = "pnlPasswordIndicator";
            this.pnlPasswordIndicator.Size = new System.Drawing.Size(12, 12);
            this.pnlPasswordIndicator.TabIndex = 4;
            //
            // lblPasswordStatus
            //
            this.lblPasswordStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblPasswordStatus.Location = new System.Drawing.Point(40, 97);
            this.lblPasswordStatus.Name = "lblPasswordStatus";
            this.lblPasswordStatus.Size = new System.Drawing.Size(200, 20);
            this.lblPasswordStatus.TabIndex = 5;
            this.lblPasswordStatus.Text = "登入密碼 - 未設定";
            //
            // btnSetPassword
            //
            this.btnSetPassword.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnSetPassword.Location = new System.Drawing.Point(300, 92);
            this.btnSetPassword.Name = "btnSetPassword";
            this.btnSetPassword.Size = new System.Drawing.Size(100, 30);
            this.btnSetPassword.TabIndex = 6;
            this.btnSetPassword.Text = "設定密碼";
            this.btnSetPassword.Click += new System.EventHandler(this.btnSetPassword_Click);
            //
            // pnlAppPathIndicator
            //
            this.pnlAppPathIndicator.Location = new System.Drawing.Point(20, 140);
            this.pnlAppPathIndicator.Name = "pnlAppPathIndicator";
            this.pnlAppPathIndicator.Size = new System.Drawing.Size(12, 12);
            this.pnlAppPathIndicator.TabIndex = 7;
            //
            // lblAppPathStatus
            //
            this.lblAppPathStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblAppPathStatus.Location = new System.Drawing.Point(40, 137);
            this.lblAppPathStatus.Name = "lblAppPathStatus";
            this.lblAppPathStatus.Size = new System.Drawing.Size(250, 20);
            this.lblAppPathStatus.TabIndex = 8;
            this.lblAppPathStatus.Text = "應用程式路徑 - 未設定";
            //
            // btnSelectApp
            //
            this.btnSelectApp.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnSelectApp.Location = new System.Drawing.Point(300, 132);
            this.btnSelectApp.Name = "btnSelectApp";
            this.btnSelectApp.Size = new System.Drawing.Size(100, 30);
            this.btnSelectApp.TabIndex = 9;
            this.btnSelectApp.Text = "選擇程式";
            this.btnSelectApp.Click += new System.EventHandler(this.btnSelectApp_Click);
            //
            // lblDelayStatus
            //
            this.lblDelayStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDelayStatus.Location = new System.Drawing.Point(20, 182);
            this.lblDelayStatus.Name = "lblDelayStatus";
            this.lblDelayStatus.Size = new System.Drawing.Size(120, 20);
            this.lblDelayStatus.TabIndex = 10;
            this.lblDelayStatus.Text = "按鍵延遲時間：";
            //
            // nudDelay
            //
            this.nudDelay.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.nudDelay.Location = new System.Drawing.Point(140, 178);
            this.nudDelay.Name = "nudDelay";
            this.nudDelay.Size = new System.Drawing.Size(80, 25);
            this.nudDelay.TabIndex = 11;
            this.nudDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudDelay.ValueChanged += new System.EventHandler(this.nudDelay_ValueChanged);
            //
            // lblDelayUnit
            //
            this.lblDelayUnit.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDelayUnit.Location = new System.Drawing.Point(225, 182);
            this.lblDelayUnit.Name = "lblDelayUnit";
            this.lblDelayUnit.Size = new System.Drawing.Size(30, 20);
            this.lblDelayUnit.TabIndex = 12;
            this.lblDelayUnit.Text = "ms";
            //
            // btnRun
            //
            this.btnRun.Enabled = false;
            this.btnRun.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.btnRun.Location = new System.Drawing.Point(20, 230);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(380, 45);
            this.btnRun.TabIndex = 13;
            this.btnRun.Text = "執行登入";
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            //
            // MainForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(420, 290);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.pnlSecretKeyIndicator);
            this.Controls.Add(this.lblSecretKeyStatus);
            this.Controls.Add(this.btnSetSecretKey);
            this.Controls.Add(this.pnlPasswordIndicator);
            this.Controls.Add(this.lblPasswordStatus);
            this.Controls.Add(this.btnSetPassword);
            this.Controls.Add(this.pnlAppPathIndicator);
            this.Controls.Add(this.lblAppPathStatus);
            this.Controls.Add(this.btnSelectApp);
            this.Controls.Add(this.lblDelayStatus);
            this.Controls.Add(this.nudDelay);
            this.Controls.Add(this.lblDelayUnit);
            this.Controls.Add(this.btnRun);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LazyTFFXIV";
            ((System.ComponentModel.ISupportInitialize)(this.nudDelay)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion
    }
}

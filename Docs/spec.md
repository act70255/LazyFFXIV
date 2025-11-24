# 專案規格書：LazyTFFXIV (自動化登入輔助工具)

## 1. 專案概述 (Overview)
本工具旨在簡化特定 Windows 應用程式的登入流程。透過自動執行程式並模擬鍵盤輸入（固定密碼 + 動態計算的 TOTP），減少使用者重複操作。專案目標為產出單一、輕量、無需安裝的可執行檔 (.exe)，供非技術人員使用。

## 2. 技術堆疊 (Tech Stack)
* **語言/框架：** C# / .NET Framework 4.8 (確保 Windows 高相容性)
* **UI 框架：** Windows Forms (WinForms)
* **依賴管理：** NuGet
    * `Otp.NET` (用於 TOTP 計算)
    * `Costura.Fody` (用於將 DLL 合併為單一 EXE)
* **設計模式：** Dependency Injection (Pure DI), Strategy Pattern
* **安全性：** Windows DPAPI (Data Protection API)

---

## 3. 功能需求 (Functional Requirements)

### 3.1 密鑰管理 (Key Management)
* **FR-01:** 使用者可輸入 Base32 格式的 Secret Key（明碼輸入，不需隱碼）。
* **FR-02:** 系統必須將 Secret Key 加密儲存（使用 `CurrentUser` 範圍的 DPAPI），不可明碼儲存。
* **FR-03:** 設定檔應存於 `%AppData%` 或執行檔同級目錄（建議 AppData 以支援權限受限環境）。
* **FR-04:** 程式啟動時自動檢查密鑰是否存在，並更新 UI 狀態。

### 3.2 密碼管理 (Password Management)
* **FR-05:** 使用者可輸入固定密碼。
* **FR-06:** 系統必須將固定密碼加密儲存（使用 `CurrentUser` 範圍的 DPAPI），與 Secret Key 相同方式處理。
* **FR-07:** 畫面僅顯示密碼是否已設定（狀態指示），不顯示密碼內容。
* **FR-08:** 提供「設定/更新密碼」功能按鈕。

### 3.3 應用程式路徑設定 (Application Path Configuration)
* **FR-09:** 使用者可透過按鈕選擇目標應用程式路徑（開啟檔案對話框）。
* **FR-10:** 應用程式路徑儲存於設定檔（可明碼儲存，無安全性風險）。
* **FR-11:** 畫面上顯示目前設定的應用程式路徑。

### 3.4 OTP 計算 (OTP Generation)
* **FR-12:** 根據儲存的密鑰與當前系統時間，計算標準 TOTP (6位數，30秒週期)。
* **FR-13:** 支援標準 SHA-1 HMAC 演算法 (相容 Google Authenticator)。

### 3.5 自動化流程 (Automation Workflow)
* **FR-14:** 啟動指定路徑的應用程式。
* **FR-15:** 等待應用程式進入可輸入狀態 (Idle)。
* **FR-16:** 模擬鍵盤輸入序列：`[固定密碼] -> [TAB] -> [OTP] -> [ENTER]`。
* **FR-17:** 每個 SendKeys 指令之間延遲，預設 **300ms**，確保輸入穩定。
* **FR-18:** 延遲時間可透過 UI 調整，並儲存於設定檔。

### 3.6 使用者介面 (UI/UX)
* **FR-19:** 單一視窗，極簡設計。
* **FR-20:** 狀態指示燈顯示各項設定狀態：
    * 密鑰狀態（已設定/未設定）
    * 密碼狀態（已設定/未設定）
    * 應用程式路徑（顯示完整路徑或「未設定」）
* **FR-21:** 主要互動功能：
    * 「設定密鑰」按鈕
    * 「設定密碼」按鈕
    * 「選擇應用程式」按鈕
    * 「設定延遲」輸入框或滑桿（調整 SendKeys 延遲時間）
    * 「執行登入」按鈕
* **FR-22:** 當所有必要設定完成時，「執行登入」按鈕才可用。

---

## 4. 架構設計 (Architecture & SOLID Implementation)

為了符合 SOLID 原則，透過介面 (Interface) 進行解耦，不將邏輯全部寫在 Form (UI) 裡面。

### 4.1 介面定義 (Abstractions) - *Interface Segregation Principle (ISP)*

定義四個核心介面，讓客戶端（Form）不依賴具體實作：

1.  **`ISecretStorage`** (負責加密儲存)
    * 用途：單一職責為「安全的讀寫敏感資料」（使用 DPAPI 加密）。
    * 方法：
        * `void SaveSecretKey(string key)` - 儲存 TOTP 密鑰
        * `string LoadSecretKey()` - 讀取 TOTP 密鑰
        * `bool SecretKeyExists()` - 檢查密鑰是否存在
        * `void SavePassword(string password)` - 儲存固定密碼
        * `string LoadPassword()` - 讀取固定密碼
        * `bool PasswordExists()` - 檢查密碼是否存在

2.  **`IConfigurationManager`** (負責一般設定)
    * 用途：單一職責為「管理非敏感設定資料」（明碼儲存）。
    * 方法：
        * `void SaveAppPath(string path)` - 儲存應用程式路徑
        * `string LoadAppPath()` - 讀取應用程式路徑
        * `bool AppPathExists()` - 檢查路徑是否已設定
        * `void SaveSendKeysDelay(int delayMs)` - 儲存 SendKeys 延遲時間 (毫秒)
        * `int LoadSendKeysDelay()` - 讀取 SendKeys 延遲時間 (預設 300ms)

3.  **`IOtpGenerator`** (負責計算)
    * 用途：單一職責為「產生驗證碼」。
    * 方法：`string Generate(string secretKey)`;

4.  **`IAppAutomator`** (負責執行)
    * 用途：單一職責為「控制外部程式與輸入」。
    * 方法：`void RunAndLogin(string appPath, string password, string otp, int delayMs)`;

### 4.2 實作類別 (Concrete Implementations) - *Single Responsibility Principle (SRP)*

每個類別只做一件事：

* **`DpapiSecretStorage`** (實作 `ISecretStorage`)
    * 負責檔案路徑處理 (`System.IO`)。
    * 負責呼叫 DPAPI (`ProtectedData`) 進行加解密。
    * 分別管理 `secretkey.dat` 與 `password.dat` 兩個加密檔案。

* **`FileConfigurationManager`** (實作 `IConfigurationManager`)
    * 負責讀寫設定檔 (`config.json` 或 `config.ini`)。
    * 儲存非敏感資料如應用程式路徑。

* **`TotpOtpGenerator`** (實作 `IOtpGenerator`)
    * 負責引用 `Otp.NET` 套件。
    * 負責 Base32 解碼與時間運算。

* **`SendKeysAutomator`** (實作 `IAppAutomator`)
    * 負責 `Process.Start`。
    * 負責 `SendKeys.SendWait` 模擬輸入。

### 4.3 依賴注入 (Dependency Injection) - *Dependency Inversion Principle (DIP)*

UI 層 (Form) **不應該** 直接實例化實作類別。它應該在建構子 (Constructor) 中要求介面。

**MainForm.cs 結構範例：**

```csharp
public partial class MainForm : Form
{
    private readonly ISecretStorage _secretStorage;
    private readonly IConfigurationManager _configManager;
    private readonly IOtpGenerator _otpGenerator;
    private readonly IAppAutomator _automator;

    // 建構子注入 (Constructor Injection)
    public MainForm(
        ISecretStorage secretStorage,
        IConfigurationManager configManager,
        IOtpGenerator otpGen,
        IAppAutomator automator)
    {
        InitializeComponent();
        _secretStorage = secretStorage;
        _configManager = configManager;
        _otpGenerator = otpGen;
        _automator = automator;

        // 初始化時更新 UI 狀態
        UpdateUIStatus();
    }

    // 更新畫面狀態指示
    private void UpdateUIStatus()
    {
        bool hasKey = _secretStorage.SecretKeyExists();
        bool hasPassword = _secretStorage.PasswordExists();
        bool hasAppPath = _configManager.AppPathExists();

        // 更新狀態指示燈與按鈕狀態...
        btnRun.Enabled = hasKey && hasPassword && hasAppPath;
    }

    // 按鈕事件只負責調用介面，不處理邏輯
    private void btnRun_Click(...)
    {
        string key = _secretStorage.LoadSecretKey();
        string password = _secretStorage.LoadPassword();
        string appPath = _configManager.LoadAppPath();
        int delayMs = _configManager.LoadSendKeysDelay();
        string otp = _otpGenerator.Generate(key);
        _automator.RunAndLogin(appPath, password, otp, delayMs);
    }
}
```
### 4.4 程式進入點 (Composition Root)
在 Program.cs 中組裝所有模組 (Pure DI)：
```csharp
static void Main()
{
    Application.EnableVisualStyles();
    Application.SetCompatibleTextRenderingDefault(false);

    // 設定檔案路徑 (存放於 %AppData%\LazyTFFXIV\)
    string appDataPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "LazyTFFXIV");

    // 在這裡決定要用哪些實作 (方便未來替換，符合 Open/Closed Principle)
    ISecretStorage secretStorage = new DpapiSecretStorage(appDataPath);
    IConfigurationManager configManager = new FileConfigurationManager(appDataPath);
    IOtpGenerator otpGen = new TotpOtpGenerator();
    IAppAutomator automator = new SendKeysAutomator();

    Application.Run(new MainForm(secretStorage, configManager, otpGen, automator));
}
```
---

## 5. SOLID 原則檢核表 (Compliance Checklist)

| 原則 | 說明 | 在本專案的體現 |
|------|------|----------------|
| **S** - 單一職責 | 一個類別只做一件事 | 儲存邏輯、設定管理、OTP 演算法、自動化邏輯、UI 顯示，全部分離在不同類別。 |
| **O** - 開放封閉 | 對擴充開放，對修改封閉 | 若未來想把 SendKeys 改成更底層的 Win32 API，只需新增一個 Win32Automator 類別並實作介面，完全不用改動 Form 的程式碼。 |
| **L** - 里氏替換 | 子類可替換父類 | 任何實作了 ISecretStorage 的類別，都能直接替換 DpapiSecretStorage 而不破壞程式。 |
| **I** - 介面隔離 | 客戶端不依賴不用的方法 | 介面設計精簡，MainForm 只看到它需要的方法。 |
| **D** - 依賴反轉 | 依賴抽象而非實作 | MainForm 依賴的是 I... 介面，而不是具體的 class。依賴關係由 Program.cs 從外部注入。 |

---

## 6. 非功能需求 (Non-Functional Requirements)

* **NFR-01 可攜性 (Portability):** 編譯設定需啟用 Costura.Fody，產出單一 EXE。
* **NFR-02 效能 (Performance):** 啟動時間應小於 1 秒。
* **NFR-03 最小權限 (Least Privilege):** 檔案存取僅限於使用者層級，不需要系統管理員權限。
* **NFR-04 相容性 (Compatibility):** 支援 Windows 7 / 10 / 11。

---

## 7. 錯誤處理策略 (Error Handling Strategy)

| 錯誤情境 | 處理方式 |
|----------|----------|
| 密鑰解密失敗 (DPAPI 錯誤) | 顯示提示訊息，引導使用者重新設定密鑰 |
| 密碼解密失敗 | 顯示提示訊息，引導使用者重新設定密碼 |
| 目標應用程式路徑不存在 | 顯示友善提示，引導使用者重新選擇路徑 |
| 目標應用程式無法啟動 | 顯示錯誤訊息，包含系統錯誤描述 |
| Secret Key 格式不正確 (非 Base32) | 在儲存前驗證，顯示格式錯誤提示 |
| 設定檔目錄無法建立 | 顯示權限不足提示 |

---

## 8. 檔案結構 (File Structure)

```
%AppData%\LazyTFFXIV\
├── secretkey.dat      # DPAPI 加密的 TOTP 密鑰
├── password.dat       # DPAPI 加密的固定密碼
└── config.json        # 明碼設定檔 (應用程式路徑等)
```

**config.json 格式範例：**
```json
{
    "appPath": "C:\\Program Files\\TargetApp\\app.exe",
    "sendKeysDelayMs": 300
}
```

---

## 9. 專案程式碼結構 (Project Structure)

```
LazyTFFXIV/
├── LazyTFFXIV.sln          # Solution 檔案
└── LazyTFFXIV/
    ├── LazyTFFXIV.csproj   # 專案檔
    ├── Program.cs                  # 程式進入點 (Composition Root)
    ├── App.config                  # 應用程式設定
    │
    ├── Interfaces/                 # 介面定義
    │   ├── ISecretStorage.cs
    │   ├── IConfigurationManager.cs
    │   ├── IOtpGenerator.cs
    │   └── IAppAutomator.cs
    │
    ├── Services/                   # 服務實作
    │   ├── DpapiSecretStorage.cs
    │   ├── FileConfigurationManager.cs
    │   ├── TotpOtpGenerator.cs
    │   └── SendKeysAutomator.cs
    │
    ├── Theme/                      # 主題與樣式
    │   └── ThemeColors.cs          # 顏色變數集中管理
    │
    ├── Validation/                 # 驗證規則
    │   └── ValidationRules.cs      # 輸入驗證常數與方法
    │
    ├── Forms/                      # UI 表單
    │   ├── MainForm.cs
    │   ├── MainForm.Designer.cs
    │   └── MainForm.resx
    │
    └── Properties/
        ├── AssemblyInfo.cs
        └── Resources.resx
```

### 命名規範 (Microsoft C# Coding Conventions)

遵循 [Microsoft C# 編碼慣例](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)：

| 類型 | 規則 | 範例 |
|------|------|------|
| 命名空間 | PascalCase | `LazyTFFXIV.Services` |
| 介面 | `I` 前綴 + PascalCase | `ISecretStorage` |
| 類別 | PascalCase | `DpapiSecretStorage` |
| 公開方法 | PascalCase | `SaveSecretKey()` |
| 公開屬性 | PascalCase | `IsConfigured` |
| 私有欄位 | `_` 前綴 + camelCase | `_secretStorage` |
| 參數 | camelCase | `secretKey` |
| 區域變數 | camelCase | `hasPassword` |
| 常數 | PascalCase | `DefaultDelayMs` |
| 列舉 | PascalCase | `ConnectionState` |

---

## 10. UI 視覺規格 (UI Visual Specification)

### 10.1 視窗尺寸

| 屬性 | 值 |
|------|-----|
| 視窗寬度 | 420 px |
| 視窗高度 | 320 px |
| 視窗可調整大小 | 否 (FormBorderStyle.FixedSingle) |
| 起始位置 | 螢幕中央 (CenterScreen) |

### 10.2 顏色變數 (ThemeColors)

為便於統一管理與快速修改，所有顏色定義於 `Theme/ThemeColors.cs` 靜態類別：

```csharp
namespace LazyTFFXIV.Theme
{
    using System.Drawing;

    /// <summary>
    /// 集中管理所有 UI 顏色，方便快速修改主題
    /// </summary>
    public static class ThemeColors
    {
        // 視窗背景
        public static readonly Color FormBackground = Color.FromArgb(245, 245, 245);

        // 狀態指示燈
        public static readonly Color StatusReady = Color.FromArgb(76, 175, 80);      // 綠色 - 已設定
        public static readonly Color StatusNotSet = Color.FromArgb(189, 189, 189);   // 灰色 - 未設定
        public static readonly Color StatusError = Color.FromArgb(244, 67, 54);      // 紅色 - 錯誤

        // 按鈕
        public static readonly Color ButtonBackground = Color.FromArgb(33, 150, 243); // 藍色
        public static readonly Color ButtonBackgroundHover = Color.FromArgb(25, 118, 210);
        public static readonly Color ButtonForeground = Color.White;
        public static readonly Color ButtonDisabled = Color.FromArgb(224, 224, 224);

        // 執行按鈕 (主要動作)
        public static readonly Color PrimaryButtonBackground = Color.FromArgb(76, 175, 80);
        public static readonly Color PrimaryButtonHover = Color.FromArgb(56, 142, 60);

        // 文字
        public static readonly Color TextPrimary = Color.FromArgb(33, 33, 33);
        public static readonly Color TextSecondary = Color.FromArgb(117, 117, 117);

        // 邊框
        public static readonly Color BorderDefault = Color.FromArgb(224, 224, 224);
        public static readonly Color BorderFocus = Color.FromArgb(33, 150, 243);

        // 輸入框
        public static readonly Color InputBackground = Color.White;
        public static readonly Color InputBorder = Color.FromArgb(189, 189, 189);
    }
}
```

### 10.3 字型設定

| 元素 | 字型 | 大小 |
|------|------|------|
| 標題 | Segoe UI Semibold | 14 pt |
| 標籤文字 | Segoe UI | 10 pt |
| 按鈕文字 | Segoe UI Semibold | 10 pt |
| 狀態文字 | Segoe UI | 9 pt |

---

## 11. 輸入驗證規則 (Input Validation Rules)

### 11.1 SendKeys 延遲時間

| 屬性 | 值 |
|------|-----|
| 最小值 | 50 ms |
| 最大值 | 2000 ms |
| 預設值 | 300 ms |
| UI 控制項 | NumericUpDown (增量: 50ms) |

**驗證邏輯：**
```csharp
public static class ValidationRules
{
    public const int MinDelayMs = 50;
    public const int MaxDelayMs = 2000;
    public const int DefaultDelayMs = 300;
    public const int DelayIncrement = 50;

    public static bool IsValidDelay(int delayMs)
    {
        return delayMs >= MinDelayMs && delayMs <= MaxDelayMs;
    }
}
```

### 11.2 Secret Key 格式

| 屬性 | 規則 |
|------|------|
| 格式 | Base32 編碼 (A-Z, 2-7) |
| 長度 | 16-32 字元 (標準 TOTP) |
| 驗證時機 | 儲存前驗證 |

### 11.3 應用程式路徑

| 屬性 | 規則 |
|------|------|
| 格式 | 有效的 Windows 檔案路徑 |
| 副檔名 | 必須為 `.exe` |
| 驗證 | 檔案必須存在 |
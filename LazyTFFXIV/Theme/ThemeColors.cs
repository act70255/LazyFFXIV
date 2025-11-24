using System.Drawing;

namespace LazyTFFXIV.Theme
{
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

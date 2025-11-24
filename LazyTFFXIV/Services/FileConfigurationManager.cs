using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using LazyTFFXIV.Interfaces;
using LazyTFFXIV.Validation;

namespace LazyTFFXIV.Services
{
    /// <summary>
    /// 設定資料模型
    /// </summary>
    public class ConfigData
    {
        public string AppPath { get; set; }
        public int SendKeysDelayMs { get; set; } = ValidationRules.DefaultDelayMs;
    }

    /// <summary>
    /// 使用 JSON 檔案儲存非敏感設定
    /// </summary>
    public class FileConfigurationManager : IConfigurationManager
    {
        private readonly string _configPath;
        private ConfigData _config;

        /// <summary>
        /// 初始化 FileConfigurationManager
        /// </summary>
        /// <param name="basePath">設定檔儲存目錄路徑</param>
        public FileConfigurationManager(string basePath)
        {
            _configPath = Path.Combine(basePath, "config.json");

            // 確保目錄存在
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            // 載入現有設定或建立預設
            LoadConfig();
        }

        /// <summary>
        /// 載入設定檔
        /// </summary>
        private void LoadConfig()
        {
            if (File.Exists(_configPath))
            {
                try
                {
                    string json = File.ReadAllText(_configPath, Encoding.UTF8);
                    var serializer = new DataContractJsonSerializer(typeof(ConfigData));
                    using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                    {
                        _config = (ConfigData)serializer.ReadObject(stream);
                    }
                }
                catch
                {
                    // 檔案損毀時使用預設值
                    _config = new ConfigData();
                }
            }
            else
            {
                _config = new ConfigData();
            }
        }

        /// <summary>
        /// 儲存設定檔
        /// </summary>
        private void SaveConfig()
        {
            var serializer = new DataContractJsonSerializer(typeof(ConfigData));
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, _config);
                string json = Encoding.UTF8.GetString(stream.ToArray());
                File.WriteAllText(_configPath, json, Encoding.UTF8);
            }
        }

        public void SaveAppPath(string path)
        {
            _config.AppPath = path;
            SaveConfig();
        }

        public string LoadAppPath()
        {
            return _config.AppPath;
        }

        public bool AppPathExists()
        {
            return !string.IsNullOrWhiteSpace(_config.AppPath);
        }

        public void SaveSendKeysDelay(int delayMs)
        {
            // 確保在有效範圍內
            if (delayMs < ValidationRules.MinDelayMs)
                delayMs = ValidationRules.MinDelayMs;
            else if (delayMs > ValidationRules.MaxDelayMs)
                delayMs = ValidationRules.MaxDelayMs;

            _config.SendKeysDelayMs = delayMs;
            SaveConfig();
        }

        public int LoadSendKeysDelay()
        {
            // 如果值無效，回傳預設值
            if (!ValidationRules.IsValidDelay(_config.SendKeysDelayMs))
            {
                return ValidationRules.DefaultDelayMs;
            }
            return _config.SendKeysDelayMs;
        }
    }
}

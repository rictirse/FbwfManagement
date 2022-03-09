using Fbwf.Base;
using Fbwf.Language;
using Fbwf.Libaray;
using Fbwf.Libaray.Method;
using Fbwf.Library.Helpers;
using System;
using System.Globalization;
using System.IO;
using System.Text.Json.Serialization;
using System.Windows;

namespace Fbwf.Library.Config
{
    public class XConfig : PropertyBase
    {
        [JsonIgnore]
        public string Version { get; private set; }

        public static string ConfigFileName => Path.Combine(AssemblyData.Path, "Config.cfg");

        #region Language
        [JsonIgnore]
        public CultureInfo Culture
        {
            get => _Culture;
            set
            {
                SetProperty(ref _Culture, value);
                LoadCultures();
            }
        }
        CultureInfo _Culture;
        public string Language { get; set; } = null;
        CulturesHelper CulturesHelper { get; init; } = new ();
        public void LoadCultures()
        {
            if (CulturesHelper == null) return;

            CulturesHelper.ChangeCulture(Culture);
        }
        #endregion

        public virtual void Load()
        {

            XConfig _base = null;
            if (new FileInfo(ConfigFileName).Exists)
            {
                try
                {
                    _base = ConfigFileName.JsonFormFile<XConfig>();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{Lang.Find("LoadConfigErr")}{ex.Message}", "failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            this.Culture = _base?.Culture ?? new CultureInfo(_base?.Language ?? "en-US", false);
            this.Version = $"{AssemblyData.AppName} {AssemblyData.AppVersion}";
        }

        public virtual void Save()
        {
            try
            {
                this.FileToJson(ConfigFileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{Lang.Find("SaveConfigErr")}{ex.Message}", "failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

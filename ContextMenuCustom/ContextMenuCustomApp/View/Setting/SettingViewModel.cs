﻿using ContextMenuCustomApp.Common;
using ContextMenuCustomApp.Service.Common.Json;
using ContextMenuCustomApp.Service.Lang;
using ContextMenuCustomApp.View.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.System;
using Windows.UI.Xaml;

namespace ContextMenuCustomApp.View.Setting
{
    public class SettingViewModel : BaseViewModel
    {
        public readonly Settings Settings;
        public readonly AppLang AppLang;
        private readonly LanguageService _languageService;
        public SettingViewModel()
        {
            Settings = AppContext.Current.AppSetting;
            AppLang = AppContext.Current.AppLang;
            _languageService = AppContext.Current.GetService<LanguageService>();
        }

        public string Version()
        {
            var version = Package.Current.Id.Version;
            return $"Version: {version.Major}.{version.Minor}.{version.Build}";
        }

        public int ThemeType
        {
            get
            {
                return Settings.ThemeType;
            }
            set
            {
                Settings.ThemeType = value;

                if (value == 1)
                {
                    ThemeHelper.RootTheme = ElementTheme.Dark;
                }
                else if (value == 2)
                {
                    ThemeHelper.RootTheme = ElementTheme.Light;
                }
                else
                {
                    ThemeHelper.RootTheme = ElementTheme.Default;
                }
            }
        }

        public async void OpenDataFolder()
        {
            var folder = ApplicationData.Current.LocalFolder;
            _ = await Launcher.LaunchFolderAsync(folder);
        }

        #region language

        private List<LangInfo> _languages;

        public List<LangInfo> Languages
        {
            get => _languages;
            set => SetProperty(ref _languages, value);
        }

        public async Task LoadLanguages()
        {
            Languages = await _languageService.QueryLangList();
        }

        public void UpdateLangSetting(LangInfo langInfo)
        {
            _languageService.UpdateLangSetting(langInfo);
        }

        public async Task ExportLang(LangInfo langInfo)
        {
            FileSavePicker fileSavePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.Desktop,
                SuggestedFileName = langInfo.FileName
            };

            fileSavePicker.FileTypeChoices.Add("Json", new List<string>() { ".json" });
            var file = await fileSavePicker.PickSaveFileAsync();
            if (file == null)
            {
                return;
            }

            AppLang applang = await _languageService.LoadDefualtAsync();
            await FileIO.WriteTextAsync(file, JsonUtil.Serialize(applang, true));
        }

        public LangInfo GetCurrentLang()
        {
            var langFileName = AppContext.Current.AppSetting.AppLang;
            return Languages.Find(x => x.FileName == langFileName);
        }

        public async void OpenLanguagesFolder()
        {
            var folder = await _languageService.GetCustomLanguagesFolderAsync();
            await Launcher.LaunchFolderAsync(folder);
        }

        #endregion

    }
}

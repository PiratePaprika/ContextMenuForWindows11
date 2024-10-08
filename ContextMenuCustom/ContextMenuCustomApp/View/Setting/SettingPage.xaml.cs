﻿using ContextMenuCustomApp.View.Common;
using System;
using Windows.ApplicationModel.Core;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ContextMenuCustomApp.View.Setting
{
    public sealed partial class SettingPage : Page
    {
        private readonly SettingViewModel _viewModel;
        public SettingPage()
        {
            _viewModel = new SettingViewModel();
            this.DataContext = _viewModel;
            NavigationCacheMode = NavigationCacheMode.Required;
            this.InitializeComponent();
            this.RegisterMessageHandler(_viewModel);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
        }

        private async void OpenIconButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                var fileOpenPicker = new FileOpenPicker
                {
                    SuggestedStartLocation = PickerLocationId.ComputerFolder
                };

                string[] fileTypes = { ".dll", ".exe", ".ico", ".png", ".bmp", ".jpeg", ".jpg", ".heic", ".tif", "*" };
                foreach (string fileType in fileTypes)
                {
                    fileOpenPicker.FileTypeFilter.Add(fileType);
                }

                var file = await fileOpenPicker.PickSingleFileAsync();
                if (null != file)
                {
                    string iconPath = file.Name.EndsWith(".dll") || file.Name.EndsWith(".exe") ? $"\"{file.Path}\",0" : $"\"{file.Path}\"";
                    switch (button.Tag)
                    {
                        case string tag when tag == "Dark":
                            MenuDarkIconPath.Text = iconPath;
                            break;
                        default:
                            MenuLightIconPath.Text = iconPath;
                            break;
                    }
                }
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private async void RestartAppBtn_Click(object sender, RoutedEventArgs e)
        {
            await CoreApplication.RequestRestartAsync(string.Empty);
        }
    }
}

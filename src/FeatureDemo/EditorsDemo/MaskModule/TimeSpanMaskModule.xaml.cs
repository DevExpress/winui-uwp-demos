﻿using DevExpress.Mvvm;
using DevExpress.UI.Xaml.Editors;
using FeatureDemo.Common;

namespace EditorsDemo {
    public class TimeSpanMaskViewModel : ViewModelBase {
        public TextEdit FocusedEditor { get { return GetProperty<TextEdit>(); } private set { SetProperty(value, OnFocusedEditorChanged); } }
        public TextInputTimeSpanMaskSettings TextInputSettings { get { return GetProperty<TextInputTimeSpanMaskSettings>(); } private set { SetProperty(value); } }
        public string Mask { get { return GetProperty<string>(); } set { SetProperty(value, OnMaskChanged); } }
        public ICommand<object> OnEditorGotFocusCommand { get; private set; }

        public TimeSpanMaskViewModel() {
            OnEditorGotFocusCommand = new DelegateCommand<object>(OnEditorGotFocus);
        }

        void OnEditorGotFocus(object editor) {
            FocusedEditor = editor as TextEdit;
        }
        void OnFocusedEditorChanged() {
            TextInputSettings = FocusedEditor?.TextInputSettings as TextInputTimeSpanMaskSettings;
            Mask = TextInputSettings?.Mask;
        }
        async void OnMaskChanged() {
            if (TextInputSettings == null || Mask == TextInputSettings.Mask)
                return;
            string maskBackup = TextInputSettings.Mask;
            try {
                TextInputSettings.Mask = Mask;
            } catch {
                await GetService<IMessageBoxService>().ShowAsync("Invalid mask", "Error");
                TextInputSettings.Mask = maskBackup;
                Mask = maskBackup;
            }
        }
    }
    public sealed partial class TimeSpanMaskModule : DemoSubModuleView {
        public TimeSpanMaskModule() {
            this.InitializeComponent();
        }
    }
}

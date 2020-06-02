﻿using System.Windows.Input;
using DevExpress.Core.Extensions;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.UI.Xaml;
using Microsoft.UI.Xaml;
using System.Collections.Generic;
using ICommand = Microsoft.UI.Xaml.Input.ICommand;

namespace ControlsDemo {
    public class ContextMenuViewModel : ViewModelBase {
        public bool IsToolbarChecked {
            get { return GetProperty<bool>(); }
            set { SetProperty(value, UpdateMenuType); }
        }
        public bool IsContextToolbarChecked {
            get { return GetProperty<bool>(); }
            set { SetProperty(value, UpdateMenuType); }
        }
        public bool IsMenuFlyoutChecked {
            get { return GetProperty<bool>(); }
            set { SetProperty(value, UpdateMenuType); }
        }
        public bool IsCustomChecked {
            get { return GetProperty<bool>(); }
            set { SetProperty(value, UpdateMenuType); }
        }

        public ContextMenuType MenuType {
            get { return GetProperty<ContextMenuType>(); }
            set { SetProperty(value); }
        }
        public bool CancelOpening {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }
        public double HorizontalMenuOffset {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }
        public double VerticalMenuOffset {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }
        public string Header {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }
        public ContextMenuItem TextFormattingGroup { get; }
        public ContextMenuToggleItem BoldItem { get; }
        public ContextMenuToggleItem ItalicItem { get; }
        public ContextMenuToggleItem UnderlineItem { get; }
        public ContextMenuItem AlignmentGroup { get; }
        public ContextMenuToggleItem AlignCenterItem { get; }
        public ContextMenuToggleItem AlignLeftItem { get; }
        public ContextMenuToggleItem AlignRightItem { get; }
        public ContextMenuToggleItem AlignJustifyItem { get; }

        public ContextMenuItem EditGroup { get; }
        public ContextMenuItem CopyItem { get; }
        public ContextMenuItem PasteItem { get; }
        public ContextMenuItem CutItem { get; }
        public ICommand MenuOpeningCommand { get; }
        public ICommand ShowContextMenuCommand { get; }
        protected IContextMenuService ContextMenuService { get { return GetService<IContextMenuService>(); } }
        public string Name { get; } = "Name";
        public ContextMenuViewModel() {
            IsContextToolbarChecked = true;
            MenuOpeningCommand = new DelegateCommand<ContextMenuOpeningEventArgs>(OnMenuOpening);
            ShowContextMenuCommand = new DelegateCommand(ShowContextMenu);
            CancelOpening = false;
            Header = "Header";
            Name = "Name";
            HorizontalMenuOffset = 0;
            VerticalMenuOffset = 0;

            EditGroup = new ContextMenuItem("Clipboard", "Clipboard");
            CutItem = new ContextMenuItem("Cut", "Cut");
            CopyItem = new ContextMenuItem("Copy", "Copy");
            PasteItem = new ContextMenuItem("Paste", "Paste");

            AlignmentGroup = new ContextMenuItem("Alignment", "AlignCenter");
            AlignJustifyItem = new ContextMenuToggleItem("Justify", "AlignJustify") { IsChecked = true };
            AlignLeftItem = new ContextMenuToggleItem("Left", "AlignLeft");
            AlignCenterItem = new ContextMenuToggleItem("Center", "AlignCenter");
            AlignRightItem = new ContextMenuToggleItem("Right", "AlignRight");

            TextFormattingGroup = new ContextMenuItem("Text Formatting", "Bold");
            ItalicItem = new ContextMenuToggleItem("Italic", "Italic");
            BoldItem = new ContextMenuToggleItem("Bold", "Bold") { IsChecked = true };
            UnderlineItem = new ContextMenuToggleItem("Underline", "Underline");
        }
        void UpdateMenuType() {
            if(IsToolbarChecked) {
                MenuType = ContextMenuType.Toolbar;
                return;
            }
            if(IsMenuFlyoutChecked) {
                MenuType = ContextMenuType.MenuFlyout;
                return;
            }
            if(IsContextToolbarChecked) {
                MenuType = ContextMenuType.ContextToolbar;
                return;
            }
            MenuType = ContextMenuType.Custom;
        }
        void ShowContextMenu() {
            ContextMenuService?.Open();
        }
        void OnMenuOpening(ContextMenuOpeningEventArgs args) {
            args.Cancel = CancelOpening;
            var pos = args.Info.TargetPosition;
            args.Info.TargetPosition = pos.Offset(HorizontalMenuOffset, VerticalMenuOffset);
        }
    }

    public class ContextMenuItem {
        public string Caption { get; set; }
        public string Icon { get; set; }
        public ContextMenuItem(string caption, string icon) {
            Caption = caption;
            Icon = string.Format("ms-appx:///RibbonDemo/Images/{0}.svg", icon);
        }
    }
    public class ContextMenuToggleItem : ContextMenuItem {
        public bool IsChecked { get; set; }
        public ContextMenuToggleItem(string caption, string icon) : base(caption, icon) {
        }
    }
    public class ContextMenuService : ServiceBase, IContextMenuService {
        public static readonly DependencyProperty MenuProperty =
            DependencyProperty.Register(nameof(Menu), typeof(ContextMenu), typeof(ContextMenuService), new PropertyMetadata(null));

        public ContextMenu Menu {
            get { return (ContextMenu)GetValue(MenuProperty); }
            set { SetValue(MenuProperty, value); }
        }

        public void Open() {
            Menu?.Open();
        }
    }
    public interface IContextMenuService {
        void Open();
    }
    public enum ContextMenuType {
        ContextToolbar,
        Toolbar,
        MenuFlyout,
        Custom
    }
}

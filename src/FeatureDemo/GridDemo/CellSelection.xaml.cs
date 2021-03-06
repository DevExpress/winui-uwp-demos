﻿using DevExpress.Data;
using DevExpress.UI.Xaml.Grid;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

namespace GridDemo {
    public sealed partial class CellSelection : GridDemoUserControl {
        public CellSelection() {
            this.InitializeComponent();
            gridControl.AutoGeneratingColumn += gridControl_AutoGeneratingColumn;
            gridControl.AutoGeneratedColumns += gridControl_ColumnsPopulated;
            gridControl.CustomSummary += gridControl_CustomSummary;
            gridControl.SelectionChanged += gridControl_SelectionChanged;
            Unloaded += OnUnloaded;
        }
        void OnUnloaded(object sender, RoutedEventArgs e) {
            gridControl.AutoGeneratingColumn -= gridControl_AutoGeneratingColumn;
            gridControl.AutoGeneratedColumns -= gridControl_ColumnsPopulated;
            gridControl.CustomSummary -= gridControl_CustomSummary;
            gridControl.SelectionChanged -= gridControl_SelectionChanged;
            gridControl.DataContext = null;
        }

        void SelectCells() {
            gridControl.BeginSelection();
            int[,] selectedCells = new int[,] {
                {0, 1}, {1, 1}, {2, 1}, {3, 1}, {4, 1}, {5, 1}, {6, 1}, {7, 1}, {6, 1},{6, 1}, {7, 1}, {8, 1}, {9, 1}, {10, 1},
                {0, 2}, {1, 2}, {2, 2}, {8, 2}, {9, 2}, {10, 2},
                {2, 3}, {3, 3}, {4, 3}, {5, 3}, {6, 3}, {7, 3}, {8, 3}, 

                {0, 5}, {1, 5}, {2, 5}, {3, 5}, {7, 5}, {8, 5},  {9, 5},  {10, 5},
                {2, 6}, {3, 6}, {4, 6}, {5, 6}, {6, 6}, {7, 6}, {8, 6},
                {0, 7}, {1, 7}, {2, 7}, {3, 7}, {7, 7}, {8, 7},  {9, 7},  {10, 7}
            };
            for(int i = 0; i < selectedCells.GetLength(0); i++)
                gridControl.SelectCell(selectedCells[i, 0], gridControl.Columns[selectedCells[i, 1]]);
            gridControl.EndSelection();
        }
        void gridControl_AutoGeneratingColumn(object sender, AutoGeneratingColumnEventArgs e) {
            e.Column = new GridTemplateColumn() { CellTemplate = Resources["dataTemplate"] as DataTemplate, FieldName = e.Column.FieldName, Width = 180 };
        }
        void gridControl_ColumnsPopulated(object sender, EventArgs e) {
            gridControl.BeginDataUpdate();
            foreach(GridColumnBase column in gridControl.Columns)
                gridControl.TotalSummary.Add(new GridSummaryItem() { FieldName = column.FieldName, SummaryType = SummaryItemType.Custom, DisplayFormat = "${0:N}" });
            gridControl.EndDataUpdate();
            SelectCells();
        }
        int sum = 0;
        void gridControl_CustomSummary(object sender, CustomSummaryEventArgs e) {
            if(object.Equals(e.SummaryProcess, CustomSummaryProcess.Start)) {
                sum = 0;
            }
            if(e.SummaryProcess == CustomSummaryProcess.Calculate) {
                if(gridControl.IsCellSelected(e.RowHandle, gridControl.Columns[((GridSummaryItem)e.Item).FieldName])) {
                    sum += (int)e.FieldValue;
                }
            }
            if(e.SummaryProcess == CustomSummaryProcess.Finalize)
                e.TotalValue = sum;
        }
        void gridControl_SelectionChanged(object sender, GridSelectionChangedEventArgs e) {
            gridControl.UpdateTotalSummary();
        }
        protected internal override GridControl Grid { get { return gridControl; } }
        protected override bool NeedChangeAutoWidth { get { return false; } }
        void CheckBox_Checked(object sender, RoutedEventArgs e) {
            gridControl.SelectionMode = MultiSelectMode.CellExtended;
        }
        void CheckBox_Unchecked(object sender, RoutedEventArgs e) {
            gridControl.SelectionMode = MultiSelectMode.Cell;
        }
    }
}

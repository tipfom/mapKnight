﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace mapKnight.ToolKit.Windows {
    /// <summary>
    /// Interaktionslogik für EditorWindow.xaml
    /// </summary>
    public partial class EditorWindow : Window {
        public const string PROJECT_FILTER = "PROJECT-Files|*.mkproj";

        public EditorWindow ( ) {
            InitializeComponent( );
            LoadConfig( );
            App.Current.MainWindow = this;
        }

        private void LoadConfig ( ) {
            this.Top = Properties.Settings.Default.Top;
            this.Left = Properties.Settings.Default.Left;
            this.Height = Properties.Settings.Default.Height;
            this.Width = Properties.Settings.Default.Width;
            // Very quick and dirty - but it does the job
            if (Properties.Settings.Default.Maximized) {
                WindowState = WindowState.Maximized;
            }
        }

        private void SetTabPageMenu (List<FrameworkElement> items) {
            while (menu_editor.Items.Count > 2)
                menu_editor.Items.RemoveAt(2);

            foreach (FrameworkElement item in items)
                menu_editor.Items.Add(item);
        }

        private void Window_Closing (object sender, System.ComponentModel.CancelEventArgs e) {
            if (WindowState == WindowState.Maximized) {
                // Use the RestoreBounds as the current values will be 0, 0 and the size of the screen
                Properties.Settings.Default.Top = RestoreBounds.Top;
                Properties.Settings.Default.Left = RestoreBounds.Left;
                Properties.Settings.Default.Height = RestoreBounds.Height;
                Properties.Settings.Default.Width = RestoreBounds.Width;
                Properties.Settings.Default.Maximized = true;
            } else {
                Properties.Settings.Default.Top = this.Top;
                Properties.Settings.Default.Left = this.Left;
                Properties.Settings.Default.Height = this.Height;
                Properties.Settings.Default.Width = this.Width;
                Properties.Settings.Default.Maximized = false;
            }

            Properties.Settings.Default.Save( );
            if (App.Project?.HasChanged ?? false)
                App.ShowSaveDialog( );
        }

        private void Map_Selected (object sender, RoutedEventArgs e) {
            SetTabPageMenu(mapeditor.Menu);
        }

        private void NewCommand_CanExecute (object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }

        private void NewCommand_Executed (object sender, ExecutedRoutedEventArgs e) {
            App.CreateNewProject(this);
        }

        private void OpenCommand_CanExecute (object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }

        private void SaveCommand_CanExecute (object sender, CanExecuteRoutedEventArgs e) {
            e.CanExecute = true;
        }

        private void SaveCommand_Executed (object sender, ExecutedRoutedEventArgs e) {
            App.SaveProject( );
        }

        private void Window_Loaded (object sender, RoutedEventArgs e) {
            App.CreateNewProject(this);
        }

        private void About_Click (object sender, RoutedEventArgs e) {
            new AboutWindow( ).ShowDialog( );
        }

        private void TabAnimation_Selected (object sender, RoutedEventArgs e) {
            SetTabPageMenu(animationeditor.Menu);
        }

        private void TabItem_Selected_1 (object sender, RoutedEventArgs e) {

        }
    }
}
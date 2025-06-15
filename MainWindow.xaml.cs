using System;
using System.Windows;
using System.Windows.Input;
using System.Diagnostics;
using System.IO;

namespace SpotlightClone
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Hide();
            SpotlightHotkey.Register(this);
        }

        public void ToggleVisibility()
        {
            if (this.Visibility == Visibility.Visible)
            {
                this.Hide();
            }
            else
            {
                this.Left = (SystemParameters.PrimaryScreenWidth - this.Width) / 2;
                this.Top = 50;
                this.Show();
                this.Activate();
                SearchBox.Focus();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Hide();
        }

private void SearchBox_KeyDown(object sender, KeyEventArgs e)
{
    if (e.Key == Key.Enter)
    {
        string input = SearchBox.Text.Trim().ToLower();

        // Map known shortcuts
        if (input == "discord")
        {
            string shortcut = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                @"Microsoft\Windows\Start Menu\Programs\Discord Inc\Discord.lnk"
            );

            if (File.Exists(shortcut))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = shortcut,
                    UseShellExecute = true
                });
            }
            else
            {
                MessageBox.Show("Discord shortcut not found.");
            }
        }
        else
        {
            // Fallback generic command
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "cmd",
                    Arguments = $"/c start \"\" \"{input}\"",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not launch '{input}':\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        this.Hide();
    }
}



    }
}

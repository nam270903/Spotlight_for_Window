using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Diagnostics;

namespace SpotlightClone
{
    public partial class MainWindow : Window
    {
        private List<(string name, string path)> shortcuts = new();

        public MainWindow()
        {
            InitializeComponent();
            this.Show(); // Show for debug
            SpotlightHotkey.Register(this);
            LoadShortcuts();
        }

        public void ToggleVisibility()
        {
            if (this.Visibility == Visibility.Visible)
                this.Hide();
            else
            {
                this.Left = (SystemParameters.PrimaryScreenWidth - this.Width) / 2;
                this.Top = 50;
                this.Show();
                this.Activate();
                SearchBox.Focus();
            }
        }

        private void LoadShortcuts()
        {
            string cacheFile = "appcache.json";

            if (File.Exists(cacheFile))
            {
                var json = File.ReadAllText(cacheFile);
                shortcuts = System.Text.Json.JsonSerializer.Deserialize<List<(string name, string path)>>(json);
                return;
            }

            string[] startMenuPaths =
            {
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu), "Programs"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu), "Programs")
            };

            foreach (var dir in startMenuPaths)
            {
                if (Directory.Exists(dir))
                {
                    var files = Directory.GetFiles(dir, "*.lnk", SearchOption.AllDirectories);
                    foreach (var file in files)
                    {
                        var name = Path.GetFileNameWithoutExtension(file).ToLower();
                        shortcuts.Add((name, file));
                    }
                }
            }

            string[] programFolders = { @"C:\\Program Files", @"C:\\Program Files (x86)" };

            foreach (var dir in programFolders)
            {
                if (Directory.Exists(dir))
                {
                    foreach (var exe in GetExecutableFilesSafely(dir))
                    {
                        var name = Path.GetFileNameWithoutExtension(exe).ToLower();
                        shortcuts.Add((name, exe));
                    }
                }
            }

            var jsonOut = System.Text.Json.JsonSerializer.Serialize(shortcuts);
            File.WriteAllText(cacheFile, jsonOut);
        }

private IEnumerable<string> GetExecutableFilesSafely(string root)
{
    var stack = new Stack<string>();
    stack.Push(root);

    while (stack.Count > 0)
    {
        string currentDir = stack.Pop();

        bool isSafe = true;

        try
        {
            var attr = File.GetAttributes(currentDir);
            if ((attr & FileAttributes.Hidden) != 0 || (attr & FileAttributes.System) != 0)
                continue;
        }
        catch
        {
            isSafe = false;
        }

        if (!isSafe) continue;

        string[] files = Array.Empty<string>();
        try { files = Directory.GetFiles(currentDir, "*.exe"); } catch { }
        foreach (var file in files)
            yield return file;

        string[] subDirs = Array.Empty<string>();
        try { subDirs = Directory.GetDirectories(currentDir); } catch { }
        foreach (var subDir in subDirs)
            stack.Push(subDir);
    }
}


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Hide();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = SearchBox.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(input))
            {
                SuggestionList.Visibility = Visibility.Collapsed;
                return;
            }

            var suggestions = shortcuts
                .OrderBy(s => LevenshteinDistance(s.name, input))
                .Take(5)
                .ToList();

            if (suggestions.Count > 0)
            {
                SuggestionList.ItemsSource = suggestions.Select(s => s.name);
                SuggestionList.Visibility = Visibility.Visible;
            }
            else
            {
                SuggestionList.Visibility = Visibility.Collapsed;
            }
        }

        private void SuggestionList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SuggestionList.SelectedItem != null)
            {
                string name = SuggestionList.SelectedItem.ToString();
                string path = shortcuts.FirstOrDefault(s => s.name == name).path;

                if (File.Exists(path))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = path,
                        UseShellExecute = true
                    });
                }

                this.Hide();
            }
        }

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string input = SearchBox.Text.Trim().ToLower();

                string matchPath = shortcuts
                    .OrderBy(s => LevenshteinDistance(s.name, input))
                    .Select(s => s.path)
                    .FirstOrDefault();

                if (!string.IsNullOrEmpty(matchPath) && File.Exists(matchPath))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = matchPath,
                        UseShellExecute = true
                    });
                }
                else
                {
                    MessageBox.Show($"No match found for '{input}'");
                }

                this.Hide();
            }
        }

        private int LevenshteinDistance(string a, string b)
        {
            if (a.Length == 0) return b.Length;
            if (b.Length == 0) return a.Length;

            int[,] d = new int[a.Length + 1, b.Length + 1];

            for (int i = 0; i <= a.Length; i++) d[i, 0] = i;
            for (int j = 0; j <= b.Length; j++) d[0, j] = j;

            for (int i = 1; i <= a.Length; i++)
            {
                for (int j = 1; j <= b.Length; j++)
                {
                    int cost = b[j - 1] == a[i - 1] ? 0 : 1;

                    d[i, j] = Math.Min(
                        Math.Min(
                            d[i - 1, j] + 1,
                            d[i, j - 1] + 1
                        ),
                        d[i - 1, j - 1] + cost
                    );
                }
            }

            return d[a.Length, b.Length];
        }
    }
}
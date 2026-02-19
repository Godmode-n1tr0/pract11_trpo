using System;
using System.IO;
using System.Windows;

public static class ThemeHelper
{
    private const string ThemeFile = "theme.config";

    public static void ApplySaved()
    {
        var theme = File.Exists(ThemeFile) ? File.ReadAllText(ThemeFile) : "Light";
        SetTheme(theme);
    }

    public static void Toggle()
    {
        var current = Application.Current.Resources.MergedDictionaries[0].Source?.OriginalString ?? "Light";
        var newTheme = current.Contains("Dark") ? "Light" : "Dark";
        SetTheme(newTheme);
        File.WriteAllText(ThemeFile, newTheme);
    }

    private static void SetTheme(string themeName)
    {
        try
        {
            var uri = new Uri($"Themes/{themeName}Theme.xaml", UriKind.Relative);
            var dict = new ResourceDictionary { Source = uri };
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }
        catch (Exception ex)
        {
            var uri = new Uri($"Themes/LightTheme.xaml", UriKind.Relative);
            var dict = new ResourceDictionary { Source = uri };
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }
    }
}
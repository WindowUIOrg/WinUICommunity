﻿namespace WinUICommunity;
public partial class ThemeService
{
    public void OnThemeComboBoxSelectionChanged(object sender)
    {
        var cmb = (ComboBox)sender;
        var selectedTheme = (cmb?.SelectedItem as ComboBoxItem)?.Tag?.ToString();
        if (selectedTheme != null)
        {
            ApplyThemeOrBackdrop<ElementTheme>(selectedTheme);
        }
    }

    public void SetThemeComboBoxDefaultItem(ComboBox themeComboBox)
    {
        var currentTheme = RootTheme.ToString();
        var item = themeComboBox.Items.Cast<ComboBoxItem>().FirstOrDefault(c => c?.Tag?.ToString() == currentTheme);
        if (item != null && (ComboBoxItem)themeComboBox.SelectedItem != item)
        {
            themeComboBox.SelectedItem = item;
        }
    }

    public void OnBackdropComboBoxSelectionChanged(object sender)
    {
        var cmb = (ComboBox)sender;
        var selectedBackdrop = (cmb?.SelectedItem as ComboBoxItem)?.Tag?.ToString();
        if (selectedBackdrop != null)
        {
            ApplyThemeOrBackdrop<BackdropType>(selectedBackdrop);
        }
    }

    public void SetBackdropComboBoxDefaultItem(ComboBox backdropComboBox)
    {
        var currentBackdrop = GetBackdropType(GetSystemBackdrop()).ToString();

        var item = backdropComboBox.Items.Cast<ComboBoxItem>().FirstOrDefault(c => c?.Tag?.ToString() == currentBackdrop);
        if (item != null && (ComboBoxItem)backdropComboBox.SelectedItem != item)
        {
            backdropComboBox.SelectedItem = item;
        }
    }

    public void OnThemeRadioButtonChecked(object sender)
    {
        var selectedTheme = ((RadioButton)sender)?.Tag?.ToString();
        if (selectedTheme != null)
        {
            ApplyThemeOrBackdrop<ElementTheme>(selectedTheme);
        }
    }

    public void SetThemeRadioButtonDefaultItem(Panel ThemePanel)
    {
        var currentTheme = RootTheme.ToString();
        var items = ThemePanel.Children.Cast<RadioButton>();
        if (items != null)
        {
            var selectedItem = items.FirstOrDefault(c => c?.Tag?.ToString() == currentTheme);
            if (selectedItem != null)
            {
                selectedItem.IsChecked = true;
            }
        }
    }

    public void OnBackdropRadioButtonChecked(object sender)
    {
        var selectedBackdrop = ((RadioButton)sender)?.Tag?.ToString();
        if (selectedBackdrop != null)
        {
            ApplyThemeOrBackdrop<BackdropType>(selectedBackdrop);
        }
    }

    public void SetBackdropRadioButtonDefaultItem(Panel BackdropPanel)
    {
        var currentBackdrop = GetBackdropType(GetSystemBackdrop()).ToString();
        var items = BackdropPanel.Children.Cast<RadioButton>();
        if (items != null)
        {
            var selectedItem = items.FirstOrDefault(c => c?.Tag?.ToString() == currentBackdrop);
            if (selectedItem != null)
            {
                selectedItem.IsChecked = true;
            }
        }
    }

    private void ApplyThemeOrBackdrop<TEnum>(string text) where TEnum : struct
    {
        if (Enum.TryParse(text, out TEnum result) && Enum.IsDefined(typeof(TEnum), result))
        {
            if (result is BackdropType backdrop)
            {
                SetBackdropType(backdrop);
            }
            else if (result is ElementTheme theme)
            {
                SetElementTheme(theme);
            }
        }
    }
}

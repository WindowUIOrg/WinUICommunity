﻿using Microsoft.UI.Xaml;
using Microsoft.Windows.ApplicationModel.Resources;

namespace WinUICommunity;
public sealed partial class AllLandingPage : ItemsPageBase
{
    internal static AllLandingPage Instance { get; private set; }

    public static readonly DependencyProperty UseFullScreenHeaderImageProperty = DependencyProperty.Register(nameof(UseFullScreenHeaderImage), typeof(bool), typeof(AllLandingPage), new PropertyMetadata(false, OnFullScreenHeaderImageChanged));
    public bool UseFullScreenHeaderImage
    {
        get { return (bool)GetValue(UseFullScreenHeaderImageProperty); }
        set { SetValue(UseFullScreenHeaderImageProperty, value); }
    }

    private static void OnFullScreenHeaderImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (AllLandingPage)d;
        if (ctl != null)
        {
            ctl.ToggleFullScreen((bool)e.NewValue);
        }
    }

    private void ToggleFullScreen(bool value)
    {
        if (HeaderPanel != null && itemGridView != null)
        {
            if (value)
            {
                Microsoft.UI.Xaml.Controls.Grid.SetRowSpan(HomePageHeaderImage, 3);
            }
            else
            {
                Microsoft.UI.Xaml.Controls.Grid.SetRowSpan(HomePageHeaderImage, 2);
            }
        }
    }
    
    public AllLandingPage()
    {
        this.InitializeComponent();
        Instance = this;
        Loaded -= AllLandingPage_Loaded;
        Loaded += AllLandingPage_Loaded;

        ToggleFullScreen(UseFullScreenHeaderImage);
    }

    private void AllLandingPage_Loaded(object sender, RoutedEventArgs e)
    {
        if (CanExecuteInternalCommand && JsonNavigationViewService != null)
        {
            GetData(JsonNavigationViewService.DataSource);
            OrderBy(i => i.Title);
        }
    }

    public void GetData(DataSource dataSource)
    {
        GetData(dataSource, new ResourceManager());
    }
    public void GetData(DataSource dataSource, ResourceManager resourceManager)
    {
        var allItems = new List<DataItem>();

        foreach (var group in dataSource.Groups.Where(g => !g.IsSpecialSection && !g.HideGroup))
        {
            foreach (var item in group.Items.Where(i => !i.HideItem))
            {
                // Recursively add the items, including nested ones
                AddLocalizedItemsRecursively(item, allItems, resourceManager);
            }
        }

        Items = allItems;
    }

    public async Task GetDataAsync(string jsonFilePath, PathType pathType = PathType.Relative)
    {
        await GetDataAsync(jsonFilePath, new ResourceManager(), pathType);
    }
    public async Task GetDataAsync(string jsonFilePath, ResourceManager resourceManager, PathType pathType = PathType.Relative)
    {
        var dataSource = new DataSource();
        await dataSource.GetGroupsAsync(jsonFilePath, pathType);

        var allItems = new List<DataItem>();

        foreach (var group in dataSource.Groups.Where(g => !g.IsSpecialSection && !g.HideGroup))
        {
            foreach (var item in group.Items.Where(i => !i.HideItem))
            {
                // Recursively add the items, including nested ones
                AddLocalizedItemsRecursively(item, allItems, resourceManager);
            }
        }

        Items = allItems;
    }

    private void AddLocalizedItemsRecursively(DataItem currentItem, List<DataItem> allItems, ResourceManager resourceManager)
    {
        currentItem.Title = Helper.GetLocalizedText(currentItem.Title, currentItem.UsexUid, resourceManager);
        currentItem.SecondaryTitle = Helper.GetLocalizedText(currentItem.SecondaryTitle, currentItem.UsexUid, resourceManager);
        currentItem.Subtitle = Helper.GetLocalizedText(currentItem.Subtitle, currentItem.UsexUid, resourceManager);
        currentItem.Description = Helper.GetLocalizedText(currentItem.Description, currentItem.UsexUid, resourceManager);

        allItems.Add(currentItem);

        // Recursively process nested items
        foreach (var nestedItem in currentItem.Items.Where(i => !i.HideItem))
        {
            AddLocalizedItemsRecursively(nestedItem, allItems, resourceManager);
        }
    }
 
    public void OrderBy(Func<DataItem, object> orderby = null)
    {
        if (orderby != null)
        {
            Items = Items?.OrderBy(orderby);
        }
        else
        {
            Items = Items?.OrderBy(i => i.Title);
        }
    }

    public void OrderByDescending(Func<DataItem, object> orderByDescending = null)
    {
        if (orderByDescending != null)
        {
            Items = Items?.OrderByDescending(orderByDescending);
        }
        else
        {
            Items = Items?.OrderByDescending(i => i.Title);
        }
    }
}

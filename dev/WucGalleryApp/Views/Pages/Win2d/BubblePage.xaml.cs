﻿using Microsoft.UI.Input;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media.Animation;

namespace WucGalleryApp.Views;

public sealed partial class BubblePage : Page
{
    private bool _isAnimateBegin;
    private readonly Storyboard _progressStoryboard;
    public BubblePage()
    {
        this.InitializeComponent();
        _progressStoryboard = CreateStoryboard();
    }

    private void OnLikeButtonTapped(object sender, TappedRoutedEventArgs e)
    {
        if (LikeButton.State != BubbleProgressState.Completed)
            LikeButton.State = BubbleProgressState.Completed;
        else
            LikeButton.State = BubbleProgressState.Idle;
    }


    private void OnGestureRecognizerTapped(object sender, TappedEventArgs e)
    {
        var progressButton = sender as BubbleProgressButton;
        if (progressButton.State == BubbleProgressState.Idle)
            progressButton.State = BubbleProgressState.Completed;
        else
            progressButton.State = BubbleProgressState.Idle;
    }


    private void OnGestureRecognizerHolding(object sender, HoldingEventArgs e)
    {
        var progressButton = sender as BubbleProgressButton;
        if (e.HoldingState == HoldingState.Started)
        {
            if (!_isAnimateBegin)
            {
                _isAnimateBegin = true;
                (_progressStoryboard.Children[0] as DoubleAnimation).From = progressButton.Minimum;
                (_progressStoryboard.Children[0] as DoubleAnimation).To = progressButton.Maximum;
                Storyboard.SetTarget(_progressStoryboard.Children[0] as DoubleAnimation, progressButton);
                _progressStoryboard.Begin();
            }
        }
        else
        {
            _isAnimateBegin = false;
            _progressStoryboard.Stop();
        }
    }


    private Storyboard CreateStoryboard()
    {
        var animation = new DoubleAnimation
        {
            EnableDependentAnimation = true,
            Duration = TimeSpan.FromSeconds(2)
        };

        Storyboard.SetTargetProperty(animation, nameof(BubbleProgressButton.Value));
        var storyboard = new Storyboard();
        storyboard.Children.Add(animation);
        storyboard.Completed += OnProgressStoryboardCompleted;
        storyboard.FillBehavior = FillBehavior.Stop;
        return storyboard;
    }

    private void OnProgressStoryboardCompleted(object sender, object e)
    {
        LikeButton.State = BubbleProgressState.Completed;
        CoinButton.State = BubbleProgressState.Completed;
        FavoriteButton.State = BubbleProgressState.Completed;
    }
}

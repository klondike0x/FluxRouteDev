using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using ScrollBar = System.Windows.Controls.Primitives.ScrollBar;

namespace FluxRoute.Helpers;

public static class ScrollViewerBehavior
{
    public static readonly DependencyProperty AutoHideScrollBarProperty =
        DependencyProperty.RegisterAttached(
            "AutoHideScrollBar",
            typeof(bool),
            typeof(ScrollViewerBehavior),
            new PropertyMetadata(false, OnAutoHideChanged));

    public static bool GetAutoHideScrollBar(DependencyObject obj) => (bool)obj.GetValue(AutoHideScrollBarProperty);
    public static void SetAutoHideScrollBar(DependencyObject obj, bool value) => obj.SetValue(AutoHideScrollBarProperty, value);

    private static readonly DependencyProperty ScrollTimerProperty =
        DependencyProperty.RegisterAttached(
            "ScrollTimer",
            typeof(DispatcherTimer),
            typeof(ScrollViewerBehavior));

    private static void OnAutoHideChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not ScrollViewer sv) return;

        if ((bool)e.NewValue)
        {
            sv.ScrollChanged += OnScrollChanged;
        }
        else
        {
            sv.ScrollChanged -= OnScrollChanged;
            if (sv.GetValue(ScrollTimerProperty) is DispatcherTimer timer)
            {
                timer.Stop();
                sv.ClearValue(ScrollTimerProperty);
            }
        }
    }

    private static void OnScrollChanged(object sender, ScrollChangedEventArgs e)
    {
        if (sender is not ScrollViewer sv) return;
        if (e.VerticalChange == 0 && e.HorizontalChange == 0) return;

        FadeScrollBars(sv, toOpacity: 1, seconds: 0.2);

        var timer = sv.GetValue(ScrollTimerProperty) as DispatcherTimer;
        if (timer is null)
        {
            timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.5) };
            timer.Tick += (_, _) =>
            {
                timer.Stop();
                FadeScrollBars(sv, toOpacity: 0, seconds: 0.5);
            };
            sv.SetValue(ScrollTimerProperty, timer);
        }

        timer.Stop();
        timer.Start();
    }

    private static void FadeScrollBars(ScrollViewer sv, double toOpacity, double seconds)
    {
        var anim = new DoubleAnimation
        {
            To = toOpacity,
            Duration = TimeSpan.FromSeconds(seconds),
            FillBehavior = FillBehavior.HoldEnd
        };

        if (sv.Template?.FindName("PART_VerticalScrollBar", sv) is ScrollBar vBar)
            vBar.BeginAnimation(UIElement.OpacityProperty, anim);

        if (sv.Template?.FindName("PART_HorizontalScrollBar", sv) is ScrollBar hBar)
            hBar.BeginAnimation(UIElement.OpacityProperty, anim);
    }
}

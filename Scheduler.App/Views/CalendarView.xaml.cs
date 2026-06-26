using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Scheduler.App.ViewModels;

namespace Scheduler.App.Views;

public partial class CalendarView : UserControl
{
    private const double SnapDistanceRatio = 0.22;
    private const double SnapVelocityThreshold = 850;

    private bool _isAnimating;
    private bool _isDragging;
    private Point _dragStartPoint;
    private double _dragStartPaperY;
    private double _lastDragY;
    private double _dragVelocity;
    private readonly Stopwatch _dragStopwatch = new();

    public CalendarView()
    {
        InitializeComponent();
    }

    private async void PreviousHintButton_Click(object sender, RoutedEventArgs e)
    {
        await AnimateAndNavigateAsync(Viewport.ActualHeight, vm => vm.PreviousMonthCommand.Execute(null));
    }

    private async void NextHintButton_Click(object sender, RoutedEventArgs e)
    {
        await AnimateAndNavigateAsync(-Viewport.ActualHeight, vm => vm.NextMonthCommand.Execute(null));
    }

    private async Task AnimateAndNavigateAsync(double targetY, Action<CalendarViewModel>? navigate)
    {
        if (_isAnimating || DataContext is not CalendarViewModel viewModel)
        {
            return;
        }

        if (Viewport.ActualHeight <= 0)
        {
            navigate?.Invoke(viewModel);
            PaperTransform.Y = 0;
            return;
        }

        _isAnimating = true;

        try
        {
            await AnimatePaperToAsync(targetY);
            navigate?.Invoke(viewModel);
            PaperTransform.BeginAnimation(System.Windows.Media.TranslateTransform.YProperty, null);
            PaperTransform.Y = 0;
            UpdatePaperLayout();
        }
        finally
        {
            _isAnimating = false;
        }
    }

    private Task AnimatePaperToAsync(double targetY, int durationMilliseconds = 260)
    {
        var completion = new TaskCompletionSource();
        var animation = new DoubleAnimation
        {
            From = PaperTransform.Y,
            To = targetY,
            Duration = TimeSpan.FromMilliseconds(durationMilliseconds),
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut },
            FillBehavior = FillBehavior.HoldEnd
        };

        animation.Completed += (_, _) => completion.SetResult();
        PaperTransform.BeginAnimation(System.Windows.Media.TranslateTransform.YProperty, animation);
        return completion.Task;
    }

    private void Viewport_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (_isAnimating || Viewport.ActualHeight <= 0)
        {
            return;
        }

        PaperTransform.BeginAnimation(System.Windows.Media.TranslateTransform.YProperty, null);
        _isDragging = true;
        _dragStartPoint = e.GetPosition(Viewport);
        _dragStartPaperY = PaperTransform.Y;
        _lastDragY = _dragStartPoint.Y;
        _dragVelocity = 0;
        _dragStopwatch.Restart();
        Mouse.Capture(Viewport);
        e.Handled = true;
    }

    private void Viewport_PreviewMouseMove(object sender, MouseEventArgs e)
    {
        if (!_isDragging)
        {
            return;
        }

        var point = e.GetPosition(Viewport);
        var rawOffset = _dragStartPaperY + point.Y - _dragStartPoint.Y;
        var clampedOffset = Clamp(rawOffset, -Viewport.ActualHeight, Viewport.ActualHeight);
        var elapsed = _dragStopwatch.Elapsed.TotalSeconds;

        if (elapsed > 0)
        {
            _dragVelocity = (point.Y - _lastDragY) / elapsed;
        }

        _lastDragY = point.Y;
        _dragStopwatch.Restart();
        PaperTransform.Y = clampedOffset;
        e.Handled = true;
    }

    private async void Viewport_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (!_isDragging)
        {
            return;
        }

        _isDragging = false;
        Mouse.Capture(null);
        e.Handled = true;
        await SnapAfterDragAsync();
    }

    private async void Viewport_LostMouseCapture(object sender, MouseEventArgs e)
    {
        if (!_isDragging)
        {
            return;
        }

        _isDragging = false;
        await SnapAfterDragAsync();
    }

    private async Task SnapAfterDragAsync()
    {
        var height = Viewport.ActualHeight;
        var currentY = PaperTransform.Y;
        var threshold = height * SnapDistanceRatio;

        if (currentY >= threshold || _dragVelocity >= SnapVelocityThreshold)
        {
            await AnimateAndNavigateAsync(height, vm => vm.PreviousMonthCommand.Execute(null));
            return;
        }

        if (currentY <= -threshold || _dragVelocity <= -SnapVelocityThreshold)
        {
            await AnimateAndNavigateAsync(-height, vm => vm.NextMonthCommand.Execute(null));
            return;
        }

        await AnimateAndNavigateAsync(0, null);
    }

    private void Viewport_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        UpdatePaperLayout();
    }

    private void UpdatePaperLayout()
    {
        var width = Viewport.ActualWidth;
        var height = Viewport.ActualHeight;

        Paper.Width = width;
        Paper.Height = height * 3;

        SetMonthViewLayout(PrevMonthView, width, height, -height);
        SetMonthViewLayout(CurrentMonthView, width, height, 0);
        SetMonthViewLayout(NextMonthView, width, height, height);
    }

    private static void SetMonthViewLayout(FrameworkElement view, double width, double height, double top)
    {
        view.Width = width;
        view.Height = height;
        Canvas.SetLeft(view, 0);
        Canvas.SetTop(view, top);
    }

    private static double Clamp(double value, double min, double max)
    {
        return Math.Min(Math.Max(value, min), max);
    }
}

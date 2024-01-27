using System.Drawing;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Metadata;
using tfcurve.curve;
using Point = Avalonia.Point;

namespace tfcurve;

public class ViewSettingFlyout : PopupFlyoutBase
{
    protected FlyoutPresenter? _presenter;

    //[Content]
    //public UserControl Setting { get; set; }

    public ViewSettingFlyout()
    {
    }

    protected override Control CreatePresenter()
    {
        _presenter = new FlyoutPresenter
        {
            MinWidth = 100,
            Content = new CurveControlSetting() 
        };

        return _presenter;
    }
}

public partial class CurveControl : UserControl
{
    ViewSettingFlyout? _setting = null;
    Point ?_prev_point = null;
    bool _hover_point = false;
    bool _snap_point = false;

    public CurveControl()
    {
        InitializeComponent();
        ZIndex = 1;

        DataContext = new CurveControlModel(this);
    }

    public void ClearCurve() { view.ClearCurve(); }

    public void AddCurve(ICurve obj) { view.AddCurve(obj); }

    public void UpdateView() { view.InvalidateVisual(); }

    public void Setting()
    {
        _setting = Resources["key_view_setting"] as ViewSettingFlyout;
        _setting.ShowAt(this);
    }

    public bool HoverPoint
    {
        get { return _hover_point; }
        set
        {
            _hover_point = value;
            if (!_hover_point)
            {
                view.HoverPoint = null;
                view.InvalidateMeasure();
            }
        } 
    }

    public bool SnapPoint
    {
        get { return _snap_point; }
        set
        {
            _snap_point = value;
            if(!_snap_point)
            {
                view.SnapPoint = null;
                view.InvalidateVisual();
            }
        }
    }

    public void ResetScale()
    {
        view_scale.Value = 0;
    }

    public double ViewScale
    {
        get
        {
            double s = Common.SliderMap(view.ViewScale, 0.001, 100);
            return s;
        }

        set
        {
            double s = Common.ScopeMap(value, 0.001, 100);
            view.ViewScale = s;
        }
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);

        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            _prev_point = e.GetPosition(this);
        }

    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);

        _prev_point = null;

        if(_setting != null && _setting.IsOpen) {
            _setting.Hide();
        }
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);

        if (_prev_point != null && e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            var pos = e.GetPosition(this);
            view.Offset += pos - (Point)_prev_point;
            _prev_point = pos;
            view.InvalidateVisual();
        }

        if(HoverPoint)
        {
            view.HoverPoint = e.GetPosition(view);
            view.InvalidateVisual();
        }

        if(SnapPoint)
        {
            view.SnapPoint = e.GetPosition(view);
            view.InvalidateVisual();
        }
    }

    protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
    {
        base.OnPointerWheelChanged(e);

        double s = view.ViewScale;
        var m = (CurveControlModel)DataContext;
        m.ViewScale += e.Delta.Y * 0.1;

        var pos = e.GetPosition(this);
        var oft = view.Offset - pos;
        view.Offset = (view.Offset - pos) * view.ViewScale / s + pos;

        InvalidateVisual();
    }
}

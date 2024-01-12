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
            MaxWidth = 200,
            Content = new CurveControlSetting() 
        };

        return _presenter;
    }
}

public partial class CurveControl : UserControl
{
    private double _view_scale = 0;

    public CurveControl()
    {
        InitializeComponent();
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);
    }

    protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
    {
        base.OnPointerWheelChanged(e);

        //ViewScale = view.ViewScale;
    }

    public void ClearCurve() { view.ClearCurve(); }

    public void AddCurve(ICurve obj) { view.AddCurve(obj); }

    public void UpdateView() { view.InvalidateVisual(); }

    public void Setting()
    {
        var fly = Resources["key_view_setting"] as ViewSettingFlyout;
        fly.ShowAt(this);
    }

    public void Test()
    {

    }

    public void ResetScale()
    {
        view_scale.Value = 0; 
    }

    public double ViewScale{
        set
        {
            _view_scale = value;
            view.ViewScale = Common.ScopeMap(-value, 0.001, 1000);
        }

        get
        {
            return _view_scale;
        }
    }
}

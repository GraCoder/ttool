using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using tfcurve.curve;

namespace tfcurve;

public partial class CurveControl : UserControl
{
    public CurveControl()
    {
        InitializeComponent();
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);

        view_setting.Opacity = view_setting.IsPointerOver ? 1.0 : 0.0;
        view_scale.Opacity = view_scale.IsPointerOver ? 1.0 : 0.0;
        view_center.Opacity = view_center.IsPointerOver ? 1.0 : 0.0;
        view_reset_scale.Opacity = view_reset_scale.IsPointerOver ? 1.0 : 0.0;
    }

    public void ClearCurve() { view.ClearCurve(); }

    public void AddCurve(ICurve obj) { view.AddCurve(obj); }

    public void UpdateView() { view.InvalidateVisual(); }

}
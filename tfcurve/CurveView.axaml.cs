using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Reactive;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Microsoft.CodeAnalysis.Scripting.Hosting;
using ReactiveUI;
using Pen = Avalonia.Media.Pen;
using Point = Avalonia.Point;
using Size = Avalonia.Size;
using ICurve = tfcurve.curve.ICurve;
using CommunityToolkit.Mvvm.ComponentModel;

namespace tfcurve;

public partial class CurveView : UserControl
{
    private int _unit_idx = 0;

    private Point _offset;
    private double _grid = 180;
    private double _scale = 1.0;
    private double _unit = 180;
    private double _yscale = 1.0;

    private List<ICurve> _curves = [];

    public double GridScale
    {
        get { return _grid / 180.0; }
        set
        {
            _grid = value * 180.0;
            InvalidateVisual();
        }
    }

    public int UnitIndex
    {
        get { return _unit_idx; }
        set
        {
            _unit_idx = value;
            if (value == 0)
            {
                _unit = _grid;
            }
            else if (value == 1)
            {
                _unit = _grid / (Math.PI / 2.0);
            }

            InvalidateVisual();
        }
    }

    public Point Offset
    {
        set
        {
            _offset = value;
            InvalidateVisual();
        }
        get { return _offset; }
    }

    public double ViewScale { get { return _scale; } set { _scale = value; InvalidateVisual(); } }

    public double YScale
    {
        get { return _yscale; }
        set { _yscale = value; InvalidateMeasure(); }
    }

    public void ResetCenter()
    {
        var w = Bounds.Size.Width;
        var h = Bounds.Size.Height;
        _offset = new Point(w / 2, h / 2);

        InvalidateVisual();
    }

    public CurveView()
    {
        InitializeComponent();

        this.SizeChanged += ViewSizeChanged;
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        return base.MeasureOverride(availableSize);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        return base.ArrangeOverride(finalSize);
    }

    void ViewSizeChanged(object sender, SizeChangedEventArgs e)
    {
        var sz = e.NewSize;
        _offset = new Point(sz.Width / 2, sz.Height / 2);
    }

    public new IBrush? Background
    {
        get { return GetValue(BackgroundProperty); }
        set { SetValue(BackgroundProperty, value); }
    }

    public sealed override void Render(DrawingContext context)
    {
        base.Render(context);

        double w = Bounds.Size.Width;
        double h = Bounds.Size.Height;

        double interval = _grid;
        if (_scale > 1.0)
        {
            double s = 1.0 + (_scale - 1.0) % 2;
            interval = s * _grid;
        }
        else
        {
            double r = Math.Floor(-Math.Log(_scale) / Math.Log(3));
            var f = 1.0 / Math.Pow(3, r) - _scale;
            f /= (1.0 / Math.Pow(3, r) - (1.0 / Math.Pow(3, r + 1)));
            interval = (1 - f) * _grid / 3.0 * 2.0 + _grid / 3.0;
        }

        int col = (int)(w / interval + 1);
        int row = (int)(h / interval + 1);

        var fx = _offset.X % interval;
        var fy = _offset.Y % interval;

        var gridColor = new Avalonia.Media.Color(255, 200, 200, 200);
        var pen = new Pen(new SolidColorBrush(gridColor));
        for (int i = 0; i < row; i++)
        {
            var dy = fy + interval * i;
            var cy = Math.Round(dy) + 0.5;
            context.DrawLine(pen, new Point(0, cy), new Point(w, cy));

            if (Math.Abs(dy - _offset.Y) > 1e-6)
            {
                var s = String.Format("{0:F3}", py2visual(dy));
                var ci = CultureInfo.GetCultureInfo("en-US");
                var tf = new Typeface(FontFamily);
                var text = new FormattedText(s, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, tf, 16, new SolidColorBrush(Colors.Black));
                context.DrawText(text, new Point(0, cy));
            }

        }

        for (int i = 0; i < col; i++)
        {
            var dx = fx + interval * i;
            var cx = Math.Round(dx) + 0.5;
            context.DrawLine(pen, new Point(cx, 0), new Point(cx, h));

            if (Math.Abs(dx - _offset.X) > 1e-6)
            {
                var s = String.Format("{0:F3}", px2visual(dx));
                var ci = CultureInfo.GetCultureInfo("en-US");
                var tf = new Typeface(FontFamily);
                var text = new FormattedText(s, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, tf, 16, new SolidColorBrush(Colors.Black));
                context.DrawText(text, new Point(cx, 0));
            }
        }

        context.DrawLine(new Pen(new SolidColorBrush(Colors.Red), 2),
            new Point(0, Math.Round(_offset.Y)), new Point(w, Math.Round(_offset.Y)));

        var g = new Avalonia.Media.Color(255, 0, 255, 0);
        context.DrawLine(new Pen(new SolidColorBrush(g), 2),
            new Point(Math.Round(_offset.X), 0), new Point(Math.Round(_offset.X), h));

        drawFuns(context);
    }

    public void ClearCurve() { _curves.Clear(); }

    public void AddCurve(ICurve obj)
    {
        _curves.Add(obj);

        InvalidateVisual();
    }

    double px2visual(double px)
    {
        return (px - _offset.X) / _unit / _scale;
    }

    double py2visual(double py)
    {
        return (_offset.Y - py) / _unit / _scale;
    }

    double visual2py(double y)
    {
        var f = y * _unit * _scale * YScale;
        return Math.Round(-f + _offset.Y) + 0.5;
    }

    void drawFuns(DrawingContext context)
    {
        var w = Bounds.Size.Width;
        var h = Bounds.Size.Height;
        for (int j = 0; j < _curves.Count; j++)
        {
            _curves[j].DrawCurve(context, 0.5, w, px2visual, visual2py);
        }
    }
}

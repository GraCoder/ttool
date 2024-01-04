using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media;
using Microsoft.VisualBasic;
using ReactiveUI;

namespace tfcurve.curve;

internal class DepthModel : ViewModelBase
{

}

internal class DepthCurve : ViewModelBase, ICurve 
{
    public bool NearMode {  get; set; } = true;
    public double Near { get; set; } = 1.0;
    public double Far { get; set; } = 100.0;

    private double _test_input = 0;
    private double _test_output = 0;

    public double TestInput
    {
        get {  return _test_input; }
        set { _test_input = value; TestOutput = this.value(_test_input); }
    }

    public double TestOutput
    {
        get { return _test_output; }
        set { this.RaiseAndSetIfChanged(ref _test_output, value); }
    }

    private double value(double v)
    {
        if (NearMode)
            return (Near * Far) / (Near - Far) / v - Far / (Near - Far);

        return (2 * Near * Far) / (Near - Far) / v - (Near + Far) / (Near - Far);
    }

    public void DrawCurve(DrawingContext context, double xmin, double xmax, Func<double, double> px2v, Func<double, double> v2py)
    {
        var polyline = new PolylineGeometry();
        for(; xmin < xmax; xmin++)
        {
            var v = px2v(xmin);
            if (NearMode && v < 0)
                continue;
            
            v = value(v);
            var y = v2py(v);

            polyline.Points.Add(new Avalonia.Point(xmin, y));
        }

        var brush = new SolidColorBrush(Colors.Black);
        context.DrawGeometry(null, new Pen(brush), polyline);
    }

}

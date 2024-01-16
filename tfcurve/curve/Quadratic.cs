using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media;

namespace tfcurve.curve;

internal class Quadratic : ICurve
{
    public double A { get; set; } = 1.0;
    public double B { get; set; } = 0.0;
    public double C { get; set; } = 0.0;

    public double Value(double x)
    {
        return A * x * x + B * x + C;
    }

    public void DrawCurve(DrawingContext context, double xmin, double xmax, Func<double, double> px2v, Func<double, double> v2py)
    {
        var polyline = new PolylineGeometry();
        for(; xmin < xmax; xmin++)
        {
            var v = px2v(xmin);
            v = Value(v);
            var y = v2py(v);

            polyline.Points.Add(new Avalonia.Point(xmin, y));
        }

        var brush = new SolidColorBrush(Colors.Black);
        context.DrawGeometry(null, new Pen(brush), polyline);
    }
}

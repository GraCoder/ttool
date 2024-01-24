using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;

namespace tfcurve.curve;

public interface ICurve
{
    public double Value(double x);

    public void DrawCurve(DrawingContext context, double xmin, double xmax, Func<double, double> px2v, Func<double, double> v2py);

    public Window? CreateParaSet() { return null; } 
}

public class SimpleCurve : ICurve
{
    protected delegate double CurveFun(double v); 
    protected CurveFun? _curveFun;

    public double Value(double x)
    {
        return _curveFun(x);
    }

    virtual public void DrawCurve(DrawingContext context, double xmin, double xmax, Func<double, double> px2v, Func<double, double> v2py)
    {
        if (_curveFun == null)
            return;

        var polyline = new PolylineGeometry();
        polyline.Points.Clear();
        for(; xmin < xmax; xmin++)
        {
            var v = px2v(xmin);
            v = _curveFun(v);
            var y = v2py(v);

            polyline.Points.Add(new Avalonia.Point(xmin, y));
        }

        var brush = new SolidColorBrush(Colors.Black);
        context.DrawGeometry(brush, new Pen(brush), polyline);
    }
}

public class SinCurve : SimpleCurve
{
    public SinCurve()
    {
        _curveFun = Math.Sin;
    }
}

public class CosCurve : SimpleCurve 
{
    public CosCurve()
    {
        _curveFun = Math.Cos;
    }
}

public class TanCurve : SimpleCurve
{
    public TanCurve()
    {
        _curveFun = Math.Tan;
    }

    public sealed override void DrawCurve(DrawingContext context, double xmin, double xmax, Func<double, double> px2v, Func<double, double> v2py)
    {
        var poly = new PolylineGeometry();
        var brush = new SolidColorBrush(Colors.Black);
        for (; xmin < xmax; xmin++)
        {
            var v = px2v(xmin);
            v = Math.Tan(v);
            var y = v2py(v);

            if(y < 0 || y > 10000)
            {
                context.DrawGeometry(null, new Pen(brush), poly);
                poly = new PolylineGeometry();
                continue;
            }
            poly.Points.Add(new Avalonia.Point(xmin, y));
        }

        context.DrawGeometry(null, new Pen(brush), poly);
    }
}

public class TestCurve : SimpleCurve
{
    double test(double v)
    {
        return 0;
    }

    public TestCurve()
    {
        _curveFun = test;
    }

}

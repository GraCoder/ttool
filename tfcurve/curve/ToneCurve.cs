using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media;

namespace tfcurve.curve
{
    public class ReinhardToneCurve : ICurve
    {
        public void DrawCurve(DrawingContext context, double xmin, double xmax, Func<double, double> px2v, Func<double, double> v2py)
        {
            var polyline = new PolylineGeometry();
            for (; xmin < xmax; xmin++)
            {
                var v = px2v(xmin);
                if (v < 0.00001)
                    continue;

                v = v / (v + 1.0);
                var y = v2py(v);

                polyline.Points.Add(new Avalonia.Point(xmin, y));
            }

            var brush = new SolidColorBrush(Colors.Black);
            context.DrawGeometry(null, new Pen(brush), polyline);
        }
    }

    public class CEToneCurve : ICurve
    {
        public void DrawCurve(DrawingContext context, double xmin, double xmax, Func<double, double> px2v, Func<double, double> v2py)
        {
            const double adapted_lum = 1.0;
            var polyline = new PolylineGeometry();
            for (; xmin < xmax; xmin++)
            {
                var v = px2v(xmin);
                if (v < 0.0001)
                    continue;

                v = 1 - Math.Exp(-adapted_lum * v);
                var y = v2py(v);

                polyline.Points.Add(new Avalonia.Point(xmin, y));
            }

            var brush = new SolidColorBrush(Colors.Black);
            context.DrawGeometry(null, new Pen(brush), polyline);
        }
    }


    public class FilmicToneCurve : ICurve
    {
        double value(double x)
        {
            const float A = 0.22f;
            const float B = 0.30f;
            const float C = 0.10f;
            const float D = 0.20f;
            const float E = 0.01f;
            const float F = 0.30f;

            return ((x * (A * x + C * B) + D * E) / (x * (A * x + B) + D * F)) - E / F;
        }

        public void DrawCurve(DrawingContext context, double xmin, double xmax, Func<double, double> px2v, Func<double, double> v2py)
        {
            const double adapted_lum = 1.0;
            double white = value(11.2);
            var polyline = new PolylineGeometry();
            for (; xmin < xmax; xmin++)
            {
                var v = px2v(xmin);
                if (v < 0.0001)
                    continue;


                v = value(1.6 * adapted_lum * v) / white;
                double y = v2py(v);

                polyline.Points.Add(new Avalonia.Point(xmin, y));
            }

            var brush = new SolidColorBrush(Colors.Black);
            context.DrawGeometry(null, new Pen(brush), polyline);
        }
    }
}

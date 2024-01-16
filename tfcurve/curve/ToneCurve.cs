using System;
using Avalonia.Media;
using Pen = Avalonia.Media.Pen;

namespace tfcurve.curve
{
    public class ReinhardToneCurve : ICurve
    {
        public double Value(double x)
        {
            return x / (x + 1.0);
        }

        public void DrawCurve(DrawingContext context, double xmin, double xmax, Func<double, double> px2v, Func<double, double> v2py)
        {
            var polyline = new PolylineGeometry();
            for (; xmin < xmax; xmin++)
            {
                var v = px2v(xmin);
                if (v < 0.00001)
                    continue;

                v = Value(v);
                var y = v2py(v);

                polyline.Points.Add(new Avalonia.Point(xmin, y));
            }

            var brush = new SolidColorBrush(Colors.Black);
            context.DrawGeometry(null, new Pen(brush), polyline);
        }
    }

    public class CEToneCurve : ICurve
    {
        public double Value(double x)
        {
            return 1 - Math.Exp(x);
        }

        public void DrawCurve(DrawingContext context, double xmin, double xmax, Func<double, double> px2v, Func<double, double> v2py)
        {
            const double adapted_lum = 1.0;
            var polyline = new PolylineGeometry();
            for (; xmin < xmax; xmin++)
            {
                var v = px2v(xmin);
                if (v < 0.0001)
                    continue;

                v = Value(-adapted_lum * v);
                var y = v2py(v);

                polyline.Points.Add(new Avalonia.Point(xmin, y));
            }

            var brush = new SolidColorBrush(Colors.Black);
            context.DrawGeometry(null, new Pen(brush), polyline);
        }
    }


    public class FilmicToneCurve : ICurve
    {
        public double Value(double x)
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
            double white = Value(11.2);
            var polyline = new PolylineGeometry();
            for (; xmin < xmax; xmin++)
            {
                var v = px2v(xmin);
                if (v < 0.0001)
                    continue;


                v = Value(1.6 * adapted_lum * v) / white;
                double y = v2py(v);

                polyline.Points.Add(new Avalonia.Point(xmin, y));
            }

            var brush = new SolidColorBrush(Colors.Black);
            context.DrawGeometry(null, new Pen(brush), polyline);
        }
    }

    public class ACEToneCurve : ICurve
    {
        public double Value(double x)
        {
            const float A = 2.51f;
            const float B = 0.03f;
            const float C = 2.43f;
            const float D = 0.59f;
            const float E = 0.14f;

            return (x * (A * x + B)) / (x * (C * x + D) + E);
        }

        public void DrawCurve(DrawingContext context, double xmin, double xmax, Func<double, double> px2v, Func<double, double> v2py)
        {
            const double adapted_lum = 1.0;
            var polyline = new PolylineGeometry();
            for (; xmin < xmax; xmin++)
            {
                var v = px2v(xmin);
                if (v < 0.0001)
                    continue;


                v = Value(adapted_lum * v);
                double y = v2py(v);

                polyline.Points.Add(new Avalonia.Point(xmin, y));
            }

            var brush = new SolidColorBrush(Colors.Black);
            context.DrawGeometry(null, new Pen(brush), polyline);
        }
    }
}

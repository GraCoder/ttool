using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media;

namespace tfcurve.curve
{
    internal class GammarCurve : ICurve
    {
        public double Value(double v)
        {
            if (v < 0.0031308)
                return 12.92 * v;

            return 1.055 * Math.Pow(v, 1.0 / 2.4) - 0.055;
        }

        public void DrawCurve(DrawingContext context, double xmin, double xmax, Func<double, double> px2v, Func<double, double> v2py)
        {
            var polyline = new PolylineGeometry();
            for (; xmin < xmax; xmin++)
            {
                var v = px2v(xmin);
                if (v < 0.00001 || v > 1)
                    continue;
                v = Value(v);
                var y = v2py(v);

                polyline.Points.Add(new Avalonia.Point(xmin, y));
            }

            var brush = new SolidColorBrush(Colors.Black);
            context.DrawGeometry(null, new Pen(brush), polyline);
        }
    }
}

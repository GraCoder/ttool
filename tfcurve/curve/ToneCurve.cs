using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media;

namespace tfcurve.curve
{
    public class ReinhardToneFun : ICurve
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

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media;
using CodingSeb.ExpressionEvaluator;
using ReactiveUI;

namespace tfcurve.curve
{
    internal class PowCurve : CurveModel, ICurve
    {
        string _exps;
        double _exp;
        string _except_string;

        public double Exp { get { return _exp; } set { this.RaiseAndSetIfChanged(ref _exp, value); } }

        public string ExceptString { get { return _except_string; } set { this.RaiseAndSetIfChanged(ref _except_string, value); } }

        public string ExpString
        {
            get { return _exps; }
            set
            {
                _exps = value;
                var eva = new ExpressionEvaluator();
                double v = Exp;
                try
                {
                    var ret = eva.Evaluate(value);
                    v = Convert.ToDouble(ret);
                    ExceptString = "";
                }
                catch (Exception ex)
                {
                    ExceptString = ex.Message;
                }

                Exp = v;
                RequestRedraw();
            }
        }

        public PowCurve()
        {
            Exp = 2.0; 
            _exps = Exp.ToString();
        }

        public double Value(double v)
        {
            if (Exp < 0)
            {
                if (v == 0)
                    return double.PositiveInfinity;
            }
            else if(Exp < 1)
            {

            }

            return Math.Pow(v, Exp);
        }

        public void DrawCurve(DrawingContext context, double xmin, double xmax, Func<double, double> px2v, Func<double, double> v2py)
        {
            var brush = new SolidColorBrush(Colors.Black);
            var polyline = new PolylineGeometry();
            for (; xmin < xmax; xmin++)
            {
                var v = px2v(xmin);
                if (Exp < 0 && v == 0)
                {
                    if (polyline.Points.Count > 1)
                    {
                        context.DrawGeometry(null, new Pen(brush), polyline);
                        polyline = new PolylineGeometry();
                    }
                    continue;
                }
                v = Value(v);
                if(double.IsNaN(v))
                {
                    if (polyline.Points.Count > 1)
                    {
                        context.DrawGeometry(null, new Pen(brush), polyline);
                        polyline = new PolylineGeometry();
                    }
                    continue;
                }
                var y = v2py(v);

                polyline.Points.Add(new Avalonia.Point(xmin, y));
            }

            context.DrawGeometry(null, new Pen(brush), polyline);
        }

        public Avalonia.Controls.Window? CreateParaSet()
        {
            var dlg = new curve.page.PowSetting();
            dlg.DataContext = this;
            return dlg;
        }

    }
}

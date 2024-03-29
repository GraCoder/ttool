﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media;
using CodingSeb.ExpressionEvaluator;
using ReactiveUI;

namespace tfcurve.curve
{
    public class ExpCurve : CurveModel, ICurve
    {
        bool _inverse = false;
        double _base;
        string _base_str;
        string _except_string;

        public bool Inverse { get { return _inverse; } set { _inverse = value; ViewUpdate(); } }

        public double Base { get { return _base; } set { this.RaiseAndSetIfChanged(ref _base, value); } }

        public string ExceptString { get { return _except_string; } set { this.RaiseAndSetIfChanged(ref _except_string, value); } }

        public string BaseString
        {
            get { return _base_str; }
            set
            {
                _base_str = value;
                var eva = new ExpressionEvaluator();
                double v = _base;
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

                Base = v;
                RequestRedraw();
            }
        }

        public ExpCurve()
        {
            Base = Math.E;
            _base_str = Base.ToString();
        }

        public double Value(double v)
        {
            if(Inverse)
            {
                return Math.Log(v) / Math.Log(Base);
            }
            return Math.Pow(Base, v);
        }

        public void DrawCurve(DrawingContext context, double xmin, double xmax, Func<double, double> px2v, Func<double, double> v2py)
        {
            var brush = new SolidColorBrush(Colors.Black);
            var polyline = new PolylineGeometry();

            for (; xmin < xmax; xmin++)
            {
                var v = px2v(xmin);

                if (Inverse && v < 0)
                    continue;

                v = Value(v);
                var y = v2py(v);

                if (y < 0 || y > maxh)
                {
                    if (polyline.Points.Count > 0)
                    {
                        polyline.Points.Add(new Avalonia.Point(xmin, y));
                        context.DrawGeometry(null, new Pen(brush), polyline);
                        polyline = new PolylineGeometry();
                    }
                    continue;
                }

                polyline.Points.Add(new Avalonia.Point(xmin, y));
            }

            context.DrawGeometry(null, new Pen(brush), polyline);
        }

        public void ResetBase()
        {
            Base = Math.E;
        }

        public Avalonia.Controls.Window? CreateParaSet()
        {
            var dlg = new curve.page.ExpSetting();
            dlg.DataContext = this;
            return dlg;
        }
    }
}

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

namespace tfcurve
{
    public partial class CurveView : UserControl
    {
        //public static readonly StyledProperty<IBrush?> BackgroundProperty = Border.BackgroundProperty.AddOwner<CurveView>();
        private int _unit_idx = 0;

        private Point _offset;
        private double _grid = 180;
        private double _scale = 1.0;
        private double _unit = 180;
        private double _yscale = 1.0;

        private bool _cap_mouse = false;
        private Point _prev_pos;

        private List<ICurve> _curves = [];

        public int GridScale {  
            get { 
                if(_grid >= 180)
                    return (int)(_grid / 180.0 * 12.5 + 37.5);
                else
                    return (int)(_grid / 180.0 * 62.5 - 12.5);
            }
            set
            {
                if (value >= 50)
                    _grid = (value - 37.5) / 12.5 * 180;
                else
                    _grid = (value + 12.5) / 62.5 * 180;

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

        public double YScale
        {
            get { return _yscale; }
            set { _yscale = value; InvalidateMeasure(); }
        }

        public CurveView()
        {
            InitializeComponent();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            return base.MeasureOverride(availableSize);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _offset = new Point(finalSize.Width / 2, finalSize.Height / 2);

            return base.ArrangeOverride(finalSize);
        }

        public new IBrush ? Background
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

            context.DrawLine(new Pen(new SolidColorBrush(Colors.Lime), 2),
                new Point(Math.Round(_offset.X), 0), new Point(Math.Round(_offset.X), h));

            drawFuns(context);
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);

            _cap_mouse = true;
            _prev_pos = e.GetPosition(this);
        }

        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            base.OnPointerReleased(e);

            _cap_mouse = false;
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            base.OnPointerMoved(e);

            if(_cap_mouse)
            {
                var pos = e.GetPosition(this);
                _offset += (pos - _prev_pos);
                _prev_pos = pos;

                InvalidateVisual();
            }
        }

        protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
        {
            base.OnPointerWheelChanged(e);
            float f = 1.0f;
            if(e.Delta.Y > 0)
                f = 0.98f; 
            else
                f = 1.02f;

            var pos = e.GetPosition(this);
            var oft = _offset - pos;
            double s = _scale;
            _scale *= f;
            _scale = Math.Clamp(_scale, 0.001f, 1000000.0f);
            _offset = (_offset - pos) * _scale / s + pos;

            InvalidateVisual();
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

        public void ResetCenter()
        {
            var w = Bounds.Size.Width;
            var h = Bounds.Size.Height;
            _offset = new Point(w / 2, h / 2);

            InvalidateVisual();
        }

        public void ResetScale()
        {
            _scale = 1;

            InvalidateVisual();
        }

        public void Setting()
        {

        }
    }
}

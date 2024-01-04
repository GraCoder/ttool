using System;
using System.Numerics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;

namespace tfcurve
{
    public partial class CurveView : UserControl
    {
        //public static readonly StyledProperty<IBrush?> BackgroundProperty = Border.BackgroundProperty.AddOwner<CurveView>();
        private Point _offset;
        private float _scale = 1.0f;
        private const int _interval = 80;

        private bool _capMouse = false;
        private Point _prevPos;

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
            _offset = new Point((float)finalSize.Width / 2, (float)finalSize.Height / 2);

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

            var renderSize = Bounds.Size;
            var clr = new SolidColorBrush(Colors.Gray);
            //context.FillRectangle(clr, new Rect(renderSize));
            //context.PushTransform();

            int col = (int)renderSize.Width / _interval + 1;
            int row = (int)renderSize.Height / _interval + 1;

            var fx = _offset.X % _interval;
            var fy = _offset.Y % _interval;

            for(int i = 0; i < row; i++)
            {
                var y = fy + _interval * i;
                context.DrawLine(new Pen(clr), new Point(0, y), new Point(renderSize.Width, y));
            }

            for(int i = 0; i < col; i++)
            {
                var x  = fx + _interval * i;
                context.DrawLine(new Pen(clr), new Point(x, 0), new Point(x, renderSize.Height));
            }

            context.DrawLine(new Pen(new SolidColorBrush(Colors.Red)), 
                _offset - new Point(renderSize.Width, 0), _offset + new Point(renderSize.Width, 0));

            context.DrawLine(new Pen(new SolidColorBrush(Colors.Green)), 
                _offset - new Point(0, renderSize.Height), _offset + new Point(0, renderSize.Height));
        }


        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);

            _capMouse = true;
            _prevPos = e.GetPosition(this);
        }

        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            base.OnPointerReleased(e);

            _capMouse = false;
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            base.OnPointerMoved(e);

            if(_capMouse)
            {
                var pos = e.GetPosition(this);
                _offset += (pos - _prevPos);
                _prevPos = pos;

                InvalidateVisual();
            }
        }

        protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
        {
            base.OnPointerWheelChanged(e);
            if(e.Delta.Y > 0)
            {
                Console.WriteLine("1111");
            }
            else
            {
                Console.WriteLine("22222");
            }
        }
    }
}

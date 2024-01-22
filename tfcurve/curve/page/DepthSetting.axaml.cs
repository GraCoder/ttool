using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace tfcurve.curve.page
{

    public partial class DepthSetting : Window
    {
        public DepthSetting()
        {
            InitializeComponent();

            var near = NerInput.GetObservable(NumericUpDown.ValueProperty);
            near.Subscribe(value => this.ReqRedraw(value));

            var far = FarInput.GetObservable(NumericUpDown.ValueProperty);
            far.Subscribe(value => this.ReqRedraw(value));

            var md = NerMode.GetObservable(CheckBox.IsCheckedProperty);
            md.Subscribe(value => this.ReqRedraw(0));
        }

        public void ReqRedraw(decimal? v)
        {
            var cm = DataContext as CurveModel;
            if(cm != null)
            {
                cm.RequestRedraw();
            }
        }
    }

}
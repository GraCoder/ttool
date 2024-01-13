using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using ReactiveUI;

namespace tfcurve
{
    internal class CurveControlModel : ViewModelBase
    {
        CurveControl _control;

        double _view_scale = 0;

        public CurveControlModel(CurveControl ctl)
        {
            _control = ctl;
        }

        public double ViewScale
        {
            get { return _view_scale; }
            set { 
                _control.ViewScale = value;
                this.RaiseAndSetIfChanged(ref _view_scale, value); 
            }
        }

    }
}

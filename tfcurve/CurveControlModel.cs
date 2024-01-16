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
        double _grid_scale = 0;
        double _y_scale = 0;

        public CurveControlModel(CurveControl ctl)
        {
            _control = ctl;
        }

        public int UnitIndex
        {
            set { _control.view.UnitIndex = value; }
            get { return _control.view.UnitIndex; }
        }

        public bool HoverPoint
        {
            set { _control.HoverPoint = value; }
            get { return _control.HoverPoint; }
        }

        public bool SnapPoint
        {
            set { _control.SnapPoint = value; }
            get { return _control.SnapPoint; }
        }

        public double ViewScale
        {
            get { return _view_scale; }
            set { 
                _control.ViewScale = value;
                this.RaiseAndSetIfChanged(ref _view_scale, value); 
            }
        }

        public double YScale
        {
            get { return _y_scale; }
            set
            {
                _control.view.YScale = Common.ScopeMap(value, 0.01, 100);
                this.RaiseAndSetIfChanged(ref _y_scale, value);
            } 
        }

        public void ResetYScale()
        {
            YScale = 0;
        }

        public double GridScale
        {
            get { return _grid_scale; }

            set
            {
                _control.view.GridScale = Common.ScopeMap(value, 0.3, 3);
                this.RaiseAndSetIfChanged(ref _grid_scale, value);
            }
        }

        public void ResetGridScale()
        {
            GridScale = 0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using tfcurve.curve;

namespace tfcurve;

public class CurveObject : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    public required string Name { get; set; }

    public required ICurve Curve { get; set; }
}

public class MainWindowModel : ViewModelBase
{
    private ObservableCollection<CurveObject> _curves;

    public MainWindowModel()
    {
        _curves = new ObservableCollection<CurveObject> {
                new CurveObject{ Name = "Pow(X^n)", Curve = new PowCurve() },
                new CurveObject{ Name = "Exp(E^x)", Curve = new ExpCurve() },
                new CurveObject{ Name = "Sin", Curve = new SinCurve() },
                new CurveObject{ Name = "Cos", Curve = new CosCurve() },
                new CurveObject{ Name = "Tan", Curve = new TanCurve() },
                new CurveObject{ Name = "Quadratic", Curve = new Quadratic() },
                new CurveObject{ Name = "Depth", Curve = new DepthCurve() },
                new CurveObject{ Name = "Reinhard Tone", Curve = new ReinhardToneCurve() },
                new CurveObject{ Name = "CE Tone", Curve = new CEToneCurve() },
                new CurveObject{ Name = "Filmic Tone", Curve = new FilmicToneCurve() },
                new CurveObject{ Name = "ACE Tone", Curve = new ACEToneCurve() },
                new CurveObject{ Name = "Gammar", Curve = new GammarCurve() },
                new CurveObject{ Name = "Test", Curve = new TestCurve() },
        };
    }

    public ObservableCollection<CurveObject> CurveFuns
    {
        get { return _curves; }
    }
}

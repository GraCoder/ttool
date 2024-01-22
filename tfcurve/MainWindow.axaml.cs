using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using CommunityToolkit.Mvvm.ComponentModel;
using tfcurve.curve;

namespace tfcurve;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        //_funs["Sin"] = new SinCurve();
        //_funs["Cos"] = new CosCurve();
        //_funs["Tan"] = new TanCurve();
        //_funs["Depth"] = new DepthCurve();
        //_funs["Reinhard Tone"] = new ReinhardToneCurve();
        //_funs["4"] = new TestObject();
        //_funs["5"] = new TestObject();
        //_funs["6"] = new TestObject();
        //_funs["7"] = new TestObject();

        //foreach(var iter in _funs)
        //{
        //    curve_list.Items.Add(iter.Key);
        //}
        curve_list.SelectionChanged += SelectCurve;
        //unitlist.SelectionChanged += UnitFun;
    }

    public void SelectCurve(object sender, SelectionChangedEventArgs arg)
    {
        curve_control.ClearCurve();
        var box = sender as ListBox;
        for (int i = 0; i < box.SelectedItems.Count; i++)
        {
            var item = box.SelectedItems[i];
            var cuv = item as CurveObject;
            if (cuv != null)
            {
                curve_control.AddCurve(cuv.Curve);
            }
        }
    }

    //public void UnitFun(object sender, SelectionChangedEventArgs arg)
    //{
    //    var box = sender as ComboBox;
    //    curvectl.SetUnit(box.SelectedIndex);
    //}

    public async void SetParam()
    {
        if (curve_list.SelectedItems.Count == 0)
            return;

        var item = curve_list.SelectedItems[0];
        if (item is not CurveObject)
            return;

        var curve = ((CurveObject)item).Curve;
        var dlg = curve.CreateParaSet(); 
        if(dlg != null)
        {
            var cm = curve as CurveModel;
            if(cm != null)
            {
                cm.ViewUpdate = curve_control.UpdateView;
            }
            dlg.ShowDialog(this);
        }
    }

    public void ResetGridScale()
    {
        //curvectl.GridScale = 50;
        //resetgrid.Value = 50;
    }

}
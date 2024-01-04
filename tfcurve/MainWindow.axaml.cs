using System.Linq;
using Avalonia.Controls;

namespace tfcurve;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var funs =  new string[]{ "1", "2", "3", "4", "5" };

        for(int i = 0; i < funs.Length; i++)
            funlist.Items.Add(funs[i]);
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Data.Converters;
using tfcurve.curve;

namespace tfcurve;

class Common
{
    public static double ScopeMap(double value, double min, double max)
    {
        double v = 0;
        if(value >= 0)
            v = Math.Tan(value / 400.0 * Math.PI) * (max - 1) + 1;
        else 
            v = -Math.Tan(-value / 400.0 * Math.PI) * (1 - min) + 1;
        return v;
    }

    public static double SliderMap(double value, double min, double max)
    {
        double v = 0;
        if (value >= 1)
            v = Math.Atan(value - 1) / Math.PI * 25;
        else
            v = Math.Atan(1 - value) / Math.PI * 25;
        return v;
    } 
}

public class SliderConverter : IValueConverter
{
    public double MinValue { get; set; } = 0.1;
    public double MaxValue { get; set; } = 10;

    object? IValueConverter.Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if(value is double)
            return Common.SliderMap((double)value, MinValue, MaxValue);

        throw new NotImplementedException();
    }

    object? IValueConverter.ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if(value is double)
            return Common.ScopeMap((double)value, MinValue, MaxValue);

        throw new NotImplementedException();
    }
}

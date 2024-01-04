using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Data.Converters;

namespace tfcurve;

class Common
{
    public static double ScopeMap(double value, double min, double max)
    {
        if (value >= 50)
            return  (max - 1) * (value - 50) / 50 + 1;

        return (1 - min) * value / 50 + min;
    }

    public static double SliderMap(double value, double min, double max)
    {
        if (value >= 1)
            return (value - 1)  / (max - 1) * 50 + 50;

        return (value - min) / (1 - min) * 50;
    } 
}

public class SliderConverter : IValueConverter
{
    public double MinValue { get; set; }
    public double MaxValue { get; set; }

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

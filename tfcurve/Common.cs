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
        value = value / 100.0;
        if(value >= 0)
        {
            v = Math.Pow(value, 2) * (max - 1) + 1;
        }
        else 
            v = -Math.Pow(value, 2) * (1 - min) + 1;
        return v;
    }

    public static double SliderMap(double value, double min, double max)
    {
        double v = 0;
        if (value >= 1)
        {
            v = Math.Sqrt((value - 1) / (max - 1));
        }
        else
        {
            v = -Math.Sqrt((1 - value) / (1 - min));
        }
        return v * 100.0;
    } 
}

public class SliderConverter : IValueConverter
{
    public double MinValue { get; set; } = 0.1;
    public double MaxValue { get; set; } = 10;

    public SliderConverter(double minValue, double maxValue)
    {
        MinValue = minValue;
        MaxValue = maxValue;
    } 

    object? IValueConverter.Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if(value is double)
        {
            double v = Common.ScopeMap((double)value, MinValue, MaxValue);
            if(parameter == "string")
                return string.Format("{0:N3}", v);

            return v;
        }

        throw new NotImplementedException();
    }

    object? IValueConverter.ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if(value is double)
            return Common.SliderMap((double)value, MinValue, MaxValue);

        throw new NotImplementedException();
    }
}

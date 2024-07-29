using AppControleFinanceiro.Models;
using System.Globalization;

namespace AppControleFinanceiro.Utils.Converters;
public class TransactionValueColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var transaction = (Transaction)value;
        if (transaction is null)
            return Colors.Black; ;
        if (transaction.Type == TransactionType.Income)
            return Color.FromArgb("#FF939E5A");
        else
            return Colors.Red;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
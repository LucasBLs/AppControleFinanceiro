using AppControleFinanceiro.Models;
using System.Globalization;

namespace AppControleFinanceiro.Utils.Converters;
public class TransactionValueConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var transaction = (Transaction)value;
        if (transaction is null)
            return 0m;
        if (transaction.Type == TransactionType.Income)
            return transaction.Value.ToString("C");
        else
            return $"- {transaction.Value:C}";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
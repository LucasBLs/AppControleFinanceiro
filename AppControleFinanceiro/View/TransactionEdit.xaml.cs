using AppControleFinanceiro.Models;
using AppControleFinanceiro.Repositories;
using CommunityToolkit.Mvvm.Messaging;
using System.Text;

namespace AppControleFinanceiro.View;

public partial class TransactionEdit : ContentPage
{
    private Transaction _transaction;

    private readonly ITransactionRepository _repository;
    public TransactionEdit(ITransactionRepository repository)
    {
        InitializeComponent();
        _repository = repository;
    }

    private void Button_Clicked_To_Save_Edit(object sender, EventArgs e)
    {
        if (!IsValidData())
            return;
        SaveTransactionInDatabase();

        Navigation.PopModalAsync();
        WeakReferenceMessenger.Default.Send<string>(string.Empty);
   }
    private void SaveTransactionInDatabase()
    {
        var transaction = new Transaction
        {
            Id = _transaction.Id,
            Name = EntryName.Text,
            Value = decimal.Parse(EntryValue.Text),
            Type = RadioIncome.IsChecked ? TransactionType.Income : TransactionType.Expense,
            Date = DatePickerDate.Date.ToLocalTime(),
        };

        _repository.Update(transaction);
    }
    public void SetTransactionToEdit(Transaction transaction)
    {
        _transaction = transaction;

        if (_transaction.Type == TransactionType.Income)
            RadioIncome.IsChecked = true;
        else
            RadioExpense.IsChecked = true;

        EntryName.Text = _transaction.Name;
        EntryValue.Text = _transaction.Value.ToString("C");
        DatePickerDate.Date = _transaction.Date.Date;
    }
    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        Navigation.PopModalAsync();
    }
    public bool IsValidData()
    {
        bool valid = true;
        var message = new StringBuilder();

        if (string.IsNullOrEmpty(EntryName.Text) || string.IsNullOrWhiteSpace(EntryName.Text))
        {
            message.AppendLine("O Nome deve ser preenchido.");
            valid = false;
        }

        if (string.IsNullOrEmpty(EntryValue.Text) || string.IsNullOrWhiteSpace(EntryValue.Text))
        {
            message.AppendLine("O Valor deve ser preenchido.");
            valid = false;
        }

        if (!string.IsNullOrEmpty(EntryValue.Text) && !decimal.TryParse(EntryValue.Text, out _))
        {
            message.AppendLine("O Valor é inválido.");
            valid = false;
        }

        if (valid is false)
        {
            LabelError.IsVisible = true;
            LabelError.Text = message.ToString();
        }

        return valid;
    }
}
using AppControleFinanceiro.Models;
using AppControleFinanceiro.Repositories;
using CommunityToolkit.Mvvm.Messaging;
using System.Text;

namespace AppControleFinanceiro.View;

public partial class TransactionAdd : ContentPage
{
    private readonly ITransactionRepository _repository;
    public TransactionAdd(ITransactionRepository repository)
	{
        InitializeComponent();
        _repository = repository;
    }

    private void Button_Clicked_Save(object sender, EventArgs e)
    {
        if (!IsValidData())
            return;
        SaveTransactionInDatabase();

        Navigation.PopModalAsync();
        WeakReferenceMessenger.Default.Send<string>(string.Empty);

        var countRegisters = _repository.GetAll().Count;
        App.Current.MainPage.DisplayAlert("Mensagem!", $"{countRegisters} - Registros no banco de dados.", "OK");
    }
    private void SaveTransactionInDatabase()
    {
        var transaction = new Transaction
        {
            Name = EntryName.Text,
            Value = decimal.Parse(EntryValue.Text),
            Type = RadioIncome.IsChecked ? TransactionType.Income : TransactionType.Expense,
            Date = DatePickerDate.Date.ToLocalTime(),
        };

        _repository.Add(transaction);
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

        if(valid is false)
        {
            LabelError.IsVisible = true;
            LabelError.Text = message.ToString();
        }

        return valid;
    }
}
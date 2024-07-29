using AppControleFinanceiro.Models;
using AppControleFinanceiro.Repositories;
using CommunityToolkit.Mvvm.Messaging;

namespace AppControleFinanceiro.View;

public partial class TransactionList : ContentPage
{
    private readonly ITransactionRepository _repository;
    public TransactionList(ITransactionRepository repository)
	{
        _repository = repository;

		InitializeComponent();

        Reload();
        WeakReferenceMessenger.Default.Register<string>(this, (e, msg) =>
        {
            Reload();
        });
	}
    private void Reload()
    {
        var items = _repository.GetAll();
        CollectionViewTransactions.ItemsSource = items;

        decimal income = items.Where(x => x.Type == Models.TransactionType.Income).Sum(i => i.Value);
        decimal expense = items.Where(x => x.Type == Models.TransactionType.Expense).Sum(i => i.Value);
        decimal balance = income - expense;

        LabelIncome.Text = income.ToString("C");
        LabelExpense.Text = expense.ToString("C");
        LabelBalance.Text = balance.ToString("C");
    }
    private void Button_Clicked_To_TransactionAdd(object sender, EventArgs e)
    {
        var transactionAdd = Handler.MauiContext.Services.GetService<TransactionAdd>();
        Navigation.PushModalAsync(transactionAdd);
    }
    private void TapGestureRecognizer_Tapped_To_TransactionEdit(object sender, TappedEventArgs e)
    {
        var grid = (Grid)sender;
        var gesture = (TapGestureRecognizer)grid.GestureRecognizers.FirstOrDefault();
        var transaction = (Transaction)gesture.CommandParameter;

        var transactionEdit = Handler.MauiContext.Services.GetService<TransactionEdit>();
        transactionEdit.SetTransactionToEdit(transaction);
        Navigation.PushModalAsync(transactionEdit);
    }
}
using AppControleFinanceiro.Models;
using LiteDB;

namespace AppControleFinanceiro.Repositories;
public class TransactionRepository : ITransactionRepository
{
    private readonly LiteDatabase _liteDatabase;
    private readonly string CollectionName = "transactions";
    public TransactionRepository(LiteDatabase database)
    {
        _liteDatabase = database;
    }

    public List<Transaction> GetAll()
    {
        return _liteDatabase.GetCollection<Transaction>(CollectionName)
          .Query()
          .OrderByDescending(x => x.Date)
          .ToList();
    }

    public void Add(Transaction transaction)
    {
        var collection = _liteDatabase.GetCollection<Transaction>(CollectionName);
        collection.Insert(transaction);
        collection.EnsureIndex(x => x.Date);
    }

    public void Update(Transaction transaction)
    {
        var collection = _liteDatabase.GetCollection<Transaction>(CollectionName);
        collection.Update(transaction);
    }

    public void Delete(Transaction transaction)
    {
        var collection = _liteDatabase.GetCollection<Transaction>(CollectionName);
        collection.Delete(transaction.Id);
    }
}
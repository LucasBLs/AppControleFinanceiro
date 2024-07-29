using AppControleFinanceiro.Repositories;
using AppControleFinanceiro.View;
using LiteDB;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace AppControleFinanceiro;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
			.ConfigureServices();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}

    public static MauiAppBuilder ConfigureServices(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton(new CultureInfo("pt-BR"));
        builder.Services.AddSingleton(CreateLiteDatabaseInstance);
        builder.Services.AddTransient<ITransactionRepository, TransactionRepository>();

        #region Register Views
        builder.Services.AddTransient<TransactionList>();
        builder.Services.AddTransient<TransactionAdd>();
        builder.Services.AddTransient<TransactionEdit>();
        #endregion

        return builder;
    }

    private static LiteDatabase CreateLiteDatabaseInstance(IServiceProvider serviceProvider)
    {
        string databaseFilePath = Path.Combine(FileSystem.AppDataDirectory, "database.db");
        return new LiteDatabase($"Filename={databaseFilePath};Connection=Shared;");
    }

}

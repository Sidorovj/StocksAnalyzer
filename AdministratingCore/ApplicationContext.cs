using System;
using Microsoft.EntityFrameworkCore;
using StockCore.Interfaces;
using StocksAnalyzer;
using StocksAnalyzer.Data;

namespace AdministratingCore
{
	class ApplicationContext : DbContext
	{
		private readonly IAdministrating m_admin;

		public DbSet<StockMarketLocation> StockMarketLocation;

		public ApplicationContext(DbContextOptions<ApplicationContext> options, IAdministrating admin) : base(options)
		{
			m_admin = admin;
		}

		internal void ExecuteCommand()
		{
			Console.WriteLine(@"Выберите, что сделать:
1. Загрузить акции, которых еще нет в бд
2. Загрузить коэффициенты и метрики, которых еще нет в БД

6. [WARN] Очистить таблицу Stock, загрузить туда новые записи
7. [WARN] Очистить таблицу метрик и коэффициентов, загрузить туда новые записи

123. Создать таблицы основные");

			while (true)
			{
				Console.WriteLine(@"Введите число и нажмите enter");
				var s = Console.ReadLine();
				if (int.TryParse(s, out int res))
					if (res == 1)
					{
						m_admin.CreateNewStocks();
						break;
					}
					else if (res == 2)
					{
						m_admin.CreateCoefficientsAndMetrics();
						break;
					}
					else if (res == 6)
					{
						m_admin.CreateNewStocks(true);
						break;
					}
					else if (res == 7)
					{
						m_admin.CreateCoefficientsAndMetrics(true);
					}
			}
		}
	}
}

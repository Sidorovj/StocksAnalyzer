using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using StocksAnalyzer;
using StocksAnalyzer.Helpers;

namespace Tests
{
	public class StockCoreTests
	{
		[Theory]
		[InlineData("a1", 1, StockMarketLocation.Russia, StockMarketCurrency.Rub)]
		public void TestSerializer(string name, double price, StockMarketLocation sml, StockMarketCurrency smc)
		{
			Serializer ser = new Serializer("tmp.dat");
			Stock st = new Stock(name, price, new StockMarket(sml, smc));
			ser.Serialize(st);

			var newSt = ser.Deserialize() as Stock;

			Assert.NotNull(newSt);
			Assert.Equal(name, st.Name);
			Assert.Equal(price, st.Price);
			Assert.Equal(sml, st.Market.Location);
			Assert.Equal(smc, st.Market.Currency);
		}

		[Theory]
		[InlineData(null)]
		[InlineData(-5000000)]
		[InlineData(50000)]
		public void TestStringParser_ToCuteStr(double? num)
		{
			var res = num.ToCuteStr();

			if (num == null)
				Assert.Equal("", res);
			else
			{
				Assert.False(string.IsNullOrWhiteSpace(res));
				Assert.Matches(new Regex(@"^-?\d+.\d\d(\s[TBM])?$"), res);
			}
		}

		[Theory]
		[InlineData("sin(3.0)", "sin")]
		[InlineData("hello abc(312)", "abc")]
		[InlineData("abc(312)", "cos")]
		public void TestStringParser_GetNumInBracketsForFunction(string where, string functName)
		{
			if (where.Contains(functName))
			{
				var res = StringParser.GetNumInBracketsForFunction(where, functName);
				Assert.False(string.IsNullOrEmpty(res));
			}
			else
			{
				Assert.Throws<ArgumentOutOfRangeException>(() => StringParser.GetNumInBracketsForFunction(where, functName));
			}
		}

		[Fact]
		public void TestStringParser_ParseCoefStrToDouble()
		{
			var res = "abc:312".ParseCoefStrToDouble();
			Assert.Equal(312, res);

			res = "abc:312}}%}%}%".ParseCoefStrToDouble();
			Assert.Equal(312, res);

			var lst = new List<string> { "1k", "1M", "1B", "1T" }.Select(s => s.ParseCoefStrToDouble());
			Assert.Equal(new List<double?> { 1000, 1000 * 1000, 1000.0 * 1000 * 1000, 1000.0 * 1000 * 1000 * 1000 }, lst);

			lst = new List<string> { "1,00.0.0", "2.0,0,0,0", "1,2", "1.3" }.Select(s => s.ParseCoefStrToDouble());
			Assert.Equal(new List<double?> { 1, 2, 1.2, 1.3 }, lst);

			lst = new List<string> { "", "n/a", "N/A", "-" }.Select(s => s.ParseCoefStrToDouble());
			Assert.Equal(new List<double?> { null, null, null, null }, lst);

			Assert.Throws<Exception>(() => "xy?".ParseCoefStrToDouble());
		}

		[Fact]
		public void TestCoefficient()
		{
			Assert.InRange(Coefficient.CoefficientList.Count, 1, 500);
			Assert.InRange(Coefficient.MetricsList.Count, 1, 500);

			var lst = Coefficient.CoefficientList;
			Assert.True(lst.All(c => !string.IsNullOrEmpty(c.Name)));
			Assert.True(lst.All(c => c.MetricWeight.Count > 0));
			Assert.True(lst.All(c => c.Normalize(1) != null));
		}

		[Fact]
		public void TestStockMarket()
		{
			Assert.Throws<KeyNotFoundException>(() => StockMarket.GetExchangeRates(StockMarketCurrency.Rub));
			Assert.InRange(StockMarket.GetExchangeRates(StockMarketCurrency.Eur), 20, 200);
			Assert.InRange(StockMarket.GetExchangeRates(StockMarketCurrency.Usd), 20, 200);
		}
	}

}

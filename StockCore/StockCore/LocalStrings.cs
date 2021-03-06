﻿using StocksAnalyzer.Helpers;

namespace StocksAnalyzer
{
	public static class LocalStrings
	{
		public static string RussianLanguage
		{
			get
			{
				switch (MainClass.Language)
				{
					case AvailableLanguages.Russian:
						return @"Русский";
					default:
						return @"Russian";
				}
			}
		}
		public static string EnglishLanguage
		{
			get
			{
				switch (MainClass.Language)
				{
					case AvailableLanguages.Russian:
						return @"Английский";
					default:
						return @"English";
				}
			}
		}
		public static string HeaderStocksButtonCaption
		{
			get
			{
				switch (MainClass.Language)
				{
					case AvailableLanguages.Russian:
						return @"Акции";
					default:
						return @"Stocks";
				}
			}
		}
	}
}

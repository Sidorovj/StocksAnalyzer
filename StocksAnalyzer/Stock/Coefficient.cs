using System;
using System.Collections.Generic;
using System.IO;

namespace StocksAnalyzer
{
    [Serializable]
    internal sealed class Coefficient
    {
        public string Name { get; private set; }
        public string Label { get; private set; }
        /// <summary>
        /// Справочная инфа (какое значение лучше, какое хуже)
        /// </summary>
        public string HelpDescription { get; private set; }

        public string SearchInHTML_Rus { get; private set; }
        public string SearchInHTML_USA { get; private set; }

        public static List<Coefficient> CoefficientList = new List<Coefficient>();

        private Coefficient()
        {

        }

        static Coefficient()
        {
            using (var reader = new StreamReader($"{Const.SettingsDirName}/{Const.CoefficientsSettings}"))
            {
                Dictionary<int, string> columnNumToName = null;
                while (!reader.EndOfStream)
                {
                    string[] data = reader.ReadLine()?.Split(';');
                    if (data?[0] == "Name")
                    {
                        columnNumToName = new Dictionary<int, string>(data.Length);
                        for (var i = 0; i < data.Length; i++)
                            columnNumToName.Add(i, data[i]);
                    }
                    else
                    {
                        CoefficientList.Add(ParseCoefficient(data, columnNumToName) ?? throw new ArgumentNullException($"Method {nameof(ParseCoefficient)} return null"));
                    }
                }
            }
        }

        private static Coefficient ParseCoefficient(string[] data, Dictionary<int, string> columnNumToName)
        {
            if (data == null || columnNumToName == null)
                throw new ArgumentNullException(data == null ? nameof(data) : nameof(columnNumToName));
            Coefficient coef = new Coefficient();
            for (var i = 0; i < data.Length; i++)
            {
                string value = data[i];
                switch (columnNumToName[i])
                {
                    case "Name":
                        coef.Name = value;
                        break;
                    case "Label":
                        coef.Label = value;
                        break;
                    case "SearchInHTML_Rus":
                        coef.SearchInHTML_Rus = value;
                        break;
                    case "SearchInHTML_USA":
                        coef.SearchInHTML_USA = value;
                        break;
                    case "HelpDescription":
                        coef.HelpDescription = value;
                        break;
                }
            }

            return coef;
        }
    }
}
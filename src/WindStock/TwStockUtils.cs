using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WindStock
{
    internal static class TwStockUtils
    {
        public static IEnumerable<TwseStockInfo> GetAllTwse()
        {
            var csvPath = Path.Combine(Directory.GetCurrentDirectory(), "Csv", "all_twse.csv");
            if (!File.Exists(csvPath))
            {
                return Enumerable.Empty<TwseStockInfo>();
            }

            return GetAllTwseImpl(csvPath);
        }

        private static IEnumerable<TwseStockInfo> GetAllTwseImpl(string csvPath)
        {
            using var stream = new StreamReader(csvPath);
            var columns = stream.ReadLine().Split(",").ToList();
            var properties = typeof(TwseStockInfo).GetProperties().ToDictionary(p => p.Name.ToLower());

            while (!stream.EndOfStream)
            {
                var model = new TwseStockInfo();
                var row = stream.ReadLine().Split(",");
                
                for (var i = 0; i < row.Length; i++)
                {
                    var column = columns[i].ToLower();
                    properties[column].SetValue(model, row[i]);
                }

                yield return model;
            }
        }
    }
}

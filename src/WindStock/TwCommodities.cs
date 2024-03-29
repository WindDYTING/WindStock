using System.Collections.Generic;
using System.Linq;

namespace WindStock
{
    public static class TwCommodities
    {
        public static IDictionary<string, TwseStockInfo> AllStocks { get; private set; }

        public static void Load()
        {
            AllStocks ??= TwStockUtils.GetAllTwse().ToDictionary(x => x.Code);
        }
    }
}

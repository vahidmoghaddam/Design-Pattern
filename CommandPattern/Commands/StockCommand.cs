using System;
using System.Collections.Generic;
using System.Text;

namespace CommandPattern
{
    public class StockCommand : AbstractTradeCommand
    {
        public override List<string> SubscribeISINs()
        {
            return new List<string>() { ISINPattern.TSEStock , ISINPattern.OTCStock };
        }
    }
}

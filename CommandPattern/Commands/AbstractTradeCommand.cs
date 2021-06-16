using System;
using System.Collections.Generic;
using System.Text;

namespace CommandPattern
{
    public abstract class AbstractTradeCommand : ITradeCommand
    {
        public TradeDTO DTO { get; set; }
        public abstract List<string> SubscribeISINs();
     
    }
}

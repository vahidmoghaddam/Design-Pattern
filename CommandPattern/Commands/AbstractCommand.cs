using System;
using System.Collections.Generic;
using System.Text;

namespace CommandPattern
{
    public abstract class AbstractCommand : ICommand
    {
        public TradeDTO DTO { get; set; }
        
        public abstract List<string> SubscribeISINs();

    }
}

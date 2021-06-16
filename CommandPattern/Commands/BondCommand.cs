using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommandPattern
{
    public class BondCommand : AbstractTradeCommand
    {

        public override List<string> SubscribeISINs()
        {
            return new List<string>() { ISINPattern.OTCBond };
        }
    }
}

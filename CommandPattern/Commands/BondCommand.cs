using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommandPattern
{
    public class BondCommand : AbstractCommand
    {
        public override List<string> SubscribeISINs()
        {
            return new List<string>() { "IRB3", "IRBE", };
        }
    }
}

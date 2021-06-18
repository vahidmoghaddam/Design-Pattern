
using System.Collections.Generic;

namespace CommandPattern
{
    public class ETFCommand : AbstractCommand
    {
        public override List<string> SubscribeISINs()
        {
            return new List<string>() { "IRT1", "IRT3",};
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CommandPattern
{
    public class TradeCommandFactory
    {
        public static Dictionary<List<string>, Func<ITradeCommand>> CommandCreators { get; private set; }

        static TradeCommandFactory()
        {
            CommandCreators = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => typeof(ITradeCommand).IsAssignableFrom(t) && t.IsInterface == false && t.IsAbstract == false)
                .Select(t => new Func<ITradeCommand>(() => Activator.CreateInstance(t) as ITradeCommand))
                .ToDictionary(f => f().SubscribeISINs());
        }

        public static ITradeCommand CreateInstance(TradeDTO dto)
        {
            var key = CommandCreators.Keys.Where(key => key.Contains(dto.ISIN.ISINAssetClassPart())).FirstOrDefault();
            return CommandCreators[key]();
        }

        public static ITradeCommand CreateCommand(TradeDTO dto)
        {
            var key = CommandCreators.Keys.Where(key => key.Contains(dto.ISIN.ISINAssetClassPart())).FirstOrDefault();
            var command = CommandCreators[key]();
            command.AddDTO(dto);
            return command;

        }

    }
}

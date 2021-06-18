using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CommandPattern
{
    public class TradeCommandFactory
    {
        public static Dictionary<List<string>, Func<ICommand>> CommandCreators { get; private set; }

        static TradeCommandFactory()
        {
            CommandCreators = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => typeof(ICommand).IsAssignableFrom(t) && t.IsInterface == false && t.IsAbstract == false)
                .Select(t => new Func<ICommand>(() => Activator.CreateInstance(t) as ICommand))
                .ToDictionary(f => f().SubscribeISINs());
        }

        public static ICommand CreateCommand(TradeDTO dto)
        {
            var key = CommandCreators.Keys.Where(key => key.Contains(dto.ISIN.ExtractSecuritiesType())).FirstOrDefault();
            var command = CommandCreators[key]();
            command.AddDTO(dto);
            return command;

        }

    }
}

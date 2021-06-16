using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace CommandPattern
{
    class Program
    {
        static void Main()
        {
            ServiceProvider serviceProvider = SetupServiceProvider();
            
            var _mediator = serviceProvider.GetRequiredService<IMediator>();

            var tradeISIN = "IRO3ZOBZ0001";
            Console.WriteLine($"trade for isin {tradeISIN} published");
            var command = TradeCommandFactory.CreateCommand(new TradeDTO() { ISIN = tradeISIN });
            _mediator.Send(command);

            tradeISIN = "IRB3TB9603A1";
            Console.WriteLine($"trade for isin {tradeISIN} published");
            command = TradeCommandFactory.CreateCommand(new TradeDTO() { ISIN = tradeISIN });
            _mediator.Send(command);

            tradeISIN = "IRO1PNES0001";
            Console.WriteLine($"trade for isin {tradeISIN} published");
            command = TradeCommandFactory.CreateCommand(new TradeDTO() { ISIN = tradeISIN });
            _mediator.Send(command);
           
            Console.ReadLine();
        }

        private static ServiceProvider SetupServiceProvider()
        {
            return new ServiceCollection()
                .AddMediatR(Assembly.GetExecutingAssembly())
                .BuildServiceProvider();
        }
    }
}

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
            
            var tradeQuantity = 10000;
            var tradePrice = 80;
            var tradeDate = DateTime.Now;
           
            //stock isin
            var tradeISIN = "IRO3ZOBZ0001";
            var command = CreateCommand(tradeQuantity, tradePrice, tradeDate, tradeISIN);
            _mediator.Send(command);

            //bond isin
            tradeISIN = "IRB3TB9603A1";
            command = CreateCommand(tradeQuantity, tradePrice, tradeDate, tradeISIN);
            _mediator.Send(command);

            //etf isin
            tradeISIN = "IRT1SKDF0001";
            command = CreateCommand(tradeQuantity, tradePrice, tradeDate, tradeISIN);
            _mediator.Send(command);

            //stock isin
            tradeISIN = "IRO1PNES0001";
            command = CreateCommand(tradeQuantity, tradePrice, tradeDate, tradeISIN);
            _mediator.Send(command);

            //bond isin
            tradeISIN = "IRBEPO1301C1";
            command = CreateCommand(tradeQuantity, tradePrice, tradeDate, tradeISIN);
            _mediator.Send(command);

            //etf isin
            tradeISIN = "IRT3SSAF0001";
            command = CreateCommand(tradeQuantity, tradePrice, tradeDate, tradeISIN);
            _mediator.Send(command);

            Console.ReadLine();
        }

        private static ICommand CreateCommand(int tradeQuantity, int tradePrice, DateTime tradeDate, string tradeISIN)
        {
            ICommand command;
            Console.WriteLine($"traded security data for isin {tradeISIN} is received");
            command = TradeCommandFactory.CreateCommand(new TradeDTO(tradeISIN, tradePrice, tradeQuantity, tradeDate));
            return command;
        }
       
        private static ServiceProvider SetupServiceProvider()
        {
            // register MediatorR
            return new ServiceCollection()
                .AddMediatR(Assembly.GetExecutingAssembly())
                .BuildServiceProvider();
        }
    }
}

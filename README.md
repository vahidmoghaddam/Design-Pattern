# Implementing Command Pattern and Creating Command By Using Strategy And Factory Pattern In .Net Core
This is an example how to implement [command pattern](https://en.wikipedia.org/wiki/Command_pattern) with creating commands by using [Strategy Pattern](https://en.wikipedia.org/wiki/Strategy_pattern) and [Factory Method Pattern](https://en.wikipedia.org/wiki/Factory_method_pattern) pattern, and this is an approach to creating command in the runtime.
The project also uses some new features in C# such as [record](https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-9#record-types) and [Default Interface Method](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-8.0/default-interface-methods) and interesting third party library like **MedaitorR**.

# Problem Statement
ٍSecurities that are traded in the market have their own international securities identification number (ISN). An ISIN is a 12-digit alphanumeric code that uniquely identifies a specific 
securities. In the Iranian capital market, the first 4 characters of ISIN specify the type of asset traded. For example, 
if an ISIN starts with “IRO1”, “IRO3” and “IRO7”  indicates stocks, and “IRB3”, “IRBK” indicates bonds, and “IRT1”, “IRT3” also  indicate ETFs and etc.

In this project, it received a numbers of traded securities data and it create appropriate command in the runtime, based on the ISIN of securities. Each command subscribe some ISIN Pattern that indicate specific securities type. securities type include: Stock, Bond and ETF. 

# Installing MediatR
You should install [MediatR with NuGet](https://www.nuget.org/packages/MediatR):

# Architecture
We assume some TradeDTOs as traded securities data.
The TradeDTO type is created by using [record](https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-9#record-types) type which is introduced in C# 9.0.
Each TradeDTO that is received, it will be encapsulated within the appropriate command based on ISIN.

```csharp
   public record TradeDTO(string ISIN, decimal Price, long Quanitity, DateTime Date);
```
we use Strategy Pattern for commands: StockCommand, BondCommand and ETFCommand which are driven from AbstractCommand, and AbstractCommand is driven from ICommand.

```csharp
    public interface ICommand : IRequest<bool>
    {
        TradeDTO DTO { get; set; }

        List<string> SubscribeISINs();

        void AddDTO(TradeDTO dto)
        {
            DTO = dto;
        }
    }
```


AddDTO method is a [Default Method](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-8.0/default-interface-methods) which is introduced in C# 8.0

```csharp
    public abstract class AbstractCommand : ICommand
    {
        public TradeDTO DTO { get; set; }
        
        public abstract List<string> SubscribeISINs();
    }
    
    public class StockCommand : AbstractCommand
    {
        public override List<string> SubscribeISINs()
        {
            return new List<string>() { "IRO1", "IRO3", "IRO7" };
        }
    }
    
    public class BondCommand : AbstractCommand
    {
        public override List<string> SubscribeISINs()
        {
            return new List<string>() { "IRB3", "IRBE", };
        }
    }
    
    public class ETFCommand : AbstractCommand
    {
        public override List<string> SubscribeISINs()
        {
            return new List<string>() { "IRT1", "IRT3",};
        }
    }

```
and each command will be caught by appropriate CommandHandler

```csharp
    public class StockCommandHandler : IRequestHandler<StockCommand,bool>
    {
        public Task<bool> Handle(StockCommand request, CancellationToken cancellationToken)
        {
            Console.WriteLine("the StockCommand has been caught by the StockCommandHandler");
            return Task.FromResult(true);
        }
    }
    
    public class BondCommandHandler : IRequestHandler<BondCommand,bool>
    {
        public Task<bool> Handle(BondCommand request, CancellationToken cancellationToken)
        {
            Console.WriteLine("the BondCommandHandler has been caught by the BondCommandHandler");
            return Task.FromResult(true); 
        }
    }
    
    public class ETFCommandHandler : IRequestHandler<ETFCommand, bool>
    {
        public Task<bool> Handle(ETFCommand request, CancellationToken cancellationToken)
        {
            Console.WriteLine("the ETFCommandHand has been caught by the ETFCommandHandler");
            return Task.FromResult(true);
        }
    }
```

we have a Factory class that is responsible to creating appropreate command based on TradeDTO.
the factory uses reflection for getting all concerete classes of ICommand. and then  It  creates a function for each Command class that will construct an instance of that type, and executes it once just to generate the list of subscribed ISINs to use as a key for the dictionary.
```csharp
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
            var key = CommandCreators.Keys.Where(key => key.Contains(dto.ISIN.ISINAssetClassPart())).FirstOrDefault();
            var command = CommandCreators[key]();
            command.AddDTO(dto);
            return command;

        }
    }
```
in the above code, we use an Extension Method for extracting 4 first character of ISN to identifying securities type.
```csharp
     public static class Extensions
     {
        public static string ExtractSecuritiesType(this string isin)
        {
            return isin.Substring(0, 4);
        }
     }
```
in the main class, at first we need to add MedatorR. And then we assume some TradeDTO with different ISIN, by using TradeCommandFactory each dto is encapsulated within the appropriate command, and then each command well be despatched by using MediatoR. at the end, each command will be cuaght by appropreate  CommandHandler.
```csharp
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

            //stock isin
            tradeISIN = "IRO1PNES0001";
            command = CreateCommand(tradeQuantity, tradePrice, tradeDate, tradeISIN);
            _mediator.Send(command);

            //bond isin
            tradeISIN = "IRBEPO1301C1";
            command = CreateCommand(tradeQuantity, tradePrice, tradeDate, tradeISIN);
            _mediator.Send(command);

            Console.ReadLine();
        }

        private static ICommand CreateCommand(int tradeQuantity, int tradePrice, DateTime tradeDate, string tradeISIN)
        {
            ICommand command;
            Console.WriteLine($"traded securities data for isin {tradeISIN} is received");
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
```


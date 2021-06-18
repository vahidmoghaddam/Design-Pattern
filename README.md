# Command Pattern and runtime command creation by .Net Core
This is an example how to implement [command pattern](https://en.wikipedia.org/wiki/Command_pattern) with command creator, and this is an approach to creating command in the runtime.
The project also uses some new features in C# such as **record** and **default interface method** and interesting third party library like **MedaitorR**.

# Problem Statement
Every security that is traded in the market has its own international securities identification number (ISN). An ISIN is a 12-digit alphanumeric code that uniquely identifies a specific 
security. In the Iranian capital market, the first 4 characters of ISIN specify the type of asset traded. For example, 
if an ISIN starts with “IRO1”, “IRO3” and “IRO7”  indicates stocks, and “IRB3”, “IRBK” indicates bonds, and “IRT1”, “IRT3” also  indicate ETFs and etc.

In this project, it received a numbers of traded securities data and it create appropriate command in the runtime, based on securities ISIN. Each command subscribe some ISIN Pattern that indicate specific securities type. Security type includes: Stock, Bond and ETF. 


# Architecture
We assume some TradeDTOs as traded securities data.
The TradeDTO type is created by using [record](https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-9#record-types) type which is introduced in C# 9.0.
Each TradeDTO that is received, it will be encapsulated within the appropriate command based on ISIN.

```csharp
   public record TradeDTO(string ISIN, decimal Price, long Quanitity, DateTime Date);
```
we have 3 classes as commands: StockCommand, BondCommand and ETFCommand which are driven from AbstractCommand, and AbstractCommand is driven from ICommand.

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


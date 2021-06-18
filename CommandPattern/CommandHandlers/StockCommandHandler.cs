using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CommandPattern
{
    public class StockCommandHandler : IRequestHandler<StockCommand,bool>
    {
        public Task<bool> Handle(StockCommand request, CancellationToken cancellationToken)
        {
            Console.WriteLine("the StockCommand has been caught by the StockCommandHandler");
            return Task.FromResult(true);
        }
    }
}

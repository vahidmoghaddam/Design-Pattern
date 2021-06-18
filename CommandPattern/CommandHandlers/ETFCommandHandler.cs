using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CommandPattern
{
    public class ETFCommandHandler : IRequestHandler<ETFCommand, bool>
    {
        public Task<bool> Handle(ETFCommand request, CancellationToken cancellationToken)
        {
            Console.WriteLine("the ETFCommandHand has been caught by the ETFCommandHandler");
            return Task.FromResult(true);
        }
    }
}

  
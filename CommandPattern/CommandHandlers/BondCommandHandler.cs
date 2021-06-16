using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CommandPattern
{
    public class BondCommandHandler : IRequestHandler<BondCommand,bool>
    {
        public Task<bool> Handle(BondCommand request, CancellationToken cancellationToken)
        {
            Console.WriteLine("BondCommandHandler received command");
            return Task.FromResult(true); 
        }
    }
}

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
                Console.WriteLine("the BondCommand has been caught by the BondCommandHandler");
                return Task.FromResult(true); 
            }
        }
}

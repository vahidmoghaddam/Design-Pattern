using MediatR;
using System.Collections.Generic;


namespace CommandPattern
{
    public interface ICommand : IRequest<bool>
    {
        TradeDTO DTO { get; set; }

        List<string> SubscribeISINs();

        void AddDTO(TradeDTO dto)
        {
            DTO = dto;
        }
    }
}

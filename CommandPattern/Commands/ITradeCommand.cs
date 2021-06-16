using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommandPattern
{
    public interface ITradeCommand : IRequest<bool>
    {
        TradeDTO DTO { get; set; }
        public void AddDTO(TradeDTO dto)
        {
            DTO = dto;
        }
        List<string> SubscribeISINs();
    }
}

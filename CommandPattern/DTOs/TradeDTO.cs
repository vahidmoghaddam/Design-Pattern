
using System;

namespace CommandPattern
{
    public record TradeDTO(string ISIN, decimal Price, long Quanitity, DateTime Date);
}

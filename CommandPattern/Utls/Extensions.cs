
namespace CommandPattern
{
    public static class Extensions
    {

        public static string ExtractSecuritiesType(this string isin)
        {
            return isin.Substring(0, 4);
        }
    }
}

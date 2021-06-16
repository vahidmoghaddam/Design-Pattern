
namespace CommandPattern
{
    public static class Extensions
    {
        /// <summary>
        /// extract 4 first characters of isin
        /// </summary>
        /// <param name="isin"></param>
        /// <returns></returns>
        public static string ISINAssetClassPart(this string isin)
        {
            return isin.Substring(0, 4);
        }
    }
}

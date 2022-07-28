namespace RamCleaner
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            foreach (Area x in (Area[])Enum.GetValues(typeof(Area)))
            {
                Cleaner.Clean(x);
            }
        }
    }
}

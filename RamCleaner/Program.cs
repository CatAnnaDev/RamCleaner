namespace RamCleaner
{
    internal class Program
    {
        private static void Main(string[] args) => Cleaner.Clean(Settings.MemoryAreas);
    }
}

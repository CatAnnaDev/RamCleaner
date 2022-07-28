namespace RamCleaner
{
    public static class Enums
    {
        public static class Memory
        {
            [Flags]
            public enum Area
            {
                None = 0,
                CombinedPageList = 1,
                ModifiedPageList = 2,
                ProcessesWorkingSet = 4,
                StandbyList = 8,
                StandbyListLowPriority = 16,
                SystemWorkingSet = 32
            }
        }

        internal static class Windows
        {
            internal enum SystemErrorCode
            {
                ERROR_SUCCESS = 0,
                ERROR_ACCESS_DENIED = 5

            }
        }
    }
}

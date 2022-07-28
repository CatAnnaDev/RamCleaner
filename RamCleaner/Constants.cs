namespace RamCleaner
{
    public class Constants
    {
        internal static class App
        {
            internal static class RegistryKey
            {
                internal const string MemoryAreas = "MemoryAreas";
                internal const string Name = @"SOFTWARE\WinMemoryCleaner";
            }
        }

        internal static class Windows
        {
            internal const string DebugPrivilege = "SeDebugPrivilege";
            internal const string IncreaseQuotaName = "SeIncreaseQuotaPrivilege";
            internal const int MemoryFlushModifiedList = 3;
            internal const int MemoryPurgeLowPriorityStandbyList = 5;
            internal const int MemoryPurgeStandbyList = 4;
            internal const int PrivilegeEnabled = 2;
            internal const string ProfileSingleProcessName = "SeProfileSingleProcessPrivilege";
            internal const int SystemCombinePhysicalMemoryInformation = 130;
            internal const int SystemFileCacheInformation = 21;
            internal const int SystemMemoryListInformation = 80;
        }
    }
}

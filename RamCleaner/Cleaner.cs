namespace RamCleaner
{
    public class Cleaner
    {
        internal static void Clean(Enums.Memory.Area areas)
        {
            // Clean Processes Working Set
            if (areas.HasFlag(Enums.Memory.Area.ProcessesWorkingSet))
            {
                try
                {
                    CleanProcessesWorkingSet();

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            // Clean System Working Set
            if (areas.HasFlag(Enums.Memory.Area.SystemWorkingSet))
            {
                try
                {
                    CleanSystemWorkingSet();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            // Clean Modified Page List
            if (areas.HasFlag(Enums.Memory.Area.ModifiedPageList))
            {
                try
                {
                    CleanModifiedPageList();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            // Clean Standby List
            if (areas.HasFlag(Enums.Memory.Area.StandbyList) || areas.HasFlag(Enums.Memory.Area.StandbyListLowPriority))
            {
                try
                {
                    CleanStandbyList(areas.HasFlag(Enums.Memory.Area.StandbyListLowPriority));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            // Clean Combined Page List
            if (areas.HasFlag(Enums.Memory.Area.CombinedPageList))
            {
                try
                {
                    CleanCombinedPageList();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }


        private static void CleanCombinedPageList()
        {
            // Windows minimum version
            if (!ComputerHelper.IsWindows8OrAbove)
            {
                return;
            }

            // Check privilege
            if (!ComputerHelper.SetIncreasePrivilege(Constants.Windows.ProfileSingleProcessName))
            {
                return;
            }

            GCHandle handle = GCHandle.Alloc(0);

            try
            {
                var memoryCombineInformationEx = new Structs.Windows.MemoryCombineInformationEx();

                handle = GCHandle.Alloc(memoryCombineInformationEx, GCHandleType.Pinned);

                int length = Marshal.SizeOf(memoryCombineInformationEx);

                if (import.NtSetSystemInformation(Constants.Windows.SystemCombinePhysicalMemoryInformation, handle.AddrOfPinnedObject(), length) != (int)Enums.Windows.SystemErrorCode.ERROR_SUCCESS)
                    throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            catch (Win32Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                try
                {
                    if (handle.IsAllocated)
                        handle.Free();
                }
                catch (InvalidOperationException)
                {
                    // ignored
                }
            }
        }

        private static void CleanModifiedPageList()
        {
            // Windows minimum version
            if (!ComputerHelper.IsWindowsVistaOrAbove)
            {
                return;
            }

            // Check privilege
            if (!ComputerHelper.SetIncreasePrivilege(Constants.Windows.ProfileSingleProcessName))
            {
                return;
            }

            GCHandle handle = GCHandle.Alloc(Constants.Windows.MemoryFlushModifiedList, GCHandleType.Pinned);

            try
            {
                if (import.NtSetSystemInformation(Constants.Windows.SystemMemoryListInformation, handle.AddrOfPinnedObject(), Marshal.SizeOf(Constants.Windows.MemoryFlushModifiedList)) != (int)Enums.Windows.SystemErrorCode.ERROR_SUCCESS)
                    throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            catch (Win32Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                try
                {
                    if (handle.IsAllocated)
                        handle.Free();
                }
                catch (InvalidOperationException)
                {
                    // ignored
                }
            }
        }

        private static void CleanStandbyList(bool lowPriority = false)
        {
            // Windows minimum version
            if (!ComputerHelper.IsWindowsVistaOrAbove)
            {
                return;
            }

            // Check privilege
            if (!ComputerHelper.SetIncreasePrivilege(Constants.Windows.ProfileSingleProcessName))
            {
                return;
            }

            object memoryPurgeStandbyList = lowPriority ? Constants.Windows.MemoryPurgeLowPriorityStandbyList : Constants.Windows.MemoryPurgeStandbyList;
            GCHandle handle = GCHandle.Alloc(memoryPurgeStandbyList, GCHandleType.Pinned);

            try
            {
                if (import.NtSetSystemInformation(Constants.Windows.SystemMemoryListInformation, handle.AddrOfPinnedObject(), Marshal.SizeOf(memoryPurgeStandbyList)) != (int)Enums.Windows.SystemErrorCode.ERROR_SUCCESS)
                    throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            catch (Win32Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                try
                {
                    if (handle.IsAllocated)
                        handle.Free();
                }
                catch (InvalidOperationException)
                {
                    // ignored
                }
            }
        }

        private static void CleanSystemWorkingSet()
        {
            // Windows minimum version
            if (!ComputerHelper.IsWindowsVistaOrAbove)
            {
                return;
            }

            // Check privilege
            if (!ComputerHelper.SetIncreasePrivilege(Constants.Windows.IncreaseQuotaName))
            {
                return;
            }

            GCHandle handle = GCHandle.Alloc(0);

            try
            {
                object systemCacheInformation;

                if (ComputerHelper.Is64Bit)
                    systemCacheInformation = new Structs.Windows.SystemCacheInformation64 { MinimumWorkingSet = -1L, MaximumWorkingSet = -1L };
                else
                    systemCacheInformation = new Structs.Windows.SystemCacheInformation32 { MinimumWorkingSet = uint.MaxValue, MaximumWorkingSet = uint.MaxValue };

                handle = GCHandle.Alloc(systemCacheInformation, GCHandleType.Pinned);

                int length = Marshal.SizeOf(systemCacheInformation);

                if (import.NtSetSystemInformation(Constants.Windows.SystemFileCacheInformation, handle.AddrOfPinnedObject(), length) != (int)Enums.Windows.SystemErrorCode.ERROR_SUCCESS)
                    throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            catch (Win32Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                try
                {
                    if (handle.IsAllocated)
                        handle.Free();
                }
                catch (InvalidOperationException)
                {
                    // ignored
                }
            }

            try
            {
                IntPtr fileCacheSize = IntPtr.Subtract(IntPtr.Zero, 1); // Flush

                if (!import.SetSystemFileCacheSize(fileCacheSize, fileCacheSize, 0))
                    throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            catch (Win32Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void CleanProcessesWorkingSet()
        {
            // Windows minimum version
            if (!ComputerHelper.IsWindowsVistaOrAbove)
            {
                return;
            }

            // Check privilege
            if (!ComputerHelper.SetIncreasePrivilege(Constants.Windows.DebugPrivilege))
            {
                return;
            }

            foreach (Process process in Process.GetProcesses().Where(process => process != null))
            {
                try
                {
                    using (process)
                    {
                        if (!process.HasExited && import.EmptyWorkingSet(process.Handle) == 0)
                            throw new Win32Exception(Marshal.GetLastWin32Error());
                    }
                }
                catch (InvalidOperationException)
                {
                    // ignored
                }
                catch (Win32Exception e)
                {
                    if (e.NativeErrorCode != (int)Enums.Windows.SystemErrorCode.ERROR_ACCESS_DENIED)
                        Console.WriteLine(e);
                }
            }
        }
    }
}

namespace RamCleaner
{
    public class Settings
    {

        internal static Enums.Memory.Area MemoryAreas;

        static Settings()
        {

            MemoryAreas = Enums.Memory.Area.StandbyListLowPriority | Enums.Memory.Area.SystemWorkingSet | Enums.Memory.Area.ProcessesWorkingSet;

            Reload();
        }
        private static void Load()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(Constants.App.RegistryKey.Name))
                {
                    if (key == null)
                    {
                        Save();
                    }
                    else
                    {
                        MemoryAreas = (Enums.Memory.Area)Enum.Parse(typeof(Enums.Memory.Area), Convert.ToString(key.GetValue(Constants.App.RegistryKey.MemoryAreas, MemoryAreas)));

                        if (MemoryAreas.HasFlag(Enums.Memory.Area.StandbyList | Enums.Memory.Area.StandbyListLowPriority))
                            MemoryAreas &= ~Enums.Memory.Area.StandbyList;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void Reload()
        {
            Load();
            Save();
        }

        internal static void Save()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(Constants.App.RegistryKey.Name))
                {
                    if (key != null)
                    {
                        key.SetValue(Constants.App.RegistryKey.MemoryAreas, (int)MemoryAreas);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}

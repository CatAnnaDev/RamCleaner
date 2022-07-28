

namespace RamCleaner
{
    public class Settings
    {

        internal static Area MemoryAreas;

        static Settings()
        {

            MemoryAreas = StandbyListLowPriority | SystemWorkingSet | ProcessesWorkingSet;

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
                        MemoryAreas = (Area)Enum.Parse(typeof(Area), Convert.ToString(key.GetValue(Constants.App.RegistryKey.MemoryAreas, MemoryAreas)));

                        if ((StandbyList | StandbyListLowPriority).HasFlag(MemoryAreas))
                            MemoryAreas &= ~StandbyList;
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

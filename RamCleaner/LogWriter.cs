namespace RamCleaner
{
    internal class LogWriter
    {

        private readonly StreamWriter logWriter = File.AppendText(Constants.App.Log.logPath);

        public void Log(string s)
        {
            DateTime date;
            logWriter.AutoFlush = true;
            if (!File.Exists(Constants.App.Log.logPath))
            {
                date = DateTime.Now;
                s = $"[{date.Day}/{date.Month} {date.Hour}:{date.Minute}:{date.Second} - LOG] {s}";
                logWriter.WriteLine(s);

            }
            else
            {
                date = DateTime.Now;
                s = $"[{date.Day}/{date.Month} {date.Hour}:{date.Minute}:{date.Second} - LOG] {s}";
                logWriter.WriteLine(s);

            }

            //logWriter.Close();
        }
    }
}

namespace UnityEngine
{
    //生成脚步要用到Unity方法才叫这个名称 -_-
    public class Debug
    {
        enum LogType
        {
            Info,
            Warning,
            Error
        }

        public static void Log(string msg)
        {
            Print(msg, LogType.Info);
        }

        public static void LogWarning(string msg)
        {
            Print(msg, LogType.Warning);
        }

        public static void LogError(string msg)
        {
            Print(msg, LogType.Error);
            Console.ReadKey();
        }

        static void Print(string msg, LogType logType)
        {
            switch (logType)
            {
                case LogType.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogType.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogType.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
            }
            //Console.WriteLine($"{DateTime.Now} [{logType}] {msg}");
            Console.WriteLine($"[{logType.ToString().ToUpper()}] {msg}");
            Console.ResetColor();
        }
    }
}
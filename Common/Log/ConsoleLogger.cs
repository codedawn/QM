using System;

namespace QM
{
    public class ConsoleLogger : ILog
    {
        public void Debug(string message)
        {
            Console.WriteLine(message);
        }

        public void Error(string message)
        {
            Console.WriteLine(message);
        }

        public void Error(Exception exception)
        {
            Console.WriteLine(exception.ToString());
        }

        public void Info(string message)
        {
            Console.WriteLine(message);
        }

        public void Warn(string message)
        {
            Console.WriteLine(message);
        }
    }
}

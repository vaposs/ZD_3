using System;
using System.IO;

namespace ZD_3
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("записываем в файл");
            Pathfinder pathfinder1 = new Pathfinder(new FileLogWritter());
            pathfinder1.Find("message");

            Console.WriteLine("выводим сообщение в консоль");
            Pathfinder pathfinder2 = new Pathfinder(new ConsoleLogWritter());
            pathfinder2.Find("message");

            Console.WriteLine("записываем в файл лог но только по пятницам");
            Pathfinder pathfinder3 = new Pathfinder(new SecureLogWriter(new FileLogWritter()));
            pathfinder3.Find("message");

            Console.WriteLine("выводим сообщение в консоль но только по пятницам");
            Pathfinder pathfinder4 = new Pathfinder(new SecureLogWriter(new ConsoleLogWritter()));
            pathfinder4.Find("message");

            Console.WriteLine("выводим сообщение в консоль и записываем в файл, но запись только по пятницам");
            Pathfinder pathfinder5 = new Pathfinder(new DualLogWriter());
            pathfinder5.Find("message");
        }
    }

    interface ILogger
    {
        void WriteError(string massage);
    }

    class Pathfinder
    {
        private ILogger _logger;
        private string _longMassage;

        public Pathfinder(ILogger logger)
        {
            _logger = logger;
        }

        public void Find(string path)
        {
            _longMassage = "[" + DateTime.Now + "] что то ищем по пути " + path;
            _logger.WriteError(_longMassage);
        }
    }

    class ConsoleLogWritter : ILogger
    {
        public void WriteError(string message)
        {
            Console.WriteLine(message);
        }
    }

    class FileLogWritter : ILogger
    {
        public void WriteError(string message)
        {
            File.WriteAllText("logg.txt", message);
        }
    }

    class SecureLogWriter : ILogger
    {
        private ILogger _logger;

        public SecureLogWriter(ILogger logger)
        {
            _logger = logger;
        }

        public void WriteError(string message)
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            {
                _logger.WriteError(message);
            }
        }
    }

    class DualLogWriter : ILogger
    {
        private ConsoleLogWritter _consoleLogger;
        private SecureLogWriter _fileLogger;

        public DualLogWriter()
        {
            _consoleLogger = new ConsoleLogWritter();
            _fileLogger = new SecureLogWriter(new FileLogWritter());
        }

        public void WriteError(string message)
        {
            _consoleLogger.WriteError(message);

            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            {
                _fileLogger.WriteError(message);
            }
        }
    }
}
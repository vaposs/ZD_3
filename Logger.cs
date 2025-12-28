using System;
using System.IO;
using System.Collections.Generic;

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
            Pathfinder pathfinder5 = new Pathfinder(new DualLogWriter(new ConsoleLogWritter(), new SecureLogWriter(new FileLogWritter())));
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

        public Pathfinder(ILogger logger)
        {
            _logger = logger;
        }

        public void Find(string path)
        {
            _logger.WriteError("[" + DateTime.Now + "] что то ищем по пути " + path);
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
        private DayOfWeek _needDay = DayOfWeek.Friday;

        public SecureLogWriter(ILogger logger)
        {
            _logger = logger;
        }

        public void WriteError(string message)
        {
            if (DateTime.Now.DayOfWeek == _needDay)
            {
                _logger.WriteError(message);
            }
        }
    }

    class DualLogWriter : ILogger
    {
        private List<ILogger> _loggers = new List<ILogger>();

        public DualLogWriter(params ILogger[] loggers)
        {
            if (loggers == null || loggers.Length == 0)
            {
                throw new ArgumentException("Должен быть передан хотя бы один логгер");
            }

            _loggers.AddRange(loggers);
        }

        public void WriteError(string message)
        {
            foreach (ILogger logger in _loggers)
            {
                logger.WriteError(message);
            }
        }
    }
}
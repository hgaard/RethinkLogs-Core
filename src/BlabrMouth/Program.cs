using System;
using RethinkDb.Driver;
using RethinkDb.Driver.Net;
using Newtonsoft.Json;

namespace ConsoleApplication
{
    public class Program
    {
        private static string Db = "logging";
        private static string Table = "logs";

        private static Connection _connection;
        public static void Main(string[] args)
        {
            _connection = RethinkDB.R.Connection()
                .Hostname("localhost")
                .Port(28015)
                .Connect();

            Log(2, "Hi there - starting awesome app");

            while (true)
            {
                Console.Write("Logconsole>");
                var input = Console.ReadLine();
                if (input == ":q")
                    break;

                var level = GetLevel(input);
                var message = input.Substring(input.IndexOf(" ") + 1);
                Log(level, message);
            }

            Log(2, "exiting - bummer..");
        }

        private static void Log(int level, string message)
        {
            EnsureDBExists();
            var result =  RethinkDB.R
                .Db(Db)
                .Table(Table)
                .Insert(new LogEvent(level, message))
                .Run(_connection);

           
            Console.WriteLine(result);
        }

        private static int GetLevel(string input)
        {
            var level = input.Substring(0, input.IndexOf(" "));
            if (string.IsNullOrEmpty(level))
                return 1;

            if (level.ToUpperInvariant().StartsWith("FA"))
                return 5;

            if (level.ToUpperInvariant().StartsWith("ER"))
                return 4;

            if (level.ToUpperInvariant().StartsWith("WA"))
                return 3;

            if (level.ToUpperInvariant().StartsWith("IN"))
                return 2;

            
            return 1;
            
        }
        public static void  EnsureDBExists()
        {
            var dbExists = RethinkDB.R.DbList().Contains(Db).Run(_connection);

            Console.WriteLine($"Does the db {Db} exist? {dbExists}");

            if (!dbExists)
            {
                RethinkDB.R.DbCreate(Db).Run(_connection);
            }

            var tableExists = RethinkDB.R.Db(Db).TableList().Contains(Table).Run(_connection);

            Console.WriteLine($"Does the table {Table} exist? {tableExists}");

            if (!tableExists)
            {
                RethinkDB.R.Db(Db).TableCreate(Table).Run(_connection);
            }
        }
    }

    public class LogEvent
    {
        public LogEvent(int level, string message)
        {
            Timestamp = DateTimeOffset.Now;
            Level = level;
            Message = message;
        }

        [JsonProperty("level")]
        public int Level { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

    	[JsonProperty("timestamp")]
        public DateTimeOffset Timestamp { get; set; }
    }
}

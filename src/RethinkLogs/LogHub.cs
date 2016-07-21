using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Infrastructure;

using RethinkDb.Driver;


namespace RethinkLogs
{
    public class LogHub : Hub
    {
        private static readonly RethinkDB R = RethinkDB.R;
        private static RethinkDb.Driver.Net.Connection _connection;

        internal static void Init()
        {
            _connection = R.Connection()
                            .Hostname(Constants.Host)
                            .Port(Constants.Port)
                            .Connect();
        }

        public static void HandleUpdates(IConnectionManager connectionManager)
        {
            var hub = connectionManager.GetHubContext<LogHub>();
            var connection = R.Connection()
                            .Hostname(Constants.Host)
                            .Port(Constants.Port)
                            .Connect();
            var feed = R.Db(Constants.LoggingDatabase)
                        .Table(Constants.LoggingTable)
                        .Changes()
                        .RunChanges<LogEvent>(connection);


            feed.Select(x => hub.Clients.All.onMessage(x.NewValue)).ToList();
        }

        public IList<LogEvent> History(int limit)
        {
            try
            {
                var output = R.Db(Constants.LoggingDatabase).Table(Constants.LoggingTable)
                .OrderBy(R.Desc("timestamp"))
                .Limit(limit)
                .OrderBy("timestamp")
                .RunResult<IList<LogEvent>>(_connection);

                Console.WriteLine(output); 

                return output;
            }
           catch (Exception e)
            {
                Console.WriteLine($"Error happend: {e.Message}");
                // ignore
            }            

            return null;
        }

        public IList<LogEvent> Query(string queryString)
        {
            try
            {
                var result = R.Db(Constants.LoggingDatabase).Table(Constants.LoggingTable)
                   .Filter(r => r["message"].Match(queryString))
                   .OrderBy("timestamp")
                   .RunResult<IList<LogEvent>>(_connection);
                    
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error happend: {e.Message}");
                // ignore
            }
            return null;
        }

        public LogEvent Get(string id)
        {
            return
                R.Db(Constants.LoggingDatabase).Table(Constants.LoggingTable)
                .Get(id).RunResult<LogEvent>(_connection);
        }
    }
}
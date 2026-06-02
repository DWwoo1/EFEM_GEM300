using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using FrameOfSystem3.SECSGEM.DefineSecsGem;

namespace ScenarioLogger
{
    public sealed class AsyncLogEntry
    {
        public LogTypes Type { get; set; }
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }
        public string FormattedMessage { get; set; }
        public string FilePath { get; set; }
    }

    public interface IAsyncLogChannel
    {
        LogTypes Type { get; }
        void Reserve(AsyncLogEntry entry);
        void Write(AsyncLogEntry entry);
        void CloseAll();
    }

    internal sealed class DateBasedFileLogChannel : IAsyncLogChannel
    {
        private readonly string _basePath;
        private readonly string _subDirectory;
        private readonly string _fileName;

        private readonly ConcurrentDictionary<string, int> _pendingCountByPath
            = new ConcurrentDictionary<string, int>();

        private readonly Dictionary<string, StreamWriter> _writersByPath
            = new Dictionary<string, StreamWriter>();

        private readonly object _syncRoot = new object();

        public DateBasedFileLogChannel(
            LogTypes type,
            string basePath,
            string subDirectory,
            string fileName = "Log.txt")
        {
            Type = type;
            _basePath = basePath;
            _subDirectory = subDirectory ?? string.Empty;
            _fileName = string.IsNullOrWhiteSpace(fileName) ? "Log.txt" : fileName;
        }

        public LogTypes Type { get; private set; }

        public void Reserve(AsyncLogEntry entry)
        {
            if (entry == null)
                return;

            string filePath = ResolveFilePath(entry.Timestamp);
            entry.FilePath = filePath;

            _pendingCountByPath.AddOrUpdate(filePath, 1, (key, oldValue) => oldValue + 1);
        }

        public void Write(AsyncLogEntry entry)
        {
            if (entry == null || string.IsNullOrWhiteSpace(entry.FilePath))
                return;

            try
            {
                lock (_syncRoot)
                {
                    StreamWriter writer = GetOrCreateWriter(entry.FilePath);
                    writer.WriteLine(entry.FormattedMessage);
                }
            }
            catch (Exception ex)
            {
                lock (_syncRoot)
                {
                    StreamWriter writer;
                    if (_writersByPath.TryGetValue(entry.FilePath, out writer) && writer != null)
                    {
                        writer.WriteLine(string.Format("##### {0} : {1} #####", ex.Message, ex.StackTrace));
                    }
                }
            }
            finally
            {
                if (DecreasePendingCount(entry.FilePath) == 0)
                {
                    CloseWriter(entry.FilePath);
                }
            }
        }

        public void CloseAll()
        {
            lock (_syncRoot)
            {
                foreach (KeyValuePair<string, StreamWriter> item in _writersByPath)
                {
                    item.Value.Close();
                    item.Value.Dispose();
                }

                _writersByPath.Clear();
                _pendingCountByPath.Clear();
            }
        }

        private string ResolveFilePath(DateTime timestamp)
        {
            string directoryPath;

            if (string.IsNullOrWhiteSpace(_subDirectory))
            {
                directoryPath = string.Format(@"{0}\{1:0000}\{2:00}\{3:00}",
                    _basePath,
                    timestamp.Year,
                    timestamp.Month,
                    timestamp.Day);
            }
            else
            {
                directoryPath = string.Format(@"{0}\{1:0000}\{2:00}\{3:00}\{4}",
                    _basePath,
                    timestamp.Year,
                    timestamp.Month,
                    timestamp.Day,
                    _subDirectory);
            }

            return Path.Combine(directoryPath, _fileName);
        }

        private StreamWriter GetOrCreateWriter(string filePath)
        {
            StreamWriter writer;
            if (_writersByPath.TryGetValue(filePath, out writer) && writer != null)
                return writer;

            string directoryPath = Path.GetDirectoryName(filePath);
            if (false == Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            writer = new StreamWriter(filePath, true) { AutoFlush = true };
            _writersByPath[filePath] = writer;

            return writer;
        }

        private int DecreasePendingCount(string filePath)
        {
            int newCount = _pendingCountByPath.AddOrUpdate(
                filePath,
                0,
                (key, oldValue) => oldValue > 0 ? oldValue - 1 : 0);

            if (newCount <= 0)
            {
                int removed;
                _pendingCountByPath.TryRemove(filePath, out removed);
                return 0;
            }

            return newCount;
        }

        private void CloseWriter(string filePath)
        {
            lock (_syncRoot)
            {
                StreamWriter writer;
                if (_writersByPath.TryGetValue(filePath, out writer))
                {
                    writer.Close();
                    writer.Dispose();
                    _writersByPath.Remove(filePath);
                }
            }
        }
    }

    public class AsyncLogger
    {
        #region <Constructors>
        public AsyncLogger()
        {
            BasePath = PATH.FILEPATH_LOG;
            LogQueue = new ConcurrentQueue<AsyncLogEntry>();
            Channels = new Dictionary<LogTypes, IAsyncLogChannel>();

            if (false == Directory.Exists(BasePath))
            {
                Directory.CreateDirectory(BasePath);
            }

            RegisterChannel(new DateBasedFileLogChannel(LogTypes.History, BasePath, "SECSGEM"));
            RegisterChannel(new DateBasedFileLogChannel(LogTypes.Terminal, BasePath, "Terminal"));
            RegisterChannel(new DateBasedFileLogChannel(LogTypes.Scenario, BasePath, "Scenario"));

            _consumerTask = Task.Run(() => ProcessLogsAsync());
        }
        #endregion </Constructors>

        #region <Fields>
        private readonly string BasePath;
        private readonly ConcurrentQueue<AsyncLogEntry> LogQueue;
        private readonly Dictionary<LogTypes, IAsyncLogChannel> Channels;
        private readonly object ChannelSyncRoot = new object();

        private readonly Task _consumerTask;
        private bool _exiting = false;

        public event deleHandlerString CallbackDisplayLog;
        #endregion </Fields>

        #region <Methods>
        public void RegisterChannel(IAsyncLogChannel channel)
        {
            if (channel == null)
                return;

            lock (ChannelSyncRoot)
            {
                Channels[channel.Type] = channel;
            }
        }

        public void EnqueueLog(LogTypes type, string message)
        {
            IAsyncLogChannel channel;
            if (false == TryGetChannel(type, out channel))
                return;

            DateTime now = DateTime.Now;

            AsyncLogEntry entry = new AsyncLogEntry
            {
                Type = type,
                Timestamp = now,
                Message = message,
                FormattedMessage = string.Format("[{0:d2}/{1:d2}-{2:d2}:{3:d2}:{4:d2}.{5:d3}] {6}",
                    now.Month,
                    now.Day,
                    now.Hour,
                    now.Minute,
                    now.Second,
                    now.Millisecond,
                    message)
            };

            channel.Reserve(entry);
            LogQueue.Enqueue(entry);
        }

        public void Exit()
        {
            _exiting = true;

            WaitForCompletion(TimeSpan.FromSeconds(5)).GetAwaiter().GetResult();

            CloseAllChannels();
        }

        private bool TryGetChannel(LogTypes type, out IAsyncLogChannel channel)
        {
            lock (ChannelSyncRoot)
            {
                return Channels.TryGetValue(type, out channel);
            }
        }

        private async Task ProcessLogsAsync()
        {
            while (true)
            {
                AsyncLogEntry entry;
                if (LogQueue.TryDequeue(out entry))
                {
                    IAsyncLogChannel channel;
                    if (TryGetChannel(entry.Type, out channel))
                    {
                        channel.Write(entry);

                        if (CallbackDisplayLog != null && entry.Type == LogTypes.History)
                        {
                            CallbackDisplayLog(entry.FormattedMessage);
                        }
                    }

                    continue;
                }

                if (_exiting)
                {
                    return;
                }

                await Task.Delay(1);
            }
        }

        private void CloseAllChannels()
        {
            lock (ChannelSyncRoot)
            {
                foreach (KeyValuePair<LogTypes, IAsyncLogChannel> item in Channels)
                {
                    item.Value.CloseAll();
                }
            }
        }

        private async Task WaitForCompletion(TimeSpan timeout)
        {
            DateTime endTime = DateTime.UtcNow + timeout;

            while (LogQueue.Count > 0 && DateTime.UtcNow < endTime)
            {
                await Task.Delay(10).ConfigureAwait(false);
            }
        }
        #endregion </Methods>
    }
}
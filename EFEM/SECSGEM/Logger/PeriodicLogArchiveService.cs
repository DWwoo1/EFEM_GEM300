using System;
using System.Threading;
using System.Threading.Tasks;

namespace ScenarioLogger
{
    public sealed class PeriodicLogArchiveService
    {
        private readonly LogArchiveManager _archiveManager;
        private readonly Action<string> _writeLog;
        private readonly string _rootPath;
        private readonly int _archiveDays;
        private readonly TimeSpan _interval;

        private CancellationTokenSource _cts;
        private Task _workerTask;
        private TaskCompletionSource<bool> _stopSignal;
        private int _isRunningArchive = 0;

        public PeriodicLogArchiveService(
            LogArchiveManager archiveManager,
            Action<string> writeLog,
            string rootPath,
            int archiveDays,
            TimeSpan interval)
        {
            _archiveManager = archiveManager ?? throw new ArgumentNullException(nameof(archiveManager));
            _writeLog = writeLog ?? (_ => { });
            _rootPath = rootPath ?? throw new ArgumentNullException(nameof(rootPath));
            _archiveDays = archiveDays;
            _interval = interval;
        }

        public void Start()
        {
            if (_workerTask != null)
                return;

            _cts = new CancellationTokenSource();
            _stopSignal = new TaskCompletionSource<bool>();
            _workerTask = Task.Run(() => RunAsync(_cts.Token));
        }

        public void Stop(TimeSpan waitTimeout)
        {
            if (_workerTask == null)
                return;

            try
            {
                _cts.Cancel();

                bool completed = _workerTask.Wait(waitTimeout);
                if (false == completed)
                {
                    _writeLog("[LOG_ARCHIVE][WARN] Stop timeout elapsed before archive worker completed.");
                    return;
                }
            }
            catch (AggregateException ex)
            {
                Exception inner = ex.Flatten().InnerException;
                _writeLog(string.Format(
                    "[LOG_ARCHIVE][ERROR] Worker stop failed. {0}",
                    inner != null ? inner.Message : ex.Message));
            }
            finally
            {
                if (_workerTask == null || _workerTask.IsCompleted)
                {
                    _cts.Dispose();
                    _cts = null;
                    _workerTask = null;
                    _stopSignal = null;
                }
            }
        }
        //public void Stop(TimeSpan waitTimeout)
        //{
        //    if (_workerTask == null)
        //        return;

        //    try
        //    {
        //        _cts.Cancel();
        //        _workerTask.Wait(waitTimeout);
        //    }
        //    catch (AggregateException)
        //    {
        //    }
        //    finally
        //    {
        //        _cts.Dispose();
        //        _cts = null;
        //        _workerTask = null;
        //    }
        //}

        private async Task RunAsync(CancellationToken token)
        {
            // 시작 직후 1회 실행
            await TryArchiveOnceAsync().ConfigureAwait(false);

            using (token.Register(() => _stopSignal.TrySetResult(true)))
            {
                while (false == token.IsCancellationRequested)
                {
                    Task delayTask = Task.Delay(_interval);
                    Task completedTask = await Task.WhenAny(delayTask, _stopSignal.Task)
                                                   .ConfigureAwait(false);

                    if (completedTask != delayTask)
                        break;

                    if (token.IsCancellationRequested)
                        break;

                    await TryArchiveOnceAsync().ConfigureAwait(false);
                }
            }
        }

        private Task TryArchiveOnceAsync()
        {
            if (Interlocked.Exchange(ref _isRunningArchive, 1) == 1)
                return Task.CompletedTask;

            try
            {
                LogArchiveResult result = _archiveManager.ArchiveOldLogs(
                    _rootPath,
                    DateTime.Now,
                    _archiveDays);

                _writeLog(string.Format(
                    "[LOG_ARCHIVE] Archived={0}, Skipped={1}, Errors={2}",
                    result.Archived,
                    result.Skipped,
                    result.Errors.Count));

                foreach (string error in result.Errors)
                {
                    _writeLog(string.Format("[LOG_ARCHIVE][SKIP] {0}", error));
                }
            }
            catch (Exception ex)
            {
                _writeLog(string.Format("[LOG_ARCHIVE][ERROR] {0}", ex.Message));
            }
            finally
            {
                Interlocked.Exchange(ref _isRunningArchive, 0);
            }

            return Task.CompletedTask;
        }
    }
}
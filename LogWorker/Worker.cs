using LogerServices.Services;
using LogWorker.Configuration;
using Microsoft.Extensions.Options;

namespace LogWorker
{
    public class Worker : BackgroundService
    {
        private readonly int _contextLines;
        private readonly ILogger<Worker> _logger;
        private readonly IOllamaService _ollamaService;
        private readonly INotificationService _notificationService;
        private readonly LogerWorkerOptions _workerOptions;
        private readonly Dictionary<string, long> _filePositions = new();
        private readonly SemaphoreSlim _semaphore = new(1, 1);
        private readonly string[] triggerKeywords = { "ERROR", "CRITICAL", "FATAL", "EXCEPTION" };

        public Worker(ILogger<Worker> logger, IOllamaService ollamaService, IOptions<LogerWorkerOptions> workerOptions, INotificationService notificationService)
        {
            _logger = logger;
            _ollamaService = ollamaService;
            _workerOptions = workerOptions.Value;
            _contextLines = _workerOptions.ContextLines;
            _notificationService = notificationService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Logger Worker Started. Searching log files...");

            if (!Directory.Exists(_workerOptions.Path))
            {
                _logger.LogError("FATAL: Log folder path not found.");
                return;
            }

            _logger.LogInformation("Log folder path located. Watching log changes...");

            foreach (var file in Directory.GetFiles(_workerOptions.Path, _workerOptions.Extension))
            {
                var fileInfo = new FileInfo(file);
                _filePositions[file] = fileInfo.Length;
            }

            var watcher = new FileSystemWatcher(_workerOptions.Path)
            {
                Filter = _workerOptions.Extension,
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.FileName,
                EnableRaisingEvents = true
            };

            watcher.Changed += async (sender, events) =>
            {
                await ProcessLogFileAsync(events.FullPath);
            };

            watcher.Created += async (sender, events) =>
            {
                _filePositions[events.FullPath] = 0;
                await ProcessLogFileAsync(events.FullPath);
            };

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(5000, stoppingToken);
            }
        }

        private async Task ProcessLogFileAsync(string filePath)
        {
            await _semaphore.WaitAsync();
            try
            {
                _logger.LogInformation("Detected log change in file {file}", filePath);

                await Task.Delay(500);

                using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                // Get the last known position for this file
                if (!_filePositions.ContainsKey(filePath))
                {
                    _filePositions[filePath] = 0;
                }

                long lastPosition = _filePositions[filePath];

                // If file was truncated/rotated, reset position
                if (fs.Length < lastPosition)
                {
                    lastPosition = 0;
                }

                // Seek to last read position
                fs.Seek(lastPosition, SeekOrigin.Begin);

                using var reader = new StreamReader(fs);
                string? line;
                var recentLines = new List<string>();
                var errorFound = false;

                // Read only new lines
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        recentLines.Add(line);
                        if (recentLines.Count > _contextLines)
                        {
                            recentLines.RemoveAt(0);
                        }

                        if (triggerKeywords.Any(k => line.Contains(k, StringComparison.OrdinalIgnoreCase)))
                        {
                            errorFound = true;
                        }
                    }
                }
                if (errorFound)
                {
                    try
                    {
                        string allLines = string.Join("\n", recentLines);
                        var response = await _ollamaService.AskAiAsync(allLines);
                        Console.WriteLine(response);
                        await _notificationService.SendNotificationAsync(response);
                    }
                    catch (Exception aiEx)
                    {
                        _logger.LogError("Error calling AI service: {ex}", aiEx.Message);
                    }
                }

                // Update the last read position
                _filePositions[filePath] = fs.Position;
            }
            catch (IOException ioEx)
            {
                _logger.LogWarning("File access error (file may be locked): {ex}", ioEx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error parsing log file: {ex}", ex.Message);
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
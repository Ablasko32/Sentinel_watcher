namespace LogWorker.Configuration
{
    public class LogerWorkerOptions
    {
        public string Path { get; set; } = String.Empty;
        public string Extension { get; set; } = "*.log";

        public int ContextLines { get; set; } = 50;
    }
}
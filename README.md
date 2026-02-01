# 🛡️ SENTINEL_WATCHER

**SENTINEL_WATCHER** is a high-performance **.NET 8 Worker Service** designed for privacy-first log monitoring. It bridges the gap between raw stack traces and actionable intelligence by analyzing local errors using **Ollama LLMs** and notifying you instantly via **Telegram**.

---

## 💡 The Concept

Sentinel operates on a simple **Observe → Analyze → Alert** pipeline:

1. **Observe:** Monitors local directories using `FileSystemWatcher` with minimal CPU overhead.
2. **Filter:** Identifies new entries marked as `Error`, `Critical`, `Fatal` or `Exception`.
3. **Analyze:** Sends the raw error context to a local `Ollama` instance. The AI interprets the stack trace and suggests a fix.
4. **Alert:** Pushes a notification report to your `Telegram Bot`.

---

## ✨ Features

- **Privacy-First:** Your logs never leave your local machine (No OpenAI/Anthropic keys required).
- **Lightweight:** Built on .NET 8 BackgroundService (Worker Service).
- **Customizable:** Works with any Ollama-supported model (`llama3`, `qwen2.5`, `mistral`).
- **Real-time:** Near-zero latency between log write and notification.

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- [Ollama](https://ollama.ai/) (Running locally)
- Telegram Bot Token & Chat ID

### Installation

1. Clone the repo: `git clone https://github.com/youruser/sentinel_watcher.git`
2. Pull your preferred model: `ollama pull qwen2.5:0.5b`
3. Configure `appsettings.json` (see below).
4. Run: `dotnet run`

### Configuration

```json
{
  "Ollama": {
    "Uri": "http://localhost:11434",
    "ModelName": "qwen2.5:0.5b"
  },
  "LoggerWorker": {
    "Path": "./logs",
    "Extension": "*.log"
  },
  "TelegramOptions": {
    "TelegramToken": "YOUR_TELEGRAM_TOKEN",
    "TelegramChatId": "YOUR_CHAT_ID"
  }
}
```

### License

- `MIT LICENSE`

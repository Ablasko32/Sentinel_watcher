using LogerServices.Configuration;
using Microsoft.Extensions.Options;
using OllamaSharp;
using System.Text;

namespace LogerServices.Services
{
    public class OllamaService : IOllamaService
    {
        private readonly OllamaApiClient _ollamaClient;

        private readonly string _systemPrompt =
            "You are an expert log analyzer and debugging specialist for production systems. " +
            "Analyze error logs and provide concise, actionable insights based ONLY on what the logs show.\n\n" +

            "IMPORTANT: If logs contain MULTIPLE UNRELATED errors, analyze each separately.\n\n" +

            "Response Format:\n\n" +

            "**Problem:**\n" +
            "[One sentence describing what failed]\n\n" +

            "**Root Cause:**\n" +
            "[2-3 sentences explaining why, based on log timeline]\n\n" +

            "**Severity:** [1-10]\n" +
            "1-3: Minor | 4-6: Moderate | 7-8: High | 9-10: Critical\n\n" +

            "**Immediate Actions:**\n" +
            "[Maximum 3-4 bullet points - only the most critical actions]\n\n" +

            "**Timeline:**\n" +
            "[Key events in chronological order with timestamps]\n\n" +

            "Critical Rules:\n" +
            "- Keep total response under 20 lines\n" +
            "- Maximum 4 immediate actions - prioritize ruthlessly\n" +
            "- No generic advice like 'check logs' or 'monitor system'\n" +
            "- Only include actions based on evidence in the logs\n" +
            "- No greetings, no filler phrases, no speculation\n" +
            "- Identify if errors are cascading (related) or independent\n" +
            "- Don't assume specific programming languages or frameworks\n" +
            "- Focus on: What broke? Why? What to do now?";

        public OllamaService(IOptions<OllamaOptions> options)
        {
            var settings = options.Value;
            Uri uri = new(settings.Uri);
            _ollamaClient = new OllamaApiClient(uri)
            {
                SelectedModel = settings.ModelName
            };
        }

        public async Task<string> AskAiAsync(string logMessage)
        {
            var fullPrompt = _systemPrompt + "\n\nRaw Log message:" + logMessage;
            var responseBuilder = new StringBuilder();
            await foreach (var stream in _ollamaClient.GenerateAsync(fullPrompt))
                if (stream != null)
                {
                    responseBuilder.Append(stream.Response);
                }
            return responseBuilder.ToString();
        }
    }
}
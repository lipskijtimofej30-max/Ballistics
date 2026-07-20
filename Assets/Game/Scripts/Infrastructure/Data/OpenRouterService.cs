using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
using ILogger = Game.Scripts.Infrastructure.Logger.ILogger;

namespace Game.Scripts.Core.Simulation
{
    public class OpenRouterService
    {
        private const string ApiKey = "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";
        private const string Model = "nvidia/nemotron-3-ultra-550b-a55b:free";

        private Infrastructure.Logger.ILogger _logger;
        
        [Inject]
        public OpenRouterService(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<string> SendPromptAsync(string prompt)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {ApiKey}");

            var payload = new ChatCompletionRequest
            {
                model = Model,
                messages = new[]
                {
                    new ChatMessage { role = "user", content = prompt }
                }
            };

            string json = JsonUtility.ToJson(payload);
           _logger.Log("Sending JSON: " + json); // отладка

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://openrouter.ai/api/v1/chat/completions", content);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"OpenRouter error {response.StatusCode}: {responseBody}");
                return null;
            }

            _logger.Log("OpenRouter response: " + responseBody);
            var result = JsonUtility.FromJson<OpenRouterResponse>(responseBody);
            return result?.choices?[0]?.message?.content ?? "Нет ответа";
        }
    }

    [Serializable]
    public class ChatCompletionRequest
    {
        public string model;
        public ChatMessage[] messages;
    }

    [Serializable]
    public class ChatMessage
    {
        public string role;
        public string content;
    }

    [Serializable]
    public class OpenRouterResponse
    {
        public Choice[] choices;
    }

    [Serializable]
    public class Choice
    {
        public ChatMessage message;
    }
}

using System.Text;
using System.Text.Json;

namespace GreenForest.AI;

public class GeminiService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public GeminiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<string> PreguntarGemini(string pregunta)
{
    var apiKey = _configuration["Gemini:ApiKey"];
    var modelo = _configuration["Gemini:Model"];

    var url = $"https://generativelanguage.googleapis.com/v1beta/models/{modelo}:generateContent?key={apiKey}";

    Console.WriteLine("URL:");
    Console.WriteLine(url);

    var body = new
    {
        contents = new[]
        {
            new
            {
                parts = new[]
                {
                    new
                    {
                        text = pregunta
                    }
                }
            }
        }
    };

    var json = JsonSerializer.Serialize(body);

    var content = new StringContent(json, Encoding.UTF8, "application/json");

    var response = await _httpClient.PostAsync(url, content);

    var resultado = await response.Content.ReadAsStringAsync();

    Console.WriteLine("Status: " + response.StatusCode);
    Console.WriteLine("Respuesta:");
    Console.WriteLine(resultado);

    if (!response.IsSuccessStatusCode)
    {
        throw new Exception(resultado);
    }

    using JsonDocument doc = JsonDocument.Parse(resultado);

    return doc.RootElement
        .GetProperty("candidates")[0]
        .GetProperty("content")
        .GetProperty("parts")[0]
        .GetProperty("text")
        .GetString() ?? "";
}
}
using Workshop4.Application.Json.Models;

namespace Workshop4.Application.Json.Factories;

public interface IJsonDocumentFactory
{
    JsonDocument CreateDocument(string json);
}

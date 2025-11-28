using Workshop4.Application.Json.Models;
using Workshop4.Application.Pipelines.Iterators;
using Workshop4.Application.Pipelines.Models;

namespace Workshop4.Presentation.Execution;

public sealed record ExecutionIteratorSnapshot(
    IPipelineIterator Iterator,
    IEnumerable<ExecutionFrame> ExecutionFrames,
    JsonDocument WorkingDocument);

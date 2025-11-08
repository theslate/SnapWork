using System.Collections.Generic;

namespace SnapWork.Models;

public sealed record Workspace
{
    public required string Version { get; init; }

    public DateTime GeneratedUtc { get; init; }

    public required IList<WindowSpec> Windows { get; init; }
}

namespace Presentation.Mvc.Models;

public sealed class PageActionItem
{
    public string Text { get; init; } = string.Empty;
    public string Variant { get; init; } = "secondary";
    public string Href { get; init; } = "#";
    public string? IconSvg { get; init; }
    public string? ActionName { get; init; }
}

namespace Overtime.Web.Services;

public sealed class FooterOptions
{
    public string Tagline { get; set; } = "";
    public string QuickLinksTitle { get; set; } = "";
    public List<FooterLink> QuickLinks { get; set; } = [];
    public string ContactTitle { get; set; } = "";
    public List<FooterLink> ContactLinks { get; set; } = [];
    public string FinePrint { get; set; } = "";
}

public sealed class FooterLink
{
    public string Label { get; set; } = "";
    public string Href { get; set; } = "";
    public string? Icon { get; set; }
    public bool External { get; set; }
}

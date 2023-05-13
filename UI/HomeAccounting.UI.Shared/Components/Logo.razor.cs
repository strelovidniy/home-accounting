using Microsoft.AspNetCore.Components;

namespace HomeAccounting.UI.Shared.Components;

public partial class Logo
{
    [Parameter]
    public string? Class { get; set; } = string.Empty;
}
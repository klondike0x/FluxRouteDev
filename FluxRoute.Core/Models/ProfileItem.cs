namespace FluxRoute.Core.Models;

public sealed class ProfileItem
{
    public string FileName { get; init; } = "";   // например: general (ALT3).bat
    public string DisplayName { get; init; } = ""; // например: general (ALT3)
    public string FullPath { get; init; } = "";    // полный путь
}

using CommunityToolkit.Mvvm.ComponentModel;

namespace FluxRoute.Core.Models;

public partial class ProfileScore : ObservableObject
{
    public string DisplayName { get; init; } = "";
    public string FileName { get; init; } = "";

    [ObservableProperty] private int score = -1;
    [ObservableProperty] private string scoreText = "—";
    [ObservableProperty] private bool isExcluded;

    public void SetScore(double rate)
    {
        Score = (int)(rate * 100);
        IsExcluded = Score == 0;
        ScoreText = IsExcluded ? "❌ 0% (исключён)" : $"{Score}%";
    }

    public void SetPending() { Score = -1; ScoreText = "⏳ проверяется..."; IsExcluded = false; }
    public void SetSkipped() { Score = -1; ScoreText = "—"; IsExcluded = false; }
}

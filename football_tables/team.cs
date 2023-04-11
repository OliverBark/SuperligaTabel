using System;

public class Team
{
    public string Name { get; set; }
    public int GamesPlayed { get; set; }
    public int GamesWon { get; set; }
    public int GamesDrawn { get; set; }
    public int GamesLost { get; set; }
    public int GoalsFor { get; set; }
    public int GoalsAgainst { get; set; }
    public int GoalDifference => GoalsFor - GoalsAgainst;
    public int Points => GamesWon * 3 + GamesDrawn;
    public string CurrentStreak { get; set; }

    // Constructor
    public Team(string name)
    {
        Name = name;
        GamesPlayed = 0;
        GamesWon = 0;
        GamesDrawn = 0;
        GamesLost = 0;
        GoalsFor = 0;
        GoalsAgainst = 0;
        CurrentStreak = "-";
    }

    // Method to update team statistics
    public void UpdateStatistics(int goalsFor, int goalsAgainst, string result)
    {
        GamesPlayed++;
        GoalsFor += goalsFor;
        GoalsAgainst += goalsAgainst;

        switch (result)
        {
            case "W":
                GamesWon++;
                CurrentStreak = CurrentStreak == "L" ? "W" : CurrentStreak + "W";
                break;
            case "D":
                GamesDrawn++;
                CurrentStreak = CurrentStreak == "L" ? "D" : CurrentStreak + "D";
                break;
            case "L":
                GamesLost++;
                CurrentStreak = CurrentStreak == "W" ? "L" : CurrentStreak + "L";
                break;
        }
    }

    // Method to reset team statistics
    public void ResetStatistics()
    {
        GamesPlayed = 0;
        GamesWon = 0;
        GamesDrawn = 0;
        GamesLost = 0;
        GoalsFor = 0;
        GoalsAgainst = 0;
        CurrentStreak = "-";
    }
}

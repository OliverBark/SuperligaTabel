class Team
{
    public string Name { get; }
    public int GamesPlayed { get; private set; }
    public int GamesWon { get; private set; }
    public int GamesDrawn { get; private set; }
    public int GamesLost { get; private set; }
    public int GoalsFor { get; private set; }
    public int GoalsAgainst { get; private set; }
    public int GoalDifference => GoalsFor - GoalsAgainst;
    public int Points => (GamesWon * 3) + (GamesDrawn * 1);

    private string lastFiveResults; // To store the last 5 results (W|D|L)

    public Team(string name)
    {
        Name = name;
        GamesPlayed = 0;
        GamesWon = 0;
        GamesDrawn = 0;
        GamesLost = 0;
        GoalsFor = 0;
        GoalsAgainst = 0;
        lastFiveResults = "";
    }

    public void UpdateStatistics(int teamScore, int opponentScore, string result)
    {
        GamesPlayed++;
        GoalsFor += teamScore;
        GoalsAgainst += opponentScore;

        // Update last five results
        if (lastFiveResults.Length == 5)
        {
            lastFiveResults = lastFiveResults.Substring(1);
        }
        lastFiveResults += result;

        if (result == "W")
        {
            GamesWon++;
        }
        else if (result == "D") // Added support for draws
        {
            GamesDrawn++;
        }
        else if (result == "L")
        {
            GamesLost++;
        }
    }

    public string GetLastFiveResults()
    {
        return lastFiveResults;
    }
}

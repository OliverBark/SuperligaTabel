using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        string teamsFile = "csv_files/teams/teams.csv"; // File path for teams.csv
        List<Team> teams = ReadTeamsFromCsv(teamsFile); // Read team names from teams.csv

        string setupFile = "csv_files/setup/setup.csv"; // Updated file path

        string setupFileContent = File.ReadAllText(setupFile, Encoding.UTF8);
        if (setupFileContent.Split(',').Length != 6)
        {
            throw new ArgumentException(
                "Invalid input file format: The setup.csv file should have 6 columns."
            );
        }

        string[] splitSetupFileContent = setupFileContent.Split(',');
        string leagueName = splitSetupFileContent[0];

        if (
            int.TryParse(splitSetupFileContent[1], out int var1)
            && int.TryParse(splitSetupFileContent[2], out int var2)
            && int.TryParse(splitSetupFileContent[3], out int var3)
            && int.TryParse(splitSetupFileContent[4], out int var4)
            && int.TryParse(splitSetupFileContent[5], out int var5)
        )
        {
            int positionsToChampionsLeague = var1;
            int positionsToEuropeLeague = var2;
            int positionsToConferenceLeague = var3;
            int positionsToUpperLeague = var4;
            int positionsToLowerLeague = var5;

            Console.WriteLine("League Name: {0}", leagueName);
            Console.WriteLine("Positions to Champions League: {0}", positionsToChampionsLeague);
            Console.WriteLine("Positions to Europe League: {0}", positionsToEuropeLeague);
            Console.WriteLine("Positions to Conference League: {0}", positionsToConferenceLeague);
            Console.WriteLine("Positions to Upper League: {0}", positionsToUpperLeague);
            Console.WriteLine("Positions to Lower League: {0}", positionsToLowerLeague);

            // Generate fixtures
            for (int i = 1; i <= 32; i++)
            {
                List<string> fixtures = GenerateFixtures(teams);
                WriteCsvFile(leagueName, i, fixtures, $"round{i}.csv");
            }

            // Generate and display league table
            List<Team> leagueTable = GenerateLeagueTable(teams);
            DisplayLeagueTable(leagueTable);
        }
        else
        {
            throw new FormatException("The setup.csv file is not valid");
        }
    }

    static List<Team> ReadTeamsFromCsv(string teamsFile)
    {
        List<Team> teams = new List<Team>();
        string[] lines = File.ReadAllLines(teamsFile, Encoding.UTF8);
        for (int i = 1; i < lines.Length; i++)
        {
            string[] fields = lines[i].Split(',');
            Team team = new Team(fields[1])
            {

                GamesPlayed = 0,
                GamesWon = 0,
                GamesDrawn = 0,
                GamesLost = 0,
                GoalsFor = 0,
                GoalsAgainst = 0,
                CurrentStreak = "-"
            };
            teams.Add(team);
        }
        return teams;
    }

    static List<string> GenerateFixtures(List<Team> teams)
    {
        List<string> fixtures = new List<string>();
        Random random = new Random();

        // Shuffle teams
        for (int i = teams.Count - 1; i > 0; i--)
        {
                            int j = random.Next(0, i + 1);
                Team temp = teams[i];
                teams[i] = teams[j];
                teams[j] = temp;
            }

        // Generate fixtures
        for (int i = 0; i < teams.Count - 1; i++)
        {
            for (int j = i + 1; j < teams.Count; j++)
            {
                string fixture = $"{teams[i].Name},{teams[j].Name}";
                fixtures.Add(fixture);
            }
        }

        return fixtures;
    }

    static void WriteCsvFile(string leagueName, int round, List<string> fixtures, string fileName)
    {
        string[] lines = new string[fixtures.Count + 1];
        lines[0] = "Fixture";
        for (int i = 0; i < fixtures.Count; i++)
        {
            lines[i + 1] = fixtures[i];
        }
        File.WriteAllLines($"csv_files/rounds/{fileName}", lines, Encoding.UTF8);
    }

    static List<Team> GenerateLeagueTable(List<Team> teams)
    {
        List<Team> leagueTable = new List<Team>(teams);
        leagueTable.Sort((x, y) =>
        {
            int pointsComparison = y.Points.CompareTo(x.Points);
            if (pointsComparison != 0)
            {
                return pointsComparison;
            }
            else
            {
                int goalDifferenceComparison = y.GoalDifference.CompareTo(x.GoalDifference);
                if (goalDifferenceComparison != 0)
                {
                    return goalDifferenceComparison;
                }
                else
                {
                    int goalsForComparison = y.GoalsFor.CompareTo(x.GoalsFor);
                    if (goalsForComparison != 0)
                    {
                        return goalsForComparison;
                    }
                    else
                    {
                        return x.Name.CompareTo(y.Name);
                    }
                }
            }
        });
        return leagueTable;
    }

    static void DisplayLeagueTable(List<Team> leagueTable)
    {
        Console.WriteLine();
        Console.WriteLine("{0,-25}{1,-10}{2,-10}{3,-10}{4,-10}{5,-10}{6,-10}{7,-10}",
            "Team", "P", "W", "D", "L", "GF", "GA", "PTS");
        foreach (var team in leagueTable)
        {
            Console.WriteLine("{0,-25}{1,-10}{2,-10}{3,-10}{4,-10}{5,-10}{6,-10}{7,-10}",
                team.Name, team.GamesPlayed, team.GamesWon, team.GamesDrawn,
                team.GamesLost, team.GoalsFor, team.GoalsAgainst, team.Points);
        }
    }
}

class Team
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


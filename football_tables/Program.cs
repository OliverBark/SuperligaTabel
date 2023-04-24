﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        string teamsFile = "csv_files/teams/teams.csv"; // File path for teams.csv
        List<string> teams = ReadTeamsFromCsv(teamsFile); // Read team names from teams.csv

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

            // Get round files
            List<string> roundFiles = GetRoundFiles();       
        

            // Print league table
            PrintLeagueTable(roundFiles);
        }
        else
        {
            throw new FormatException("The setup.csv file is not valid");
        }
    }


     static List<string> GetRoundFiles()
    {
        List<string> roundFiles = new List<string>();

        for (int i = 1; i <= 32; i++)
        {
            string roundFilePath = $"csv_files/rounds/round{i}.csv";
            roundFiles.Add(roundFilePath);
        }

        return roundFiles;
    }

    static List<string> ReadTeamsFromCsv(string teamsFile)
    {
        List<string> teams = new List<string>();
        string[] lines = File.ReadAllLines(teamsFile, Encoding.UTF8);
        for (int i = 1; i < lines.Length; i++)
        {
            string[] fields = lines[i].Split(',');
            teams.Add(fields[0]);
        }
        return teams;
    }

     static List<string> GenerateFixtures(List<string> teams)
    {
        List<string> fixtures = new List<string>();
        Random random = new Random();

        // Shuffle teams
        for (int i = teams.Count - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);
            string temp = teams[i];
            teams[i] = teams[j];
            teams[j] = temp;
        }

        for (int i = 0; i < teams.Count; i += 2)
        {
            string fixture = $"{teams[i]},{teams[i + 1]},{random.Next(0, 5)},{random.Next(0, 5)}";
            fixtures.Add(fixture);
        }

        return fixtures;
    }

    

    static void WriteCsvFile(
        string leagueName,
        int roundNumber,
        List<string> fixtures,
        string fileName
    )
    {
        string filePath = $"csv_files/rounds/{fileName}";

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine($"HomeTeam vs AwayTeam,HomeScore,AwayScore");
            foreach (string fixture in fixtures)
            {
                writer.WriteLine(fixture);
            }
        }

        Console.WriteLine("Round {0} fixtures generated and written to {1}", roundNumber, fileName);
    }


 static void PrintLeagueTable(List<string> roundFiles)
{
    Dictionary<string, Team> teams = new Dictionary<string, Team>();
    foreach (string roundFile in roundFiles)
    {
        string[] lines = File.ReadAllLines(roundFile, Encoding.UTF8);
        for (int i = 1; i < lines.Length; i++)
        {
            string[] fields = lines[i].Split(',');
            string homeTeamName = fields[0].Trim(); // Extract home team name
            string awayTeamName = fields[1].Trim(); // Extract away team name
            int homeScore = Convert.ToInt32(fields[2]);
            int awayScore = Convert.ToInt32(fields[3]);

            // Update teams dictionary with home team
            if (!teams.ContainsKey(homeTeamName))
            {
                teams.Add(homeTeamName, new Team(homeTeamName));
            }

            // Update teams dictionary with away team
            if (!teams.ContainsKey(awayTeamName))
            {
                teams.Add(awayTeamName, new Team(awayTeamName));
            }

            // Update team statistics for home team
            Team homeTeam = teams[homeTeamName];
            Team awayTeam = teams[awayTeamName];

            if (homeScore > awayScore)
            {
                homeTeam.UpdateStatistics(homeScore, awayScore, "W");
                awayTeam.UpdateStatistics(awayScore, homeScore, "L");
            }
            else if (homeScore < awayScore)
            {
                homeTeam.UpdateStatistics(homeScore, awayScore, "L");
                awayTeam.UpdateStatistics(awayScore, homeScore, "W");
            }
            else
            {
                homeTeam.UpdateStatistics(homeScore, awayScore, "D");
                awayTeam.UpdateStatistics(awayScore, homeScore, "D");
            }
        }
    }

    // Sort teams by points, goal difference, goals for, goals against, and team name
    var sortedTeams = teams.Values.OrderByDescending(t => t.Points)
        .ThenByDescending(t => t.GoalDifference)
        .ThenByDescending(t => t.GoalsFor)
        .ThenBy(t => t.GoalsAgainst)
        .ThenBy(t => t.Name);

    // Print league table
    Console.WriteLine("League Table:");
    Console.WriteLine("{0,-4} {1,-20} {2,-15} {3,-10} {4,-10} {5,-10} {6,-10} {7,-10} {8,-15} {9,-10} {10,-15}",
        "Pos", "Team", "GP", "Won", "Drawn", "Lost", "G/F", "G/A", "G/D", "Points", "Last 5 Results");
    int position = 1;
    foreach (var team in sortedTeams)
    {
        Console.WriteLine("{0,-4} {1,-20} {2,-15} {3,-10} {4,-10} {5,-10} {6,-10} {7,-10} {8,-15} {9,-10} {10,-15}",
            position, team.Name, team.GamesPlayed, team.GamesWon, team.GamesDrawn, team.GamesLost,
            team.GoalsFor, team.GoalsAgainst, team.GoalDifference, team.Points, team.GetLastFiveResults());
        position++;
    }
}


}

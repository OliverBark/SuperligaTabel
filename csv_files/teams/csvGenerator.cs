/* using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CsvGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Define the updated teams with their respective data
            List<Team> teams = new List<Team>
            {
                new Team { Abbreviation = "FCN", FullName = "FC Nordsjælland", SpecialRanking = "-" },
                new Team { Abbreviation = "FCK", FullName = "F.C. København", SpecialRanking = "-" },
                new Team { Abbreviation = "VFF", FullName = "Viborg AGF", SpecialRanking = "-" },
                new Team { Abbreviation = "RFC", FullName = "Randers FC", SpecialRanking = "-" },
                new Team { Abbreviation = "BIF", FullName = "Brøndby IF", SpecialRanking = "-" },
                new Team { Abbreviation = "SIF", FullName = "Silkeborg IF", SpecialRanking = "-" },
                new Team { Abbreviation = "FCM", FullName = "FC Midtjylland", SpecialRanking = "-" },
                new Team { Abbreviation = "OB", FullName = "OB AC Horsens", SpecialRanking = "-" },
                new Team { Abbreviation = "LBBK", FullName = "Lyngby BK", SpecialRanking = "-" },
                new Team { Abbreviation = "AaB", FullName = "AaB", SpecialRanking = "-" }
            };

            // Generate the CSV file
            string csvFileName = "teams.csv";
            WriteCsvFile(csvFileName, teams);

            Console.WriteLine($"CSV file '{csvFileName}' has been generated successfully.");
        }

        static void WriteCsvFile(string fileName, List<Team> teams)
        {
            using (var writer = new StreamWriter(fileName))
            {
                writer.WriteLine("Abbreviation,FullName,SpecialRanking");
                foreach (var team in teams)
                {
                    writer.WriteLine($"{team.Abbreviation},{team.FullName},{team.SpecialRanking}");
                }
            }
        }
    }

    class Team
    {
        public string Abbreviation { get; set; }
        public string FullName { get; set; }
        public string SpecialRanking { get; set; }
    }
}
 */
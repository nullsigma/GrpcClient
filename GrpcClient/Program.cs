using System;
using System.Threading.Tasks;
using GrpcService1;
using Grpc.Net.Client;
using System.Text;
using System.Collections.Generic;
using static GrpcService1.CurrentStandingsResponse.Types;

namespace GrpcClient
{
    class Program
    {
        const string serviceAddress = "https://localhost:5001";
        const int leagueId = 2; // English Premier League

        static async Task Main(string[] args)
        {
            Console.WriteLine("Press any key to begin...");
            Console.ReadKey();

            try
            {
                // Calling the service
                using var channel = GrpcChannel.ForAddress(serviceAddress);
                var client = new Greeter.GreeterClient(channel);
                var response = await client.GetCurrentStandingsAsync(
                                  new CurrentStandingsRequest { LeagueId = leagueId });

                if (response?.Stangings == null)
                    throw new NullReferenceException("Could not get data from service");

                // Generating result view
                Console.WriteLine(GenerateResult(response.Stangings));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured: {ex.Message}");
            }
        }

        static string GenerateResult(IEnumerable<Standing> standings)
        {
            // Column names
            var sb = new StringBuilder();
            sb.Append("Team".PadRight(30));
            sb.Append("N".PadRight(5));
            sb.Append("MP".PadRight(5));
            sb.Append("W".PadRight(5));
            sb.Append("D".PadRight(5));
            sb.Append("L".PadRight(5));
            sb.Append("GF".PadRight(5));
            sb.Append("GA".PadRight(5));
            sb.Append("GD".PadRight(5));
            sb.AppendLine("Pts".PadRight(5));
            sb.AppendLine();

            // Rows
            foreach (var standing in standings)
            {
                sb.Append(standing.TeamName.PadRight(30));
                sb.Append(standing.Rank.ToString().PadRight(5));
                sb.Append(standing.MatchesPlayed.ToString().PadRight(5));
                sb.Append(standing.Win.ToString().PadRight(5));
                sb.Append(standing.Draw.ToString().PadRight(5));
                sb.Append(standing.Lose.ToString().PadRight(5));
                sb.Append(standing.GoalsFor.ToString().PadRight(5));
                sb.Append(standing.GoalsAgainst.ToString().PadRight(5));
                sb.Append(standing.GoalsDiff.ToString().PadRight(5));
                sb.Append(standing.Points.ToString().PadRight(5));

                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
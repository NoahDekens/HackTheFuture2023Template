using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SolutionPathA.Solution;

struct Position
{
    public int X { get; set; }
    public int Y { get; set; }

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override string ToString()
    {
        return $"{X}, {Y}";
    }
}

class Challenge2
{

    public static async Task Solve(HackTheFutureClient client)
    {
        // start challenge
        await client.GetAsync("/api/path/a/medium/start");

        bool testSuccess = await SolveForUrl(client, "/api/path/a/medium/sample");
        Console.WriteLine(testSuccess);

        if (testSuccess)
            Console.WriteLine(await SolveForUrl(client, "/api/path/a/medium/puzzle"));

    }

    private static async Task<bool> SolveForUrl(HackTheFutureClient client, string url)
    {
        var response = await client.GetFromJsonAsync<VineNavigationChallengeDto>(url);

        Console.WriteLine(response);

        var startPositionText = response!.Start.Split(',');

        var startPositionX = int.Parse(startPositionText[0]);
        var startPositionY = int.Parse(startPositionText[1]);

        var position = new Position(startPositionX, startPositionY);

        Console.WriteLine($"Start position {position}");

        foreach (var direction in response.Directions)
        {
            var original = position;

            foreach (var single in direction)
            {
                UpdatePosition(ref position, single);
            }

            if (!IsValidPosition(ref position, response.AmountOfVines))
            {
                position = original; // rollback;
            }
            Console.WriteLine($"{position} {direction}");
        }
        Console.WriteLine(position);
        
        var result = await client.PostAsJsonAsync(url, position.ToString());

        return result.IsSuccessStatusCode;
    }

    private static void UpdatePosition(ref Position position, char direction)
    {
        switch (direction)
        {
            case 'U':
                position.Y += 1;
                break;
            case 'D':
                position.Y -= 1;
                break;
            case 'L':
                position.X -= 1;
                break;
            case 'R':
                position.X += 1;
                break;
            default:
                Console.WriteLine("No can do :(");
                break;
        }
    }

    private static bool IsValidPosition(ref Position position, int amountOfVines)
    {
        if (position.X >= amountOfVines || position.X < 0)
            return false;

        if (position.Y >= amountOfVines || position.Y < 0)
            return false;

        return true;
    }
}

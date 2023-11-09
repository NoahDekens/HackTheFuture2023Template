using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SolutionPathA.Solution;

class Challenge1
{
    public static async Task Solve(HackTheFutureClient client)
    {
        // start challenge
        await client.GetAsync("/api/path/a/easy/start");

        bool testSuccess = await SolveForUrl(client, "/api/path/a/easy/sample");
        Console.WriteLine(testSuccess);

        if (testSuccess)
            Console.WriteLine(await SolveForUrl(client, "/api/path/a/easy/puzzle"));

    }

    private static async Task<bool> SolveForUrl(HackTheFutureClient client, string url)
    {
        var response = await client.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        var output = new StringBuilder();

        foreach (var c in content)
        {
            output.Append(
                HieroglyphAlphabet.Characters.ContainsKey(c) ? HieroglyphAlphabet.Characters[c] : c
            );
        }

        Console.WriteLine(output.ToString());
        var result = await client.PostAsJsonAsync(url, output.ToString());

        return result.IsSuccessStatusCode;
    }
}
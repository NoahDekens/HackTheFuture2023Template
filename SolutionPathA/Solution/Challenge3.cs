using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SolutionPathA.Solution;

class Challenge3
{
    public static async Task Solve(HackTheFutureClient client)
    {
        // start challenge
        await client.GetAsync("/api/path/a/hard/start");

        bool testSuccess = await SolveForUrl(client, "/api/path/a/hard/sample");
        Console.WriteLine(testSuccess);

        if (testSuccess)
            Console.WriteLine(await SolveForUrl(client, "/api/path/a/hard/puzzle"));

    }

    private static async Task<bool> SolveForUrl(HackTheFutureClient client, string url)
    {
        var animals = (await client.GetFromJsonAsync<List<Animal>>(url))!;
        var result = new List<Animal>();

        var animalsInCommon = new Dictionary<Animal, List<Animal>>();
        animals.ForEach(a => animalsInCommon[a] = new());

        // For each animal, check which attributes they have in common
        foreach (var animal in animals)
        {
            foreach (var animal2 in animals)
            {
                if (animal == animal2)
                    continue;

                if (HasAttributeInCommon(animal, animal2))
                    animalsInCommon[animal].Add(animal2);
            }
        }

        // Shallow copy animals
        var remainingAnimals = new List<Animal>(animals);

        // Find start and remove
        var first = remainingAnimals.FirstOrDefault(a => animalsInCommon[a].Count == 1)!;
        result.Add(first);
        remainingAnimals.Remove(first);

        while (remainingAnimals.Count > 0)
        {
            var last = result[^1];
            var next = remainingAnimals.FirstOrDefault(a => HasAttributeInCommon(a, last))!;
            result.Add(next);
            remainingAnimals.Remove(next);
        }

        var output = result.Select(a => a.Name).ToList();

        
        var response = await client.PostAsJsonAsync(url, output);

        return response.IsSuccessStatusCode;
    }

    private static bool HasAttributeInCommon(Animal animal1, Animal animal2)
    {
        return animal1.Species == animal2.Species ||
               animal1.WeightInGrams == animal2.WeightInGrams ||
               animal1.HeightInCm == animal2.HeightInCm ||
               animal1.AgeInDays == animal2.AgeInDays;
    }
}

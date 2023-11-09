using Common;
using System.Net.Http.Json;
using System.Text;

namespace SolutionPathB.Solution
{
	internal class Challenge3_B
	{
		private const string startEndpoint = "/api/path/b/hard/start";

		private const string sampleEndpoint = "/api/path/b/hard/puzzle";

		Dictionary<char, int> lookup = new Dictionary<char, int>()
			{
				{ 'Ⱄ', 0 },
				{ '·', 1 },
				{ '|', 5 }
			};

		public async Task StartChallenge(HackTheFutureClient hackTheFutureClient)
		{
			var response = await hackTheFutureClient.GetAsync(startEndpoint);

			var content = await response.Content.ReadAsStringAsync();

			if (response.IsSuccessStatusCode)
				Console.WriteLine("succes");
			else
				Console.WriteLine("error");

			await SolveChallenge(hackTheFutureClient);
		}

		public async Task PostAnswer(HackTheFutureClient hackTheFutureClient, string result)
		{
			var response = await hackTheFutureClient.PostAsJsonAsync(sampleEndpoint, result);

			var content = await response.Content.ReadAsStringAsync();

			Console.WriteLine(response.StatusCode);

		}

		public async Task SolveChallenge(HackTheFutureClient hackTheFutureClient)
		{
			List<string> response = await hackTheFutureClient.GetFromJsonAsync<List<string>>(sampleEndpoint);

			int deciamResult = 0;
			foreach (var item in response)
			{
				deciamResult += Solve(item);
			}

			string reverseResult = SolveReverse(deciamResult);

			await PostAnswer(hackTheFutureClient, reverseResult);

		}
		public int Solve(string ex)
		{
			var terms = ex.Split(" ");
			int cumulative = 0;

			for (int i = 0; i < terms.Length; i++)
			{
				var term = terms[terms.Length - i - 1];
				int innerCumulative = 0;
				foreach (var s in term)
				{
					innerCumulative += lookup[s];
				}
				cumulative += innerCumulative * (int)Math.Pow(20, i);
			}
			return cumulative;
		}


		string SolveReverse(int value)
		{
			var remainder = value;

			var result = new List<string>();
			do
			{
				var temp = remainder / 20;
				var term = remainder % 20;

				var count5s = term / 5;
				int rem = term % 5;

				if (term == 0)
					result.Add("Ⱄ");
				else
					result.Add($"{new string('|', count5s)}{new string('·', rem)}");

				remainder = temp;

			} while (remainder > 0);

			var builder = new StringBuilder();

			result.Reverse();

			foreach (var term in result)
			{
				builder.Append(term);
				builder.Append(' ');
			}

			return builder.ToString().TrimEnd();
		}

	}
}

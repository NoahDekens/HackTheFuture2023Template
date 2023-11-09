using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SolutionPathB.Solution
{
	internal class Challenge3_B
	{
		private const string startEndpoint = "/api/path/b/hard/start";

		private const string sampleEndpoint = "/api/path/b/hard/sample";

		Dictionary<char,int> lookup = new Dictionary<char, int>()
			{
				{ '.', 1 },
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
		

		public async Task SolveChallenge(HackTheFutureClient hackTheFutureClient)
		{
			var response = await hackTheFutureClient.GetAsync(sampleEndpoint);

			var content = await response.Content.ReadAsStringAsync();

			string test = "| |";
			var terms = test.Split(" ");
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
			
		}

	}
}

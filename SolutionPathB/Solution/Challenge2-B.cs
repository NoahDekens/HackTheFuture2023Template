using Common;
using SolutionPathB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SolutionPathB.Solution
{
	internal class Challenge2_B
	{
		private const string startEndpoint = "/api/path/b/medium/start";

		private const string sampleEndpoint = "/api/path/b/medium/puzzle";

		public async Task StartChallenge(HackTheFutureClient hackTheFutureClient)
		{
			var response = await hackTheFutureClient.GetAsync(startEndpoint);

			var content = await response.Content.ReadAsStringAsync();

			if (response.IsSuccessStatusCode)
				Console.WriteLine("succes");
			else
				Console.WriteLine("error");

			await Challenge(hackTheFutureClient);
		}


		public async Task Solve(HackTheFutureClient hackTheFutureClient,string result)
		{
			var response = await hackTheFutureClient.PostAsJsonAsync(sampleEndpoint, result);

			var content = await response.Content.ReadAsStringAsync();

			Console.WriteLine(response.StatusCode);

		}


		public async Task Challenge(HackTheFutureClient hackTheFutureClient)
		{
			var response = await hackTheFutureClient.GetFromJsonAsync<List<string>>(sampleEndpoint);
			HashSet<char> keys = new HashSet<char>();

			foreach (var item in response)
			{
				Console.WriteLine(item);
			}


			foreach (var str in response)
			{
				foreach (var chr in str) {
					if (!keys.Contains(chr))
					{
                       if (CheckIfElementContain(response, chr))
						{
							keys.Add(chr);
						}
                    }
				}
			}

			string result = "";

			foreach (var item in keys)
			{
				result += item;
			}

			await Solve(hackTheFutureClient, result);
		}

		public bool CheckIfElementContain(List<string> list, char chr)
		{
			for (int i = 0; i < list.Count; i++)
			{
				string element = list[i];
				if (!element.Contains(chr))
					return false;
			}

			return true;
		}
	}
}

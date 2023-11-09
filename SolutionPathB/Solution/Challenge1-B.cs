using Common;
using SolutionPathB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SolutionPathB.Solution
{
	internal class Challenge1_B
	{
		private const string startEndpoint = "/api/path/b/easy/start";
		private const string challengeEndpoint = "/api/path/b/easy/puzzle";

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
		public async Task SolveChallenge(HackTheFutureClient hackTheFutureClient, int number)
		{
			var response = await hackTheFutureClient.PostAsJsonAsync(challengeEndpoint,number.ToString());

			var content = await response.Content.ReadAsStringAsync();

			Console.WriteLine(response.StatusCode);

		}
		public async Task Challenge(HackTheFutureClient hackTheFutureClient)
		{
			var response = await hackTheFutureClient.GetFromJsonAsync<Dates>(challengeEndpoint);

			string dayToCheck = response.Day;
			string dayOfTheWeek = response.StartDate.DayOfWeek.ToString();
			DateTime startDate = response.StartDate;


			ReadMessage(dayOfTheWeek);
			int amountDays = (response.EndDate - response.StartDate).Days;
			string[] days = new string[amountDays];


			int amountDaysOfWeek = 0;
            for (int i = 0; i < days.Length; i++)
            {
				days[i] = startDate.DayOfWeek.ToString();
				startDate = startDate.AddDays(1);
            }

            for (int i = 0; i < days.Length; i++)
            {
				if (dayToCheck == days[i])
					amountDaysOfWeek++;
            }
			amountDays++;

			Console.WriteLine(amountDaysOfWeek);

			await SolveChallenge(hackTheFutureClient, amountDaysOfWeek);
        }

		public void ReadMessage(string message)
		{
			Console.WriteLine(message);
			Console.ReadLine();
		}
	}
}

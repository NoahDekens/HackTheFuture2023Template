// See https://aka.ms/new-console-template for more information

using BonusChallenge.Solution;
using Common;

var httpClient = new HackTheFutureClient();
await httpClient.Login("CompileCrew", "q463YLzRFx");

await Solution.Solve(httpClient);
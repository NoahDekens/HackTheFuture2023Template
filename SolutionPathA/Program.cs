using Common;
using SolutionPathA.Solution;

var httpClient = new HackTheFutureClient();
await httpClient.Login("CompileCrew", "q463YLzRFx");

//await Challenge1.Solve(httpClient);
//await Challenge2.Solve(httpClient);
await Challenge3.Solve(httpClient);
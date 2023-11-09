// See https://aka.ms/new-console-template for more information


using Common;
using SolutionPathB.Solution;


Console.OutputEncoding = System.Text.Encoding.Unicode;
HackTheFutureClient client = new HackTheFutureClient();

await client.Login("CompileCrew", "q463YLzRFx");

Challenge1_B challenge1_B = new Challenge1_B();

//await challenge1_B.StartChallenge(client);

Challenge2_B challenge2_B = new Challenge2_B();

//await challenge2_B.StartChallenge(client);

Challenge3_B challenge3_B = new Challenge3_B();

await challenge3_B.StartChallenge(client);

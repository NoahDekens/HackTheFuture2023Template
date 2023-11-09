using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BonusChallenge.Solution;

using Area = List<List<char>>;
using NodeArea = List<List<Node>>;

class Node
{
    public char Type { get; set; }
    public List<Node> Edges { get; set; }
    public int Row { get; set; }
    public int Column { get; set; }
    public bool IsExplored { get; set; }
    public Node? Parent { get; set; }

    public Node()
    {

    }

    public Node(char type, List<Node> edges, int row, int column)
    {
        Type = type;
        Edges = edges;
        Row = row;
        Column = column;
        IsExplored = false;
    }
}

class Solution
{

    public static async Task Solve(HackTheFutureClient client)
    {
        // start challenge
        await client.GetAsync("/api/temple/bonus/start");

        bool testSuccess = await SolveForUrl(client, "/api/temple/bonus/sample");
        Console.WriteLine(testSuccess);

        if (testSuccess)
            Console.WriteLine(await SolveForUrl(client, "/api/path/b/hard/puzzle"));

    }

    private static async Task<bool> SolveForUrl(HackTheFutureClient client, string url)
    {
        var response = (await client.GetFromJsonAsync<Area>(url))!;
        
        foreach (var row in response)
        {
            foreach (var cell in row)
            {
                Console.Write(cell);
            }
            Console.WriteLine();
        }

        GenerateNodes(response);
        /*
        Console.WriteLine(output.ToString());
        var result = await client.PostAsJsonAsync(url, output.ToString());

        return result.IsSuccessStatusCode;
        */
        return false;
    }

    private static void GenerateNodes(Area area)
    {
        // create area
        var result = new NodeArea();
        Node root = null!;

        for (int i = 0; i < area.Count; i++)
        {
            result.Add(new(area[i].Count));

            for (int j = 0; j < area[i].Count; j++)
            {
                result[j].Add(new Node());
            }
        }

        for (int rowIndex = 0; rowIndex < area.Count; rowIndex++)
        {
            var areaRow = area[rowIndex];
            var resultRow = result[rowIndex];

            for (int columnIndex = 0; columnIndex < areaRow.Count; columnIndex++)
            {
                var cell = areaRow[columnIndex];
                var node = new Node(cell, new(), rowIndex, columnIndex);
                resultRow[columnIndex] = node;

                if (node.Type == 'S')
                    root = node;
            }
        }

        // fill edges
        for (int rowIndex = 0; rowIndex < result.Count; rowIndex++)
        {
            var row = result[rowIndex];
            for (int columnIndex = 0; columnIndex < row.Count; columnIndex++)
            {
                var node = row[columnIndex];

                FillEdges(result, node);
            }
        }

        BFS(result, root);
    }

    private static void FillEdges(NodeArea area, Node node)
    {
        var right = GetNodeAtPosition(node.Row + 1, node.Column, area);
        if (IsValidEdgeNode(right)) node.Edges.Add(right!);

        var left = GetNodeAtPosition(node.Row - 1, node.Column, area);
        if (IsValidEdgeNode(left)) node.Edges.Add(left!);

        var up = GetNodeAtPosition(node.Row, node.Column + 1, area);
        if (IsValidEdgeNode(up)) node.Edges.Add(up!);

        var down = GetNodeAtPosition(node.Row, node.Column - 1, area);
        if (IsValidEdgeNode(down)) node.Edges.Add(down!);
    }

    private static bool IsValidEdgeNode(Node? node)
    {
        return node is not null && (node.Type == '·' || node.Type == 'S' || node.Type == 'E');
    }

    private static Node? GetNodeAtPosition(int row, int column, NodeArea area)
    {
        if (row < 0 || row >= area.Count)
            return null;

        if (column < 0 || column >= area[row].Count)
            return null;

        return area[row][column];
    }

    private static Node? BFS(NodeArea area, Node root)
    {
        var queue = new Queue<Node>();
        root.IsExplored = true;
        queue.Enqueue(root);

        while (queue.Count != 0)
        {
            var node = queue.Dequeue();

            if (node.Type == 'E')
                return node;

            foreach (var edge in node.Edges)
            {
                if (!edge.IsExplored)
                {
                    edge.IsExplored = true;
                    edge.Parent = node;
                    queue.Enqueue(edge);
                }
            }
        }

        return null;
    }
}
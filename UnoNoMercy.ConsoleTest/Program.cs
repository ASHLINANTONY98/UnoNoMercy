using System.Net.Http.Json;

Console.WriteLine(
    "=== READ CONSISTENCY TEST ===");

var sessionToken =
    "874d4ea7-2c53-442b-ac6d-693bd6ef8c9d";

using var http =
    new HttpClient();

http.BaseAddress =
    new Uri("https://localhost:7285");

Console.WriteLine(
    "Running concurrent reads and writes...");

var readTasks =
    Enumerable.Range(0, 500)
        .Select(async i =>
        {
            try
            {
                var response =
                    await http.PostAsJsonAsync(
                        "/api/Game/state",
                        new
                        {
                            SessionToken = sessionToken
                        });

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine(
                        $"READ FAILED: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"READ ERROR: {ex.Message}");
            }
        });

var writeTasks =
    Enumerable.Range(0, 100)
        .Select(async i =>
        {
            try
            {
                await http.PostAsJsonAsync(
                    "/api/Game/next-turn",
                    new
                    {
                        SessionToken = sessionToken
                    });
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"WRITE ERROR: {ex.Message}");
            }
        });

await Task.WhenAll(
    readTasks.Concat(writeTasks));

Console.WriteLine();
Console.WriteLine(
    "TEST FINISHED");

Console.ReadLine();
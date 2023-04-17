using CaptchaOCR;

internal class Program
{
    private static async Task Main(string[] args)
    {
        string url = "http://challenge01.root-me.org/programmation/ch8/";
        string sessionId = "75661ce426d4ed3cc7455efd0fc8d3a7";
        var solver = new CaptchaSolver(url, sessionId);

        await solver.Solve();

        //Si le captcha n'est pas résolu
        if (solver.Response.Contains("retente ta chance"))
        {
            await Task.Delay(500);
            await Program.Main(args);
        }
        else { Console.ReadLine(); }
    }
}
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Xml.Linq;
using CaptchaOCR;
using IronOcr;

internal class Program
{
    private static async Task Main(string[] args)
    {
        string response;

        var baseAddress = new Uri("http://challenge01.root-me.org/programmation/ch8/");
        var cookieContainer = new CookieContainer();
        using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
        using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
        {
            //Cookie de session
            cookieContainer.Add(baseAddress, new Cookie("PHPSESSID", "1443961b271cf3e325ba79f1949bf27d"));

            Console.WriteLine("Récupération du captcha");
            var getCaptcha = new GetImage("http://challenge01.root-me.org/programmation/ch8/", client);

            Console.WriteLine("Lecture du captcha");
            var readCaptcha = new ReadCaptcha(await getCaptcha.Captcha());

            string solvedCaptcha = readCaptcha.Solve();
            Console.WriteLine("Catpcha : " + solvedCaptcha);

            Console.WriteLine("Envoi du formulaire");
            var values = new Dictionary<string, string>
            {
                //Id du champ de saisie du captcha
                { "cametu", solvedCaptcha }
            };

            var content = new FormUrlEncodedContent(values);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var postResponse = await client.PostAsync("http://challenge01.root-me.org/programmation/ch8/", content);

            Console.WriteLine("Réponse : ");
            response = await postResponse.Content.ReadAsStringAsync();
            Console.WriteLine(response);
        }

        if (response.Contains("retente ta chance"))
        {
            await Task.Delay(500);
            await Program.Main(args);
        }
        else { Console.ReadLine(); }
    }
}
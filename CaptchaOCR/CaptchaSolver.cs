using System.Net;
using System.Net.Http.Headers;

namespace CaptchaOCR
{
    internal class CaptchaSolver
    {
        public string Response { get; private set; }

        private Uri _baseUri;
        private string _sessionId;
        private HttpClient client;
        private HttpClientHandler cookieHandler;
        private string solvedCaptcha;

        public CaptchaSolver(string url, string sessionId)
        {
            this._baseUri = new Uri(url);
            this._sessionId = sessionId;
        }


        internal async Task Solve()
        {
            var cookieContainer = new CookieContainer();
            //Cookie de session
            cookieContainer.Add(_baseUri, new Cookie("PHPSESSID", _sessionId));

            using (cookieHandler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (client = new HttpClient(cookieHandler) { BaseAddress = _baseUri })
            {
                Console.WriteLine("Récupération du captcha");
                var getImage = new GetImage(client.BaseAddress.ToString(), client);

                Console.WriteLine("Lecture du captcha");
                var readCaptcha = new ReadCaptcha(getImage.Base64Image);

                solvedCaptcha = readCaptcha.Solve();
                Console.WriteLine("Catpcha : " + solvedCaptcha);

                Console.WriteLine("Envoi du formulaire");
                HttpResponseMessage postResponse = await SendCaptcha();

                Response = await postResponse.Content.ReadAsStringAsync(); 
                
                Console.WriteLine("Réponse : ");
                Console.WriteLine(Response);
            }
        }

        private async Task<HttpResponseMessage> SendCaptcha()
        {
            var values = new Dictionary<string, string>
            {
                //Id du champ de saisie du captcha
                { "cametu", solvedCaptcha }
            };
            var content = new FormUrlEncodedContent(values);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var postResponse = await client.PostAsync(_baseUri.ToString(), content);
            return postResponse;
        }
    }
}

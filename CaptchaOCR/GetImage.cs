namespace CaptchaOCR
{
    internal class GetImage
    {
        private string _url;
        private HttpClient _client;

        public GetImage(string url, HttpClient client)
        {
            _url = url;
            _client = client;
        }

        public async Task<byte[]> Captcha()
        {
            string htmlCode;

            htmlCode = await _client.GetStringAsync(_url);

            string[] tab = htmlCode.Split(new char[] { '"' });

            string image = tab[1].Remove(0, 22);

            return Convert.FromBase64String(image);
        }
    }
}

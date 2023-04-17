namespace CaptchaOCR
{
    internal class GetImage
    {
        private string _url;
        private HttpClient _client;
        private byte[]? _Base64Image = null;

        public byte[] Base64Image
        {
            get
            {
                if (_Base64Image == null)
                    SetImageArray();

                return _Base64Image;
            }
            private set
            {
                _Base64Image = value;
            }
        }

        public GetImage(string url, HttpClient client)
        {
            _url = url;
            _client = client;
        }

        public void SetImageArray()
        {
            string htmlCode = _client.GetStringAsync(_url).Result;

            string[] htmlTab = htmlCode.Split(new char[] { '"' });

            //Suppression de 'data:image/png;base64,'
            string imageBase64 = htmlTab[1].Remove(0, 22);

            Base64Image = Convert.FromBase64String(imageBase64);
        }
    }
}

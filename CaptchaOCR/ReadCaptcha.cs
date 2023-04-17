using IronOcr;

namespace CaptchaOCR
{
    internal class ReadCaptcha
    {
        private IronTesseract OCR { get; }
        private byte[] _image;
        private string solvedCaptcha;

        public ReadCaptcha(byte[] image)
        {
            _image = image;

            IronOcr.License.LicenseKey = "IRONOCR.ACAILLOL.24317-C5C6794B67-EL34SQ-YXESGSWURUME-DHFETUAYGGBV-I6D46QASD2HR-U6ZKTONMXXXE-HRQVW3UCQ47I-5IWBSS-TUDAMQUU7FKJUA-DEPLOYMENT.TRIAL-OM26OJ.TRIAL.EXPIRES.11.MAY.2023";

            OCR = new IronTesseract(); // nothing to configure
            OCR.Language = OcrLanguage.EnglishBest;
            OCR.MultiThreaded = true;
            OCR.Configuration.WhiteListCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        }

        public string Solve()
        {
            using (var input = new OcrInput())
            {
                input.AddImage(_image);
                input.ReplaceColor(IronSoftware.Drawing.Color.Black, IronSoftware.Drawing.Color.White);
                solvedCaptcha = OCR.Read(input).Text;

                SaveCaptcha();

                return solvedCaptcha;
            }
        }

        private void SaveCaptcha()
        {
            using (var image = System.IO.File.Create("captchas\\" + solvedCaptcha + ".png"))
            {
                image.Write(_image);
                image.Close();
            }
        }
    }
}

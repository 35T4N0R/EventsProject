using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ap1
{
    public class QRCodeMaker
    {
        static byte[] imageBytes;
        string text;

        public QRCodeMaker(int EventId, string Type, string Name, string Localization, DateTime? Date, TimeSpan time)
        {
            DateTime date = Convert.ToDateTime(Date);
            text = $"EventId:{EventId}, EventType:{Type}, EventName:{Name},Localization:{Localization},Date:{date.ToString("dd.MM.yyyy")},Time:{time.Hours}:{time.Minutes}";//{DateTime.Day}.{DateTime.Month}.{DateTime.Year}   {DateTime.Hour}:{DateTime.Minute}
        }

        public QRCodeMaker(int userId, int eventId, int ticketAmount)
        {
            var baseUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);

            text = baseUrl + $"/api/webapi/{userId}/{eventId}/{ticketAmount}"; 
        }

        public byte[] GenerateQRCode()
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);

            using (Bitmap bitmap = qrCode.GetGraphic(20))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bitmap.Save(ms, ImageFormat.Png);
                    return imageBytes = ms.ToArray();
                }
            }
        }
    }
}
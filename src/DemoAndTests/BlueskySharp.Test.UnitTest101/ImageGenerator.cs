using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueskySharp.Test.UnitTest101
{
    internal static class ImageGenerator
    {
        public static MemoryStream CreateTestImage()
        {
            // 画像のサイズを指定
            int width = 640;
            int height = 360;

            // 背景色とテキストの設定
            Color backgroundColor = Color.LightBlue;
            string text = "Test image";
            string dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // Bitmap オブジェクトを作成
            using (Bitmap bitmap = new Bitmap(width, height))
            {
                // グラフィックスオブジェクトを作成
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    // 背景色を塗りつぶす
                    graphics.Clear(backgroundColor);

                    // テキストのフォントとブラシを設定
                    Font font = new Font("Arial", 24);
                    Brush brush = Brushes.Black;

                    // テキストの描画
                    graphics.DrawString(text, font, brush, new PointF(50, 50));
                    graphics.DrawString(dateTime, font, brush, new PointF(50, 100));
                }

                // MemoryStream に画像を書き込む
                MemoryStream memoryStream = new MemoryStream();
                bitmap.Save(memoryStream, ImageFormat.Png);
                memoryStream.Position = 0; // ストリームの位置を先頭に戻す
                return memoryStream;
            }
        }
    }
}

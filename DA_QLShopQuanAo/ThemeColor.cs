using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA_QLShopQuanAo
{
    public static class ThemeColor
    {

        public static Color PrimaryColor { get; set; }
        public static Color SecondaryColor { get; set; }
        public static List<string> ColorList = new List<string>() {
    "#1A2A45", // Navy đậm - màu chủ đạo sidebar
   "#0F3C5D", // Xanh đậm chuyển tiếp
    "#006E7F", // Xanh teal đậm
    "#008F8F", // Xanh ngọc trung tính
    "#00A6A6", // Xanh tươi nổi bật
   
    "#40C7C7", // Xanh pastel



   
};


        public static Color ChangeColorBrightness(Color color, double correctionFactor)
        {
            double red = color.R;
            double green = color.G;
            double blue = color.B;

            //If correction factor is less than 0, darken color.
            if (correctionFactor < 0)
            {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }
            //If correction factor is greater than zero, lighten color.
            else
            {
                red = (255 - red) * correctionFactor + red;
                green = (255 - green) * correctionFactor + green;
                blue = (255 - blue) * correctionFactor + blue;
            }

            return Color.FromArgb(color.A, (byte)red, (byte)green, (byte)blue);
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace CircleApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

          

            //    DrawFace(e.Graphics);
            //    return;

            /*
             Test(e);
              return;
*/

            var text = "Asadasdasdsadasd";
            var font = new Font(FontFamily.GenericSansSerif, 24, FontStyle.Bold);
            var rectX = 100f;
            var rectY = 100f;
            var rectWidth = 300f;
            var rectHeight = 300f;
            e.Graphics.DrawRectangle(new Pen(Color.Red), rectX, rectY, rectWidth, rectHeight);
            #region
            /*
             * 圆形的直径 = 两边中的最小值
             * offx = 矩形X点 + （（矩形宽度- 圆形直径） / 2）
             * offy = 矩形Y点 + （（矩形高度 - 矩形直径） / 2）
             * 
             计算原点：DX = 圆形的半径
                     DY =矩形的半径
             */
            #endregion
            var diameter = Math.Min(rectHeight, rectWidth);//直径
            var offx = rectX + ((rectWidth - diameter) / 2);//偏移量
            var offy = rectY + ((rectWidth - diameter) / 2);

            for (int i = 0; i < text.Length; ++i)
            {
                var c = new String(text[i], 1);

                var fontSize = e.Graphics.MeasureString(c, font);

                var charRadius = (diameter / 2) + fontSize.Height;

       
                var angle = (((float)i / text.Length) - 0.25) * 2 * Math.PI;
        
                var x = (int)(offx  + (diameter / 2) + Math.Cos(angle) * charRadius);

                var y = (int)(offy + (diameter / 2) + Math.Sin(angle) * charRadius);

                e.Graphics.TranslateTransform(x, y);//

                e.Graphics.RotateTransform((float)(90 + 360 * angle / (2 * Math.PI)));//按照顺时针方向旋转指定的对象

                e.Graphics.DrawString(c, font, Brushes.Red, 0, 0);




                e.Graphics.ResetTransform();//将Graphics的世界矩阵转换为单位矩阵

                e.Graphics.DrawArc(new Pen(Brushes.Blue, 1.0f), offx + fontSize.Height, offy + fontSize.Height, diameter - (fontSize.Height * 2), diameter - (fontSize.Height * 2), 0, 360);

                e.Graphics.DrawLine(new Pen(Color.Black), rectX + rectWidth / 2, 0, 2, 1000000f);

                e.Graphics.DrawLine(new Pen(Color.Black), 0, rectY + rectHeight / 2, 100000f, 2);

            }

        }



        private void Test(PaintEventArgs e)
        {


            e.Graphics.DrawRectangle(new Pen(Color.Red), 100, 100, 300, 300);
            var center = new Point(300, 300);
            var radius = Math.Min(300, 300) / 2;
            var text = "Asdadasdasd";

            var font = new Font(FontFamily.GenericSansSerif, 24, FontStyle.Bold);
            for (var i = 0; i < text.Length; ++i)
            {
                var c = new String(text[i], 1);

                var size = e.Graphics.MeasureString(c, font);
                var charRadius = radius + size.Height;


                var angle = (((float)i / text.Length) - 0.25) * 2 * Math.PI;

                var x = (int)(250 + Math.Cos(angle) * charRadius);
                var y = (int)(250 + Math.Sin(angle) * charRadius);


                e.Graphics.TranslateTransform(x, y);

                e.Graphics.RotateTransform((float)(90 + 360 * angle / (2 * Math.PI)));
                e.Graphics.DrawString(c, font, Brushes.Red, 0, 0);

                e.Graphics.ResetTransform();
                e.Graphics.DrawLine(new Pen(Color.Black), 0, radius * 2, 100000f, 2);
                e.Graphics.DrawLine(new Pen(Color.Black), radius * 2, 0, 2, 100000f);


                e.Graphics.DrawArc(new Pen(Brushes.DarkGreen, 2.0f), 100 + size.Height, 100 + size.Height, radius * 2 - (2 * size.Height), radius * 2 - (2 * size.Height), 0, 360);
            }

        }





        private void SetScale(Graphics g)
        {

            g.TranslateTransform(Width / 2, Height / 2);

            float inches = Math.Min(Width / g.DpiX, Height / g.DpiX);

            g.ScaleTransform(inches * g.DpiX / 2000, inches * g.DpiY / 2000);
        }

        private void DrawFace(Graphics g)
        {
            Brush brush = new SolidBrush(ForeColor);
            Font font = new Font("Arial", 20);

            float x, y;

            const int numHours = 12;
            const int deg = 360 / numHours;
            const int FaceRadius = 150;

            for (int i = 1; i <= numHours; i++)
            {
                x = GetCos(i * deg + 90) * FaceRadius;
                y = GetSin(i * deg + 90) * FaceRadius;

                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;

                g.DrawString(i.ToString(), font, brush, -x, -y, format);

            }
            brush.Dispose();
            font.Dispose();
        }

        private static float GetSin(float degAngle)
        {
            return (float)Math.Sin(Math.PI * degAngle / 180f);
        }

        private static float GetCos(float degAngle)
        {
            return (float)Math.Cos(Math.PI * degAngle / 180f);
        }





        private void Test2( PaintEventArgs e)
        {

            e.Graphics.DrawRectangle(new Pen(Color.Red), 100, 200, 300, 300);
            e.Graphics.DrawArc(new Pen(Color.Red), new Rectangle(0100, 0200, 300, 300), 0, 360);

            int count = 8; //8个等分点
            var radians = (Math.PI / 180) * Math.Round(360.0 / count); //弧度

            double ox = 100.0;
            double oy = 200.0;

            int r = 30;

            for (int i = 0; i < count; i++)
            {
                double x = ox + r * Math.Sin(radians * i);
                double y = oy + r * Math.Cos(radians * i);

                Pen blackPen = new Pen(Color.Black, 3);
                Rectangle rect = new Rectangle(Convert.ToInt32(x), Convert.ToInt32(y), 20, 20);
                e.Graphics.DrawRectangle(blackPen, rect);
            }


        }
    }





}

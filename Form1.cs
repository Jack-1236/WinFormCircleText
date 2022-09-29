using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
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
            /*
                        Test3(e);
                        return;
            */
            //    DrawFace(e.Graphics);
            //    return;

            /*
             Test(e);
              return;
*/

            //字体大小
            //弧度


            /*  TestLabel(e);
              return;*/

            //   OrigationTestLabel2(e);
            //   DrawCurvedText(e.Graphics, "sdsds", new Point(300, 300), distanceFromCentreToBaseOfText: 150, radiansToTextCentre: 50, new Font(FontFamily.GenericSerif, 25), new SolidBrush(Color.Red));

            /* var text = "sadaddasd";
             for (int i = 0; i < text.Length; i++) {
                 e.Graphics.RotateTransform(90+(i*10));
                 e.Graphics.DrawString(text[i].ToString(), this.Font, Brushes.Red, new Point(300, 300));

             }*/

            //    OrigationTestLabel2(e);

            //  OrigationTestLabel2(e);
            Test12(e);
            return;
        }



        private void OrigationTestLabel2(PaintEventArgs e)
        {
            var text = "ABCDEFG";
            var font = new Font(FontFamily.GenericSansSerif, 40, FontStyle.Regular);
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


            DrawCurvedText(e.Graphics, text, new Point((int)rectX, (int)rectY), 20, 60, font, Brushes.Red);

            for (int i = 0; i < text.Length; ++i)
            {
                var c = new String(text[i], 1);

                var fontSize = e.Graphics.MeasureString(c, font);

                var angle = (((float)i / text.Length) - 0.25) * 2 * Math.PI;



                //-1.5707963267948966
                var x = (int)(offx + (diameter / 2) + Math.Cos(angle) * (diameter / 2));

                var y = (int)(offy + (diameter / 2) + Math.Sin(angle) * (diameter / 2));

                e.Graphics.TranslateTransform(x, y);//

                e.Graphics.RotateTransform((float)(90 + 360 * angle / (2 * Math.PI)));//按照顺时针方向旋转指定的对象

                e.Graphics.DrawString(c, font, Brushes.Red, 0, 0);
                e.Graphics.DrawRectangle(new Pen(Color.Red), 0, 0, fontSize.Width, fontSize.Height);

                e.Graphics.ResetTransform();//将Graphics的世界矩阵转换为单位矩阵

                e.Graphics.DrawArc(new Pen(Brushes.Blue, 1.0f), offx + fontSize.Height, offy + fontSize.Height, diameter - (fontSize.Height * 2), diameter - (fontSize.Height * 2), 0, 360);

                e.Graphics.DrawLine(new Pen(Color.Black), rectX + rectWidth / 2, 0, 2, 1000000f);

                e.Graphics.DrawLine(new Pen(Color.Black), 0, rectY + rectHeight / 2, 100000f, 2);

            }




        }



        private void Test12(PaintEventArgs e)
        {

      
          //  e.Graphics.TranslateTransform(100, 100);
            var radio = 0;
            for (int i = 0; i < 13; i++)
            {
                radio += 10;
                e.Graphics.RotateTransform(radio);
                e.Graphics.DrawString("A", this.Font, SystemBrushes.Highlight, 50, 100);

            }
            e.Graphics.DrawRectangle(new Pen(Color.Red), new Rectangle(50, 100, 100, 100));

            e.Graphics.FillPie()
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="graphics">画笔</param>
        /// <param name="text">文本</param>
        /// <param name="centre">位置</param>
        /// <param name="distanceFromCentreToBaseOfText">间隔</param>
        /// <param name="radiansToTextCentre">弧度</param>
        /// <param name="font">字体</param>
        /// <param name="brush">笔刷</param>
        private static void DrawCurvedText(Graphics graphics, string text, Point centre, float distanceFromCentreToBaseOfText, float radiansToTextCentre, Font font, Brush brush)
        {
            //以后使用的周长
            var circleCircumference = (float)(Math.PI * 2 * distanceFromCentreToBaseOfText);
            //获得每个字符的宽度
            var characterWidths = GetCharacterWidths(graphics, text, font).ToArray();

            // 文本高度
            var characterHeight = graphics.MeasureString(text, font).Height;

            //文本宽度总和
            var textLength = characterWidths.Sum();

            //上面的字符串长度是我们将用于渲染字符串的弧长。求出所需的起始角度
            //将文本放在radiansToTextCentre的中间。
            float fractionOfCircumference = textLength / circleCircumference;

            float currentCharacterRadians = radiansToTextCentre + (float)(Math.PI * fractionOfCircumference);

            for (int characterIndex = 0; characterIndex < text.Length; characterIndex++)
            {
                char @char = text[characterIndex];

                // Polar to cartesian
                float x = (float)(distanceFromCentreToBaseOfText * Math.Sin(currentCharacterRadians));
                float y = -(float)(distanceFromCentreToBaseOfText * Math.Cos(currentCharacterRadians));

                using (GraphicsPath characterPath = new GraphicsPath())
                {
                    characterPath.AddString(@char.ToString(), font.FontFamily, (int)font.Style, font.Size, Point.Empty,
                                            StringFormat.GenericTypographic);

                    var pathBounds = characterPath.GetBounds();

                    //转换矩阵，将字符移动到正确的位置。

                    //注意，所有对矩阵类的操作都是前置的，所以我们反过来应用它们。
                    var transform = new Matrix();

                    // Translate to the final position
                    transform.Translate(centre.X + x, centre.Y + y);

                    // Rotate the character
                    var rotationAngleDegrees = currentCharacterRadians * 180F / (float)Math.PI - 180F;
                    transform.Rotate(rotationAngleDegrees);

                    // Translate the character so the centre of its base is over the origin
                    transform.Translate(-pathBounds.Width / 2F, -characterHeight);

                    characterPath.Transform(transform);

                    // Draw the character
                    graphics.FillPath(brush, characterPath);
                }

                if (characterIndex != text.Length - 1)
                {
                    // Move "currentCharacterRadians" on to the next character
                    var distanceToNextChar = (characterWidths[characterIndex] + characterWidths[characterIndex + 1]) / 2F;
                    float charFractionOfCircumference = distanceToNextChar / circleCircumference;
                    currentCharacterRadians -= charFractionOfCircumference * (float)(2F * Math.PI);
                }
            }
        }

        private static IEnumerable<float> GetCharacterWidths(Graphics graphics, string text, Font font)
        {
            // The length of a space. Necessary because a space measured using StringFormat.GenericTypographic has no width.
            // We can't use StringFormat.GenericDefault for the characters themselves, as it adds unwanted spacing.
            var spaceLength = graphics.MeasureString(" ", font, Point.Empty, StringFormat.GenericDefault).Width;

            return text.Select(c => c == ' ' ? spaceLength : graphics.MeasureString(c.ToString(), font, Point.Empty, StringFormat.GenericTypographic).Width);

        }

        private void OrigationTestLabel(PaintEventArgs e)
        {
            var text = "Asadadsfdfdsfdsfdssdasdsadasd";
            var font = new Font(FontFamily.GenericSansSerif, 25, FontStyle.Regular);
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


                var angle = (((float)i / text.Length) - 0.25) * 2 * Math.PI;

                var x = (int)(offx + (diameter / 2) + Math.Cos(angle) * (diameter / 2));

                var y = (int)(offy + (diameter / 2) + Math.Sin(angle) * (diameter / 2));

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





        private void Test2(PaintEventArgs e)
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




        protected void Test3(PaintEventArgs e)
        {
            string s = "测试文字测试文字测试文字测试文字测试文字测试文字测试文字";
            var origin = new Point(200, 200);
            var font = new Font("Arial", 20);
            var format = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center,
            };

            var fontSize = e.Graphics.MeasureString(s, font);
            e.Graphics.DrawRectangle(new Pen(Color.Red), 200, 200, 150 + fontSize.Width, fontSize.Height + 150);

            e.Graphics.DrawArc(new Pen(Color.Red), origin.X, origin.Y, 150, 150, 0, 360);
            int radius = 150;

            Graphics g = e.Graphics;
            var brush = new SolidBrush(ForeColor);

            g.TranslateTransform(origin.X, origin.Y);

            foreach (var c in s)
            {
                g.RotateTransform(360f / s.Length);
                g.DrawString(c.ToString(), font, brush, 0, -radius, format);
            }
            brush.Dispose();
            font.Dispose();
            base.OnPaint(e);

        }





        private void Test8(PaintEventArgs e)
        {

            for (int i = 0; i <= 90; i += 10)
            {
                Matrix matrix = new Matrix();
                matrix.Rotate(90 + i);
                Graphics g = e.Graphics;
                g.Transform = matrix;
                g.DrawLine(Pens.Blue, 0, 0, 250, 0);
                g.DrawString("zhuzhao", this.Font, Brushes.Blue, new RectangleF(250, 0, 100, 100));
            }
        }


        private void Test9(PaintEventArgs e)
        {
            var text = "ABCDEFG";
            //    e.Graphics.DrawRectangle(new Pen(Color.Red), 0, 0, 300, 300);

            float radio = 0f;
            var font = new Font(FontFamily.GenericSerif, 25);
            for (int i = 0; i < text.Length; i++)
            {
                var c = new String(text[i], 1);
                var fontSize = e.Graphics.MeasureString(c, font);
                Matrix matrix = new Matrix();
                radio += fontSize.Width * i;
                matrix.Rotate(radio);

                e.Graphics.Transform = matrix;
                e.Graphics.DrawLine(Pens.Blue, 0, 0, 250, 0);
                e.Graphics.DrawString(c, font, Brushes.Red, new RectangleF(150, 0, 100, 100));

            }
        }


    }






    public class GraphicsText
    {
        private Graphics _graphics;

        public GraphicsText()
        {

        }

        public Graphics Graphics
        {
            get { return _graphics; }
            set { _graphics = value; }
        }

        /// <summary>
        /// 绘制根据矩形旋转文本
        /// </summary>
        /// <param name="s">文本</param>
        /// <param name="font">字体</param>
        /// <param name="brush">填充</param>
        /// <param name="layoutRectangle">局部矩形</param>
        /// <param name="format">布局方式</param>
        /// <param name="angle">角度</param>
        public void DrawString(string s, Font font, Brush brush, RectangleF layoutRectangle, StringFormat format, float angle)
        {
            // 求取字符串大小
            SizeF size = _graphics.MeasureString(s, font);

            // 根据旋转角度，求取旋转后字符串大小
            SizeF sizeRotate = ConvertSize(size, angle);

            // 根据旋转后尺寸、布局矩形、布局方式计算文本旋转点
            PointF rotatePt = GetRotatePoint(sizeRotate, layoutRectangle, format);

            // 重设布局方式都为Center
            StringFormat newFormat = new StringFormat(format);
            newFormat.Alignment = StringAlignment.Center;
            newFormat.LineAlignment = StringAlignment.Center;

            // 绘制旋转后文本
            DrawString(s, font, brush, rotatePt, newFormat, angle);
        }

        /// <summary>
        /// 绘制根据点旋转文本，一般旋转点给定位文本包围盒中心点
        /// </summary>
        /// <param name="s">文本</param>
        /// <param name="font">字体</param>
        /// <param name="brush">填充</param>
        /// <param name="point">旋转点</param>
        /// <param name="format">布局方式</param>
        /// <param name="angle">角度</param>
        public void DrawString(string s, Font font, Brush brush, PointF point, StringFormat format, float angle)
        {
            // Save the matrix
            Matrix mtxSave = _graphics.Transform;

            Matrix mtxRotate = _graphics.Transform;
            mtxRotate.RotateAt(angle, point);
            _graphics.Transform = mtxRotate;

            _graphics.DrawString(s, font, brush, point, format);

            // Reset the matrix
            _graphics.Transform = mtxSave;
        }

        private SizeF ConvertSize(SizeF size, float angle)
        {
            Matrix matrix = new Matrix();
            matrix.Rotate(angle);

            // 旋转矩形四个顶点
            PointF[] pts = new PointF[4];
            pts[0].X = -size.Width / 2f;
            pts[0].Y = -size.Height / 2f;
            pts[1].X = -size.Width / 2f;
            pts[1].Y = size.Height / 2f;
            pts[2].X = size.Width / 2f;
            pts[2].Y = size.Height / 2f;
            pts[3].X = size.Width / 2f;
            pts[3].Y = -size.Height / 2f;
            matrix.TransformPoints(pts);

            // 求取四个顶点的包围盒
            float left = float.MaxValue;
            float right = float.MinValue;
            float top = float.MaxValue;
            float bottom = float.MinValue;

            foreach (PointF pt in pts)
            {
                // 求取并集
                if (pt.X < left)
                    left = pt.X;
                if (pt.X > right)
                    right = pt.X;
                if (pt.Y < top)
                    top = pt.Y;
                if (pt.Y > bottom)
                    bottom = pt.Y;
            }

            SizeF result = new SizeF(right - left, bottom - top);
            return result;
        }

        private PointF GetRotatePoint(SizeF size, RectangleF layoutRectangle, StringFormat format)
        {
            PointF pt = new PointF();

            switch (format.Alignment)
            {
                case StringAlignment.Near:
                    pt.X = layoutRectangle.Left + size.Width / 2f;
                    break;
                case StringAlignment.Center:
                    pt.X = (layoutRectangle.Left + layoutRectangle.Right) / 2f;
                    break;
                case StringAlignment.Far:
                    pt.X = layoutRectangle.Right - size.Width / 2f;
                    break;
                default:
                    break;
            }

            switch (format.LineAlignment)
            {
                case StringAlignment.Near:
                    pt.Y = layoutRectangle.Top + size.Height / 2f;
                    break;
                case StringAlignment.Center:
                    pt.Y = (layoutRectangle.Top + layoutRectangle.Bottom) / 2f;
                    break;
                case StringAlignment.Far:
                    pt.Y = layoutRectangle.Bottom - size.Height / 2f;
                    break;
                default:
                    break;
            }

            return pt;


        }






    }





}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PPTBoardEditor_WPF {
    class UIHelper {
        private static void drawRectangle(ref Canvas canvas, double x, double y, double w, double h, Color c, Color b) {
            Rectangle mino = new Rectangle() {
                Width = w,
                Height = h,
                Stroke = new SolidColorBrush(b),
                StrokeThickness = 1,
                Fill = new SolidColorBrush(c)
            };

            Canvas.SetLeft(mino, x);
            Canvas.SetTop(mino, y);

            canvas.Children.Add(mino);
        }

        public static void drawBoard(ref Canvas canvas, int[,] board, bool active) {
            canvas.Children.Clear();
            if (!active) return;

            for (int i = 0; i < 10; i++) {
                for (int j = 0; j < 40; j++) {
                    drawRectangle(ref canvas, i * (canvas.Width / 10), (39 - j) * (canvas.Height / 40), canvas.Width / 10, canvas.Height / 40, Tetromino.BoardColor(board[i, j]), Colors.Black);
                }
            }

            //gfx.DrawLine(new Pen(Color.Red), 0, canvas.Height / 2, canvas.Width, canvas.Height / 2);
            //gfx.Flush();
        }

        public static void drawSelector(ref Canvas canvas, int[] colors, bool active) {
            canvas.Children.Clear();
            if (!active) return;

            for (int i = 0; i < 10; i++) {
                int j = i;
                if (j == 0) j = -1;
                else if (j != 9) j--;

                drawRectangle(ref canvas, i * (canvas.Width / 10), 0, canvas.Width / 10, canvas.Height, Tetromino.BoardColor(j), Colors.Black);
            }

            for (int i = 0; i < 2; i++) {
                if (colors[i] == 8) return;
                if (colors[i] == -1) colors[i] = 0;
                else if (colors[i] != 9) colors[i]++;
            }

            drawRectangle(ref canvas, colors[0] * (canvas.Width / 10), 0, canvas.Width / 10, canvas.Height, Colors.Transparent, Colors.White);
            drawRectangle(ref canvas, colors[0] * (canvas.Width / 10) + 1, 1, canvas.Width / 10 - 2, canvas.Height - 2, Colors.Transparent, Colors.Black);

            drawRectangle(ref canvas, colors[1] * (canvas.Width / 10) + 2, 2, canvas.Width / 10 - 4, canvas.Height - 4, Colors.Transparent, Colors.White);
            drawRectangle(ref canvas, colors[1] * (canvas.Width / 10) + 3, 3, canvas.Width / 10 - 6, canvas.Height - 6, Colors.Transparent, Colors.Black);
        }
    }
}

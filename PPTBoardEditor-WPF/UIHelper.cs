using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace PPTBoardEditor_WPF {
    class UIHelper {
        public static void drawBoard(ref System.Windows.Controls.Image canvas, int[,] board, bool active) {
            if (!active) {
                canvas.Source = null;
                return;
            }

            int w = (int)(canvas.Width * 1.2), h = (int)(canvas.Height * 1.2);
            Bitmap image = new Bitmap(w, h);

            using (Graphics gfx = Graphics.FromImage(image)) {
                gfx.FillRectangle(new SolidBrush(Color.FromArgb(10, 10, 10)), new Rectangle(0, 0, w, h));

                for (int i = 0; i < 10; i++) {
                    for (int j = 0; j < 40; j++) {
                        Rectangle mino = new Rectangle(i * (w / 10), (39 - j) * (h / 40), w / 10, h / 40);
                        gfx.FillRectangle(new SolidBrush(Tetromino.BoardColor(board[i, j])), mino);

                        mino.Width--;
                        mino.Height--;
                        gfx.DrawRectangle(new Pen(Color.Black), mino);
                    }
                }

                gfx.DrawLine(new Pen(Color.Red), 0, h / 2, w, h / 2);
                gfx.Flush();
            }

            BitmapImage bitmap = new BitmapImage();
            using (MemoryStream memory = new MemoryStream() {Position = 0}) {
                image.Save(memory, ImageFormat.Bmp);
                bitmap.BeginInit();
                bitmap.StreamSource = memory;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
            }

            canvas.Source = bitmap;
        }

        public static void drawSelector(ref System.Windows.Controls.Image canvas, int[] colors, bool active) {
            if (!active) {
                canvas.Source = null;
                return;
            }

            int w = (int)(canvas.Width * 1.2), h = (int)(canvas.Height * 1.2);
            Bitmap image = new Bitmap(w, h);

            using (Graphics gfx = Graphics.FromImage(image)) {
                gfx.FillRectangle(new SolidBrush(Color.FromArgb(10, 10, 10)), new Rectangle(0, 0, w, h));

                for (int i = 0; i < 10; i++) {
                    int j = i;
                    if (j == 0) j = -1;
                    else if (j != 9) j--;

                    Rectangle mino = new Rectangle(i * (w / 10), 0, w / 10, h);
                    gfx.FillRectangle(new SolidBrush(Tetromino.BoardColor(j)), i * (w / 10), 0, w / 10, h);
                    
                    mino.Width--;
                    mino.Height--;
                    gfx.DrawRectangle(new Pen(Color.Black), mino);
                }

                for (int i = 0; i < 2; i++) {
                    if (colors[i] == 8) return;
                    if (colors[i] == -1) colors[i] = 0;
                    else if (colors[i] != 9) colors[i]++;
                }

                gfx.DrawRectangle(new Pen(Color.White), colors[0] * (w / 10), 0, w / 10 - 1, h - 1);
                gfx.DrawRectangle(new Pen(Color.Black), colors[0] * (w / 10) + 1, 1, w / 10 - 3, h - 3);

                gfx.DrawRectangle(new Pen(Color.White), colors[1] * (w / 10) + 2, 2, w / 10 - 5, h - 5);
                gfx.DrawRectangle(new Pen(Color.Black), colors[1] * (w / 10) + 3, 3, w / 10 - 7, h - 7);

                gfx.Flush();
            }



            BitmapImage bitmap = new BitmapImage();
            using (MemoryStream memory = new MemoryStream() { Position = 0 }) {
                image.Save(memory, ImageFormat.Bmp);
                bitmap.BeginInit();
                bitmap.StreamSource = memory;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
            }

            canvas.Source = bitmap;
        }
    }
}

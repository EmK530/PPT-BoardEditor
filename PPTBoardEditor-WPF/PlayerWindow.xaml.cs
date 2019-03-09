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
    public partial class PlayerWindow: Window {
        DispatcherTimer scanTimer = new DispatcherTimer() {
            Interval = TimeSpan.FromMilliseconds(50)
        };

        public PlayerWindow(int index) {
            InitializeComponent();
            windowIndex = index;

            scanTimer.Tick += scanTimer_Tick;
            scanTimer.Start();
        }

        int windowIndex, playerIndex;

        int playerID {
            get => Convert.ToInt32(windowIndex != playerIndex);
        }

        int[,] board = new int[10, 40];
        int[] selectedColor = new int[2] { 9, -1 };

        int pieces = 0;
        int holdPTR = 0x0;
        bool dropState = false;
        
        private void scanTimer_Tick(object sender, EventArgs e) {
            playerIndex = GameHelper.FindPlayer();

            int boardAddress = GameHelper.BoardAddress(playerID);
            bool active = boardAddress > 0x08000000;

            if (active) {
                for (int i = 0; i < 10; i++) {
                    int columnAddress = GameHelper.DirectRead(boardAddress + i * 0x08);
                    for (int j = 0; j < 40; j++) {
                        board[i, j] = GameHelper.DirectRead(columnAddress + j * 0x04);
                    }
                }

                bool drop = GameHelper.PieceDropped(playerID);
                if (drop != dropState) {
                    if (!drop) pieces++;
                    dropState = drop;
                }

                int queueAddress = GameHelper.QueueAddress(playerID);
                int current = GameHelper.CurrentPiece(playerID);
                if (current == 255 && GameHelper.FrameCount() < 140 && listQueue.Items.Count > 0) {
                    for (int i = 0; i < (checkLoop.IsChecked.Value? 5 : Math.Min(5, listQueue.Items.Count)); i++) {
                        GameHelper.DirectWrite(queueAddress + i * 0x04, ((Tetromino)listQueue.Items[(pieces + i) % listQueue.Items.Count]).Index);
                    }
                }

                int hold = GameHelper.HoldPointer(playerID);
                if (holdPTR != hold && holdPTR < 0x08000000 && hold >= 0x08000000) {
                    int rot = GameHelper.RotationPointer(playerID);
                    if (rot >= 0x08000000) {
                        pieces++;
                    } else {
                        hold = 0x8;
                    }
                }
                holdPTR = hold;

                if (current != 255 && (pieces + 5 < listQueue.Items.Count || (checkLoop.IsChecked.Value && listQueue.Items.Count > 0))) {
                    GameHelper.DirectWrite(queueAddress + 0x10, ((Tetromino)listQueue.Items[(pieces + 5) % listQueue.Items.Count]).Index);
                }

            } else {
                int[,] board = new int[10, 40];

                pieces = 0;
                dropState = false;
            }

            UIHelper.drawBoard(ref canvasBoard, board, active);
            UIHelper.drawSelector(ref canvasSelector, (int[])selectedColor.Clone(), active);

            Title = (boardAddress >= 0x08000000) ? GameHelper.PlayerName(playerID) : $"Player {windowIndex + 1}";
        }

        bool canvasBoardPressed = false;

        private void CanvasBoard_MouseDown(object sender, MouseButtonEventArgs e) {
            canvasBoardPressed = true;
            CanvasBoard_MouseMove(sender, e);
        }

        private void CanvasBoard_MouseUp(object sender, MouseButtonEventArgs e) {
            if (e.LeftButton == MouseButtonState.Released && e.RightButton == MouseButtonState.Released)
                canvasBoardPressed = false;
        }

        private void CanvasBoard_MouseMove(object sender, MouseEventArgs e) {
            if (canvasBoardPressed) {
                Point pt = e.GetPosition(canvasBoard);

                int x = (int)Math.Floor(pt.X / 15);
                int y = 39 - (int)Math.Floor(pt.Y / 15);
                int boardAddress = GameHelper.BoardAddress(playerID);
                
                if (boardAddress >= 0x08000000 && 0 <= x && x <= 9 && 0 <= y && y <= 39 && board[x, y] != -2) {
                    int pixelAddress = GameHelper.DirectRead(
                        boardAddress + x * 0x08
                    ) + y * 0x04;

                    if (e.LeftButton == MouseButtonState.Pressed) {
                        GameHelper.DirectWrite(pixelAddress, selectedColor[0]);
                    } else if (e.RightButton == MouseButtonState.Pressed) {
                        GameHelper.DirectWrite(pixelAddress, selectedColor[1]);
                    }
                   
                }
            }
        }

        bool canvasSelectorPressed = false;

        private void CanvasSelector_MouseDown(object sender, MouseButtonEventArgs e) {
            canvasSelectorPressed = true;
            CanvasSelector_MouseMove(sender, e);
        }

        private void CanvasSelector_MouseMove(object sender, MouseEventArgs e) {
            if (canvasSelectorPressed) {
                int x = (int)Math.Floor(e.GetPosition(canvasBoard).X / 15);

                if (GameHelper.BoardAddress(playerID) > 0x08000000 && 0 <= x && x <= 9) {
                    if (x == 0) x = -1;
                    else if (x != 9) x--;

                    if (e.LeftButton == MouseButtonState.Pressed) {
                        selectedColor[0] = x;
                    }
                    if (e.RightButton == MouseButtonState.Pressed) {
                        selectedColor[1] = x;
                    }
                }
            }
        }

        private void CanvasSelector_MouseUp(object sender, MouseButtonEventArgs e) {
            if (e.LeftButton == MouseButtonState.Released && e.RightButton == MouseButtonState.Released)
                canvasSelectorPressed = false;
        }

        private void Window_Closed(object sender, EventArgs e) {
            Application.Current.Shutdown();
        }
    }
}

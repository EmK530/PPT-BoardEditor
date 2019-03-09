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
            Interval = TimeSpan.FromMilliseconds(1)
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

        private void Window_Closed(object sender, EventArgs e) {
            Application.Current.Shutdown();
        }
    }
}

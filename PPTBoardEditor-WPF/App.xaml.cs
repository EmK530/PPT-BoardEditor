using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PPTBoardEditor_WPF {
    public partial class App: Application {
        static PlayerWindow[] players = new PlayerWindow[2];

        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);

            for (int i = 0; i < 2; i++) {
                players[i] = new PlayerWindow(i);
                players[i].Show();
            }
        }
    }
}

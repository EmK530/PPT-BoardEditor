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
    public class Tetromino {
        private int _index;

        public int Index {
            get {
                return _index;
            }
        }

        public static Color BoardColor(int index) {
            
            switch (index) {
                case 0: return (Color)ColorConverter.ConvertFromString("#0F0");
                case 1: return (Color)ColorConverter.ConvertFromString("#F00");
                case 2: return (Color)ColorConverter.ConvertFromString("#00F");
                case 3: return (Color)ColorConverter.ConvertFromString("#F40");
                case 4: return (Color)ColorConverter.ConvertFromString("#40F");
                case 5: return (Color)ColorConverter.ConvertFromString("#FF0");
                case 6: return (Color)ColorConverter.ConvertFromString("#0FF");
                case 7:
                case 8: return Colors.Goldenrod;
                case 9: return (Color)ColorConverter.ConvertFromString("#FFF");
                case -2: return (Color)ColorConverter.ConvertFromString("#000");
                case -1: return Colors.Transparent;
            }

            return (Color)ColorConverter.ConvertFromString("#444");
        }

        public static Color Color(int index) {
            switch (index) {
                case 0:
                case 8: return (Color)ColorConverter.ConvertFromString("#0F0");
                case 1:
                case 9: return (Color)ColorConverter.ConvertFromString("#F00");
                case 2:
                case 10: return (Color)ColorConverter.ConvertFromString("#00F");
                case 3:
                case 11: return (Color)ColorConverter.ConvertFromString("#F40");
                case 4:
                case 12: return (Color)ColorConverter.ConvertFromString("#40F");
                case 5:
                case 13: return (Color)ColorConverter.ConvertFromString("#FF0");
                case 6:
                case 14: return (Color)ColorConverter.ConvertFromString("#0FF");
                case 7: return Colors.Goldenrod;
            }

            return Colors.Transparent;
        }

        public override string ToString() {
            switch (_index) {
                case 0: return "S";
                case 1: return "Z";
                case 2: return "J";
                case 3: return "L";
                case 4: return "T";
                case 5: return "O";
                case 6: return "I";
                case 7: return "M";
                case 8: return "S5";
                case 9: return "Z5";
                case 10: return "J5";
                case 11: return "L5";
                case 12: return "T5";
                case 13: return "O5";
                case 14: return "I5";
            }

            return ".";
        }

        public Tetromino(int index) {
            _index = index;
        }
    }
}

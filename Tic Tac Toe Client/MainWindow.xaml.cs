using System.Net.Sockets;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Windows.Threading;

namespace Tic_Tac_Toe_Client;

public partial class MainWindow : Window
{
    TcpClient? client = new TcpClient();

    IPAddress? ip = null;
    int port = 0;
    IPEndPoint? endPoint = null;
    public bool Turn { get; set; } = true;

    Dispatcher Dispatcher { get; set; }

    public MainWindow()
    {
        InitializeComponent();
        ip = IPAddress.Parse("127.0.0.1");
        port = 27001;
        endPoint = new IPEndPoint(ip, port);
        Dispatcher = Dispatcher.CurrentDispatcher;
        client.Connect(endPoint);
    }

    private void btn_Click(object sender, RoutedEventArgs e)
    {
        var btn = sender as Button;

        
        var text = "";
        var row = "";
        var column = "";
        if (btn == btn1 || btn == btn2 || btn == btn3)
        {
            row = "0";
        }
        else if (btn == btn4 || btn == btn5 || btn == btn6)
        {
            row = "1";
        }
        else if (btn == btn7 || btn == btn8 || btn == btn9)
        {
            row = "2";
        }

        if (btn == btn1 || btn == btn4 || btn == btn7)
        {
            column = "0";
        }
        else if (btn == btn2 || btn == btn5 || btn == btn8)
        {
            column = "1";
        }
        else if (btn == btn3 || btn == btn6 || btn == btn9)
        {
            column = "2";
        }
        var stream = client.GetStream();
        var bw = new BinaryWriter(stream);
        var br = new BinaryReader(stream);
        if (btn is not null)
        {
            try
            {
                if (Turn)
                {
                    if (client.Connected)
                    {

                        text = "X";
                        var str = text + " " + row + " " + column;
                        bw.Write(str);
                        if (br.ReadString() == "X" || br.ReadString() == "O")
                            btn.Content = br.ReadString();
                        else
                            MessageBox.Show($"Congrats {br.ReadString()} you are the winner");
                    }
                }
                else
                {
                    if (client.Connected)
                    {
                        var writer = Task.Run(() =>
                        {

                            text = "O";
                            bw.Write(text + " " + row + " " + column);

                        });


                        if (br.ReadString() == "X" || br.ReadString() == "O")
                            btn.Content = br.ReadString();
                        else
                            MessageBox.Show($"Congrats {br.ReadString()} you are the winner");



                    }
                }
                Turn = !Turn;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }
}
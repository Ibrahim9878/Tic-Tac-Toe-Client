using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

TcpListener listener = null;
BinaryReader br = null;
BinaryWriter bw = null;
List<TcpClient> clients = [];

var ip = IPAddress.Parse("127.0.0.1");
var port = 27001;
var endPoint = new IPEndPoint(ip, port);
listener = new TcpListener(endPoint);
listener.Start(2);
Console.WriteLine($"Listening on {listener.LocalEndpoint}");


string[][] TicTacToe = [];

while (true)
{
    var client = listener.AcceptTcpClient();
    clients.Add(client);
    Console.WriteLine($"{client.Client.RemoteEndPoint} connected...");

    var reader = Task.Run(() =>
    {
        foreach (var client in clients)
        {
            Task.Run(() =>
            {
                while (true)
                {
                    var stream = client.GetStream();
                    br = new BinaryReader(stream);
                    bw = new BinaryWriter(stream);
                    try
                    {
                        var msg = br.ReadString();
                        Console.WriteLine($"Client {client.Client.RemoteEndPoint}: {msg[0]}");
                        Console.WriteLine($"Client {client.Client.RemoteEndPoint}: {msg[1]}");
                        Console.WriteLine($"Client {client.Client.RemoteEndPoint}: {msg[2]}");
                        var row = int.Parse(msg[1].ToString());
                        var column = int.Parse(msg[2].ToString());
                        TicTacToe[row][column] = msg[0].ToString();
                        
                        var winner = CheckWinner(bw);
                        if (winner != string.Empty)
                        {
                            bw.Write(winner);
                        }
                        bw.Write(msg[0]);              
                    }
                    catch (Exception)
                    {
                        clients.Remove(client);
                    }
                }
            }).Wait(50);
        }
    });
}
string CheckWinner(BinaryWriter bw)
{
    if (TicTacToe[0][0] == TicTacToe[0][1] && TicTacToe[0][0] == TicTacToe[0][2])
    {
        return TicTacToe[0][0] ;
    }
    else if (TicTacToe[0][0] == TicTacToe[1][0] && TicTacToe[0][0] == TicTacToe[2][0])
    {
        return (TicTacToe[0][0]);
    }
    else if (TicTacToe[0][0] == TicTacToe[1][1] && TicTacToe[0][0] == TicTacToe[2][2])
    {
        return (TicTacToe[0][0]);
    }
    else if (TicTacToe[1][0] == TicTacToe[1][1] && TicTacToe[1][0] == TicTacToe[1][2])
    {
        return (TicTacToe[1][0]);
    }
    else if (TicTacToe[2][0] == TicTacToe[2][1] && TicTacToe[2][0] == TicTacToe[2][2])
    {
        return (TicTacToe[1][0]);
    }
    else if (TicTacToe[0][1] == TicTacToe[1][1] && TicTacToe[0][1] == TicTacToe[2][1])
    {
        return (TicTacToe[0][1]);
    }
    else if (TicTacToe[0][2] == TicTacToe[1][2] && TicTacToe[0][2] == TicTacToe[2][2])
    {
        return (TicTacToe[0][2]);
    }
    else if (TicTacToe[0][2] == TicTacToe[1][1] && TicTacToe[0][2] == TicTacToe[2][1])
    {
        return (TicTacToe[0][2]);
    }
    return string.Empty;
}
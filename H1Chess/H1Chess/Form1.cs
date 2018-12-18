using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace H1Chess
{
    public partial class Form1 : Form
    {
        ChessClient chessClient;
        TcpListener chessServer;
        Thread child;
        bool isRunning = true;

        public Form1()
        {
            InitializeComponent();
            child = new Thread(new ThreadStart(ChessBoardRenderer));
            chessClient = new ChessClient();
        }

        private void ChessBoardRenderer()
        {
            Bitmap canvas = new Bitmap(64 * 8, 64 * 8);
            Graphics graphics = Graphics.FromImage(canvas);
            ChessBoard board;

            SolidBrush blackBrush = new SolidBrush(Color.Black);
            SolidBrush whiteBrush = new SolidBrush(Color.White);

            while (isRunning)
            {
                board = chessClient.GetBoard();

                for (int y = 0; y < 8; y++)
                {
                    for (int x = 0; x < 8; x++)
                    {
                        if ((x + y) % 2 == 0)
                            graphics.FillRectangle(blackBrush, x * 64, y * 64, 64, 64);
                        else
                            graphics.FillRectangle(whiteBrush, x * 64, y * 64, 64, 64);
                    }
                }

                if (!chessClient.Connected)
                    connectionStatusLabel.Text = "Not connected";
                else
                {
                    connectionStatusLabel.Text = chessClient.Client.RemoteEndPoint.ToString();

                    for (int y = 0; y < 8; y++)
                    {
                        for (int x = 0; x < 8; x++)
                        {
                            Piece p = board.GetPieceAt(x, y);
                            if (p == null)
                                continue;

                            graphics.DrawImage(p.GetImage(), x * 64, y * 64, 64, 64);
                        }
                    }
                    chessImageHolder.Image = canvas;
                }
                
                Thread.Sleep(250);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            child.Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            isRunning = false;
        }
    }
}

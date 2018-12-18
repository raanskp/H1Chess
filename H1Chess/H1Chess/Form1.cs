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
        ChessServer chessServer;
        Thread child;
        bool isRunning = true;

        int mouseClickX = -1;
        int mouseClickY = -1;

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
                    board = chessClient.GetBoard();
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

                    graphics.FillRectangle(new SolidBrush(Color.FromArgb(128, 128, 128, 128)), mouseClickX*64, mouseClickY*64, 64, 64);
                    graphics.DrawRectangle(new Pen(Color.LightGreen, 3), mouseClickX*64, mouseClickY*64, 64, 64);
                }

                chessImageHolder.Image = canvas;

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

        private void chessImageHolder_MouseClick(object sender, MouseEventArgs e)
        {
            int newMouseX = e.X / 64;
            int newMouseY = e.Y / 64;

            if (newMouseX == mouseClickX && newMouseY == mouseClickY)
            {
                mouseClickX = -1;
                mouseClickY = -1;
            }
            else
            {
                mouseClickX = newMouseX;
                mouseClickY = newMouseY;
            }

            
        }

        private void startServerMenu_ClickAsync(object sender, EventArgs e)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 65535);
            chessServer = new ChessServer(endPoint);
            chessServer.Start();
            chessServer.Run();

            stopServerMenu.Visible = true;
            startServerMenu.Visible = false;
            localhostConnectMenu.Visible = true;
        }

        private void stopServerMenu_Click(object sender, EventArgs e)
        {
            chessServer.Stop();
            chessServer = null;

            stopServerMenu.Visible = false;
            startServerMenu.Visible = true;
            localhostConnectMenu.Visible = false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isRunning = false;
            chessServer?.Stop();
            if (chessClient.Connected)
                chessClient.Close();

            Close();
        }

        private void enterIPToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void localhostConnectMenu_Click(object sender, EventArgs e)
        {
            chessClient.Connect(IPAddress.Loopback, 65535);
        }
    }
}

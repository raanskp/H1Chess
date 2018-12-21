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
using System.Windows;
using System.Windows.Forms;

namespace H1Chess
{
    public partial class Form1 : Form
    {
        ChessClient chessClient;
        ChessServer chessServer;
        Thread child;
        bool isRunning = true;

        // Last point when a user clicked a square on the board
        int mouseClickX = -1;
        int mouseClickY = -1;

        public Form1()
        {
            InitializeComponent();
            child = new Thread(new ThreadStart(ChessBoardRenderer));
            chessClient = new ChessClient();
        }

        /// <summary>
        /// Helper function to render the Chessboard as an image. Eventually it would be neat to only render it when needed
        /// aka if it needs a redraw or whenever a piece was moved.
        /// </summary>
        private void ChessBoardRenderer()
        {
            Bitmap canvas = new Bitmap(64 * 8, 64 * 8);
            Graphics graphics = Graphics.FromImage(canvas);

            SolidBrush blackBrush = new SolidBrush(Color.Black);
            SolidBrush whiteBrush = new SolidBrush(Color.White);
            SolidBrush selectedBrush = new SolidBrush(Color.FromArgb(128, 128, 128, 128));
            Pen selectedPen = new Pen(Color.LightGreen, 3);

            while (isRunning)
            {
                // Paint all the squares. This could eventually be merged with the piece renderer below, but for current
                // development and debugging it is easier to just keep them seperate.
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
                    ChessBoard board = chessClient.GetBoard();
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

                    // Draw the selection if there is one (else it is just rendered outside of the board, which we cannot see)
                    graphics.FillRectangle(selectedBrush, mouseClickX*64, mouseClickY*64, 64, 64);
                    graphics.DrawRectangle(selectedPen, mouseClickX*64, mouseClickY*64, 64, 64);
                }

                chessImageHolder.Image = canvas;

                Thread.Sleep(250);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Start the renderer thread
            child.Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            isRunning = false;
        }

        /// <summary>
        /// Click event function. Handles all the things to do with selecting pieces.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chessImageHolder_MouseClick(object sender, MouseEventArgs e)
        {
            int newMouseX = e.X / 64;
            int newMouseY = e.Y / 64;

            // If we click on a piece already selected, we unselect it by setting it out of bounds
            if (newMouseX == mouseClickX && newMouseY == mouseClickY)
            {
                mouseClickX = -1;
                mouseClickY = -1;
            }
            else
            {
                // If we click on a new position and the old one wasn't unselected, then we need to move a piece
                if (mouseClickX != -1 && mouseClickY != -1)
                {
                    chessClient.MovePiece(new Vector(mouseClickX, mouseClickY), new Vector(newMouseX, newMouseY));

                    // Unselect after moving
                    mouseClickX = -1;
                    mouseClickY = -1;
                }
                else
                {
                    // If nothing was selected before, we make a selection
                    mouseClickX = newMouseX;
                    mouseClickY = newMouseY;
                }
            }    
        }

        /// <summary>
        /// Event handler for whenever the user requests starting a new server.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Event handler for whenever the user requests ending the current server.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stopServerMenu_Click(object sender, EventArgs e)
        {
            chessServer.Stop();
            chessServer = null;

            stopServerMenu.Visible = false;
            startServerMenu.Visible = true;
            localhostConnectMenu.Visible = false;
        }

        /// <summary>
        /// Event handler for the user wanting to exit the program through the menus.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isRunning = false;
            chessServer?.Stop();
            if (chessClient.Connected)
                chessClient.Close();

            Close();
        }

        /// <summary>
        /// Event handler for when the user wants to enter an IP to connect to.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void enterIPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 GetIPForm = new Form2();
            GetIPForm.ShowDialog();

            try
            {
                chessClient.Connect(IPAddress.Parse(GetIPForm.Result), 65535);
                chessClient.RunAsync();
            }
            catch (Exception error)
            {
                MessageBox.Show(this, "An error occured while trying to connect to the server.", "Error", MessageBoxButtons.OK);
            }
        }

        /// <summary>
        /// Event handler for when the user wants to connect to the local server.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void localhostConnectMenu_Click(object sender, EventArgs e)
        {
            try
            {
                chessClient.Connect(IPAddress.Loopback, 65535);
                chessClient.RunAsync();
            } catch (Exception error)
            {
                MessageBox.Show(this, "An error occured while trying to connect to the server.", "Error", MessageBoxButtons.OK);
            }
            
        }
    }
}

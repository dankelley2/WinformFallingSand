using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace Sand
{
    public partial class SandboxForm : Form
    {

        private Game game;
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private readonly FastLoop _fastLoop;
        private long _msFrameTime;
        private long _msLastFrame;
        private long _msPerDrawCycle;
        private long _msThisFrame;
        private long _frameTime;

        public SandboxForm()
        {
            InitializeComponent();
            _fastLoop = new FastLoop(GameLoop);
            _stopwatch.Start();
        }

        private void GameLoop(double elapsedTime)
        {

            if (_stopwatch.ElapsedMilliseconds - _frameTime > 1000 / 60)
            {
                RunEngine(elapsedTime);
                Render();
            }
        }

        private void Render()
        {
            _frameTime = _stopwatch.ElapsedMilliseconds;
            game.Render();
            _msPerDrawCycle = _stopwatch.ElapsedMilliseconds - _frameTime;
            _msLastFrame = _msThisFrame;
            _msThisFrame = _stopwatch.ElapsedMilliseconds;
            _msFrameTime = _msThisFrame - _msLastFrame;
            lblMsPerFrame.Text = $"ms Per Frame: {_msPerDrawCycle}";
            lblParticleCount.Text = $"activeZones: {game.ActiveZones}";
        }

        private void RunEngine(double elapsedTime)
        {
            game.RunGame();
            //_physicsSystem.Tick(elapsedTime);
        }

        private void SandboxForm_Load(object sender, EventArgs e)
        {
            ImgSandbox.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            game = new Game(ref ImgSandbox, 256);

        }

        private void ImgSandbox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && ImgSandbox.Focused)
            {
                game.ImgSandbox_LMB(sender, e);
            }
        }

        private void ImgSandbox_MouseClick(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Left && ImgSandbox.Focused)
            //{
            //    game.ImgSandbox_LMB(sender, e);
            //}
        }

        private void ImgSandbox_MouseEnter(object sender, EventArgs e)
        {
            ImgSandbox.Focus();
        }

        private void ImgSandbox_MouseLeave(object sender, EventArgs e)
        {
            this.Focus();
        }

        private void ImgSandbox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && ImgSandbox.Focused)
            {
                game.cycleMaterials(sender, e);
            }
        }
    }
}

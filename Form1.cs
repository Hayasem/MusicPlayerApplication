using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicPlayerApplication
{
    public partial class Form1 : Form
    {

        private List<string> musicFiles;
        private string currentSong;
        private bool isPaused;
        private bool isChangingPosition;
        public Form1()
        {


            InitializeComponent();
            musicFiles = new List<string>();
            isPaused = false;
            isChangingPosition = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "MP3 Files | *.mp3";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in openFileDialog.FileNames)
                {
                    musicFiles.Add(file);
                    listBox1.Items.Add(Path.GetFileName(file));
                }
                if (musicFiles.Count > 0)
                {
                    btnStart.Enabled = true;
                }
            }

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex >= 0)
            {
                if (isPaused)
                {
                    MusicPlayer.Ctlcontrols.play();
                    isPaused = false;
                }
                else
                {
                    currentSong = musicFiles[listBox1.SelectedIndex];
                    MusicPlayer.URL = currentSong;
                    MusicPlayer.Ctlcontrols.play();
                }

                TimerPlayback.Enabled = true;

            }

        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            MusicPlayer.Ctlcontrols.stop();
            isPaused = false;
            TimerPlayback.Enabled = false;

        }

        private void btnPause_Click(object sender, EventArgs e)
        {

            if (isPaused == false)
            {
                MusicPlayer.Ctlcontrols.pause();
                isPaused = true;
            }
            else
            {
                MusicPlayer.Ctlcontrols.play();
                isPaused = false;
            }

        }

        private void TimerPlayback_Tick(object sender, EventArgs e)
        {
            if (!isChangingPosition)
            {
                txtCounter.Text = "Length: " + FormatTime(MusicPlayer.Ctlcontrols.currentPosition) + " / " + FormatTime(MusicPlayer.currentMedia.duration);
            }

        }
        private string FormatTime(double seconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);
            return time.ToString(@"mm\:ss");
        }


        //Prosseguir para a póxima música no momento em que ela termina:
        private void MusicPlayer_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (e.newState == 8)
            {
                int nextIndex = listBox1.SelectedIndex += 1;
                if(nextIndex < musicFiles.Count)
                {
                    listBox1.SelectedIndex = nextIndex;
                    currentSong = musicFiles[nextIndex];
                    MusicPlayer.URL = currentSong;
                    MusicPlayer.Ctlcontrols.play();
                    isPaused = true;
                }
                else
                {
                    MusicPlayer.Ctlcontrols.stop();
                    isPaused=false;
                }
            }
        }

        private void VolumeBar_Scroll(object sender, ScrollEventArgs e)
        {
            MusicPlayer.settings.volume = VolumeBar.Value;
        }
    }
}

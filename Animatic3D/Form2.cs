using AxWMPLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Animatic3D
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

            try
            { 
                string mp4Pth = Environment.CurrentDirectory + "/sonic-lowpoly.mp4";
                video.URL = mp4Pth;
                video.settings.setMode("loop", true);
                video.settings.rate = 2;
                video.Ctlcontrols.play();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Se genero un error al cargar e intentar reproducir el video: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            video.Ctlcontrols.stop();
        }
    }
}

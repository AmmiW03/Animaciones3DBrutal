using NAudio.Wave;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Animatic3D
{
    public partial class Form1 : Form
    {
        private GLControl glcontrol = new GLControl();
        public Form1()
        {
            InitializeComponent();
            Controls.Add(glcontrol);

            glcontrol.Load += Form1_Load;
            glcontrol.Paint += glcontrol_paint;
            glcontrol.Height = 500;
            glcontrol.Width = 500;
        }

        private void playwav()
        {
            string wavPth = Environment.CurrentDirectory + "/PalmtreePanic.wav";

            // Lector de audio usando NAudio
            WaveFileReader reader = new WaveFileReader(wavPth);

            // Se crea el reproductor para el audio
            WaveOutEvent waveOut = new WaveOutEvent();

            waveOut.Init(reader);


            // Reproducir el audio
            waveOut.Play();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Se establece el contexto
            glcontrol.MakeCurrent();

            // Config de OpenGL
            GL.ClearColor(System.Drawing.Color.Black);
            GL.Enable(EnableCap.DepthTest);

            playwav();
        }

        // Variable para el ángulo de rotación
        private float angle = 0.0f;
        // Velocidad de la rotación
        private float rotationSpeed = 0.5f;

        private void glcontrol_paint(object sender, PaintEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Dibujar partículas blancas al azar
            GL.PointSize(2.0f); // Ajusta el tamaño de los puntos
            GL.Begin(PrimitiveType.Points);
            GL.Color3(Color.White); // Color blanco para las partículas

            Random random = new Random();

            for (int i = 0; i < 200; i++) // Dibuja 200 partículas
            {
                double x = (random.NextDouble() - 0.5) * 10.0; // Ajusta los límites según tu necesidad
                double y = (random.NextDouble() - 0.5) * 10.0; // Ajusta los límites según tu necesidad
                double z = (random.NextDouble() - 0.5) * 10.0; // Ajusta los límites según tu necesidad

                GL.Vertex3(x, y, z);
            }

            GL.End();
            GL.PointSize(1.0f); ; // Restablece el tamaño de los puntos a su valor predeterminado

            // Config de la matriz de proyeccion
            Matrix4 proyect = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), (float)glcontrol.Width / glcontrol.Height, 0.1f, 100.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref proyect);

            // Configuracion de la camara
            Matrix4 modelview = Matrix4.LookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.UnitY);
            GL.MatrixMode(MatrixMode.Modelview);

            // Aplicacion de la rotación a la camara
            GL.LoadMatrix(ref modelview);
            // Rotacion sobre el eje Y
            GL.Rotate(angle, 1.0f, 0.0f, 2.0f);

            // Renderizar el prisma en una posición
            GL.PushMatrix();
            GL.Translate(0.0f, -1.0f, 0.5f);
            RenderizarPrisma();
            GL.PopMatrix();

            // Renderizar la esfera en otra posición
            GL.PushMatrix();
            GL.Translate(0.0f, 1.0f, -0.5f);
            RenderizarEsfera();
            GL.PopMatrix();

            glcontrol.SwapBuffers();

            // Incrementar el ángulo para el siguiente frame con velocidad de rotación un poco más lenta
            angle += rotationSpeed;
            if (angle >= 360.0f)
            {
                angle -= 360.0f;
            }

            // Solicitar un nuevo renderizado
            glcontrol.Invalidate();
        }
        private void RenderizarPrisma()
        {
            // Tamaño del prisma
            float size = 0.5f;

            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

            GL.Begin(PrimitiveType.Quads);

            // Cara frontal
            GL.Color3(AplicarColorPsicodelico(true));
            GL.Vertex3(-size, -size, size);
            GL.Color3(AplicarColorPsicodelico());
            GL.Vertex3(size, -size, size);
            GL.Color3(AplicarColorPsicodelico(true));
            GL.Vertex3(size, size, size);
            GL.Color3(AplicarColorPsicodelico());
            GL.Vertex3(-size, size, size);

            // Cara posterior
            GL.Color3(AplicarColorPsicodelico(true));
            GL.Vertex3(-size, -size, -size);
            GL.Color3(AplicarColorPsicodelico());
            GL.Vertex3(size, -size, -size);
            GL.Color3(AplicarColorPsicodelico());
            GL.Vertex3(size, size, -size);
            GL.Color3(AplicarColorPsicodelico(true));
            GL.Vertex3(-size, size, -size);

            // Cara lateral izquierda
            GL.Color3(AplicarColorPsicodelico());
            GL.Vertex3(-size, -size, size);
            GL.Color3(AplicarColorPsicodelico(true));
            GL.Vertex3(-size, -size, -size);
            GL.Color3(AplicarColorPsicodelico(true));
            GL.Vertex3(-size, size, -size);
            GL.Color3(AplicarColorPsicodelico());
            GL.Vertex3(-size, size, size);

            // Cara lateral derecha
            GL.Color3(AplicarColorPsicodelico(true));
            GL.Vertex3(size, -size, size);
            GL.Color3(AplicarColorPsicodelico());
            GL.Vertex3(size, -size, -size);
            GL.Color3(AplicarColorPsicodelico(true));
            GL.Vertex3(size, size, -size);
            GL.Color3(AplicarColorPsicodelico(true));
            GL.Vertex3(size, size, size);

            // Cara superior
            GL.Color3(AplicarColorPsicodelico());
            GL.Vertex3(-size, size, size);
            GL.Color3(AplicarColorPsicodelico());
            GL.Vertex3(size, size, size);
            GL.Color3(AplicarColorPsicodelico());
            GL.Vertex3(size, size, -size);
            GL.Color3(AplicarColorPsicodelico());
            GL.Vertex3(-size, size, -size);

            // Cara inferior
            GL.Color3(AplicarColorPsicodelico(true));
            GL.Vertex3(-size, -size, -size);
            GL.Color3(AplicarColorPsicodelico(true));
            GL.Vertex3(size, -size, -size);
            GL.Color3(AplicarColorPsicodelico(true));
            GL.Vertex3(size, -size, size);
            GL.Color3(AplicarColorPsicodelico(true));
            GL.Vertex3(-size, -size, size);

            GL.End();

            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
        }

        private Color AplicarColorPsicodelico()
        {
            float t = (float)(Math.Sin(angle) * 0.5 + 0.5); // Utiliza el ángulo para crear un patrón
            return HSVToRGB(t * 360, 1.0f, 1.0f); // Convierte el valor a grados para HSV
        }
        private Color AplicarColorPsicodelico(bool flag)
        {
            float t = (float)(Math.Sin(angle) * 1 + 1); // Utiliza el ángulo para crear un patrón
            return HSVToRGB(t * 360, 1.0f, 1.0f); // Convierte el valor a grados para HSV
        }

        private Color HSVToRGB(float hue, float saturation, float value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            float f = hue / 60 - (float)Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            switch (hi)
            {
                case 0:
                    return Color.FromArgb(255, v, t, p);
                case 1:
                    return Color.FromArgb(255, q, v, p);
                case 2:
                    return Color.FromArgb(255, p, v, t);
                case 3:
                    return Color.FromArgb(255, p, q, v);
                case 4:
                    return Color.FromArgb(255, t, p, v);
                default:
                    return Color.FromArgb(255, v, p, q);
            }
        }

        private void RenderizarEsfera()
        {
            // Radio de la esfera
            float radius = 1.0f;
            int slices = 30;
            int stacks = 30;


            for (int i = 0; i < stacks; i++)
            {
                double lat1 = Math.PI * (-0.5 + (double)(i) / stacks);
                double z0 = Math.Sin(lat1);
                double zr0 = Math.Cos(lat1);

                double lat2 = Math.PI * (-0.5 + (double)(i + 1) / stacks);
                double z1 = Math.Sin(lat2);
                double zr1 = Math.Cos(lat2);
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

                GL.Begin(PrimitiveType.QuadStrip);
                for (int j = 0; j <= slices; j++)
                {
                    double lon = 2 * Math.PI * (double)(j - 1) / slices;
                    double x = Math.Cos(lon);
                    double y = Math.Sin(lon);

                    double colorIntensity = Math.Abs(Math.Cos(lat1)); // Controla la división de colores

                    GL.Color3(AplicarColorPsicodelico()); // Asigna colores basados en la intensidad

                    GL.Normal3(x * zr0, y * zr0, z0);
                    GL.Vertex3(x * zr0 * radius, y * zr0 * radius, z0 * radius);

                    colorIntensity = Math.Abs(Math.Cos(lat2));

                    GL.Color3(AplicarColorPsicodelico(true)); ;

                    GL.Normal3(x * zr1, y * zr1, z1);
                    GL.Vertex3(x * zr1 * radius, y * zr1 * radius, z1 * radius);
                }
                GL.End();
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            }
        }



        private void btnNewForm_Click(object sender, EventArgs e)
        {
            Form2 reproducir = new Form2();
            reproducir.ShowDialog();
        }
    }
}

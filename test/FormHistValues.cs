using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test
{
    public partial class FormHistValues : Form
    {
        public Mat imagen_a_mostrar;
        private VectorOfMat g;

        public FormHistValues()
        {
            InitializeComponent();
        }

        public FormHistValues(Mat img)
        {
            InitializeComponent();
            imagen_a_mostrar = img;
            ShowValores();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void ShowValores() {
            listView1.Clear();
            Mat hist = new Mat(imagen_a_mostrar.Cols, imagen_a_mostrar.Rows, DepthType.Cv8U, 3);
            g = new VectorOfMat();
            g.Push(imagen_a_mostrar);

            CvInvoke.CalcHist(g, new int[] { 0 }, new Mat(), hist, new int[] { 256 }, new float[] { 0, 255 }, true);

            int hist_w = 512; int hist_h = 400;
            int bin_w = (int)Math.Round((double)hist_w / 256);//
            Mat im2 = new Mat(hist_h, hist_w, DepthType.Cv8U, 3);
            listView1.Clear();
            int i = 0;
            for (i = 1; i < 255; i++)
            {
                int t = (hist.GetData(i)[1]);
                listView1.Items.Add(i.ToString() + " -> " + t.ToString());
                CvInvoke.Line(im2, new Point(bin_w * i, hist_w),
                                   new Point(bin_w * i, hist_h - t),
                                   new MCvScalar(255, 0, 0), bin_w, LineType.EightConnected, 0);//;// hist_h - (int)Math.Round((decimal) rg.GetData(i)[0])),
                                                                                                //istView1.Items.Add(t.ToString());
                
            }
            CvInvoke.Imshow("histograma 1 ", im2);


        }



    }
}

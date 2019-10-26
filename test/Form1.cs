using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using System.Collections;

namespace test
{
    public partial class Form1 : Form
    {
        private const byte V = 4;
        private Mat imagen_dest;
        private Mat[] hsv_planos = new Mat[3];
        private Mat[] bgr_planos = new Mat[4];
        private Mat imagen_cargada;
        List<Image<Gray, byte>> bgr_plans = new List<Image<Gray, byte>>();
        Image<Bgr, float> template1;
        Image<Bgr, byte> source;
        Image<Bgr, float> source1;
        //Image<Bgr, byte> imageToShow;
        Image<Bgr, float> vist;
        Mat img_matriz_rotacion;
        private double set = 0.5;

        public Form1()
        {
            InitializeComponent();
            label3.Text = set.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // CONVERTIR ESPACIO COLOR
        private void button4_Click(object sender, EventArgs e)
        {
            if (!(imagen_cargada == null))
            {
                imagen_dest = imagen_cargada;
                CvInvoke.CvtColor(imagen_cargada, imagen_dest, Emgu.CV.CvEnum.ColorConversion.Rgb2Hsv);
            }
            imageBox1.Image = imagen_dest;
            CvInvoke.Imshow("imagen convertida", imagen_dest);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ajusto_tracks();
        }

        /////   IMAGEN_CARGADA LOAD FROM FILE   ///////
        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            openFileDialog1.Title = "Abrir Fichero";
            openFileDialog1.InitialDirectory = "d:\\";
            openFileDialog1.CheckFileExists = false;
            openFileDialog1.CheckPathExists = false;
            openFileDialog1.ReadOnlyChecked = true;
            openFileDialog1.ShowHelp = true;
            openFileDialog1.Multiselect = false;
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try {
                    imagen_cargada = new Mat();
                    imagen_cargada = CvInvoke.Imread(openFileDialog1.FileName);
                    imageBox1.Image = imagen_cargada;
                    detalles(imagen_cargada, listBox1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        //////   CONVOLUCION    /////////
        private void button2_Click(object sender, EventArgs e)
        {
            if (imagen_cargada == null) return;
            Mat Kernel;
            // double[] r = {4};
            // Mat y = new Mat();
            // y = { 4};
            Kernel = Mat.Ones(2, 2, DepthType.Cv32F, 1);
            /*
            for (int i = 0; i < Kernel.Rows; i++)
                {
                    for (int j = 0; j < Kernel.Cols; j++)
                    {
                    byte[] m = Kernel.GetData(i, j);
                     //   Kernel.Data.GetValue(i, j);

                    for (int k = 1; k < m.Length; k++)
                        {
                        m[k] = m[k]/V;
                        }
                        Kernel.put(i, j, m);
                    }
                }*/
            Mat imagen_dests = new Mat();
            CvInvoke.Filter2D(imagen_cargada, imagen_dests, Kernel, new Point(-1, -1), 1, BorderType.Default);
            // imageBox1.Image = imagen_dest;
        }

        //////  DETALLES   DE LA IMAGEN    ///////
        private void button5_Click(object sender, EventArgs e)
        {
            detalles(imagen_cargada, listBox1);
        }

        /////  LIMPIA LISTBOX ///////
        private void button6_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        /////   GIRA IMAGEN    //////
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (imagen_cargada == null) {
                trackBar1.Value = 0;
                return;
            }
            set = trackBar1.Value / 10;
            label3.Text = trackBar1.Value.ToString();
            label4.Text = set.ToString();
            int len;
            if (source.Cols > source.Rows)
            {
                len = source.Cols;
            }
            else
            {
                len = source.Rows;
            }

            double angulo = trackBar1.Value;
            PointF p = new PointF(len / 2, len / 2);
            img_matriz_rotacion = new Mat();
            CvInvoke.GetRotationMatrix2D(p, angulo, .3, img_matriz_rotacion);
            CvInvoke.WarpAffine(source, imagen_dest, img_matriz_rotacion, new Size(len, len));
            CvInvoke.Imshow("rotada", imagen_dest);
        }

        /////  FLIP IMAGEN   ///////
        private void btnRotacion_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    source = new Image<Bgr, byte>(openFileDialog1.FileName);
                    imagen_dest = new Mat();
                    CvInvoke.Resize(source, imagen_dest, new Size(1 / 2, 1 / 2), .3, .3, Emgu.CV.CvEnum.Inter.Linear);
                    CvInvoke.Imshow("escala", imagen_dest);
                    // traslacion
                    // source()
                    //por aqui:;
                    /*     Rectangle R = new Rectangle(100,100, source.Cols-100, source.Rows-100);
                         Rectangle R1 = new Rectangle(source, R, CvScalar Color.Beige, 1);
                         source.ROI = R;// rectangulo ROI 

                         Mat dest = new Mat(100, 100, DepthType.Cv8U, 3);
                         source(R)
                         source.CopyTo(LineSegment2DF,sd);*/

                    //Mat copiado = new Mat(source, R);
                    //source(new Rectangle(new Point(100, 100), source.Rows - 100, source.Cols - 100));
                    //source(Rectangle(100, 100, source.Cols - 100, source.Rows - 100)).copyTo(imagen_dest(Rectangle(0, 0, source.Cols - 100, source.Rows - 100)));
                    Image<Bgr, Byte> img = new Image<Bgr, Byte>(source.Rows, source.Cols);

                    img = imagen_dest.ToImage<Bgr, Byte>();
                    imagen_dest = Mat.Zeros(source.Rows, source.Cols, DepthType.Cv8U, 3);
                    Rectangle rect = new Rectangle(100, 100, img.Cols - 100, img.Rows - 100);
                    source.Draw(rect, new Bgr(Color.Green), 5);

                    Rectangle rect2 = new Rectangle(0, 0, img.Cols - 100, img.Rows - 100);
                    img.Draw(rect2, new Bgr(Color.Blue), 5);

                    CvInvoke.Imshow("Translate", img);
                    //rotate
                    int len;
                    if (source.Cols > source.Rows)
                    {
                        len = source.Cols;
                    }
                    else {
                        len = source.Rows;
                    }
                    double angulo = 90;
                    PointF p = new PointF(len / 2, len / 2);
                    img_matriz_rotacion = new Mat();
                    CvInvoke.GetRotationMatrix2D(p, angulo, .3, img_matriz_rotacion);
                    CvInvoke.WarpAffine(source, imagen_dest, img_matriz_rotacion, new Size(len, len));
                    CvInvoke.Imshow("rotada", imagen_dest);

                    //reflection
                    CvInvoke.Flip(source, imagen_dest, FlipType.Horizontal);
                    CvInvoke.Imshow("flip", imagen_dest);
                    trackBar1.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            else this.Close();

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        //////       CORRELACION     /////////
        private void btnCorrelacion_Click(object sender, EventArgs e)
        {
            //CARGAMOS IMAGEN Y PATRON
            openFileDialog1.Title = "Abrir Fichero";
            openFileDialog1.InitialDirectory = "d:\\";
            openFileDialog1.CheckFileExists = false;
            openFileDialog1.CheckPathExists = false;
            openFileDialog1.ReadOnlyChecked = true;
            openFileDialog1.ShowHelp = true;
            openFileDialog1.Multiselect = false;

            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    source1 = new Image<Bgr, float>(openFileDialog1.FileName);
                    source = new Image<Bgr, byte>(openFileDialog1.FileName);
                    imageBox1.Image = source1;
                    vist = source1.Copy();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            else this.Close();

            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    template1 = new Image<Bgr, float>(openFileDialog1.FileName);
                    imageBox2.Image = template1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            else this.Close();
            int i = 0;
            int iwidth = source1.Cols - template1.Cols + 1;
            int iheight = source1.Rows - template1.Rows + 1;
            double[] minVal, maxVal; Point[] minLoc, maxLoc, matchLoc;
            Image<Gray, float> fuente1 = new Image<Gray, float>(iheight, iwidth);

            TemplateMatchingType[] match_method = { TemplateMatchingType.CcoeffNormed, TemplateMatchingType.Ccoeff, TemplateMatchingType.Ccorr,
                TemplateMatchingType.CcorrNormed, TemplateMatchingType.Sqdiff, TemplateMatchingType.SqdiffNormed};
            TemplateMatchingType tipo = match_method[0];

            double rango = 10;
            while (rango > 0.92)
            {
                using (fuente1 = source1.MatchTemplate(template1, tipo))
                {
                    fuente1.MinMax(out minVal, out maxVal, out minLoc, out maxLoc);
                    if (tipo == TemplateMatchingType.Sqdiff || tipo == TemplateMatchingType.SqdiffNormed)
                    { matchLoc = minLoc; } else { matchLoc = maxLoc; }
                    rango = maxVal[0];
                    Rectangle match = new Rectangle(maxLoc[0], template1.Size);

                    source1.Draw(match, new Bgr(Color.Black), -1);
                    fuente1.Draw(match, new Gray(), -1);
                    vist.Draw(match, new Bgr(Color.Red), 3);
                    source.Draw(match, new Bgr(Color.Red), 3);
                    listBox1.Items.Add(matchLoc[0]);
                }
                listBox1.Items.Add("veces  " + i);
                CvInvoke.Imshow("E1", source1);
                CvInvoke.Imshow("Ocurrencias", vist);
                CvInvoke.Imshow("Ocurrencias  22", source);
            }
        }

        //////      BLUR        /////////
        ListViewItem lvi = new ListViewItem();

        private void button7_Click(object sender, EventArgs e)
        {
            if (imagen_cargada == null) this.Close();
            Mat imagen_dest = new Mat();
            try
            {
                CvInvoke.Blur(imagen_cargada, imagen_dest, new Size(3, 3), new Point(-1, -1), BorderType.Isolated);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

            CvInvoke.Imshow("Blur", imagen_dest);
            // lv2.Columns.Add("12", "w");
            lvi.SubItems.Add("dsfgsd");
            lvi.SubItems.Add("dsfsadfsd");
            lvi.SubItems.Add("sdfsdfasdfas");
            lv2.Items.Add(lvi);

            ListViewItem lvi2 = new ListViewItem();
            lvi2.SubItems.Add("dsfgsd");
            lvi2.SubItems.Add("dsfsadfsd");
            lvi2.SubItems.Add("sdfsdfasdfas");
            listView1.Items.Add(lvi2);

            // Set the view to show details.
            listView1.View = View.Details;
            // Allow the user to edit item text.
            listView1.LabelEdit = true;
            // Allow the user to rearrange columns.
            listView1.AllowColumnReorder = true;
            // Display check boxes.
            listView1.CheckBoxes = true;
            // Select the item and subitems when selection is made.
            listView1.FullRowSelect = true;
            // Display grid lines.
            listView1.GridLines = true;
            // Sort the items in the list in ascending order.
            listView1.Sorting = SortOrder.Ascending;

            // Create three items and three sets of subitems for each item.
            ListViewItem item1 = new ListViewItem("item1", 0);
            item1.Checked = true;
            item1.SubItems.Add("1");
            item1.SubItems.Add("2");
            item1.SubItems.Add("3");
            ListViewItem item2 = new ListViewItem("item2", 1);
            item2.SubItems.Add("4");
            item2.SubItems.Add("5");
            item2.SubItems.Add("6");
            ListViewItem item3 = new ListViewItem("item3", 0);
            // Place a check mark next to the item.
            //  item3.Checked = true;
            item3.SubItems.Add("7");
            item3.SubItems.Add("8");
            item3.SubItems.Add("9");

        }

        /////   GAUSIAN BLUR      ////////
        private void btnBlur_Click(object sender, EventArgs e)
        {
            if (imagen_cargada == null) this.Close();
            Mat imagen_dest = new Mat();
            CvInvoke.GaussianBlur(imagen_cargada, imagen_dest, new Size(3, 3), 0, 0);
            CvInvoke.Imshow("Gausian Blur", imagen_dest);
        }

        /////   MEDIAN BLUR      ////////
        private void btnMedianBlur_Click(object sender, EventArgs e)
        {
            if (imagen_cargada == null) this.Close();
            Mat imagen_dest = new Mat();
            CvInvoke.MedianBlur(imagen_cargada, imagen_dest, 5);
            CvInvoke.Imshow("Median Blur", imagen_dest);
        }

        /////   BILATERAL      ////////
        private void btnBilateral_Click(object sender, EventArgs e)
        {
            if (imagen_cargada == null) this.Close();
            Mat imagen_dest = new Mat();
            CvInvoke.BilateralFilter(imagen_cargada, imagen_dest, 15, 80, 80);
            CvInvoke.Imshow("Bilateral", imagen_dest);
        }

        /////   ECUALIZACION DEL HISTOGRAMAS    /////////
        private void button7_Click_1(object sender, EventArgs e)
        {// mejora de la imagen mediante metodo de ecualizacion del histograma
            if (imagen_cargada == null) return;
            //VARIABLES PARA EL HISTOGRAMA
            Mat img_gris = new Mat(imagen_cargada.Rows, imagen_cargada.Cols, DepthType.Cv8U, 1);
            int histSize = 256; int hist_w = 512; int hist_h = 400;
            float[] ranges = new float[] { 0, 255 };
            int bin_w = (int)Math.Round((double)hist_w / histSize);
            label1.Text = bin_w.ToString();

            //los histogramas se mostraran en las dos siguientes
            Mat histImage = new Mat(hist_h, hist_w, DepthType.Cv8U, 3); Mat equalizedHistImage = new Mat(hist_h, hist_w, DepthType.Cv8U, 3);

            Mat original_hist, equaliz_img, normalized_hist, equalized_hist, equalized_normalized_hist;
            VectorOfMat vm = new VectorOfMat(); VectorOfMat vm2 = new VectorOfMat();
            original_hist = new Mat(); equalized_normalized_hist = new Mat(); normalized_hist = new Mat(); equaliz_img = new Mat();
            equalized_hist = new Mat();

            //imagen a escala de grises
            CvInvoke.CvtColor(imagen_cargada, img_gris, ColorConversion.Rgba2Gray);

            // CALCULO DE LOS HISTOGRAMAS ETC 
            try
            {
                // histograma de imagen, imagen con ecualizacion de histograma(equalized_hist) y normalizo el histograma de imagen original en Gris
                vm.Push(img_gris);
                CvInvoke.CalcHist(vm, new int[] { 0 }, null, original_hist, new int[] { 256 }, ranges, false);// histograma
                CvInvoke.EqualizeHist(img_gris, equaliz_img);//imagen con ecualizacion de histograma(equalized_img)
                //histograma normalizado para verlo
                CvInvoke.Normalize(original_hist, normalized_hist, 0, histImage.Rows, NormType.MinMax, DepthType.Cv8U, new Mat());

                //Calculo histograma equalizado, real de la imagen obtenida antes por el metodo de equalizacion de histograma               
                vm2.Push(equaliz_img);
                CvInvoke.CalcHist(vm2, new int[] { 0 }, null, equalized_hist, new int[] { 256 }, ranges, false);
                // normalizo el histograma ecualizado para mostrarlo bien
                CvInvoke.Normalize(equalized_hist, equalized_normalized_hist, 0, histImage.Rows, NormType.MinMax, DepthType.Cv8U, new Mat());

                // dibujar histogramas en graficos
                for (int i = 1; i < 256; i++)
                {
                    int t1 = (normalized_hist.GetData(i)[0]);
                    CvInvoke.Line(histImage, new Point(bin_w * i, hist_w),
                                             new Point(bin_w * i, hist_h - t1),
                                       new MCvScalar(0, 120, 0), 2, LineType.Filled, 0);

                    int t2 = (equalized_normalized_hist.GetData(i)[0]);
                    CvInvoke.Line(equalizedHistImage, new Point(bin_w * i, hist_w),
                                          new Point(bin_w * i, hist_h - t2),
                                          new MCvScalar(0, 0, 255), 2, LineType.Filled, 0);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            imageBox2.Image = histImage;

            CvInvoke.Imshow("Original picture", img_gris);
            CvInvoke.Imshow("Equalized picture", equaliz_img);
            CvInvoke.Imshow("Original histogram", histImage);
            CvInvoke.Imshow("Equalized histogram", equalizedHistImage);

        } ///////////// FIN ECUALIZACION DEL HISTOGRAMAS  ////////////

        private void label1_Click(object sender, EventArgs e)
        {

        }

        //////  ***********   METODOS MIOS   ************ //////////////
        void detalles(Mat imagen, ListBox lb) {
            if (imagen == null) this.Close();
            lb.Items.Clear();
            lb.Items.Add(" Columnas:  " + imagen_cargada.Cols);
            lb.Items.Add(" Filas:  " + imagen_cargada.Rows);
            lb.Items.Add(" Depth:  " + imagen_cargada.Depth);
            lb.Items.Add("Dimension  " + imagen_cargada.Dims);
            lb.Items.Add("Element size  " + imagen_cargada.ElementSize);
            lb.Items.Add("Height  " + imagen_cargada.Height);
            lb.Items.Add("Canales  " + imagen_cargada.NumberOfChannels);
            lb.Items.Add("Size  " + imagen_cargada.Size);
            lb.Items.Add("width  " + imagen_cargada.Width);
            lb.Items.Add("Total  " + imagen_cargada.Total);
            lb.Items.Add("Dimensiones  " + imagen_cargada.SizeOfDimemsion);
        }

        void muestra_histograma(Mat histograma_Image, ListView lv)
        {
            Bgr color = new Bgr(100, 40, 243);
            Image<Bgr, Byte> original = new Image<Bgr, byte>(1024, 768);

            original[0, 1] = color;
            original.Data[0, 0, 2] = 243;
            Byte g = histograma_Image.GetData(0)[0];
            int suma = 0;
            lv.Clear();

            for (int h = 0; h < 255; h++)
            {
                // byte a = histograma_Image.Data;
                suma = suma + histograma_Image.GetData(h)[0];
                double[] valores = new double[100];
                for (int i = 0; i < 100; i++)
                {
                    valores[i] = histograma_Image.GetValue(0, i);
                }
                //int e = histograma_Image.GetValue(1, 50);
                listView1.Items.Add((h.ToString()) + " -->  " + (histograma_Image.GetData(h))[0]);//.GetHashCode().ToString());
                                                                                                  // listView1.Items.Add((h.ToString()) + " -->  " + (histograma_Image.GetData(h))[0].ToString());
                                                                                                  // listView1.Items.Add((h.ToString()) + " -->  " + (histograma_Image.b);
            }
            label2.Text = "cantidad suma = " + suma.ToString();
            detalles(histograma_Image, listBox1);
        }

        //variables histogramas
        //Variables para el histograma
#pragma warning disable CS0414 // El campo 'Form1.histSize' está asignado pero su valor nunca se usa
        int histSize = 256;
#pragma warning restore CS0414 // El campo 'Form1.histSize' está asignado pero su valor nunca se usa
        /// el rango del nivel del gris 0-255
        /*  float range[] = { 0, 256 };
        const float* histRange = { range };
        */
        float[] nivel_gris = new float[256];

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click_2(object sender, EventArgs e)
        {
            //listView1.Clear();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void imageBox2_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {   // muestra histograma de imagen cargada   HISTOGRAMA_1
            if (imagen_cargada == null) return;
            ShowHistograma(imagen_cargada);
            //////********  FIN  ****/////
        }

        private void ShowHistograma(Mat img_source)
        {
            VectorOfMat g = new VectorOfMat();
            Mat histImage = new Mat(img_source.Rows, img_source.Cols, DepthType.Cv8U, 3);
            int i = 0;
            try
            {
                int[] channels = { 0, 1, 2 };
                Mat hist = new Mat(img_source.Cols, img_source.Rows, DepthType.Cv8U, 3);
                Mat mask = new Mat();
                int[] histSize = { 8, 8, 8 };
                float[] ranges = { 256, 256, 256 };

                g.Push(img_source);
                CvInvoke.CalcHist(g, new int[] { 0 }, new Mat(), hist, new int[] { 256 }, new float[] { 0, 255 }, true);

                int hist_w = 512; int hist_h = 400;
                int bin_w = (int)Math.Round((double)hist_w / 256);//
                Mat im2 = new Mat(hist_h, hist_w, DepthType.Cv8U, 3);
                listView1.Clear();
                int pixeles = 0;
                for (i = 0; i < 256; i++)
                {
                    int t = (hist.GetData(i)[1]);
                    listView1.Items.Add(i.ToString() + " -> " + t.ToString());
                    CvInvoke.Line(im2, new Point(bin_w * i, hist_w),
                                       new Point(bin_w * i, hist_h - t),
                                       new MCvScalar(255, 0, 0), bin_w, LineType.EightConnected, 0);//;// hist_h - (int)Math.Round((decimal) rg.GetData(i)[0])),
                                                                                                    //listView1.Items.Add(t.ToString());
                    pixeles = pixeles + t;
                }
                TBpixeles.Text = pixeles.ToString();

                CvInvoke.Imshow("histograma 1 ", im2);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "  indice:   " + i.ToString());
            }
        }

        //////////////      REALCE DE BORDES POR LAPLACIANO Y MEDIA
        private void button9_Click(object sender, EventArgs e)
        {   //MEDIA CON GANANCIA VARIABLE
            if (imagen_cargada == null) return; //if (label5.Text == "") this.Close();
            try
            {
                Mat kernel = new Mat(3, 3, DepthType.Cv8S, 1); ;// (3, 3, DepthType.Cv32F, 1);
#pragma warning disable CS0219 // La variable 'sd' está asignada pero su valor nunca se usa
                float sd;
#pragma warning restore CS0219 // La variable 'sd' está asignada pero su valor nunca se usa
                //  float g = (float) 1.12;// GANANCIA
                // float center = (float)(g - 1/9);

                sd = (float)(-1 / 9.0);
                sbyte sd1 = -1; sbyte center1 = 9;

                kernel.SetValue(0, 0, sd1); kernel.SetValue(0, 1, sd1); kernel.SetValue(0, 2, sd1);
                kernel.SetValue(1, 0, sd1); kernel.SetValue(1, 1, center1); kernel.SetValue(1, 2, sd1);
                kernel.SetValue(2, 0, sd1); kernel.SetValue(2, 1, sd1); kernel.SetValue(2, 2, sd1);

                Mat imagen_filt = new Mat(imagen_cargada.Rows, imagen_cargada.Cols, DepthType.Cv8U, 3);
                CvInvoke.Filter2D(imagen_cargada, imagen_filt, kernel, new Point(1, 1), 0, 0);
                CvInvoke.Imshow("resultado", imagen_filt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        /////////     LAPLACIANO
        private void button10_Click(object sender, EventArgs e)
        {
            Mat kernekk = new Mat(3, 3, DepthType.Cv8S, 1);
            Form2 fr = new Form2(kernekk);
            fr.ShowDialog();
            laplaciano(imagen_cargada, kernekk);
        }
        //  ********  FIN DE REALCE DE BORDES POR LAPLACIANO Y MEDIA ***********


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        void prepara_histogramas() {
            Mat histImage = new Mat(400, 500, DepthType.Cv8U, 3);
            int hist_h = 400; int hist_w = 512;
            Mat equalizedHistImage = new Mat(hist_h, hist_w, DepthType.Cv8U, 3);
            //calcular el histograma
            int[] channels = { 0, 1, 2 };
            Mat hist = new Mat();
            Mat mask = new Mat();
            int[] histSize = { 32, 32, 32 };
            float[] ranges = { 0.0f, 256.0f, 0.0f, 256.0f, 0.0f, 256.0f };
            //   Mat original_hist, normalized_hist, equalized_hist, equalized_normalized_hist;
            CvInvoke.CalcHist(imagen_cargada, channels, mask, hist, histSize, ranges, true);
            //   calcHist(&original_img, 1, 0, Mat(), original_hist, 1, &histSize, &histRange, true, false);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            insert_result();
        }
        private void insert_result()
        {
            // DETECTAR NUMERO DE ID
            int cantidad = listView1.Items.Count + 1;

            ListViewItem lista = new ListViewItem(cantidad.ToString());
            lista.SubItems.Add("xcvzbdbd");
            lista.SubItems.Add("subitem1");
            lista.SubItems.Add("subitem2");
            listView1.Items.Add(lista);


        }

        private void button12_Click(object sender, EventArgs e)
        {

        }

        private void imageBox1_MouseClick(object sender, MouseEventArgs e)
        {
            // TBpixeles.Text = MousePosition.ToString();
        }

        private void TBpixeles_TextChanged(object sender, EventArgs e)
        {

        }

        private void imageBox1_Click(object sender, EventArgs e)
        {
            TBpixeles.Text = MousePosition.ToString();
        }

        private void imageBox1_DragEnter(object sender, DragEventArgs e)
        {
            // TBpixeles.Text = MousePosition.ToString();
        }

        private void imageBox1_DragOver(object sender, DragEventArgs e)
        {
            //textBox1.Text = MousePosition.ToString();
        }

        private void imageBox1_MouseUp(object sender, MouseEventArgs e)
        {
            textBox1.Text = MousePosition.ToString();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        public Mat calchist(Mat ent)
        {
            int pixel = 1;
            int[] pixels = new int[256];
            Mat img_input = new Mat(ent.Cols, ent.Rows, DepthType.Cv8U, 3);
            Mat img_output = new Mat(1, 255, DepthType.Cv8U, 3);
            for (int filas = 1; filas < ent.Rows - 1; filas++)
            {
                for (int Columnas = 1; Columnas < ent.Cols - 1; Columnas++)
                {
                    pixel = ent.GetData(Columnas, filas)[1];

                    for (int i = 1; i < 256; i++)
                    {
                        if (pixel == i)
                        {
                            pixels[i] = pixels[i] + 1;
                            //img_output.SetValue(1, i, (byte) pixels[i]);
                            // img_output.GetData(0, i)[0] = img_output.GetData(0, i)[0] ++;
                        }
                    }
                }
            }
            return img_output;
        }

        private void gpbcontrastes_Enter(object sender, EventArgs e)
        {

        }

        private void aRCHIVOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // ESTO ESTA POR MENU
            Mat kernekk = new Mat(3, 3, DepthType.Cv8S, 1);
            Form2 fr = new Form2(kernekk);
            fr.ShowDialog();
            laplaciano(imagen_cargada, kernekk);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void laplaciano(Mat img_ent, Mat kernel_lap) {
            Mat imagen_filt = new Mat(img_ent.Rows, img_ent.Cols, DepthType.Cv8U, 3);
            CvInvoke.Filter2D(img_ent, imagen_filt, kernel_lap, new Point(1, 1), 0, 0);
            CvInvoke.Imshow("resultado", imagen_filt);
        }

        //  DETECCION DE BORDES BASADO EN GRADIENTES
        private void button9_Click_1(object sender, EventArgs e)
        {

        }

        Mat imagen_sobel = new Mat();
        Mat grad = new Mat();
        private void button9_Click_2(object sender, EventArgs e)
        {// Gausian_blur
            if (imagen_cargada == null) return;
            CvInvoke.GaussianBlur(imagen_cargada, imagen_sobel, new Size(3, 3), 0, 0);
            CvInvoke.Imshow("Gausian Blur", imagen_sobel);
        }

        private void button10_Click_1(object sender, EventArgs e)
        {// aplico ambos sobel y los sumo
            if (imagen_sobel == null) return;
            Mat grad_x = new Mat();
            Mat grad_y = new Mat();
            int scale = 1;
            int delta = 0;
            Mat abs_grad_x = new Mat();
            Mat abs_grad_y = new Mat();
            // gradienite X
            CvInvoke.Sobel(imagen_sobel, grad_x, DepthType.Cv16S, 1, (1/2), 3, scale, delta, BorderType.Default);
            CvInvoke.ConvertScaleAbs(grad_x, abs_grad_x, 1, 1);
            // gradiente Y
            CvInvoke.Sobel(imagen_sobel, grad_y, DepthType.Cv16S, 0, 1, 3, scale, delta, BorderType.Default);
            CvInvoke.ConvertScaleAbs(grad_y, abs_grad_y, 1, 1);
            // total gradiente (aproximada)
            CvInvoke.AddWeighted(abs_grad_x, 0.5, abs_grad_y, 0.5, 0, grad);
            Mat tmp = new Mat();
            CvInvoke.Threshold(grad, tmp, 40, 255, ThresholdType.Binary);
            CvInvoke.Imshow("grad", tmp);
            trackBar2.Enabled = true;

        }

        private void button12_Click_1(object sender, EventArgs e)
        {

        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            if (grad == null) return;
            int valor = this.trackBar2.Value;
            Mat tmp = new Mat();
            CvInvoke.Threshold(grad, tmp, valor, 255, ThresholdType.Binary);
            CvInvoke.Imshow("grad", tmp);
        }

        private void lv2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// //////// CALCULO CANNY
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button12_Click_2(object sender, EventArgs e)
        {   //CANNY
            if (imagen_cargada == null) return;
            // Initializes the variables to pass to the MessageBox.Show method.
            string message = "Este Metodo contiene una serie de operaciones que se realizan directamente en las OpenCV";
            message = message + ", Umbral superior: " + label9.Text + "  , Umbral Inferior:  " + label10.Text;
            string caption = "Notece";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;
            // Displays the MessageBox.
            result = MessageBox.Show(message, caption, buttons);
            if (result != System.Windows.Forms.DialogResult.Yes) return;
            calculo_canny();

        }

        private void calculo_canny()
        {
            ///// Calculo CANNY
            Double thres_sup = TBSuperior.Value;
            Double thres_inf = trackBar3.Value;
            Mat Imagen_canny = new Mat();
            CvInvoke.Canny(imagen_cargada, Imagen_canny, thres_inf, thres_sup, 3);
            CvInvoke.Imshow("original", imagen_cargada);
            CvInvoke.Imshow("Canny", Imagen_canny);
            // REPORT CANNY
            //  Id: ver ultimoi id y sumar 1
            //  Operacion = canny, thresfold inferior y superior. objeto Imagen fuente y destino

        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            int inf = trackBar3.Value;
            int sup = TBSuperior.Value;
            if (inf == 100) return;
            if (inf > sup) TBSuperior.Value = inf + 1;
            ajusto_tracks();
            if (imagen_cargada == null) return;
            calculo_canny();
        }

        private void TBSuperior_Scroll(object sender, EventArgs e)
        {
            int inf = trackBar3.Value;
            int sup = TBSuperior.Value;
            if (sup < 1) return;
            if (sup < inf) trackBar3.Value = sup - 1;
            ajusto_tracks();
            if (imagen_cargada == null) return;
            calculo_canny();
        }

        private void ajusto_tracks() {
            double inf = trackBar3.Value;
            double sup = TBSuperior.Value;

            label10.Text = inf.ToString();
            label9.Text = sup.ToString();
        }

        private void listView1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            // listBox2.Items.AddRange();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            int selected = listView1.SelectedIndices[0];
            string mensaj = listView1.Items[selected].SubItems[2].Text.ToString();
            listBox2.Items.Add(mensaj);            
        }

        private void panAndZoomPictureBox1_Click(object sender, EventArgs e)
        {

        }

        ///////////////////////
        /////  FIN DEL CALCULO CANNY
        ///////////


        /* private void button10_Click(object sender, EventArgs e)
{  
lis
}*/
    }

    
}

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
using Emgu.CV.CvEnum;
using Emgu.CV.Util;

namespace test
{
    public partial class Form1 : Form
    {
        private const byte V = 4;
        private Mat imagen_dest, imagen_dest1, patron;
        Image<Gray, byte> hsv_planes;
        Image<Gray, byte> bgr_planes;
        private Mat[] hsv_planos = new Mat[3];
        private Mat[] bgr_planos = new Mat[4];
        private Mat imagen_cargada;
        List<Image<Gray, byte>> bgr_plans = new List<Image<Gray, byte>>();
        Image<Bgr, byte> template;
        Image<Bgr, byte> source;
        Image<Bgr, byte> imageToShow;
        Mat img_matriz_rotacion;
        private double set= 0.5;

        public Form1()
        {
            InitializeComponent();
            label3.Text = set.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!(imagen_cargada == null))
            {
                imagen_dest = imagen_cargada;
                CvInvoke.CvtColor(imagen_cargada, imagen_dest, Emgu.CV.CvEnum.ColorConversion.Rgb2Hsv);                
            }
            imageBox1.Image = imagen_dest;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

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
                    imagen_dest = new Mat();
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
            if (!(imagen_cargada == null))
                { 
                Mat Kernel;
               // double[] r = {4};
                Mat y = new Mat();
                // y = { 4};
                Kernel = Mat.Ones(2, 2, DepthType.Cv32F,1);
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
                imagen_dest = imagen_cargada;
                CvInvoke.Filter2D(imagen_cargada, imagen_dest, Kernel, new Point(-1, -1),1 , BorderType.Default);
                }
            imageBox1.Image = imagen_dest;
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
            set = trackBar1.Value/10;
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
                    CvInvoke.Resize(source, imagen_dest, new Size(1/2, 1/2), .3, .3, Emgu.CV.CvEnum.Inter.Linear);
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
                    source.Draw(rect, new Bgr(Color.Green),5);

                    Rectangle rect2 = new Rectangle(0, 0, img.Cols - 100, img.Rows - 100);
                    img.Draw(rect2, new Bgr(Color.Blue),5);

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
                    double angulo = 60;
                    PointF p = new PointF(len / 2, len / 2);
                    img_matriz_rotacion = new Mat();
                    CvInvoke.GetRotationMatrix2D(p, angulo,.3, img_matriz_rotacion);
                    CvInvoke.WarpAffine(source, imagen_dest, img_matriz_rotacion,new Size(len, len));
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
                    source = new Image<Bgr, byte>(openFileDialog1.FileName);
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
                    template = new Image<Bgr, byte>(openFileDialog1.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            else this.Close();
            
            int iwidth = source.Cols - template.Cols + 1;
            int iheight = source.Rows - template.Rows + 1;

            // Mat imagen_dest1 = new Mat(iheight, iwidth, DepthType.Cv32F, 3);
            // Mat Kernel = Mat.Ones(template.Cols, template.Rows, DepthType.Cv32F, 1);
            // CvInvoke.MatchTemplate(imagen_cargada, patron, imagen_dest1, Emgu.CV.CvEnum.TemplateMatchingType.CcoeffNormed);
            // CvInvoke.Normalize(imagen_dest1, imagen_dest1, 0, 1, Emgu.CV.CvEnum.NormType.MinMax, DepthType.Cv32F);

            double[] minVal, maxVal;
            Point[] minLoc, maxLoc, matchLoc;

            TemplateMatchingType[] match_method = { TemplateMatchingType.CcoeffNormed, TemplateMatchingType.Ccoeff, TemplateMatchingType.Ccorr,
                TemplateMatchingType.CcorrNormed, TemplateMatchingType.Sqdiff, TemplateMatchingType.SqdiffNormed};
            Image<Gray, float> fuente=  new Image<Gray, float>(iheight, iwidth);
            imageToShow = source.Copy();
            TemplateMatchingType tipo = match_method[0];

            // foreach (TemplateMatchingType tipo in match_method)
            // {
            int i = 0;
                using (fuente = source.MatchTemplate(template, tipo))
                {
                   // CvInvoke.Normalize(source, fuente, 0, 1, NormType.MinMax, DepthType.Cv8U);
                    fuente.MinMax(out minVal, out maxVal, out minLoc, out maxLoc);

                    if (tipo == TemplateMatchingType.Sqdiff || tipo == TemplateMatchingType.SqdiffNormed)
                    {
                        matchLoc = minLoc;
                    }
                    else
                    {
                        matchLoc = maxLoc;
                    }

                    if (maxVal[0] > 0.5)
                    {
                    i++;
                        // This is a match. Do something with it, for example draw a rectangle around it.
                        Rectangle match = new Rectangle(maxLoc[0], template.Size);
                        // imageToShow = source.Copy();
                        imageToShow.Draw(match, new Bgr(Color.Red), 3);
                        CvInvoke.Imshow("E", fuente);
                    }                    
                    //CvInvoke.Imshow("E", fuentes1[i]);
                    /* CvInvoke.Rectangle(source, matchLoc, Point(matchLoc.x + template.Cols, matchLoc.y + template.Rows), Scalar(255, 0, 0), 4, 8, 0);
                     CvInvoke.Rectangle(dst, Point(matchLoc.x - (templ.cols / 2), matchLoc.y - (templ.rows / 2)), Point(matchLoc.x + (templ.cols / 2), matchLoc.y + (templ.rows / 2)), Scalar::all(0), 4, 8, 0);
                     */
                    listBox1.Items.Add(matchLoc[0]);
                //  }
                listBox1.Items.Add("veces  " + i);
                listBox1.Items.Add(matchLoc.ToString());
            }            
            CvInvoke.Imshow("E", template);
            CvInvoke.Imshow("f", imageToShow);
        }

        //////      BLUR        /////////
        private void button7_Click(object sender, EventArgs e)
        {
            if (imagen_cargada == null) this.Close();
            try
            {
                CvInvoke.Blur(imagen_cargada, imagen_dest, new Size(3, 3), new Point(-1, -1), BorderType.Isolated);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            CvInvoke.Imshow("Blur", imagen_dest);
        }

        /////   GAUSIAN BLUR      ////////
        private void btnBlur_Click(object sender, EventArgs e)
        {
            if (imagen_cargada == null) this.Close();
            CvInvoke.GaussianBlur(imagen_cargada, imagen_dest, new Size(3, 3), 0, 0);
            CvInvoke.Imshow("Gausian Blur", imagen_dest);
        }

        /////   MEDIAN BLUR      ////////
        private void btnMedianBlur_Click(object sender, EventArgs e)
        {
            if (imagen_cargada == null) this.Close();
            CvInvoke.MedianBlur(imagen_cargada, imagen_dest, 5);
            CvInvoke.Imshow("Median Blur", imagen_dest);
        }

        /////   BIATERAL      ////////
        private void btnBilateral_Click(object sender, EventArgs e)
        {
            if (imagen_cargada == null) this.Close();
            CvInvoke.BilateralFilter(imagen_cargada, imagen_dest, 15, 80, 80);
            CvInvoke.Imshow("Bilateral", imagen_dest);
        }

       
        /////    HISTOGRAMAS    /////////
        private void button7_Click_1(object sender, EventArgs e)
        {
            if (imagen_cargada == null) this.Close();
            // Mat histImage = new Mat();// (648, 912, DepthType.Cv8U, 3);
            Emgu.CV.Structure.MCvScalar sc = new MCvScalar(0,0,0);
            int[] channels = { 0, 1, 2 };
            int[] histSize = { 256, 256, 256 };
            float[] ranges = new float[] { 0, 255 };
            Mat equaliz_img, imagen_gray;
            Mat original_hist, normalized_hist, equalized_hist, equalized_normalized_hist;

            int histSize1 = 256;int hist_w = 512; int hist_h = 400;
            int ancho = imagen_cargada.Rows; int largo = imagen_cargada.Cols;

            hist_w = imagen_cargada.Cols; hist_h = imagen_cargada.Rows;
            int bin_w = (int)Math.Round((double)hist_w / histSize1);
            label1.Text = bin_w.ToString();
            
            Mat histImage = new Mat(hist_h, hist_w, DepthType.Cv8U, 3);
            Mat equalizedHistImage = new Mat(hist_h, hist_w, DepthType.Cv8U, 3);
            
            /////// calcular el histograma
            Emgu.CV.Util.VectorOfMat vm = new Emgu.CV.Util.VectorOfMat();
            vm.Push(imagen_cargada);  // Mat histImage1 = new Mat();// (400, 512, DepthType.Cv8U, 1);
            try
            {
                original_hist = new Mat();
                normalized_hist = new Mat();
                equaliz_img = new Mat();
                imagen_gray = new Mat();
                equalized_hist = new Mat();
                equalized_normalized_hist = new Mat();
                // CALCULO HISTOGRAMA
                CvInvoke.CalcHist(vm, new int[] { 0 }, null, original_hist, new int[] { 256 }, ranges, false);
                                
                // NORMALIZO HISTOGRAMA A [ 0, histImage.rows ]
                CvInvoke.Normalize(original_hist, normalized_hist,0, histImage.Rows, NormType.MinMax, DepthType.Cv8U, new Mat());
                detalles(normalized_hist, listBox1);

                //  Equalize histogram from a grayscale image	
                CvInvoke.CvtColor(imagen_cargada, imagen_gray, ColorConversion.Bgr2Gray);
                CvInvoke.EqualizeHist(imagen_gray, equaliz_img);

                // Normalizar el histograma ecualizado
                Emgu.CV.Util.VectorOfMat vm2 = new Emgu.CV.Util.VectorOfMat();
                vm2.Push(equaliz_img);
                CvInvoke.CalcHist(vm2, new int[] { 0 }, null, equalized_hist, new int[] { 256 }, ranges, false);
                                               
                CvInvoke.Normalize(equalized_hist, equalized_normalized_hist, 0, histImage.Rows, NormType.MinMax, DepthType.Cv8U, new Mat());
                muestra_histograma(equalized_normalized_hist, listView1);
                CvInvoke.Imshow("asd2", equalized_normalized_hist);

                /// dibujar los histogramas
                //Point p1 = new Point(bin_w * i, hist_w;)
                for (int i = 1; i < 256; i++)
                {
                    CvInvoke.Line(histImage, new Point(bin_w * i, hist_w),
                                         // new Point(bin_w * i, 100),
                                         new Point(bin_w * i, hist_h - (int)Math.Round((decimal)histImage.GetData(i)[0].GetHashCode())),
                                       new MCvScalar(255,120,0), 2 ,LineType.Filled, 0);//;// hist_h - (int)Math.Round((decimal) rg.GetData(i)[0])),
                                                                                        // 
                //    int hh = (int)normalized_hist.Data.GetValue(i)[0];

                 CvInvoke.Line(equalized_normalized_hist, new Point(bin_w * i, hist_w),
                                       new Point(bin_w * i, hist_h - (int)Math.Round((decimal) equalized_normalized_hist.GetData(i)[0].GetHashCode())),
                                       new MCvScalar(255, 255, 0), 2, LineType.Filled, 0);
                }

                imageBox2.Image = histImage;
              CvInvoke.Imshow("essss", histImage);
              CvInvoke.Imshow("esss111s", equalized_normalized_hist);
              CvInvoke.Imshow("histograma", original_hist);

                    // char* input = (unsigned char*)(rg.data);
                    // PixelRGB* rgb_color_info = new PixelRGB[your_rgb_image_width * your_rgb_image_width];
                    //  char* input = rg.Data;
                    // Mat input2;
                    //  int i3, j, r, g, b;
                    /*  for (int i1 = 0; i1 < rg.Rows; i1++)
                      {
                          for (int j = 0; j < rg.Cols; j++)
                          {
                              b =(int) rg.Data.GetValue(rg.Step * j + i1);
                             // g = input[rg.Step * j + i1 + 1];
                             // r = input[img.step * j + i1 + 2];
                          }
                      }*/
              
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            
        } ///////////// FIN HISTOGRAM ////////////

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
            original.Data[0,0, 2] = 243;
            Byte g = histograma_Image.GetData(0)[0];
            int suma = 0;
            lv.Clear();

            for (int h = 0; h < 255; h++)
               {
               // byte a = histograma_Image.Data;
               suma = suma + histograma_Image.GetData(h)[0];
                double[] valores = new double[100];
                for (int i= 0; i <100; i++)
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
        Mat original_img, equaliz_img;
        //Variables para el histograma
        int histSize = 256;
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
            listView1.Clear();
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

            VectorOfMat g = new VectorOfMat();
        private void button8_Click(object sender, EventArgs e)
        {   // muestra histograma de imagen cargada
            if(imagen_cargada == null) this.Close();
            Mat histImage = new Mat(imagen_cargada.Rows, imagen_cargada.Cols, DepthType.Cv8U, 3);
            Mat im1 = new Mat(400, 550, DepthType.Cv8U, 3);
            Mat im2 = new Mat();// (400, 512, DepthType.Cv8U, 3);
            int i = 1;
            try
            {
                int[] channels = { 0 , 1, 2 };
                Mat hist = new Mat(imagen_cargada.Cols, imagen_cargada.Rows, DepthType.Cv8U,3);
                Mat mask = new Mat();
                int[] histSize = { 8, 8, 8 };
                float[] ranges = { 256, 256, 256 };// { 0.0f, 256.0f, 0.0f, 256.0f, 0.0f, 256.0f };
                //  Mat original_hist, normalized_hist, equalized_hist, equalized_normalized_hist;
               
                g.Push(imagen_cargada);
                //CvInvoke.CalcHist(g, channels, mask, hist, histSize, ranges, true);
                CvInvoke.CalcHist(g, new int[] { 0}, null, hist, new int[] {256 }, new float[] { 0, 255 }, true);

             // CvInvoke.CalcHist(g, channels, new Mat(), hist, histSize, ranges, false);
               // CvInvoke.CalcHist(g, new int[] { 0 }, null, hist, new int[] { 256 }, ranges, false);
                Byte[] T = new Byte[256]; T = hist.GetData(0,1);
                //hist.GetData(0);
                listView1.Clear();
                Mat R, G, B;  Byte[] br; br = hist.GetData(1);
                
                int hist_w = 600;// imagen_cargada.Cols, 
                hist_h = 400;// imagen_cargada.Rows;
                int bin_w = (int)Math.Round((double) hist_w / 256);
                listView1.Clear();
              
                for (i = 1; i < 256; i++)
                {
                   
                    int t = (hist.GetData(i)[1]);
                    listView1.Items.Add(t.ToString());
                  CvInvoke.Line(im1, new Point(bin_w * i+2, hist_w),
                                         new Point(bin_w * i+2, hist_h -t), 
                                       new MCvScalar(255, 0, 0), bin_w, LineType.Filled, 0);//;// hist_h - (int)Math.Round((decimal) rg.GetData(i)[0])),
                    listView1.Items.Add(t.ToString());
                }
                CvInvoke.Imshow("histograma 1", im1);


            /*    for (var l = 0; l < hist.getlength; l++)
                {
                    var val = (float)hist.GetValue(i);
                val = (float)(val * (maxVal != 0 ? height / maxVal : 0.0));
                    Point s = new Point(i, height);
                    Point e = new Point(i, height - (int)val);
                    g.DrawLine(penGray, s, e);
                }*/


                /*  listView1.Clear();
                  for (i = 1; i < 256; i++)
                  {
                      //im1.Dispose();
                      int t = (hist.GetData(i)[2]);
                      CvInvoke.Line(im1, new Point(bin_w * i + 2, hist_w),
                                             new Point(bin_w * i + 2, hist_h - (hist.GetData(i)[2])),
                                           new MCvScalar(0, 255, 0), bin_w, LineType.Filled, 0);//;// hist_h - (int)Math.Round((decimal) rg.GetData(i)[0])),
                      listView1.Items.Add(t.ToString());
                  }
                  CvInvoke.Imshow("histograma 2", im1);

                  for (i = 1; i < 256; i++)
                  {
                     // im1.Dispose();
                      CvInvoke.Line(im1, new Point(bin_w * i + 2, hist_w),
                                             new Point(bin_w * i + 2, hist_h - (hist.GetData(i)[3])),
                                           new MCvScalar(0, 0, 255), bin_w, LineType.Filled, 0);//;// hist_h - (int)Math.Round((decimal) rg.GetData(i)[0])),
                  }
                  CvInvoke.Imshow("histograma 3", im1);*/

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString()+ "  indice:   " + i.ToString());
            }
           //////********  FIN  ****/////
        }
              
     //   int bin_w = (int)(512 / 400);// cvRound((double)hist_w / histSize);
      static private int hist_w = 512, hist_h = 400;
        
        void prepara_histogramas() {
       /// imagen del histograma
       //private float bin_f =512/400; // hist_w / histSize;
       
        Mat histImage = new Mat(400, 500, DepthType.Cv8U, 3);
        Mat equalizedHistImage = new Mat(hist_h, hist_w, DepthType.Cv8U, 3);
        //calcular el histograma
        int[] channels = { 0, 1, 2 };
        Mat hist = new Mat();
        Mat mask = new Mat();
        int[] histSize = { 32, 32, 32 };
        float[] ranges = { 0.0f, 256.0f, 0.0f, 256.0f, 0.0f, 256.0f };
      //  Mat original_hist, normalized_hist, equalized_hist, equalized_normalized_hist;
       CvInvoke.CalcHist(imagen_cargada, channels, mask, hist, histSize, ranges, true );
       //calcHist(&original_img, 1, 0, Mat(), original_hist, 1, &histSize, &histRange, true, false);
    }
}
}

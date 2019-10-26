using Emgu.CV;
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
    public partial class Form2 : Form
    {
        public Mat kernel;
      /*  public Form2()
        {
            InitializeComponent();
        }*/

        public Form2(Mat k)
        {
            InitializeComponent();
            this.kernel = k;
        }

     /*   private Mat Form2_Closed(object sender, EventArgs e) {

            return kernel;
        }*/
        
        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            sbyte k00 = sbyte.Parse(textBox1.Text);
            sbyte k01 = sbyte.Parse(textBox6.Text); sbyte k02 = sbyte.Parse(textBox9.Text);

            sbyte k10 = sbyte.Parse(textBox2.Text);
            sbyte k11 = sbyte.Parse(textBox5.Text); sbyte k22 = sbyte.Parse(textBox8.Text);
            sbyte k20 = sbyte.Parse(textBox3.Text); sbyte k21 = sbyte.Parse(textBox4.Text);sbyte k12 = sbyte.Parse(textBox7.Text);
            
            kernel.SetValue(0, 0, k00);

            kernel.SetValue(0, 1, k01); kernel.SetValue(0, 2, k02);
            kernel.SetValue(1, 0, k10); kernel.SetValue(1, 1, k11); kernel.SetValue(1, 2, k22);
            kernel.SetValue(2, 0, k20); kernel.SetValue(2, 1, k21); kernel.SetValue(2, 2, k12);
            Close();
        }
    }
}

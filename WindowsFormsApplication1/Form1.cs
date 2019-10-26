using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
       


        public Form1()
        {
            InitializeComponent();
           // OPERACIONES Arraylist = new ArrayList();
            List<operacion> OPERACIONES = new List<operacion>();



        }


        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Items[2].UseItemStyleForSubItems = false;
            listView1.Items[2].BackColor = Color.AliceBlue;

            //prueba de operador id static

            operacion miop1 = new operaciontipo1();

            TbOperacion.Text = operacion.id_op.ToString();









        }

        private void btnEditOp_Click(object sender, EventArgs e)
        {
            //CARGA OPERACION EN PANEL OPERACION DESCRIPCION
        }

        private void btnDelOp_Click(object sender, EventArgs e)
        {
            //BORRAR OPERACION, ELIMINAR LINEA
        }

        private void btnAddOp_Click(object sender, EventArgs e)
        {
            //AGREGAR OPERACION

        }

        private void nUEVOToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }


    public class Item1 {

    }
    public abstract class operacion
    {
        public static int id_op;
        public string op_name;
        public Mat img_ent;
        public Mat img_salida;

        public operacion() {
            id_op = id_op + 1;
        }



    }

    public class operaciontipo1 : operacion {

        public operaciontipo1() {

        }

    }

}

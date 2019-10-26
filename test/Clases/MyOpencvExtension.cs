using System;
using Emgu.CV;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.ComponentModel;
/// <summary>
/// clase para modelar el proceso de relacionar la funcion de OpenCv con el listbox etc.
/// </summary>
namespace test.Clases
{

    public abstract class MyOpencvExtension
    {
#pragma warning disable CS0649 // El campo 'MyOpencvExtension.lbresults' nunca se asigna y siempre tendrá el valor predeterminado null
        private ListBox lbresults;
#pragma warning restore CS0649 // El campo 'MyOpencvExtension.lbresults' nunca se asigna y siempre tendrá el valor predeterminado null
#pragma warning disable CS0169 // El campo 'MyOpencvExtension.imagen_name' nunca se usa
        private Mat imagen_name;
#pragma warning restore CS0169 // El campo 'MyOpencvExtension.imagen_name' nunca se usa
#pragma warning disable CS0169 // El campo 'MyOpencvExtension.imagen_result' nunca se usa
        private Mat imagen_result;
#pragma warning restore CS0169 // El campo 'MyOpencvExtension.imagen_result' nunca se usa
        public string result;
        public string error;
        public int Identificador;
        public abstract void Operacion(Mat imagen, ListBox lbresults);
        private BindingList<Item> items = new BindingList<Item>();
                     
        public void Resultado() {
            lbresults.Items.Add(result);
            lbresults.DataContext = items;
         }



        // ejemplo
        //     MyOpemasdfas fr = new Myopene(lb);
        //     fr.gauss();


/////////////////   CLASES      //////////
    private class Item
    {
            private int identificador_operacion;
            private string descripcion_operacion;
            private string imagen_source_name;
            private string imagen_result_name;
                   
        public Item(int id, string descripcion, string img_source, string img_dest)
        {
            this.identificador_operacion = id;
            this.descripcion_operacion = descripcion;
            this.imagen_source_name = img_source;
            this.imagen_result_name = img_dest;
        }
                   
    }
}

}






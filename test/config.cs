using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    public class config
    {
#pragma warning disable CS0169 // El campo 'config.last' nunca se usa
        private string last;
#pragma warning restore CS0169 // El campo 'config.last' nunca se usa
#pragma warning disable CS0169 // El campo 'config.first' nunca se usa
        private string first;
#pragma warning restore CS0169 // El campo 'config.first' nunca se usa

        public config(string ruta)
        {
            //abro ruta y cargo file de BBDD sqlite

        }
        // chequeo nombre de la aplicacion si es la que me llamó
        // listo los campos y chequeo existan valores para todos.
        // get y set de los campos de datos




    }


}

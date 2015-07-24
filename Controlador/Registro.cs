using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controlador
{
    public class Registro
    {
        Int64 idPersona;
        dsRegistroTableAdapters.registro_redessocialesTableAdapter q;
        dsRegistroTableAdapters.ip2location_db5_ipv6TableAdapter q_ip;
        dsRegistro.registro_redessocialesRow datos;

        public Registro()
        {
            this.idPersona = -1;
            this.q = new dsRegistroTableAdapters.registro_redessocialesTableAdapter();
        }

        public Registro(Int64 idPersona)
        {
            this.idPersona = idPersona;
            this.q = new dsRegistroTableAdapters.registro_redessocialesTableAdapter();
            dsRegistro.registro_redessocialesDataTable tabla = this.q.GetDataByIdPersona(Convert.ToInt32(this.idPersona));
            if (tabla.Count > 0)
            {
                this.datos = tabla[0];
            }
        }

        public DataTable SeleccionarLoc(String ipnum)
        {
            DataTable dtLocaliz=new DataTable();
            dsRegistroTableAdapters.ip2location_db5_ipv6TableAdapter adapter = new dsRegistroTableAdapters.ip2location_db5_ipv6TableAdapter();
            this.q_ip = new dsRegistroTableAdapters.ip2location_db5_ipv6TableAdapter();
            dtLocaliz=q_ip.GetDataByIP(ipnum);
                      

            return dtLocaliz;
        }

        public Int64 Insertar(String nombre,String email,String geoposicion)
        {
            MySqlTransaction tr = null;
            Int64 idPersona;
            dsRegistroTableAdapters.registro_redessocialesTableAdapter adapter = new dsRegistroTableAdapters.registro_redessocialesTableAdapter();

            //Abrimos conexión

            

            //Iniciamos transacción 

           
            try
            {
                adapter.Connection.Open();
                
                tr = adapter.Connection.BeginTransaction();
                //Aquí hacemos el insert y buscamos el último id de la noticia metida
                this.q = new dsRegistroTableAdapters.registro_redessocialesTableAdapter();
                q.InsertarRegistro(nombre,email,geoposicion);
                idPersona = Convert.ToInt64(q.GetLastID());
                //Realizamos el commit de la transacción
                tr.Commit();
                //Cerramos conexión
                adapter.Connection.Close();
                return (Int64)idPersona;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //Rollback
                tr.Rollback();
                adapter.Connection.Close();
                return -1;
            }
           

        }



    }
}

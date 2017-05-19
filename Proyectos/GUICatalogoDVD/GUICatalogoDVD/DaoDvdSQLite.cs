using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//-----------
using System.Data;
using System.Collections.ObjectModel;
using System.Data.SQLite;

namespace CatalogoDVD
{
    class DaoDvdSQLite : IDAO
    {
        private SQLiteConnection conexion;

        public bool Conectar(string srv, string db, string user, string pwd)
        {
            string cadenaConexion = "Data source=" + db + ";Version=3;" + "FailIfMissing=true;";

            try
            {
                conexion = new SQLiteConnection(cadenaConexion);
                conexion.Open();
                return true;
            }
            catch (Exception)
            {
                throw new Exception("Error de conexion: ");
            }
        }

        public bool Desconectar()
        {
            try
            {
                if (conexion != null)
                {
                    conexion.Close();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Conectado()
        {

            if (conexion != null)
            {
                return conexion.State == System.Data.ConnectionState.Open;
            }
            else
            {
                return false;
            }
        }

        public DataTable SeleccionarTB(string codigo)
        {
            DataTable dt = new DataTable();
            string sql;

            if (codigo == null)
            {
                sql = "select codigo,titulo,artista,pais,compania,precio,anio from dvd";
            }
            else
            {
                sql = "select codigo,titulo,artista,pais,compania,precio,anio from dvd where codigo = "+codigo;
            }

            SQLiteCommand cmd = new SQLiteCommand(sql, conexion);
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            da.Fill(dt);

            return dt;
        }

        public ObservableCollection<Dvd> Seleccionar(string codigo)
        {
            ObservableCollection<Dvd> resultado = new ObservableCollection<Dvd>();
            string orden;

            if (codigo == null)
            {
                orden = "select codigo,titulo,artista,pais,compania,precio,anio from dvd";
            }
            else
            {
                orden = "select codigo,titulo,artista,pais,compania,precio,anio from dvd where codigo=" + codigo;
            }

            SQLiteCommand cmd = new SQLiteCommand(orden, conexion);
            //cmd.CommandType = System.Data.CommandType.Text; //no hace falta porque ya esta por defecto

            try
            {
                SQLiteDataReader lector = cmd.ExecuteReader();

                while (lector.Read())
                {
                    Dvd undvd = new Dvd();
                    undvd.Codigo = ushort.Parse(lector["codigo"].ToString());
                    undvd.Titulo = lector["titulo"].ToString();
                    undvd.Artista = lector["artista"].ToString();
                    undvd.Pais = lector["pais"].ToString();
                    undvd.Compania = lector["compania"].ToString();
                    if (!DBNull.Value.Equals(lector["precio"]))
                    {
                        undvd.Precio = double.Parse(lector["precio"].ToString());
                    }
                    undvd.Anio = lector["anio"].ToString();

                    //undvd.ItemEdit += new listadodvd.itemediteventhandeler(undvd.itemedit());

                    resultado.Add(undvd);
                }

                lector.Close();
                return resultado;
            }
            catch (SQLiteException)
            {
                throw;
            }
        }

        public Pais SeleccionarPais(string iso2)
        {
            Pais resultado = null;
            string orden;

            if (iso2 != null)
            {
                resultado = new Pais();
                orden = "select nombre from pais where iso2='" + iso2 + "'";

                SQLiteCommand cmd = new SQLiteCommand(orden, conexion);

                try
                {
                    object salida = cmd.ExecuteScalar();
                    if (salida != null)
                    {
                        resultado.Iso2 = iso2;
                        resultado.Nombre = salida.ToString();
                    } 
                }
                catch (SQLiteException)
                {
                    throw;
                }
            }
            return resultado;
        }

        public ObservableCollection<Dvd> SeleccionarPA(string codigo)
        {
            throw new NotImplementedException();//no hacer
        }

        public void Insertar()
        {
            throw new NotImplementedException();
        }

        public int Actualizar(Dvd unDVD)
        {
            string orden;
            if (unDVD != null)
            {
                orden = "update dvd set titulo='" + unDVD.Titulo + "', artista='" + unDVD.Artista + "', pais='" + unDVD.Pais + "', compania='" + unDVD.Compania + "', precio='" + unDVD.Precio.ToString().Replace(",",".") + "', anio='" + unDVD.Anio + "' where codigo=" + unDVD.Codigo;
                SQLiteCommand cmd = new SQLiteCommand(orden, conexion);
                return cmd.ExecuteNonQuery();
            }
            else
            {
                return -1;
            }
        }

        public int Borrar(Dvd unDVD)
        {
            string orden;
            if (unDVD != null)
            {
                orden = "delete from dvd where codigo=" + unDVD.Codigo;
                SQLiteCommand cmd = new SQLiteCommand(orden, conexion);
                return cmd.ExecuteNonQuery();
            }
            else
            {
                return -1;
            }
        }
    }
}

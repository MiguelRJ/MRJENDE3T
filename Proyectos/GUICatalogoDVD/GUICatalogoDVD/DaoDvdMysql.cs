﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
//-------------------------
using System.Collections.ObjectModel;
using System.Data;

namespace CatalogoDVD
{
    class DaoDvdMysql : IDAO
    {
        public MySqlConnection conexion;

        public bool Conectar(string srv, string db, string user, string pwd)
        {
            string cadenaConexion = "server=" + srv + ";" + "database=" + db + ";" + "uid=" + user + ";" + "pwd=" + pwd + ";";
            try
            {
                conexion = new MySqlConnection(cadenaConexion);
                conexion.Open();
                return true;
            }
            catch (MySqlException e)
            {
                switch (e.ErrorCode)
                {
                    case 0:
                        throw new Exception("Error de conexion");
                    case 1045:
                        throw new Exception("Usuario o contraseña incorrectos");
                    default:
                        throw new Exception(e.Message);
                }
            }
            
        }

        public bool Desconectar()
        {
            try
            {
                conexion.Close();
                return true;
            }
            catch (MySqlException)
            {
                throw;
            } 
        }

        public bool Conectado()
        {
            if (conexion != null)
            {
                return conexion.State == System.Data.ConnectionState.Open;
                // Posbile depuracion devuelve si la conexion es igual a una conexion abierta
            }
            else
            {
                return false;
            }
        }

        public DataTable SeleccionarTB(string codigo)
        {
            DataTable dt = new DataTable();
            string orden;

            if (codigo == null)
            {
                orden = "select codigo,titulo,artista,pais,compania,precio,anio from dvd";
            }
            else
            {
                orden = "select codigo,titulo,artista,pais,compania,precio,anio from dvd where codigo='" + codigo + "'";
            }

            MySqlCommand cmd = new MySqlCommand(orden, conexion);

            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
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
                orden = "select codigo,titulo,artista,pais,compania,precio,anio from dvd where codigo='" + codigo + "'";
            }

            MySqlCommand cmd = new MySqlCommand(orden, conexion);
            //cmd.CommandType = System.Data.CommandType.Text; no hace falta porque ya esta por defecto

            try
            {
                MySqlDataReader lector = cmd.ExecuteReader();

                while (lector.Read())
                {
                    Dvd undvd = new Dvd();
                    undvd.Codigo = ushort.Parse(lector["codigo"].ToString());
                    undvd.Titulo = lector["titulo"].ToString();
                    undvd.Artista = lector["artista"].ToString();
                    undvd.Pais = lector["pais"].ToString();
                    undvd.Compania = lector["compania"].ToString();
                    undvd.Precio = double.Parse(lector["precio"].ToString());
                    undvd.Anio = lector["anio"].ToString();

                    resultado.Add(undvd);
                }

                lector.Close();
                return resultado;
            }
            catch (MySqlException)
            {
                throw;
            }
        }

        public ObservableCollection<Dvd> SeleccionarPA(string codigo)
        {
            ObservableCollection<Dvd> resultado = new ObservableCollection<Dvd>();
            int resul;

            MySqlCommand cmd = new MySqlCommand("selectDVD", conexion);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            // Armado de parametros del procedure selectDVD
            cmd.Parameters.AddWithValue("@codi", codigo);
            cmd.Parameters.AddWithValue("@titu", null);
            cmd.Parameters.AddWithValue("@arti", null);
            cmd.Parameters.AddWithValue("@elPais", null);
            cmd.Parameters.AddWithValue("@comp", null);
            cmd.Parameters.AddWithValue("@prec", null);
            cmd.Parameters.AddWithValue("@elAnio", null);
            cmd.Parameters.AddWithValue("@resul", MySqlDbType.Int16);
            cmd.Parameters["@resul"].Direction = System.Data.ParameterDirection.Output;

            try
            {
                MySqlDataReader lector = cmd.ExecuteReader();
            
                resul = (int)cmd.Parameters["@resul"].Value;

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
                
                    resultado.Add(undvd); 
	            }

                lector.Close();
                return resultado;
            }
            catch (MySqlException)
            {
                throw;
            }
        }

        public void Insertar()
        {
        }

        public int Actualizar(Dvd unDVD)
        {
            string orden;
            if (unDVD != null)
            {
                orden = "update dvd set titulo='" + unDVD.Titulo + "', artista='" + unDVD.Artista + "', pais='" + unDVD.Pais + "', compania='" + unDVD.Compania + "', precio='" + unDVD.Precio + "', anio='" + unDVD.Anio + "' where codigo=" + unDVD.Codigo;
                MySqlCommand cmd = new MySqlCommand(orden, conexion);
                return cmd.ExecuteNonQuery();
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Borrado de filas de la tabla dvd
        /// </summary>
        /// <param name="codigo">Clave a eliminar</param>
        /// <returns>n = numero de fials afectadas, -1 parametro dado es null</returns>
        public int Borrar(Dvd unDVD)
        {
            string orden;
            if (unDVD != null)
            {
                orden = "delete from dvd where codigo=" + unDVD.Codigo;
                MySqlCommand cmd = new MySqlCommand(orden, conexion);
                return cmd.ExecuteNonQuery();
            }
            else
            {
                return -1;
            }
        }

        public void BorrarPA()
        {
        }


        public Pais SeleccionarPais(string iso2)
        {
            Pais resultado = null;
            string orden;

            if (iso2 != null)
            {
                resultado = new Pais();
                orden = "select nombre from pais where iso2='" + iso2 + "'";

                MySqlCommand cmd = new MySqlCommand(orden, conexion);

                try
                {
                    object salida = cmd.ExecuteScalar();
                    if (salida != null)
                    {
                        resultado.Iso2 = iso2;
                        resultado.Nombre = salida.ToString();
                    }
                }
                catch (MySqlException)
                {
                    throw;
                }
            }
            return resultado;
        }
    }
}

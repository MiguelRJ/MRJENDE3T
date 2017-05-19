using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//-------------------------
using System.Collections.ObjectModel;
using System.Data;

namespace CatalogoDVD
{
    class UI
    {
        static DaoDvdMysql dao;
        static string host = "localhost";
        static string db = "catalogo";
        static string usr = "usr_catalogo";
        static string pwd = "123";
        static ObservableCollection<Dvd> listado;
        static DataTable listadoTB;

        public UI()
        {
            dao = new DaoDvdMysql();
            PedirOpcion();
        }

        /// <summary>
        /// Opciones de menu principal
        /// </summary>
        static void Menu()
        {
            Console.WriteLine("CATALOGO DE DVDs - Menu de opciones");
            Console.WriteLine("===================================");
            Console.WriteLine("(0) Conectar con la Base de Datos");
            Console.WriteLine("(1) Desconectar con la Base de Datos");
            Console.WriteLine("(2) Listar dvd por codigo [PA]");
            Console.WriteLine("(3) Listar dvd por codigo [SQL Directo]");
            Console.WriteLine("(4) Listar dvd por codigo [DataAdapter]");
            Console.WriteLine("(5) Borrar dvd por codigo ");
            Console.WriteLine("(9) Estas conectado con la Base de Datos??");
            Console.WriteLine("(Q) Fin del programa");
            Console.WriteLine("");
            Console.Write("Opcion? ");
        }

        /// <summary>
        /// Recorrido de la llamada a un selecet de los registros solicitados
        /// </summary>
        static void MostrarListado()
        {
            if (dao.Conectado())
            {
                foreach (Dvd unDvd in listado)
                {
                    Console.WriteLine(unDvd.ToString());
                }
            }
            else
            {
                Console.WriteLine("No hay conexion valida");
            }
        }

        /// <summary>
        /// Solicitud de opcion por consola
        /// </summary>
        static void PedirOpcion()
        {
            ConsoleKeyInfo opcion;
            do
            {
                Console.WriteLine();
                Menu();
                opcion = Console.ReadKey();
                Console.Clear();
                try
                {
                    switch ((char)opcion.KeyChar)
                    {
                        case '0': // Conexion a la base de datos
                            if (!dao.Conectado())
                            {
                                if (dao.Conectar(host, db, usr, pwd))
                                {
                                    Console.WriteLine("Conexion existosa a la base de datos {0}", db);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Ya hay conexion establecida");
                            }
                            break;

                        case '1': // Desconexion a la base de datos
                            if (dao.Conectado())
                            {
                                if (dao.Desconectar())
                                {
                                    Console.WriteLine("Desconexion correcta, Hasta la vista!");
                                }
                            }
                            else
                            {
                                Console.WriteLine("No hay conexion establecida");
                            }
                            break;

                        case '2': // Seleccionar de dvd a traves de Procedimiento almacenado
                            listado = new ObservableCollection<Dvd>();
                            listado = dao.SeleccionarPA(null);
                            MostrarListado();
                            break;

                        case '3': // Seleccionar de dvd a traves de SQL Directo
                            listado = new ObservableCollection<Dvd>();
                            listado = dao.Seleccionar(null);
                            MostrarListado();
                            break;

                        case '4': // Seleccionar de dvd a traves de DataAdapter
                            listadoTB = new DataTable();
                            listadoTB = dao.SeleccionarTB(null);
                            //MostrarListadoTB();
                            break;

                        case '5': // Borrar de dvd 
                            Console.WriteLine("Codigo a borrar?");
                            string codigo;
                            codigo = Console.ReadLine();
                            Console.WriteLine(dao.Borrar(codigo) + " filas borradas");
                            break;

                        case '9': // Preguntar si estas conectado
                            if (dao.Conectado())
                            {
                                Console.WriteLine("Si hay conexion establecida");
                            }
                            else
                            {
                                Console.WriteLine("No hay conexion establecida");
                            }
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Ha ocurrido un error: " + e.Message);
                }
                
            } while (opcion.Key != ConsoleKey.Q);
        }
    }
}

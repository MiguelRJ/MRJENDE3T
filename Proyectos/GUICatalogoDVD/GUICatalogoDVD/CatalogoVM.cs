using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//-----------------------------
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GUICatalogoDVD;

namespace CatalogoDVD
{
    class CatalogoVM : INotifyPropertyChanged
    {
        #region campos
        IDAO _dao;
        bool _tipoConexion = true;//Mysql: true, SQlite: false
        ObservableCollection<Dvd> _listado;
        string _mensaje = "<Sin datos>";
        string nombrePais = string.Empty;
        Dvd _dvd;

        
        #endregion

        #region propiedades
        public Dvd DVDSeleccionado
        {
            get { return _dvd; }
            set
            {
                if (_dvd != value)
                {
                    _dvd = value;
                    if (_dao.Conectado() && _dvd != null)
                    {
                        NombrePais = _dao.SeleccionarPais(_dvd.Pais).Nombre;
                    }
                    else
                    {
                        NombrePais = "<sin identificar>";
                    }
                    NotificarCambioPropiedad("DVDSeleccionadoNoNulo");
                }
            }
        }
        public bool DVDSeleccionadoNoNulo
        {
            get { return DVDSeleccionado != null; }
        }
        public bool TipoConexion
        {
            get { return _tipoConexion; }
            set 
            {
                if (_tipoConexion != value)
                {
                    _tipoConexion = value;
                    NotificarCambioPropiedad("TipoConexion");
                }
            }
        }

        public ObservableCollection<Dvd> Listado
        {
            get { return _listado; }
            set 
            {
                if (_listado != value)
                {
                    _listado = value;
                    NotificarCambioPropiedad("Listado");
                }
            }
        }
        
        public string Mensaje
        {
            get { return _mensaje; }
            set 
            {
                if (_mensaje != value)
                {
                    _mensaje = value;
                    NotificarCambioPropiedad("Mensaje");
                }
            }
        }
        public bool Conectado
        {
            get
            {
                if (_dao == null)
                {
                    return false;
                }
                else
                {
                    return _dao.Conectado();
                }
            }
        }
        public string ColorConectar
        {
            get
            {
                if (Conectado)
                {
                    return "green";
                }
                else
                {
                    return "red";
                }
            }
            //set
            //{
            //    NotificarCambioPropiedad("ColorConectar");
            //}
        }
        public string NombrePais
        {
            get { return nombrePais; }
            set
            {
                if (nombrePais != value)
                {
                    nombrePais = value;
                    NotificarCambioPropiedad("NombrePais");
                }

            }
        }
        #endregion

        #region eventos
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotificarCambioPropiedad(string propiedad)
        {
            PropertyChangedEventHandler manejador = PropertyChanged;
            if (manejador != null)
            {
                manejador(this, new PropertyChangedEventArgs(propiedad));
            }
        }
        #endregion

        #region comandos
        private void ConectarBD()
        {
            try
            {
                _dao = null;
                if (TipoConexion) //MySQL
                {
                    _dao = new DaoDvdMysql();
                    _dao.Conectar("localhost", "catalogo", "usr_catalogo", "123");
                    Mensaje = "Conectado a MySQL / MariaDB ";
                }
                else // SQLite
                {
                    _dao = new DaoDvdSQLite();
                    _dao.Conectar(null, "catalogo.db", null, null);
                    Mensaje = "Conectado a SQLite ";
                }
            }
            catch (Exception e)
            {
                Mensaje = e.Message;
            }
            //ColorConectar = "green";
            NotificarCambioPropiedad("ColorConectar");
            NotificarCambioPropiedad("Conectado");
        }
        private void DesconectarBD()
        {
            _dao.Desconectar();
            Mensaje = "Desconexion de la BD con exito.";
            Listado = null;
            NotificarCambioPropiedad("ColorConectar");
            NotificarCambioPropiedad("Conectado");
        }
        private void LeerTodosDVD()
        {
            if (TipoConexion)
            { // MySQL
                Listado = _dao.SeleccionarPA(null);
            }
            else
            { // SQLite
                Listado = _dao.Seleccionar(null);
            }
            Mensaje = "Datos cargados.";
        }
        private void BorrarDVD()
        {
            if (DVDSeleccionado != null)
            {
                if (_dao.Borrar(DVDSeleccionado) == 1)
                {
                    Mensaje = "DVD eliminado correctamente.";
                }
                else
                {
                    Mensaje = "Error al intentar eliminar el dvd.";
                }
            }
        }
        private void ActualizarDVD()
        {
            if (DVDSeleccionado != null)
            {
                if (_dao.Actualizar(DVDSeleccionado) == 1)
                {
                    Mensaje = "DVD actualizado correctamente.";
                }
                else
                {
                    Mensaje = "Error al intentar actualizar el dvd.";
                }
            }
        }
        public ICommand ConectarBDClick// Esto no es un metodo, es la instancia del ICommand
        {
            get
            {
                return new RelayCommand(o => ConectarBD(), o => true);
            }
        }
        public ICommand DesonectarBDClick// Esto no es un metodo, es la instancia del ICommand
        {
            get
            {
                return new RelayCommand(o => DesconectarBD(), o => true);
            }
        }
        public ICommand ListadoClick// Esto no es un metodo, es la instancia del ICommand
        {
            get
            {
                return new RelayCommand(o => LeerTodosDVD(), o => true);
            }
        }
        public ICommand BorrarClick// Esto no es un metodo, es la instancia del ICommand
        {
            get
            {
                return new RelayCommand(o => BorrarDVD(), o => true);
            }
        }
        public ICommand ActualizarClick// Esto no es un metodo, es la instancia del ICommand
        {
            get
            {
                return new RelayCommand(o => ActualizarDVD(), o => true);
            }
        }
        #endregion
    }
}

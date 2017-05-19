//-----------
using System.Data;
using System.Collections.ObjectModel;

namespace CatalogoDVD
{
    interface IDAO
    {
        bool Conectar(string srv, string db, string user, string pwd);
        bool Desconectar();
        bool Conectado();
        DataTable SeleccionarTB(string codigo);
        ObservableCollection<Dvd> Seleccionar(string codigo);
        ObservableCollection<Dvd> SeleccionarPA(string codigo);
        void Insertar();
        int Borrar(Dvd unDVD);
        Pais SeleccionarPais(string iso2);
        int Actualizar(Dvd unDVD);
    }
}

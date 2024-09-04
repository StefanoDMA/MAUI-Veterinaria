using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEndHealthPets.Modelos
{
    public class UsuarioViewModel : INotifyPropertyChanged
    {
        private string nombre;
        private string apellido;
        private string correoElectronico;
        private string password;

        public string Nombre
        {
            get => nombre;
            set
            {
                nombre = value;
                OnPropertyChanged(nameof(Nombre));
            }
        }

        public string Apellido
        {
            get => apellido;
            set
            {
                apellido = value;
                OnPropertyChanged(nameof(Apellido));
            }
        }

        public string CorreoElectronico
        {
            get => correoElectronico;
            set
            {
                correoElectronico = value;
                OnPropertyChanged(nameof(CorreoElectronico));
            }
        }

        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

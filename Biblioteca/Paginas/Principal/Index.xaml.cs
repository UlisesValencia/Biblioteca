using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Biblioteca.Paginas.Principal
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Index : ContentPage
	{
		public Index ()
		{
			InitializeComponent ();
		}

        private void Button_Usuario_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Biblioteca.Paginas.Usuarios.Usuario());

        }

        private void Button_Libro_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Biblioteca.Paginas.Libros.Libro());
        }
    }
}
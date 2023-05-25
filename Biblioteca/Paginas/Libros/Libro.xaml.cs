using Biblioteca.Paginas.Usuarios;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Biblioteca.Paginas.Libros
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Libro : ContentPage
	{
        public String urlUsuario = "https://backend-biblioteca.vercel.app/usuarios";
        public String urlLibros = "https://backend-biblioteca.vercel.app/libros";
        public Libro ()
		{
			InitializeComponent ();
            LoadUsuarios();
            LoadLibros();

        }

        private async void LoadUsuarios()
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(urlUsuario);
            Console.WriteLine(response);

            var usuarios = JsonConvert.DeserializeObject<List<UsuarioItem>>(response);
            usuariosListView.ItemsSource = usuarios;
        }

        private void OnItemSelected(object sender, ItemTappedEventArgs e)
        {
            var usuario = (UsuarioItem)e.Item;
        }

        private async void OnGuardarClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tituloEntry.Text) || string.IsNullOrWhiteSpace(autorEntry.Text) || fechaPrestamoDatePicker.Date == null)
            {
                await DisplayAlert("Error", "Por favor, completa todos los campos.", "OK");
                return;
            }

            if (usuariosListView.SelectedItem is UsuarioItem usuarioSeleccionado)
            {
                var httpClient = new HttpClient();

                var libro = new LibroItem
                {
                    titulo = tituloEntry.Text,
                    autor = autorEntry.Text,
                    fecha_prestamo = fechaPrestamoDatePicker.Date.ToString("yyyy-MM-dd")
                };

                var json = JsonConvert.SerializeObject(libro);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var apiUrlWithUserId = urlUsuario + $"/{usuarioSeleccionado.id}/libros";
                var response = await httpClient.PostAsync(apiUrlWithUserId, content);

                if (response.IsSuccessStatusCode)
                {
                    // Limpia los campos de entrada
                    tituloEntry.Text = string.Empty;
                    autorEntry.Text = string.Empty;
                    fechaPrestamoDatePicker.Date = DateTime.Today;
                    LoadLibros();
                    // Muestra un mensaje de éxito
                    await DisplayAlert("Éxito", "El libro se ha creado correctamente.", "OK");
                }
                else
                {
                    // Muestra un mensaje de error
                    await DisplayAlert("Error", "No se pudo crear el libro.", "OK");
                }
            }
            else
            {
                // Muestra un mensaje de error si no se ha seleccionado un usuario
                await DisplayAlert("Error", "Por favor, selecciona un usuario.", "OK");
            }
        }

        private async void LoadLibros()
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(urlLibros);
            Console.WriteLine(response);

            var libros = JsonConvert.DeserializeObject<List<LibroItem>>(response);
            librosListView.ItemsSource = libros;
        }

    }

    public class UsuarioItem
    {
        public int id { get; set; }
        public string nombre { get; set; }
    }

    public class LibroItem
    {
        public string id { get; set; }
        public string titulo { get; set; }
        public string autor { get; set; }
        public string fecha_prestamo { get; set; }
        public int usuario_id { get; set; }
        public string fechaFormateada
        {
            get
            {
                if (DateTime.TryParse(fecha_prestamo, out DateTime fecha))
                {
                    return fecha.ToString("yyyy/MM/dd");
                }
                return fecha_prestamo;
            }
        }
    }
}
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Biblioteca.Paginas.Usuarios
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Usuario : ContentPage
	{
        public String url = "https://backend-biblioteca.vercel.app/usuarios";
        public Usuario ()
		{
			InitializeComponent ();
            LoadUsuarios();
        }

        private async void LoadUsuarios()
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(url);
            Console.WriteLine(response);

            var usuarios = JsonConvert.DeserializeObject<List<UsuarioItem>>(response);
            usuariosListView.ItemsSource = usuarios;
        }

        private void OnItemSelected(object sender, ItemTappedEventArgs e)
        {
            var usuario = (UsuarioItem)e.Item;
        }

        private async void OnCreateClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(idEntry.Text) && !string.IsNullOrEmpty(nombreEntry.Text))
            {
                var httpClient = new HttpClient();

                var usuario = new UsuarioItem
                {
                    id = int.Parse(idEntry.Text),   // Convert the input to int.
                    nombre = nombreEntry.Text
                };

                var json = JsonConvert.SerializeObject(usuario);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    // Clear the input fields.
                    idEntry.Text = string.Empty;
                    nombreEntry.Text = string.Empty;

                    // Reload the list of usuarios.
                    LoadUsuarios();
                }
                else
                {
                    // Show an error message.
                    await DisplayAlert("Error", "No se pudo crear el usuario", "OK");
                }
            }
            else
            {
                // Show a message to enter ID and nombre.
                await DisplayAlert("Advertencia", "Por favor, ingresa un ID y un nombre para crear", "OK");
            }
        }

        private async void OnEditClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(idEntry.Text) && !string.IsNullOrEmpty(nombreEntry.Text))
            {
                var httpClient = new HttpClient();

                var usuario = new UsuarioItem
                {
                    id = int.Parse(idEntry.Text),   // Convert the input to int.
                    nombre = nombreEntry.Text
                };

                var json = JsonConvert.SerializeObject(usuario);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PutAsync(url+$"/{usuario.id}", content);

                if (response.IsSuccessStatusCode)
                {
                    // Clear the input fields.
                    idEntry.Text = string.Empty;
                    nombreEntry.Text = string.Empty;

                    // Reload the list of usuarios.
                    LoadUsuarios();
                }
                else
                {
                    // Show an error message.
                    await DisplayAlert("Error", "No se pudo editar el usuario", "OK");
                }
            }
            else
            {
                // Show a message to enter ID and nombre.
                await DisplayAlert("Advertencia", "Por favor, ingresa un ID y un nombre para editar", "OK");
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(idEntry.Text))
            {
                var id = int.Parse(idEntry.Text);

                var httpClient = new HttpClient();

                var response = await httpClient.DeleteAsync(url+$"/{id}");

                if (response.IsSuccessStatusCode)
                {
                    // Clear the input field.
                    idEntry.Text = string.Empty;

                    // Reload the list of usuarios.
                    LoadUsuarios();
                }
                else
                {
                    // Show an error message.
                    await DisplayAlert("Error", "No se pudo borrar el usuario", "OK");
                }
            }
            else
            {
                // Show a message to enter ID.
                await DisplayAlert("Advertencia", "Por favor, ingresa un ID para borrar", "OK");
            }
        }





    }
    public class UsuarioItem
    {
        public int id { get; set; }
        public string nombre { get; set; }
    }
}
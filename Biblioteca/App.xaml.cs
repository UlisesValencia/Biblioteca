using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Biblioteca
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage (new Biblioteca.Paginas.Principal.Index());

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

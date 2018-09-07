using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;

namespace Industria4.Pages
{
    public partial class CustomNavigationPage : NavigationPage
    {
        public CustomNavigationPage() : base()
        {
            Debug.WriteLine("Construtor navigation base");
            InitializeComponent();
        }

        public CustomNavigationPage(Page root) : base(root)
        {
            Debug.WriteLine("Construtor navigation base with root page");
            InitializeComponent();
        }
    }
}

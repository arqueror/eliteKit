using eliteKit.eliteElements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace eliteKitDevelopment.appPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class eliteCheckBoxTestPage : ContentPage
    {
        public eliteCheckBoxTestPage()
        {
            InitializeComponent();
            checkBox.Checked += (s, a) =>
            {
                var isChecked = true;
            };


            checkBox.Unchecked += (s, a) =>
            {
                var isNotChecked = true;
            };


            checkBox.CheckedCommand = new Command<bool>(isChecked =>
            {


            });
        }
    }
}
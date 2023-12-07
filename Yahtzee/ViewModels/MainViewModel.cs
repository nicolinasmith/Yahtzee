using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Yahtzee.Commands;
using Yahtzee.Views;

namespace Yahtzee.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private static MainViewModel? _instance;

        public BaseViewModel CurrentViewModel { get; set; } = new StartViewModel();

        public static MainViewModel Instance { get => _instance ?? (_instance = new MainViewModel()); }
    }
}

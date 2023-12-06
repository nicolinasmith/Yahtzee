using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Yahtzee.Commands;

namespace Yahtzee.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private static MainViewModel? _instance;

        public BaseViewModel CurrentViewModel { get; set; } = new StartViewModel();

        public static MainViewModel Instance { get => _instance ?? (_instance = new MainViewModel()); }


        public ICommand StartGameCommand { get; set; }

        public string? SinglePlayer { get; set; }

        public string? PlayerOne { get; set; }

        public string? PlayerTwo { get; set; }


        public MainViewModel()
        {
            StartGameCommand = new RelayCommand(x => StartGame());
        }

        private void StartGame()
        {
            //MessageBox.Show($"hej {SinglePlayer}");

            MainViewModel.Instance.CurrentViewModel = new SingleGameViewModel();

        }
    }
}

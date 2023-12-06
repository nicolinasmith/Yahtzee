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

        public ICommand StartGameCommand { get; set; }

        public ICommand ReadRulesCommand { get; set; }

        public string? PlayerOne { get; set; }

        public string? PlayerTwo { get; set; }

        public bool SinglePlayerChecked { get; set; } = true;


        private void OpenRulesPopUp()
        {
            RulesPopUp popupWindow = new RulesPopUp();
            popupWindow.Show();
        }

        public MainViewModel()
        {
            StartGameCommand = new RelayCommand(x => StartGame());
            ReadRulesCommand = new RelayCommand(x => OpenRulesPopUp());
        }

        private void StartGame()
        {
            if (SinglePlayerChecked)
            {
                MainViewModel.Instance.CurrentViewModel = new SingleGameViewModel(PlayerOne);
            }
            else
            {
                MainViewModel.Instance.CurrentViewModel = new MultipleGameViewModel();
            }

        }
    }
}

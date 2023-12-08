using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Yahtzee.Commands;
using Yahtzee.Views;

namespace Yahtzee.ViewModels
{
    public class StartViewModel : BaseViewModel
    {
        public ICommand StartGameCommand { get; set; }

        public ICommand ReadRulesCommand { get; set; }

        public string? SinglePlayer {  get; set; }

        public string? PlayerOne { get; set; }

        public string? PlayerTwo { get; set; }

        public bool SinglePlayerChecked { get; set; } = true;

        public StartViewModel() 
        {
            StartGameCommand = new RelayCommand(x => StartGame());
        }


        private void StartGame()
        {
            if (SinglePlayerChecked)
            {
                MainViewModel.Instance.CurrentViewModel = new SingleGameViewModel(SinglePlayer);
            }
            else
            {
                MainViewModel.Instance.CurrentViewModel = new MultipleGameViewModel(PlayerOne, PlayerTwo);
            }

        }
    }
}

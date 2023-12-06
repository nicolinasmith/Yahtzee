using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Yahtzee.Models;
using Yahtzee.Views;

namespace Yahtzee.ViewModels
{
    class SingleGameViewModel : BaseViewModel
    {
        ScoreSheet scoreSheet = new ScoreSheet();
        //public ObservableCollection<Dice> dices = new ObservableCollection<Dice>();

        public ObservableCollection<Dice> Dices { get; set; }

        public SingleGameViewModel(string playerName) 
        {
            scoreSheet.PlayerName = playerName;
            GetDices();
        }

        private void GetDices()
        {
            Dices = new ObservableCollection<Dice>();

            for (int i = 0; i < 5; i++)
            {
                var dice = new Dice();
                dice.Index = i;
                Dices.Add(dice);
            }
        }
    }
}

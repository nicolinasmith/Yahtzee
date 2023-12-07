using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Yahtzee.Commands;
using Yahtzee.Enums;
using Yahtzee.Models;
using Yahtzee.Views;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection;

namespace Yahtzee.ViewModels
{
    class SingleGameViewModel : BaseViewModel
    {
        public ObservableCollection<Dice> Dices { get; set; }

        public ScoreSheet ScoreSheet { get; set; }

        Random random = new Random();

        int rollDiceCounter = 0;
        int subscoreCount = 0;

        public bool CanRollDice { get; set; } = true;

        public string PlayerName { get; set; }

        public ICommand RollDiceCommand { get; set; }

        public ICommand OnesCommand { get; set; }
        public ICommand TwosCommand { get; set; }
        public ICommand ThreesCommand { get; set; }
        public ICommand FoursCommand { get; set; }
        public ICommand FivesCommand { get; set; }
        public ICommand SixesCommand { get; set; }

        public SingleGameViewModel(string playerName) 
        {
            GetDices();

            PlayerName = playerName;
            ScoreSheet = new ScoreSheet();

            RollDiceCommand = new RelayCommand(x => SetDiceValue());

            OnesCommand = new RelayCommand(x => CalculateOnesUpToSixes(1, "Ones"));
            TwosCommand = new RelayCommand(x => CalculateOnesUpToSixes(2, "Twos"));
            ThreesCommand = new RelayCommand(x => CalculateOnesUpToSixes(3, "Threes"));
            FoursCommand = new RelayCommand(x => CalculateOnesUpToSixes(4, "Fours"));
            FivesCommand = new RelayCommand(x => CalculateOnesUpToSixes(5, "Fives"));
            SixesCommand = new RelayCommand(x => CalculateOnesUpToSixes(6, "Sixes"));
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

        private void SetDiceValue()
        {
            rollDiceCounter++;

            foreach (var dice in Dices)
            {
                if (dice.IsChosen == DiceChosen.False)
                {
                    dice.Value = random.Next(1, 7);
                    dice.DiceStatus = (DiceStatus)dice.Value;
                }
            }

            if (rollDiceCounter == 3)
            {
                CanRollDice = false;
            }
        }

        private void CalculateOnesUpToSixes(int number, string stringNum)
        {
            int points = 0;
            int diceCount = 0;

            foreach (var dice in Dices)
            {
                if (dice.Value == number)
                {
                    diceCount++;
                    points = diceCount * number;
                }
            }

            MessageBoxResult result = MessageBox.Show($"You have {diceCount} of {number}'s ({points} points). Save?", "Yatzy", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                subscoreCount++;
                typeof(ScoreSheet).GetProperty(stringNum)?.SetValue(ScoreSheet, points.ToString());
                //NewRollTries();

                //if (sender is Button button)
                //{
                //    button.IsEnabled = false;
                //}

                if (subscoreCount == 6)
                {
                    CountSubscore();
                }
            }
            else
            {
                return;
            }
        }

        private void CountSubscore()
        {
            int subscore = int.Parse(ScoreSheet.Ones) + int.Parse(ScoreSheet.Twos) + int.Parse(ScoreSheet.Threes) + int.Parse(ScoreSheet.Fours) + int.Parse(ScoreSheet.Fives) + int.Parse(ScoreSheet.Sixes);
            ScoreSheet.Subscore = subscore.ToString();

            if (subscore > 62)
            {
                ScoreSheet.Bonus = 50.ToString();
            }
            else
            {
                ScoreSheet.Bonus = 0.ToString();
            }
        }
    }
}

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

        int rollDiceCount = 0;
        int subscoreClick = 0;
        int totalScoreClick = 0;

        public bool CanRollDice { get; set; } = true;

        public string PlayerName { get; set; }

        public string GuideText { get; set; }

        public string NumberOfTries { get; set; }

        #region ICommands
        public ICommand ToStartCommand { get; set; }

        public ICommand RollDiceCommand { get; set; }

        public ICommand SaveDiceCommand { get; set; }

        public ICommand OnesCommand { get; set; }

        public ICommand TwosCommand { get; set; }

        public ICommand ThreesCommand { get; set; }

        public ICommand FoursCommand { get; set; }

        public ICommand FivesCommand { get; set; }

        public ICommand SixesCommand { get; set; }

        public ICommand OnePairCommand { get; set; }

        public ICommand TwoPairCommand { get; set; }

        public ICommand ThreeOfAKindCommand { get; set; }

        public ICommand FourOfAKindCommand { get; set; }

        public ICommand FullHouseCommand { get; set; }

        public ICommand SmallStraightCommand { get; set; }

        public ICommand LargeStraightCommand { get; set; }

        public ICommand YatzyCommand { get; set; }

        public ICommand ChanceCommand { get; set; }
        #endregion

        public SingleGameViewModel(string playerName) 
        {
            GetDices();

            PlayerName = playerName;
            ScoreSheet = new ScoreSheet();

            GuideText = $"Time to roll the dices, {PlayerName}!";
            NumberOfTries = "0 of 3";
            RollDiceCommand = new RelayCommand(x => SetDiceValue());

            OnesCommand = new RelayCommand(x => CalculateOnesUpToSixes(1, "Ones"));
            TwosCommand = new RelayCommand(x => CalculateOnesUpToSixes(2, "Twos"));
            ThreesCommand = new RelayCommand(x => CalculateOnesUpToSixes(3, "Threes"));
            FoursCommand = new RelayCommand(x => CalculateOnesUpToSixes(4, "Fours"));
            FivesCommand = new RelayCommand(x => CalculateOnesUpToSixes(5, "Fives"));
            SixesCommand = new RelayCommand(x => CalculateOnesUpToSixes(6, "Sixes"));
            OnePairCommand = new RelayCommand(x => CalculateOnePair());
            TwoPairCommand = new RelayCommand(x => CalculateTwoPair());
            ThreeOfAKindCommand = new RelayCommand(x => CalculateThreeOfAKind());
            FourOfAKindCommand = new RelayCommand(x => CalculateFourOfAKind());
            FullHouseCommand = new RelayCommand(x => CalculateFullHouse());
            SmallStraightCommand = new RelayCommand(x => CalculateSmallStraight());
            LargeStraightCommand = new RelayCommand(x => CalculateLargeStraight());
            YatzyCommand = new RelayCommand(x => CalculateYatzy());
            ChanceCommand = new RelayCommand(x => CalculateChance());

            SaveDiceCommand = new RelayCommand(HandlesChosenDice);
            ToStartCommand = new RelayCommand(x => ReturnToStartPage());

        }

        private void ReturnToStartPage()
        {
            MainViewModel.Instance.CurrentViewModel = new StartViewModel();
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
            rollDiceCount++;

            foreach (var dice in Dices)
            {
                if (dice.IsChosen == DiceChosen.False)
                {
                    dice.Value = random.Next(1, 7);
                    dice.DiceStatus = (DiceStatus)dice.Value;
                }
            }

            switch(rollDiceCount)
            {
                case 1:
                    GuideText = "";
                    NumberOfTries = "1 of 3";
                    break;
                case 2:
                    GuideText = "";
                    NumberOfTries = "2 of 3";
                    break;
                case 3:
                    GuideText = $"Time to fill out the scoresheet!";
                    NumberOfTries = "3 of 3";
                    CanRollDice = false;
                    break;

            }
        }

        private void HandlesChosenDice(object index)
        {
            int diceIndex = (int)index;

            if (Dices[diceIndex].IsChosen == DiceChosen.True)
            {
                Dices[diceIndex].IsChosen = DiceChosen.False;
            }
            else
            {
                Dices[diceIndex].IsChosen = DiceChosen.True;
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

            switch(result) { 
                case MessageBoxResult.Yes:
                    subscoreClick++;
                    EnableRollDice();
                    typeof(ScoreSheet).GetProperty(stringNum)?.SetValue(ScoreSheet, points.ToString());

                    if (subscoreClick == 6)
                    {
                        CalculateSubscore();
                        CalculateTotalScore();
                    }
                    break;
                case MessageBoxResult.No: return;
            }
        }

        private void CalculateSubscore()
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

        private void CalculateOnePair()
        {
            int pairOne = 0;
            int pairTwo = 0;

            for (int i = 0; i < 5; i++)
            {
                for (int j = i + 1; j < 5; j++)
                {
                    if (Dices[i].Value == Dices[j].Value)
                    {
                        if (pairOne == 0)
                        {
                            pairOne = Dices[i].Value;
                        }
                        else if (pairTwo == 0 && Dices[i].Value != pairOne)
                        {
                            pairTwo = Dices[i].Value;
                        }
                    }
                }
            }

            if (pairOne != 0 && pairTwo != 0)
            {
                int sumPairOne = 2 * pairOne;
                int sumPairTwo = 2 * pairTwo;
                int highestPair = Math.Max(pairOne, pairTwo);
                int highestSum = Math.Max(sumPairOne, sumPairTwo);

                MessageBoxResult result = MessageBox.Show($"You have two of 'one pair', but get the highest sum by pair of {highestPair}'s ({highestSum} points). Save?", "Yatzy", MessageBoxButton.YesNo, MessageBoxImage.Question);

                switch(result)
                {
                    case MessageBoxResult.Yes:
                        ScoreSheet.OnePair = highestSum.ToString();
                        totalScoreClick++;
                        CalculateTotalScore();
                        break;
                    case MessageBoxResult.No: return;
                }
            }
            else if (pairOne != 0)
            {
                int sumPair = 2 * pairOne;
                MessageBoxResult result = MessageBox.Show($"You have one pair of {pairOne}'s ({sumPair} points). Save?", "Yatzy", MessageBoxButton.YesNo, MessageBoxImage.Question);

                switch(result)
                {
                    case MessageBoxResult.Yes:
                        ScoreSheet.OnePair = sumPair.ToString();
                        totalScoreClick++;
                        CalculateTotalScore();
                        break;
                    case MessageBoxResult.No: return;
                }
            }
            else
            {
                MessageBoxResult result = MessageBox.Show($"You don't have one pair. Do you still want to save your score here and recieve 0 points?", "Three of a kind", MessageBoxButton.YesNo, MessageBoxImage.Question);

                switch(result)
                {
                    case MessageBoxResult.Yes:
                        ScoreSheet.OnePair = 0.ToString();
                        totalScoreClick++;
                        CalculateTotalScore();
                        break;
                    case MessageBoxResult.No: return;
                }
            }
        }

        private void CalculateTwoPair()
        {
            int pairOne = 0;
            int pairTwo = 0;

            for (int i = 0; i < 5; i++)
            {
                for (int j = i + 1; j < 5; j++)
                {
                    if (Dices[i].Value == Dices[j].Value)
                    {
                        if (pairOne == 0)
                        {
                            pairOne = Dices[i].Value;
                        }
                        else if (pairTwo == 0 && Dices[i].Value != pairOne)
                        {
                            pairTwo = Dices[i].Value;
                        }
                    }
                }
            }

            if (pairOne != 0 && pairTwo != 0)
            {
                int sumPairOne = 2 * pairOne;
                int sumPairTwo = 2 * pairTwo;
                int totalSum = sumPairOne + sumPairTwo;

                MessageBoxResult result = MessageBox.Show($"You have two pairs of {pairOne}'s and {pairTwo}'s ({totalSum} points). Save?", "Yatzy", MessageBoxButton.YesNo, MessageBoxImage.Question);

                switch(result)
                {
                    case MessageBoxResult.Yes:
                        ScoreSheet.TwoPairs = totalSum.ToString();
                        totalScoreClick++;
                        CalculateTotalScore();
                        break;
                    case MessageBoxResult.No: return;
                }
            }
            else
            {
                MessageBoxResult result = MessageBox.Show($"You don't have two pairs. Do you still want to save your score here and recieve 0 points?", "Three of a kind", MessageBoxButton.YesNo, MessageBoxImage.Question);

                switch(result)
                {
                    case MessageBoxResult.Yes:
                        ScoreSheet.TwoPairs = 0.ToString();
                        totalScoreClick++;
                        CalculateTotalScore();
                        break;
                    case MessageBoxResult.No: return;
                }
            }
        }

        private void CalculateThreeOfAKind()
        {
            int diceValue = 0;
            int points = 0;
            bool isThreeOfAKind = false;

            for (int i = 0; i < 5; i++)
            {
                for (int j = i + 1; j < 5; j++)
                {
                    for (int k = j + 1; k < 5; k++)
                    {
                        if (Dices[i].Value == Dices[j].Value && Dices[j].Value == Dices[k].Value)
                        {
                            diceValue = Dices[i].Value;
                            points = diceValue * 3;
                            isThreeOfAKind = true;
                        }
                    }
                }
            }

            if (isThreeOfAKind)
            {
                MessageBoxResult result = MessageBox.Show($"You have three of a kind of {diceValue}'s ({points} points). Save?", "Yatzy", MessageBoxButton.YesNo, MessageBoxImage.Question);

                switch(result)
                {
                    case MessageBoxResult.Yes:
                        ScoreSheet.ThreeOfAKind = points.ToString();
                        totalScoreClick++;
                        CalculateTotalScore();
                        break;
                    case MessageBoxResult.No: return;

                }
            }
            else
            {
                MessageBoxResult result = MessageBox.Show($"You don't have three of a kind. Do you still want to save your score here and recieve 0 points?", "Three of a kind", MessageBoxButton.YesNo, MessageBoxImage.Question);

                switch(result)
                {
                    case MessageBoxResult.Yes:
                        ScoreSheet.ThreeOfAKind = 0.ToString();
                        totalScoreClick++;
                        CalculateTotalScore();
                        break;
                    case MessageBoxResult.No: return;
                }
            }
        }

        private void CalculateFourOfAKind()
        {
            int diceValue = 0;
            int points = 0;
            bool isFourOfAKind = false;

            for (int i = 0; i < 5; i++)
            {
                for (int j = i + 1; j < 5; j++)
                {
                    for (int k = j + 1; k < 5; k++)
                    {
                        for (int l = k + 1; l < 5; l++)
                        {
                            if (Dices[i].Value == Dices[j].Value && Dices[j].Value == Dices[k].Value && Dices[k].Value == Dices[l].Value)
                            {
                                diceValue = Dices[i].Value;
                                points = diceValue * 4;
                                isFourOfAKind = true;
                            }
                        }
                    }
                }
            }

            if (isFourOfAKind)
            {
                MessageBoxResult result = MessageBox.Show($"You have four of a kind of {diceValue}'s ({points} points). Save?", "Yatzy", MessageBoxButton.YesNo, MessageBoxImage.Question);

                switch(result)
                {
                    case MessageBoxResult.Yes:
                        ScoreSheet.FourOfAKind = points.ToString();
                        totalScoreClick++;
                        CalculateTotalScore();
                        break;
                    case MessageBoxResult.No: return;
                }
            }
            else
            {
                MessageBoxResult result = MessageBox.Show($"You don't have four of a kind. Do you still want to save your score here and receive 0 points?", "Four of a kind", MessageBoxButton.YesNo, MessageBoxImage.Question);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        ScoreSheet.FourOfAKind = 0.ToString();
                        totalScoreClick++;
                        CalculateTotalScore();
                        break;
                    case MessageBoxResult.No: return;
                }
            }
        }

        private void CalculateFullHouse()
        {
            int threesValue = 0;
            int pairValue = 0;

            for (int i = 1; i <= 6; i++)
            {
                int count = Dices.Count(d => d.Value == i);

                if (count >= 3)
                {
                    threesValue = i;
                }
                else if (count >= 2 && pairValue == 0)
                {
                    pairValue = i;
                }
            }

            if (threesValue != 0 && pairValue != 0)
            {
                int totalSum = threesValue * 3 + pairValue * 2;

                MessageBoxResult result = MessageBox.Show($"You have a full house with {threesValue}'s and {pairValue}'s ({totalSum} points). Save?", "Yatzy", MessageBoxButton.YesNo, MessageBoxImage.Question);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        ScoreSheet.FullHouse = totalSum.ToString();
                        totalScoreClick++;
                        CalculateTotalScore();
                        break;
                    case MessageBoxResult.No: return;
                }
            }
            else
            {
                MessageBoxResult result = MessageBox.Show($"You don't have a full house. Do you still want to save your score here and recieve 0 points?", "Full House", MessageBoxButton.YesNo, MessageBoxImage.Question);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        ScoreSheet.FullHouse = 0.ToString();
                        totalScoreClick++;
                        CalculateTotalScore();
                        break;
                    case MessageBoxResult.No: return;
                }
            }
        }

        private void CalculateSmallStraight()
        {
            int[] smallStraightArray = { 1, 2, 3, 4, 5 };
            int[] diceArray = new int[5];

            for (int i = 0; i < 5; i++)
            {
                diceArray[i] = Dices[i].Value;
            }

            bool isSmallStraight = smallStraightArray.OrderBy(x => x).SequenceEqual(diceArray.OrderBy(x => x));

            if (isSmallStraight)
            {
                MessageBoxResult result = MessageBox.Show($"You have a small straight which gives you 15 points. Save?", "Yatzy", MessageBoxButton.YesNo, MessageBoxImage.Question);
                
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        ScoreSheet.SmallStraight = 15.ToString();
                        totalScoreClick++;
                        CalculateTotalScore();
                        break;
                    case MessageBoxResult.No: return;
                }
            }
            else
            {
                MessageBoxResult result = MessageBox.Show($"You don't have a small straight. Do you still want to save your score here and recieve 0 points?", "Small straight", MessageBoxButton.YesNo, MessageBoxImage.Question);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        ScoreSheet.SmallStraight = 0.ToString();
                        totalScoreClick++;
                        CalculateTotalScore();
                        break;
                    case MessageBoxResult.No: return;
                }
            }
        }

        private void CalculateLargeStraight()
        {
            int[] largeStraightArray = { 2, 3, 4, 5, 6 };
            int[] diceArray = new int[5];

            for (int i = 0; i < 5; i++)
            {
                diceArray[i] = Dices[i].Value;
            }

            bool isLargeStraight = largeStraightArray.OrderBy(x => x).SequenceEqual(diceArray.OrderBy(x => x));

            if (isLargeStraight)
            {
                MessageBoxResult result = MessageBox.Show($"You have a large straight which gives you 20 points. Save?", "Yatzy", MessageBoxButton.YesNo, MessageBoxImage.Question);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        ScoreSheet.LargeStraight = 20.ToString();
                        totalScoreClick++;
                        CalculateTotalScore();
                        break;
                    case MessageBoxResult.No: return;
                }
            }
            else
            {
                MessageBoxResult result = MessageBox.Show($"You don't have a large straight. Do you still want to save your score here and recieve 0 points?", "Large straight", MessageBoxButton.YesNo, MessageBoxImage.Question);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        ScoreSheet.LargeStraight = 0.ToString();
                        totalScoreClick++;
                        CalculateTotalScore();
                        break;
                    case MessageBoxResult.No: return;
                }
            }

        }

        private void CalculateYatzy()
        {
            int value = Dices[0].Value;
            int count = 0;

            foreach (var dice in Dices)
            {
                if (dice.Value == value)
                {
                    count++;
                }
            }

            if (count == 5)
            {
                MessageBoxResult result = MessageBox.Show($"Congrats, you got yazty of {value}'s which gives you 50 points. Save?", "Yatzy", MessageBoxButton.YesNo, MessageBoxImage.Question);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        ScoreSheet.Yatzy = 50.ToString();
                        totalScoreClick++;
                        CalculateTotalScore();
                        break;
                    case MessageBoxResult.No: return;
                }
            }
            else
            {
                MessageBoxResult result = MessageBox.Show($"You don't have yatzy. Do you still want to save your score here and recieve 0 points?", "Yatzy", MessageBoxButton.YesNo, MessageBoxImage.Question);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        ScoreSheet.Yatzy = 0.ToString();
                        totalScoreClick++;
                        CalculateTotalScore();
                        break;
                    case MessageBoxResult.No: return;
                }
            }
        }

        private void CalculateChance()
        {
            int points = 0;

            foreach (var dice in Dices)
            {
                points += dice.Value;
            }

            MessageBoxResult result = MessageBox.Show($"Total from all the dices gives you {points} points. Save?", "Yatzy", MessageBoxButton.YesNo, MessageBoxImage.Question);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    ScoreSheet.Chance = points.ToString();
                    totalScoreClick++;
                    CalculateTotalScore();
                    break;
                case MessageBoxResult.No: return;
            }
        }

        private void CalculateTotalScore()
        {
            EnableRollDice();

            if (subscoreClick == 6 && totalScoreClick == 9)
            {
                int totalScore = int.Parse(ScoreSheet.Subscore) + int.Parse(ScoreSheet.Bonus) + int.Parse(ScoreSheet.OnePair) + int.Parse(ScoreSheet.TwoPairs) + int.Parse(ScoreSheet.ThreeOfAKind) + int.Parse(ScoreSheet.FourOfAKind) + int.Parse(ScoreSheet.FullHouse) + int.Parse(ScoreSheet.SmallStraight) + int.Parse(ScoreSheet.LargeStraight) + int.Parse(ScoreSheet.Yatzy) + int.Parse(ScoreSheet.Chance);
                ScoreSheet.TotalScore = totalScore.ToString();
            }
            else
            {
                return;
            }
        }

        private void EnableRollDice()
        {
            CanRollDice = true;
            rollDiceCount = 0;

            foreach(var dice in Dices)
            {
                GuideText = $"Time to roll the dices again, {PlayerName}!";
                NumberOfTries = "0 / 3";
                dice.IsChosen = DiceChosen.False;
            }
        }

    }
}

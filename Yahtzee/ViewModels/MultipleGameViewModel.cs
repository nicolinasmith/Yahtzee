using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Yahtzee.Commands;
using Yahtzee.Enums;
using Yahtzee.Models;
using Yahtzee.Views;

namespace Yahtzee.ViewModels
{
    class MultipleGameViewModel : BaseViewModel
    {
        public ObservableCollection<Dice> Dices { get; set; }

        public ScoreSheet P1ScoreSheet { get; set; }

        public ScoreSheet P2ScoreSheet { get; set; }

        Random random = new Random();

        int rollDiceCount = 0;

        int P1SubscoreClick = 0;
        int P1TotalScoreClick = 0;
        int P2SubscoreClick = 0;
        int P2TotalScoreClick = 0;

        public string P1Name { get; set; }

        public string P2Name { get; set; }

        public string GuideText { get; set; }

        public string NumberOfTries { get; set; }

        public string PlayersTurn { get; set; }

        public string P1Brush { get; set; } = "Green";

        public string P2Brush { get; set; } = "Black";

        #region Bools
        public bool PlayerOnesTurn { get; set; } = true;

        public bool PlayerTwosTurn { get; set; } = false;

        public bool CanRollDice { get; set; } = true;

        public bool P1OnesButtonEnabled { get; set; } = true;

        public bool P1TwosButtonEnabled { get; set; } = true;

        public bool P1ThreesButtonEnabled { get; set; } = true;

        public bool P1FoursButtonEnabled { get; set; } = true;

        public bool P1FivesButtonEnabled { get; set; } = true;

        public bool P1SixesButtonEnabled { get; set; } = true;

        public bool P1OnePairButtonEnabled { get; set; } = true;

        public bool P1TwoPairButtonEnabled { get; set; } = true;

        public bool P1ThreeOfAKindButtonEnabled { get; set; } = true;

        public bool P1FourOfAKindButtonEnabled { get; set; } = true;

        public bool P1FullHouseButtonEnabled { get; set; } = true;

        public bool P1SmallStraightButtonEnabled { get; set; } = true;

        public bool P1LargeStraightButtonEnabled { get; set; } = true;

        public bool P1YatzyButtonEnabled { get; set; } = true;

        public bool P1ChanceButtonEnabled { get; set; } = true;

        public bool P2OnesButtonEnabled { get; set; } = true;

        public bool P2TwosButtonEnabled { get; set; } = true;

        public bool P2ThreesButtonEnabled { get; set; } = true;

        public bool P2FoursButtonEnabled { get; set; } = true;

        public bool P2FivesButtonEnabled { get; set; } = true;

        public bool P2SixesButtonEnabled { get; set; } = true;

        public bool P2OnePairButtonEnabled { get; set; } = true;

        public bool P2TwoPairButtonEnabled { get; set; } = true;

        public bool P2ThreeOfAKindButtonEnabled { get; set; } = true;

        public bool P2FourOfAKindButtonEnabled { get; set; } = true;

        public bool P2FullHouseButtonEnabled { get; set; } = true;

        public bool P2SmallStraightButtonEnabled { get; set; } = true;

        public bool P2LargeStraightButtonEnabled { get; set; } = true;

        public bool P2YatzyButtonEnabled { get; set; } = true;

        public bool P2ChanceButtonEnabled { get; set; } = true;

        #endregion

        #region ICommands
        public ICommand ToStartCommand { get; set; }

        public ICommand ReadRulesCommand { get; set; }

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

        public MultipleGameViewModel(string playerOne, string playerTwo) 
        {
            GetDices();

            P1Name = playerOne;
            P2Name = playerTwo;

            P1ScoreSheet = new ScoreSheet();
            P2ScoreSheet = new ScoreSheet();

            GuideText = $"Time to roll the dices, {P1Name}!";
            PlayersTurn = $"Player 1's turn";
            NumberOfTries = "0 of 3";

            RollDiceCommand = new RelayCommand(x => RollTheDices());

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
            ReadRulesCommand = new RelayCommand(x => OpenRulesPopUp());
        }

        private void ReturnToStartPage()
        {
            MainViewModel.Instance.CurrentViewModel = new StartViewModel();
        }

        private void OpenRulesPopUp()
        {
            RulesPopUp popupWindow = new RulesPopUp();
            popupWindow.Show();
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

        private async void RollTheDices()
        {
            var soundPlayer = new SoundPlayer(Properties.Resources.RollDice);
            soundPlayer.Play();
            rollDiceCount++;

            for (int i = 0; i < 5; i++)
            {
                foreach (var dice in Dices)
                {
                    if (dice.IsChosen == DiceChosen.False)
                    {
                        await Task.Delay(80);
                        dice.Value = random.Next(1, 7);
                        dice.DiceStatus = (DiceStatus)dice.Value;
                    }
                }
            }

            switch (rollDiceCount)
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

        private void EnableRollDice()
        {
            CanRollDice = true;
            rollDiceCount = 0;
            string correctPlayer = P1Name;

            switch (PlayerOnesTurn)
            {
                case true:
                    PlayerOnesTurn = false;
                    PlayerTwosTurn = true;
                    PlayersTurn = "Player 2's turn";
                    P2Brush = "Green";
                    P1Brush = "Black";
                    correctPlayer = P2Name;
                    break;
                case false:
                    PlayerOnesTurn = true;
                    PlayerTwosTurn = false;
                    PlayersTurn = "Player 1's turn";
                    P1Brush = "Green";
                    P2Brush = "Black";
                    correctPlayer = P1Name;
                    break;
            }

            foreach (var dice in Dices)
            {
                GuideText = $"Time to roll the dices, {correctPlayer}!";
                NumberOfTries = "0 / 3";
                dice.IsChosen = DiceChosen.False;
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
               
            switch (result)
            {
                case MessageBoxResult.Yes:

                    var correctScoreSheet = P1ScoreSheet;

                    switch (PlayerOnesTurn)
                    {
                        case true:
                            correctScoreSheet = P1ScoreSheet;
                            P1SubscoreClick++;
                            break;
                        case false:
                            correctScoreSheet = P2ScoreSheet;
                            P2SubscoreClick++;
                            break;
                    }

                    typeof(ScoreSheet).GetProperty(stringNum)?.SetValue(correctScoreSheet, points.ToString());
                    GetType().GetProperty(stringNum + "ButtonEnabled")?.SetValue(this, false);
                    if (P1SubscoreClick == 6 || P2SubscoreClick == 6)
                    {
                        CalculateSubscore();
                        CalculateTotalScore();
                    }
                    EnableRollDice();
                    break;

                case MessageBoxResult.No: return;
            }
        }

        private void CalculateSubscore()
        {
            switch(PlayerOnesTurn)
            {
                case true:

                    int P1Subscore = int.Parse(P1ScoreSheet.Ones) + int.Parse(P1ScoreSheet.Twos) + int.Parse(P1ScoreSheet.Threes) + int.Parse(P1ScoreSheet.Fours) + int.Parse(P1ScoreSheet.Fives) + int.Parse(P1ScoreSheet.Sixes);
                    P1ScoreSheet.Subscore = P1Subscore.ToString();

                    if (P1Subscore > 62)
                    {
                        P1ScoreSheet.Bonus = 50.ToString();
                    }
                    else
                    {
                        P1ScoreSheet.Bonus = 0.ToString();
                    }
                    break;
                case false:

                    int P2Subscore = int.Parse(P2ScoreSheet.Ones) + int.Parse(P2ScoreSheet.Twos) + int.Parse(P2ScoreSheet.Threes) + int.Parse(P2ScoreSheet.Fours) + int.Parse(P2ScoreSheet.Fives) + int.Parse(P2ScoreSheet.Sixes);
                    P2ScoreSheet.Subscore = P2Subscore.ToString();

                    if (P2Subscore > 62)
                    {
                        P2ScoreSheet.Bonus = 50.ToString();
                    }
                    else
                    {
                        P2ScoreSheet.Bonus = 0.ToString();
                    }
                    break;
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

                switch (result)
                {
                    case MessageBoxResult.Yes:

                        var correctScoreSheet = P1ScoreSheet;
                        bool correctButtonEnabled = P1OnePairButtonEnabled;

                        switch (PlayerOnesTurn)
                        {
                            case true:
                                correctScoreSheet = P1ScoreSheet;
                                correctButtonEnabled = P1OnePairButtonEnabled;
                                P1TotalScoreClick++;
                                break;
                            case false:
                                correctScoreSheet = P2ScoreSheet;
                                correctButtonEnabled = P2OnePairButtonEnabled;
                                P2TotalScoreClick++;
                                break;
                        }

                        correctScoreSheet.OnePair = highestSum.ToString();
                        correctButtonEnabled = false;
                        CalculateTotalScore();
                        break;

                    case MessageBoxResult.No: return;
                }
            }
            else if (pairOne != 0)
            {
                int sumPair = 2 * pairOne;
                MessageBoxResult result = MessageBox.Show($"You have one pair of {pairOne}'s ({sumPair} points). Save?", "Yatzy", MessageBoxButton.YesNo, MessageBoxImage.Question);

                switch (result)
                {
                    case MessageBoxResult.Yes:

                        var correctScoreSheet = P1ScoreSheet;
                        bool correctButtonEnabled = P1OnePairButtonEnabled;

                        switch (PlayerOnesTurn)
                        {
                            case true:
                                correctScoreSheet = P1ScoreSheet;
                                correctButtonEnabled = P1OnePairButtonEnabled;
                                P1TotalScoreClick++;
                                break;
                            case false:
                                correctScoreSheet = P2ScoreSheet;
                                correctButtonEnabled = P2OnePairButtonEnabled;
                                P2TotalScoreClick++;
                                break;
                        }

                        correctScoreSheet.OnePair = sumPair.ToString();
                        correctButtonEnabled = false;
                        CalculateTotalScore();
                        break;

                    case MessageBoxResult.No: return;
                }
            }
            else
            {
                MessageBoxResult result = MessageBox.Show($"You don't have one pair. Do you still want to save your score here and recieve 0 points?", "Three of a kind", MessageBoxButton.YesNo, MessageBoxImage.Question);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        var correctScoreSheet = P1ScoreSheet;
                        bool correctButtonEnabled = P1OnePairButtonEnabled;

                        switch (PlayerOnesTurn)
                        {
                            case true:
                                correctScoreSheet = P1ScoreSheet;
                                correctButtonEnabled = P1OnePairButtonEnabled;
                                P1TotalScoreClick++;
                                break;
                            case false:
                                correctScoreSheet = P2ScoreSheet;
                                correctButtonEnabled = P2OnePairButtonEnabled;
                                P2TotalScoreClick++;
                                break;
                        }

                        correctScoreSheet.OnePair = 0.ToString();
                        P1TotalScoreClick++;
                        correctButtonEnabled = false;
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

                switch (result)
                {
                    case MessageBoxResult.Yes:

                        var correctScoreSheet = P1ScoreSheet;
                        bool correctButtonEnabled = P1OnePairButtonEnabled;

                        switch (PlayerOnesTurn)
                        {
                            case true:
                                correctScoreSheet = P1ScoreSheet;
                                correctButtonEnabled = P1OnePairButtonEnabled;
                                P1TotalScoreClick++;
                                break;
                            case false:
                                correctScoreSheet = P2ScoreSheet;
                                correctButtonEnabled = P2OnePairButtonEnabled;
                                P2TotalScoreClick++;
                                break;
                        }

                        correctScoreSheet.TwoPairs = totalSum.ToString();
                        correctButtonEnabled = false;
                        CalculateTotalScore();
                        break;
                    case MessageBoxResult.No: return;
                }
            }
            else
            {
                MessageBoxResult result = MessageBox.Show($"You don't have two pairs. Do you still want to save your score here and recieve 0 points?", "Three of a kind", MessageBoxButton.YesNo, MessageBoxImage.Question);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        var correctScoreSheet = P1ScoreSheet;
                        bool correctButtonEnabled = P1OnePairButtonEnabled;

                        switch (PlayerOnesTurn)
                        {
                            case true:
                                correctScoreSheet = P1ScoreSheet;
                                correctButtonEnabled = P1OnePairButtonEnabled;
                                P1TotalScoreClick++;
                                break;
                            case false:
                                correctScoreSheet = P2ScoreSheet;
                                correctButtonEnabled = P2OnePairButtonEnabled;
                                P2TotalScoreClick++;
                                break;
                        }

                        correctScoreSheet.TwoPairs = 0.ToString();
                        P1TotalScoreClick++;
                        correctButtonEnabled = false;
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

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        var correctScoreSheet = P1ScoreSheet;
                        bool correctButtonEnabled = P1OnePairButtonEnabled;

                        switch (PlayerOnesTurn)
                        {
                            case true:
                                correctScoreSheet = P1ScoreSheet;
                                correctButtonEnabled = P1OnePairButtonEnabled;
                                P1TotalScoreClick++;
                                break;
                            case false:
                                correctScoreSheet = P2ScoreSheet;
                                correctButtonEnabled = P2OnePairButtonEnabled;
                                P2TotalScoreClick++;
                                break;
                        }

                        correctScoreSheet.ThreeOfAKind = points.ToString();
                        correctButtonEnabled = false;
                        CalculateTotalScore();
                        break;

                    case MessageBoxResult.No: return;
                }
            }
            else
            {
                MessageBoxResult result = MessageBox.Show($"You don't have three of a kind. Do you still want to save your score here and recieve 0 points?", "Three of a kind", MessageBoxButton.YesNo, MessageBoxImage.Question);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        var correctScoreSheet = P1ScoreSheet;
                        bool correctButtonEnabled = P1OnePairButtonEnabled;

                        switch (PlayerOnesTurn)
                        {
                            case true:
                                correctScoreSheet = P1ScoreSheet;
                                correctButtonEnabled = P1OnePairButtonEnabled;
                                P1TotalScoreClick++;
                                break;
                            case false:
                                correctScoreSheet = P2ScoreSheet;
                                correctButtonEnabled = P2OnePairButtonEnabled;
                                P2TotalScoreClick++;
                                break;
                        }
                        correctScoreSheet.ThreeOfAKind = 0.ToString();
                        correctButtonEnabled = false;
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

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        var correctScoreSheet = P1ScoreSheet;
                        bool correctButtonEnabled = P1OnePairButtonEnabled;

                        switch (PlayerOnesTurn)
                        {
                            case true:
                                correctScoreSheet = P1ScoreSheet;
                                correctButtonEnabled = P1OnePairButtonEnabled;
                                P1TotalScoreClick++;
                                break;
                            case false:
                                correctScoreSheet = P2ScoreSheet;
                                correctButtonEnabled = P2OnePairButtonEnabled;
                                P2TotalScoreClick++;
                                break;
                        }
                        correctScoreSheet.FourOfAKind = points.ToString();
                        P1TotalScoreClick++;
                        P2TotalScoreClick++;
                        correctButtonEnabled = false;
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
                        var correctScoreSheet = P1ScoreSheet;
                        bool correctButtonEnabled = P1OnePairButtonEnabled;

                        switch (PlayerOnesTurn)
                        {
                            case true:
                                correctScoreSheet = P1ScoreSheet;
                                correctButtonEnabled = P1OnePairButtonEnabled;
                                P1TotalScoreClick++;
                                break;
                            case false:
                                correctScoreSheet = P2ScoreSheet;
                                correctButtonEnabled = P2OnePairButtonEnabled;
                                P2TotalScoreClick++;
                                break;
                        }
                        correctScoreSheet.FourOfAKind = 0.ToString();
                        correctButtonEnabled = false;
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
                        var correctScoreSheet = P1ScoreSheet;
                        bool correctButtonEnabled = P1OnePairButtonEnabled;

                        switch (PlayerOnesTurn)
                        {
                            case true:
                                correctScoreSheet = P1ScoreSheet;
                                correctButtonEnabled = P1OnePairButtonEnabled;
                                P1TotalScoreClick++;
                                break;
                            case false:
                                correctScoreSheet = P2ScoreSheet;
                                correctButtonEnabled = P2OnePairButtonEnabled;
                                P2TotalScoreClick++;
                                break;
                        }
                        correctScoreSheet.FullHouse = totalSum.ToString();
                        correctButtonEnabled = false;
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
                        var correctScoreSheet = P1ScoreSheet;
                        bool correctButtonEnabled = P1OnePairButtonEnabled;

                        switch (PlayerOnesTurn)
                        {
                            case true:
                                correctScoreSheet = P1ScoreSheet;
                                correctButtonEnabled = P1OnePairButtonEnabled;
                                P1TotalScoreClick++;
                                break;
                            case false:
                                correctScoreSheet = P2ScoreSheet;
                                correctButtonEnabled = P2OnePairButtonEnabled;
                                P2TotalScoreClick++;
                                break;
                        }
                        correctScoreSheet.FullHouse = 0.ToString();
                        correctButtonEnabled = false;
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
                        var correctScoreSheet = P1ScoreSheet;
                        bool correctButtonEnabled = P1OnePairButtonEnabled;

                        switch (PlayerOnesTurn)
                        {
                            case true:
                                correctScoreSheet = P1ScoreSheet;
                                correctButtonEnabled = P1OnePairButtonEnabled;
                                P1TotalScoreClick++;
                                break;
                            case false:
                                correctScoreSheet = P2ScoreSheet;
                                correctButtonEnabled = P2OnePairButtonEnabled;
                                P2TotalScoreClick++;
                                break;
                        }
                        correctScoreSheet.SmallStraight = 15.ToString();
                        correctButtonEnabled = false;
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
                        var correctScoreSheet = P1ScoreSheet;
                        bool correctButtonEnabled = P1OnePairButtonEnabled;

                        switch (PlayerOnesTurn)
                        {
                            case true:
                                correctScoreSheet = P1ScoreSheet;
                                correctButtonEnabled = P1OnePairButtonEnabled;
                                P1TotalScoreClick++;
                                break;
                            case false:
                                correctScoreSheet = P2ScoreSheet;
                                correctButtonEnabled = P2OnePairButtonEnabled;
                                P2TotalScoreClick++;
                                break;
                        }
                        correctScoreSheet.SmallStraight = 0.ToString();
                        correctButtonEnabled = false;
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
                        var correctScoreSheet = P1ScoreSheet;
                        bool correctButtonEnabled = P1OnePairButtonEnabled;

                        switch (PlayerOnesTurn)
                        {
                            case true:
                                correctScoreSheet = P1ScoreSheet;
                                correctButtonEnabled = P1OnePairButtonEnabled;
                                P1TotalScoreClick++;
                                break;
                            case false:
                                correctScoreSheet = P2ScoreSheet;
                                correctButtonEnabled = P2OnePairButtonEnabled;
                                P2TotalScoreClick++;
                                break;
                        }
                        correctScoreSheet.LargeStraight = 20.ToString();
                        correctButtonEnabled = false;
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
                        var correctScoreSheet = P1ScoreSheet;
                        bool correctButtonEnabled = P1OnePairButtonEnabled;

                        switch (PlayerOnesTurn)
                        {
                            case true:
                                correctScoreSheet = P1ScoreSheet;
                                correctButtonEnabled = P1OnePairButtonEnabled;
                                P1TotalScoreClick++;
                                break;
                            case false:
                                correctScoreSheet = P2ScoreSheet;
                                correctButtonEnabled = P2OnePairButtonEnabled;
                                P2TotalScoreClick++;
                                break;
                        }
                        correctScoreSheet.LargeStraight = 0.ToString();
                        correctButtonEnabled = false;
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
                        var correctScoreSheet = P1ScoreSheet;
                        bool correctButtonEnabled = P1OnePairButtonEnabled;

                        switch (PlayerOnesTurn)
                        {
                            case true:
                                correctScoreSheet = P1ScoreSheet;
                                correctButtonEnabled = P1OnePairButtonEnabled;
                                P1TotalScoreClick++;
                                break;
                            case false:
                                correctScoreSheet = P2ScoreSheet;
                                correctButtonEnabled = P2OnePairButtonEnabled;
                                P2TotalScoreClick++;
                                break;
                        }
                        correctScoreSheet.Yatzy = 50.ToString();
                        correctButtonEnabled = false;
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
                        var correctScoreSheet = P1ScoreSheet;
                        bool correctButtonEnabled = P1OnePairButtonEnabled;

                        switch (PlayerOnesTurn)
                        {
                            case true:
                                correctScoreSheet = P1ScoreSheet;
                                correctButtonEnabled = P1OnePairButtonEnabled;
                                P1TotalScoreClick++;
                                break;
                            case false:
                                correctScoreSheet = P2ScoreSheet;
                                correctButtonEnabled = P2OnePairButtonEnabled;
                                P2TotalScoreClick++;
                                break;
                        }
                        correctScoreSheet.Yatzy = 0.ToString();
                        correctButtonEnabled = false;
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
                    var correctScoreSheet = P1ScoreSheet;
                    bool correctButtonEnabled = P1OnePairButtonEnabled;

                    switch (PlayerOnesTurn)
                    {
                        case true:
                            correctScoreSheet = P1ScoreSheet;
                            correctButtonEnabled = P1OnePairButtonEnabled;
                            P1TotalScoreClick++;
                            break;
                        case false:
                            correctScoreSheet = P2ScoreSheet;
                            correctButtonEnabled = P2OnePairButtonEnabled;
                            P2TotalScoreClick++;
                            break;
                    }
                    correctScoreSheet.Chance = points.ToString();
                    correctButtonEnabled = false;
                    CalculateTotalScore();
                    break;
                case MessageBoxResult.No: return;
            }
        }

        private void CalculateTotalScore()
        {
            EnableRollDice();

            if (P1SubscoreClick == 6 && P1TotalScoreClick == 9)
            {
                int totalScore = int.Parse(P1ScoreSheet.Subscore) + int.Parse(P1ScoreSheet.Bonus) + int.Parse(P1ScoreSheet.OnePair) + int.Parse(P1ScoreSheet.TwoPairs) + int.Parse(P1ScoreSheet.ThreeOfAKind) + int.Parse(P1ScoreSheet.FourOfAKind) + int.Parse(P1ScoreSheet.FullHouse) + int.Parse(P1ScoreSheet.SmallStraight) + int.Parse(P1ScoreSheet.LargeStraight) + int.Parse(P1ScoreSheet.Yatzy) + int.Parse(P1ScoreSheet.Chance);
                P1ScoreSheet.TotalScore = totalScore.ToString();
            }
            else if (P2SubscoreClick == 6 && P2TotalScoreClick == 9)
            {
                int totalScore = int.Parse(P2ScoreSheet.Subscore) + int.Parse(P2ScoreSheet.Bonus) + int.Parse(P2ScoreSheet.OnePair) + int.Parse(P2ScoreSheet.TwoPairs) + int.Parse(P2ScoreSheet.ThreeOfAKind) + int.Parse(P2ScoreSheet.FourOfAKind) + int.Parse(P2ScoreSheet.FullHouse) + int.Parse(P2ScoreSheet.SmallStraight) + int.Parse(P2ScoreSheet.LargeStraight) + int.Parse(P2ScoreSheet.Yatzy) + int.Parse(P2ScoreSheet.Chance);
                P2ScoreSheet.TotalScore = totalScore.ToString();
            }
            else
            {
                return;
            }
        }
    }
}
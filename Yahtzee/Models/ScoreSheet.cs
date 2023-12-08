using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Yahtzee.Models
{
    public class ScoreSheet : INotifyPropertyChanged
    {
        public string? Ones { get; set; }

        public string? Twos { get; set; }

        public string? Threes { get; set; }

        public string? Fours { get; set; }

        public string? Fives { get; set; }

        public string? Sixes { get; set; }

        public string? Subscore { get; set; }

        public string? Bonus { get; set; }

        public string? OnePair { get; set; }

        public string? TwoPairs { get; set; }

        public string? ThreeOfAKind { get; set; }

        public string? FourOfAKind { get; set; }

        public string? FullHouse { get; set; }

        public string? SmallStraight { get; set; }

        public string? LargeStraight { get; set; }

        public string? Yatzy { get; set; }

        public string? Chance { get; set; }

        public string? TotalScore { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

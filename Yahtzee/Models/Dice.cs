using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Yahtzee.Enums;

namespace Yahtzee.Models
{
    public class Dice : DependencyObject, INotifyPropertyChanged
    {
        public int Value { get; set; }

        public int Index { get; set; }

        public DiceChosen IsChosen
        {
            get { return (DiceChosen)GetValue(IsChosenProperty); }
            set { SetValue(IsChosenProperty, value); }
        }

        public static readonly DependencyProperty IsChosenProperty =
            DependencyProperty.Register("DiceChosen", typeof(DiceChosen), typeof(Dice), new PropertyMetadata(DiceChosen.False));


        public DiceStatus DiceStatus
        {
            get { return (DiceStatus)GetValue(DiceStatusProperty); }
            set { SetValue(DiceStatusProperty, value); }
        }

        public static readonly DependencyProperty DiceStatusProperty =
            DependencyProperty.Register("DiceStatus", typeof(DiceStatus), typeof(Dice), new PropertyMetadata(DiceStatus.Three));


        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

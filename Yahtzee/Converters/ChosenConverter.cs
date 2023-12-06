using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Yahtzee.Enums;

namespace Yahtzee.Converters
{
    class ChosenConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isChosen = (DiceChosen)value;

            switch (isChosen)
            {
                case DiceChosen.True:
                    return "Green";
                case DiceChosen.False:
                    return "Black";
                default:
                    throw new NotImplementedException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

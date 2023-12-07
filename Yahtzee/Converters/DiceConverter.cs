using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Yahtzee.Enums;
using System.Diagnostics;
using System.Windows;

namespace Yahtzee.Converters
{
    class DiceConverter : IValueConverter
    {
        private static Dictionary<DiceStatus, Brush> diceBrushCache = new Dictionary<DiceStatus, Brush>();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var diceStatus = (DiceStatus)value;

            switch (diceStatus)
            {
                case DiceStatus.Zero:
                    if (!diceBrushCache.ContainsKey(diceStatus))
                    {
                        var imageBrush = new ImageBrush();
                        var imageUri = new Uri("pack://application:,,,/Yahtzee;component/Images/DiceZero.jpg");
                        imageBrush.ImageSource = new BitmapImage(imageUri);
                        diceBrushCache[diceStatus] = imageBrush;
                    }
                    return diceBrushCache[diceStatus];
                case DiceStatus.One:
                    if (!diceBrushCache.ContainsKey(diceStatus))
                    {
                        var imageBrush = new ImageBrush();
                        var imageUri = new Uri("pack://application:,,,/Yahtzee;component/Images/DiceOne.jpg");
                        imageBrush.ImageSource = new BitmapImage(imageUri);
                        diceBrushCache[diceStatus] = imageBrush;
                    }
                    return diceBrushCache[diceStatus];
                case DiceStatus.Two:
                    if (!diceBrushCache.ContainsKey(diceStatus))
                    {
                        var imageBrush = new ImageBrush();
                        var imageUri = new Uri("pack://application:,,,/Yahtzee;component/Images/DiceTwo.jpg");
                        imageBrush.ImageSource = new BitmapImage(imageUri);
                        diceBrushCache[diceStatus] = imageBrush;
                    }
                    return diceBrushCache[diceStatus];
                case DiceStatus.Three:
                    if (!diceBrushCache.ContainsKey(diceStatus))
                    {
                        var imageBrush = new ImageBrush();
                        var imageUri = new Uri("pack://application:,,,/Yahtzee;component/Images/DiceThree.jpg");
                        imageBrush.ImageSource = new BitmapImage(imageUri);
                        diceBrushCache[diceStatus] = imageBrush;
                    }
                    return diceBrushCache[diceStatus];
                case DiceStatus.Four:
                    if (!diceBrushCache.ContainsKey(diceStatus))
                    {
                        var imageBrush = new ImageBrush();
                        var imageUri = new Uri("pack://application:,,,/Yahtzee;component/Images/DiceFour.jpg");
                        imageBrush.ImageSource = new BitmapImage(imageUri);
                        diceBrushCache[diceStatus] = imageBrush;
                    }
                    return diceBrushCache[diceStatus];
                case DiceStatus.Five:
                    if (!diceBrushCache.ContainsKey(diceStatus))
                    {
                        var imageBrush = new ImageBrush();
                        var imageUri = new Uri("pack://application:,,,/Yahtzee;component/Images/DiceFive.jpg");
                        imageBrush.ImageSource = new BitmapImage(imageUri);
                        diceBrushCache[diceStatus] = imageBrush;
                    }
                    return diceBrushCache[diceStatus];
                case DiceStatus.Six:
                    if (!diceBrushCache.ContainsKey(diceStatus))
                    {
                        var imageBrush = new ImageBrush();
                        var imageUri = new Uri("pack://application:,,,/Yahtzee;component/Images/DiceSix.jpg");
                        imageBrush.ImageSource = new BitmapImage(imageUri);
                        diceBrushCache[diceStatus] = imageBrush;
                    }
                    return diceBrushCache[diceStatus];
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

using Diplom.Common.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace Diplom.Mobile
{
    class ConverterPaymentType : IValueConverter
    {

        Dictionary<object, string> Statuss = new Dictionary<object, string>
        {
            [StatusType.Processing] = "обробатывается",
            [StatusType.Completed] = "приготовлен",
            [StatusType.Rejected] = "отклонен",
            [StatusType.accept] = "принят"
        };
        Dictionary<object, string> Paymentt = new Dictionary<object, string>
        {
            [PaymentType.Cash] = "Наличными",
            [PaymentType.Card] = "Картой"
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
           if(value is PaymentType)
            {
                return (Paymentt[value]);
            }
            else if (value is StatusType)
            {
                return (Statuss[value]);
            }
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

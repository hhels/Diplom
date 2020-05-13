using Diplom.Common.Models;
using System.Windows.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using PropertyChanged;

namespace Diplom.Mobile.ViewModels
{
    // подключенная  PropertyChanged.Fody и файлик FodyWeavers.xml
    [AddINotifyPropertyChangedInterface]
    public class BasketViewModel
    {
        public ObservableCollection<BasketList> BasketList { get; set; }
        public BasketList SelectedBasket { get; set; } 

        public ICommand QuantityPlusCommand { get; set; }
        public ICommand QuantityMinusCommand { get; set; }
        //public ICommand DeketeCommand { get; set; }

        public void deleteBasket(BasketList del)
        {
            using (var db = new ApplicationContext())
            {
                //удалить из локальной БД 
                db.BasketList.Remove(del);
                var basket = db.Basket.FirstOrDefault(x => x.BasketId == del.BasketId);
                //удалить из передаваемой таблицы
                db.Basket.Remove(basket);
                //удалиь из лист вьюш
                BasketList.Remove(del);

                db.SaveChangesAsync();

            }
        }
        public void AddQuantity(BasketList del)
        {
            using (var db = new ApplicationContext())
            {
                db.BasketList.FirstOrDefault(x => x.BasketListId == del.BasketListId).Quantity++; //изменить в БД
                db.Basket.FirstOrDefault(x => x.BasketId == del.BasketId).Quantity++; //изменить в передаваемой таблице
                BasketList.FirstOrDefault(x => x.BasketListId == del.BasketListId).Quantity++; //изменить в лист вьюш
                db.SaveChangesAsync();
                
                BasketList = new ObservableCollection<BasketList>(db.BasketList);

            }
        }
        public BasketViewModel()
        {
            using (var db = new ApplicationContext())
            {
                BasketList = new ObservableCollection<BasketList>(db.BasketList);
            }

            //DeketeCommand = new Command<BasketList>(commandParameter =>
            //{
            //    BasketList.Remove(commandParameter);
            //});

            QuantityPlusCommand = new Command<BasketList>(commandParameter =>
            {
                commandParameter.Quantity++;
                if (commandParameter.Quantity > 10)
                {
                    commandParameter.Quantity = 10;
                }
            }, commandParameter => commandParameter != null);

            QuantityMinusCommand = new Command<BasketList>(commandParameter =>
            {
                commandParameter.Quantity--;
                if (commandParameter.Quantity < 1)
                {
                    commandParameter.Quantity = 1;
                }
            }, commandParameter => commandParameter != null);
        }
    }
}
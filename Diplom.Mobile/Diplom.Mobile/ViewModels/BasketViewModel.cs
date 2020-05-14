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
using Diplom.Mobile.Views;

namespace Diplom.Mobile.ViewModels
{
    // подключенная  PropertyChanged.Fody и файлик FodyWeavers.xml
    [AddINotifyPropertyChangedInterface]
    public class BasketViewModel
    {
        public ObservableCollection<BasketList> BasketList { get; set; }
        public BasketList SelectedBasket { get; set; }
        public string AllPrice { get; set; }

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
                NumberPrice();

                db.SaveChangesAsync();
            }
        }
        public void AddQuantity(BasketList del)
        {
            using (var db = new ApplicationContext())
            {
                var quantity = BasketList.FirstOrDefault(x => x.BasketListId == del.BasketListId).Quantity;
                if (quantity >= 10 )
                {
                    return;
                }
                else
                {
                    db.BasketList.FirstOrDefault(x => x.BasketListId == del.BasketListId).Quantity++; //изменить в БД
                    db.Basket.FirstOrDefault(x => x.BasketId == del.BasketId).Quantity++; //изменить в передаваемой таблице
                    BasketList.FirstOrDefault(x => x.BasketListId == del.BasketListId).Quantity++; //изменить в лист вьюш
                    db.SaveChangesAsync();

                    //посчет цены с учетом изменения количества
                    var Price = Convert.ToInt32(BasketList.FirstOrDefault(x => x.BasketListId == del.BasketListId).Price);//цена товара
                    var Quantity = Convert.ToInt32(BasketList.FirstOrDefault(x => x.BasketListId == del.BasketListId).Quantity);//количество товара
                    BasketList.FirstOrDefault(x => x.BasketListId == del.BasketListId).OverallPrice = Price * Quantity; //изменение цены с учетом кол-ва товара
                    db.BasketList.FirstOrDefault(x => x.BasketListId == del.BasketListId).OverallPrice = Price * Quantity; //изменение цены с учетом кол-ва товара в БД

                    //подсчет общей цены и передача на страницу
                    NumberPrice();


                    BasketList = new ObservableCollection<BasketList>(db.BasketList);
                }
            }
        }
        public void LowerQuantity(BasketList del)
        {
            using (var db = new ApplicationContext())
            {
                var quantity = BasketList.FirstOrDefault(x => x.BasketListId == del.BasketListId).Quantity;
                if (quantity <= 1)
                {
                    return;
                }
                else
                {
                    db.BasketList.FirstOrDefault(x => x.BasketListId == del.BasketListId).Quantity--; //изменить в БД
                    db.Basket.FirstOrDefault(x => x.BasketId == del.BasketId).Quantity--; //изменить в передаваемой таблице
                    BasketList.FirstOrDefault(x => x.BasketListId == del.BasketListId).Quantity--; //изменить в лист вьюш
                    db.SaveChangesAsync();

                    //посчет цены с учетом изменения количества
                    var Price = Convert.ToInt32(BasketList.FirstOrDefault(x => x.BasketListId == del.BasketListId).Price);
                    var Quantity = Convert.ToInt32(BasketList.FirstOrDefault(x => x.BasketListId == del.BasketListId).Quantity);
                    BasketList.FirstOrDefault(x => x.BasketListId == del.BasketListId).OverallPrice = Price * Quantity;
                    db.BasketList.FirstOrDefault(x => x.BasketListId == del.BasketListId).OverallPrice = Price * Quantity;

                    //подсчет общей цены и передача на страницу
                    NumberPrice();


                    BasketList = new ObservableCollection<BasketList>(db.BasketList);
                }

            }
        }
        public void NumberPrice()
        {
            AllPrice = BasketList.Sum(x => x.OverallPrice).ToString();
        }
        
        public BasketViewModel()
        {
            using (var db = new ApplicationContext())
            {
                BasketList = new ObservableCollection<BasketList>(db.BasketList);
                NumberPrice();
            }
            //_basketPage = new BasketPage();
            // var AllPrice = BasketList.Sum(x => x.OverallPrice).ToString();

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
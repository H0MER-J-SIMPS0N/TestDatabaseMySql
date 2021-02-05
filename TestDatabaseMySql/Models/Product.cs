using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace TestDatabaseMySql.Models
{
    public class Product : DomainObject, IDataErrorInfo
    {        
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        private Order _order;
        public Order Order
        {
            get => _order;
            set
            {
                _order = value;
                OnPropertyChanged("Order");
            }
        }
        public int? OrderId { get; set; }
        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged("Quantity");
            }
        }
        private decimal _price;
        public decimal Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged("Price");
            }
        }
        public string Error => throw new NotImplementedException();

        public string this[string propertyName]
        {
            get
            {
                string error = string.Empty;
                switch (propertyName)
                {
                    case "Name":
                        if (string.IsNullOrEmpty(Name))
                        {
                            error = "У продукта должно быть название!";
                        }
                        break;
                    case "Quantity":
                        if (Quantity <= 0)
                        {
                            error = "Количество товара должно быть больше 0!";
                        }
                        break;
                    case "Price":
                        if (Price <= 0)
                        {
                            error = "Цена товара должна быть больше 0!";
                        }
                        break;
                }
                return error;
            }
        }
        public override string ToString()
        {
            return Name ?? string.Empty;
        }
    }
}

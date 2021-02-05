using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace TestDatabaseMySql.Models
{
    public class Order : DomainObject, IDataErrorInfo
    {
        private string _customerName;
        public string CustomerName
        {
            get => _customerName;
            set
            {
                _customerName = value;
                OnPropertyChanged("CustomerName");
            }
        }
        private DateTime _creationDate;
        public DateTime CreationDate
        {
            get => _creationDate;
            set
            {
                _creationDate = value;
                OnPropertyChanged("CreationDate");
            }
        }
        private Employee _creator;
        public Employee Creator
        {
            get => _creator;
            set
            {
                _creator = value;
                OnPropertyChanged("Creator");
            }
        }
        public int? CreatorId { get; set; }

        public List<Product> Products { get; set; }
        public string Error => throw new NotImplementedException();

        public string this[string propertyName]
        {
            get
            {
                string error = string.Empty;
                switch (propertyName)
                {
                    case "CustomerName":
                        if (string.IsNullOrEmpty(CustomerName))
                        {
                            error = "В заказе должно быть название контрагента!";
                        }
                        break;
                }
                return error;
            }
        }

        public override string ToString()
        {
            return $"{Id} {CustomerName}";
        }
    }
}

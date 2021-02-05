using System.Collections.Generic;
using System.ComponentModel;

namespace TestDatabaseMySql.Models
{
    public class Department : DomainObject, IDataErrorInfo
    {
        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }
        private Employee _manager;
        public Employee Manager
        {
            get => _manager;
            set
            {
                _manager = value;
                OnPropertyChanged("Manager");
            }
        }
        public int? ManagerId { get; set; }
        public List<Employee> Managers { get; set; }

        public string Error => throw new System.NotImplementedException();

        public string this[string propertyName]
        {
            get
            {
                string error = string.Empty;
                switch (propertyName)
                {
                    case "Title":
                        if (string.IsNullOrEmpty(Title))
                        {
                            error = "У подразделения должно быть название!";
                        }
                        break;                    
                }
                return error;
            }
        }

        public override string ToString()
        {
            return Title ?? string.Empty;
        }
        
    }
}

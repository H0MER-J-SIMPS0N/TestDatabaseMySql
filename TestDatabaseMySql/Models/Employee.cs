using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace TestDatabaseMySql.Models
{    
    public class Employee : DomainObject, IDataErrorInfo
    {
        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged("LastName");
            }
        }

        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged("FirstName");
            }
        }
        private string _middleName;
        public string MiddleName
        {
            get => _middleName;
            set
            {
                _middleName = value;
                OnPropertyChanged("MiddleName");
            }
        }
        private DateTime _birthDate = DateTime.Now.AddYears(-40);
        public DateTime BirthDate
        {
            get => _birthDate;
            set
            {
                _birthDate = value;
                OnPropertyChanged("BirthDate");
            }
        }
        private Department _department;
        public Department Department
        {
            get => _department;
            set
            {
                _department = value;
                OnPropertyChanged("Department");
            }
        }
        public int? DepartmentId { get; set; }

        public List<Order> Orders { get; set; }
        public List<Department> Departments { get; set; }

        private Gender _gender;
        public Gender Gender
        {
            get => _gender;
            set
            {
                _gender = value;
                OnPropertyChanged("Gender");
            }
        }
        public string Error => throw new System.NotImplementedException();

        public string this[string propertyName]
        {
            get
            {
                string error = string.Empty;
                switch (propertyName)
                {
                    case "FirstName":
                        if (string.IsNullOrEmpty(FirstName))
                        {
                            error = "У сотрудника должно быть имя!";
                        }
                        break;
                }
                return error;
            }
        }

        public override string ToString()
        {
            return $"{LastName ?? string.Empty} {FirstName} {MiddleName ?? string.Empty}".Trim();
        }

    }
}

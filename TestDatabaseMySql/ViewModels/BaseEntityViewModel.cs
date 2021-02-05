using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TestDatabaseMySql.Models;

namespace TestDatabaseMySql.ViewModels
{
    public abstract class BaseEntityViewModel<T> : INotifyPropertyChanged where T : DomainObject
    {
        protected Logger _logger;
        protected DatabaseContext _dbContext;
        public virtual string Name { get; protected set; }
        public string TextToFind { get; set; }
        private string _statusText;
        public string StatusText
        {
            get => _statusText;
            set
            {
                _statusText = value;
                OnPropertyChanged("StatusText");
            }
        }
        private List<T> _foundItems;
        public List<T> FoundItems
        {
            get => _foundItems;
            set
            {
                _foundItems = value;
                OnPropertyChanged("FoundItems");
            }
        }        
        private T _selectedFoundItem;
        public T SelectedFoundItem
        {
            get => _selectedFoundItem;
            set
            {
                _selectedFoundItem = value;
                OnPropertyChanged("SelectedFoundItem");
            }
        }

        private RealizedCommand _findCommand;
        public RealizedCommand FindCommand
        {
            get
            {
                return _findCommand ??= new RealizedCommand(obj =>
                  {
                      Find();
                  });
            }
        }

        private RealizedCommand _deleteCommand;
        public RealizedCommand DeleteCommand
        {
            get
            {
                return _deleteCommand ??= new RealizedCommand(obj =>
                  {
                      Delete();
                  });
            }
        }
        private RealizedCommand _addCommand;
        public RealizedCommand AddCommand
        {
            get
            {
                return _addCommand ??= new RealizedCommand(obj =>
                  {
                      Add();
                  });
            }
        }
        private RealizedCommand _updateCommand;
        public RealizedCommand UpdateCommand
        {
            get
            {
                return _updateCommand ??= new RealizedCommand(obj =>
                  {
                      Update();
                  });
            }
        }
        public virtual void RenewData() { }
        public virtual void Find()
        {
            StatusText = "Поиск";
            _logger.Info(StatusText);
            FindItems();
            StatusText = $"Найдено {FoundItems.Count} записей";
            _logger.Info(StatusText);
        }
        protected virtual void FindItems() { }
        public virtual void Delete() { }
        public virtual void Add() { }
        public virtual void Update() { }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

    }
}

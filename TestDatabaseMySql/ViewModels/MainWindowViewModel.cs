using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using TestDatabaseMySql.Models;

namespace TestDatabaseMySql.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged, IDisposable
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private List<BaseEntityViewModel<DomainObject>> _viewModels;
        private readonly DatabaseContext _dbContext;
        public List<BaseEntityViewModel<DomainObject>> ViewModels
        {
            get => _viewModels;
            set
            {
                _viewModels = value;
                OnPropertyChanged("ViewModels");
            }
        }
        private BaseEntityViewModel<DomainObject> _selectedViewModel;
        public BaseEntityViewModel<DomainObject> SelectedViewModel
        {
            get => _selectedViewModel;
            set
            {
                _selectedViewModel = value;
                OnPropertyChanged("SelectedViewModel");
                _selectedViewModel.StatusText = string.Empty;
                _selectedViewModel.RenewData();
            }
        }

        public MainWindowViewModel()
        {
            _dbContext = new DatabaseContext(_logger);
            ViewModels = new List<BaseEntityViewModel<DomainObject>>()
            {                
                new DepartmentViewModel(_logger, _dbContext),
                new EmployeeViewModel(_logger, _dbContext),
                new OrderViewModel(_logger, _dbContext),
                new ProductViewModel(_logger, _dbContext)
            };
            SelectedViewModel = ViewModels is null || ViewModels.Count == 0 ? null : ViewModels[0];
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}

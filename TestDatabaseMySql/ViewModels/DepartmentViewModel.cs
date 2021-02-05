using Microsoft.EntityFrameworkCore;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDatabaseMySql.Models;

namespace TestDatabaseMySql.ViewModels
{
    public class DepartmentViewModel : BaseEntityViewModel<DomainObject>
    {
        public override string Name { get; protected set; } = "Подразделение";
        
        private List<Employee> _allEmployees;
        public List<Employee> AllEmployees
        {
            get => _allEmployees;
            set
            {
                _allEmployees = value;
                OnPropertyChanged("AllEmployees");
            }
        }

        public DepartmentViewModel(Logger logger, DatabaseContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
            AllEmployees = _dbContext.Set<Employee>().ToList();
        }

        public override void RenewData()
        {
            AllEmployees = _dbContext.Set<Employee>().ToList();
            FoundItems = null;
            SelectedFoundItem = null;
            if (!string.IsNullOrWhiteSpace(TextToFind))
            {
                FindItems();
            }
        }
        protected override void FindItems()
        {
            try
            {
                SelectedFoundItem = null;
                FoundItems = new List<DomainObject>(_dbContext.Set<Department>()
                    .AsQueryable()
                    .Where(d => EF.Functions.Like(d.Id, $"%{TextToFind}%")
                        || EF.Functions.Like(d.Title, $"%{TextToFind}%")
                        || EF.Functions.Like(d.Manager.LastName, $"%{TextToFind}%")
                        || EF.Functions.Like(d.Manager.FirstName, $"%{TextToFind}%")
                        || EF.Functions.Like(d.Manager.MiddleName, $"%{TextToFind}%")
                     )
                 );
            }
            catch (Exception ex)
            {
                StatusText = $"Поиск не удался по причине:{Environment.NewLine}{ex}";
                _logger.Error($"Поиск не удался по причине:{Environment.NewLine}{ex}");
            }
        }
        public override void Delete()
        {
            if (SelectedFoundItem != null && _dbContext.Set<Department>().Where(x => x.Id == SelectedFoundItem.Id).Count() > 0)
            {
                StatusText = $"Удаление '{SelectedFoundItem as Department}'";
                _logger.Info(StatusText);
                new DatabaseWorker(_dbContext, _logger).Delete(SelectedFoundItem as Department, out string statusText);
                StatusText = statusText;
                RenewData();
            }
        }
        public override void Update()
        {
            if (SelectedFoundItem != null)
            {
                StatusText = $"Запись изменений для '{SelectedFoundItem as Department}'";
                _logger.Info(StatusText);
                new DatabaseWorker(_dbContext, _logger).Update(SelectedFoundItem as Department, out string statusText);
                StatusText = statusText;
                RenewData();
            }
            else
            {
                _logger.Info("SelectedFoundItem is null");
            }
        }
        public override void Add()
        {
            RenewData();
            SelectedFoundItem = new Department();
            new DatabaseWorker(_dbContext, _logger).Add(SelectedFoundItem, FoundItems);
            StatusText = "Можно вводить данные";
        }
    }
}

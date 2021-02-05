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
    public class EmployeeViewModel : BaseEntityViewModel<DomainObject>
    {
        public override string Name { get; protected set; } = "Сотрудники";
        private List<Department> _allDepartments;
        public List<Department> AllDepartments
        {
            get => _allDepartments;
            set
            {
                _allDepartments = value;
                OnPropertyChanged("AllDepartments");
            }
        }

        public EmployeeViewModel(Logger logger, DatabaseContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
            AllDepartments = _dbContext.Set<Department>().ToList();
        }

        public override void RenewData()
        {
            AllDepartments = _dbContext.Set<Department>().ToList();
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
                FoundItems = new List<DomainObject>
                    (_dbContext.Set<Employee>()
                        .AsQueryable()
                        .Where(e => EF.Functions.Like(e.Id, $"%{TextToFind}%")
                            || EF.Functions.Like(e.LastName, $"%{TextToFind}%")
                            || EF.Functions.Like(e.FirstName, $"%{TextToFind}%")
                            || EF.Functions.Like(e.MiddleName, $"%{TextToFind}%")
                            || EF.Functions.Like(e.Department.Title, $"%{TextToFind}%")
                        )
                    );
            }
            catch (Exception ex)
            {
                _logger.Error($"Поиск не удался по причине:{Environment.NewLine}{ex}");
            }
        }
        public override void Delete()
        {
            if (SelectedFoundItem != null && _dbContext.Set<Employee>().Where(x => x.Id == SelectedFoundItem.Id).Count() > 0)
            {
                StatusText = $"Удаление '{SelectedFoundItem as Employee}'";
                _logger.Info(StatusText);
                new DatabaseWorker(_dbContext, _logger).Delete(SelectedFoundItem as Employee, out string statusText);
                StatusText = statusText;
                RenewData();
            }
        }
        public override void Update()
        {
            if (SelectedFoundItem != null)
            {
                StatusText = $"Запись изменений для '{SelectedFoundItem as Employee}'";
                _logger.Info(StatusText);
                new DatabaseWorker(_dbContext, _logger).Update(SelectedFoundItem as Employee, out string statusText);
                StatusText = statusText;
                RenewData();
            }
        }
        public override void Add()
        {
            RenewData();
            SelectedFoundItem = new Employee();
            new DatabaseWorker(_dbContext, _logger).Add(SelectedFoundItem, FoundItems);
            StatusText = "Можно вводить данные";
        }
    }
}

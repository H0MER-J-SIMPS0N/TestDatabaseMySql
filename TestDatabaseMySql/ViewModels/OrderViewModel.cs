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
    public class OrderViewModel : BaseEntityViewModel<DomainObject>
    {
        public override string Name { get; protected set; } = "Заказы";
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

        public OrderViewModel(Logger logger, DatabaseContext dbContext)
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
                FoundItems = new List<DomainObject>(_dbContext.Set<Order>()
                    .AsQueryable()
                    .Where(o => EF.Functions.Like(o.Id, $"%{TextToFind}%")
                        || EF.Functions.Like(o.CustomerName, $"%{TextToFind}%")
                        || EF.Functions.Like(o.Creator.FirstName, $"%{TextToFind}%")
                        || EF.Functions.Like(o.Creator.LastName, $"%{TextToFind}%")
                        || EF.Functions.Like(o.Creator.MiddleName, $"%{TextToFind}%")
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
            if (SelectedFoundItem != null && _dbContext.Set<Order>().Where(x => x.Id == SelectedFoundItem.Id).Count() > 0)
            {
                StatusText = $"Удаление '{SelectedFoundItem as Order}'";
                _logger.Info(StatusText);
                new DatabaseWorker(_dbContext, _logger).Delete(SelectedFoundItem as Order, out string statusText);
                StatusText = statusText;
                RenewData();
            }
        }
        public override void Update()
        {
            if (SelectedFoundItem != null)
            {
                StatusText = $"Запись изменений для '{SelectedFoundItem as Order}'";
                _logger.Info(StatusText);
                new DatabaseWorker(_dbContext, _logger).Update(SelectedFoundItem as Order, out string statusText);
                StatusText = statusText;
                RenewData();
            }
        }
        public override void Add()
        {
            RenewData();
            SelectedFoundItem = new Order();
            new DatabaseWorker(_dbContext, _logger).Add(SelectedFoundItem, FoundItems);
            StatusText = "Можно вводить данные";
        }
    }
}

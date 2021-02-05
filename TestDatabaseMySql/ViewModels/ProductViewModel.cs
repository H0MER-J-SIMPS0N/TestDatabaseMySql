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
    public class ProductViewModel : BaseEntityViewModel<DomainObject>
    {
        public override string Name { get; protected set; } = "Товары";
        private List<Order> _allOrders;
        public List<Order> AllOrders
        {
            get => _allOrders;
            set
            {
                _allOrders = value;
                OnPropertyChanged("AllOrders");
            }
        }

        public ProductViewModel(Logger logger, DatabaseContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
            AllOrders = _dbContext.Set<Order>().ToList();
        }

        public override void RenewData()
        {
            AllOrders = _dbContext.Set<Order>().ToList();
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
                FoundItems = new List<DomainObject>(_dbContext.Set<Product>()
                    .AsQueryable()
                    .Where(p => EF.Functions.Like(p.Name, $"%{TextToFind}%")
                        || EF.Functions.Like(p.Id, $"%{TextToFind}%")
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
            if (SelectedFoundItem != null && _dbContext.Set<Product>().Where(x => x.Id == SelectedFoundItem.Id).Count() > 0)
            {
                StatusText = $"Удаление '{SelectedFoundItem as Product}'";
                _logger.Info(StatusText);
                new DatabaseWorker(_dbContext, _logger).Delete(SelectedFoundItem as Product, out string statusText);
                StatusText = statusText;
                RenewData();
            }
        }
        public override void Update()
        {
            if (SelectedFoundItem != null)
            {
                StatusText = $"Запись изменений для '{SelectedFoundItem as Product}'";
                _logger.Info(StatusText);
                new DatabaseWorker(_dbContext, _logger).Update(SelectedFoundItem as Product, out string statusText);
                StatusText = statusText;
                RenewData();
            }
        }
        public override void Add()
        {
            RenewData();
            SelectedFoundItem = new Product();
            new DatabaseWorker(_dbContext, _logger).Add(SelectedFoundItem, FoundItems);
            StatusText = "Можно вводить данные";
        }
    }
}

using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestDatabaseMySql.Models
{
    public class DatabaseWorker
    {
        protected Logger _logger;
        protected DatabaseContext _dbContext;
        public DatabaseWorker(DatabaseContext dbContext, Logger logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public void Delete<T>(T selectedFoundItem, out string statusText) where T : DomainObject
        {            
            try
            {
                _dbContext.Set<T>().Remove(selectedFoundItem);
                _dbContext.SaveChanges();
                statusText = $"Удаление произведено";
                _logger.Info(statusText);
            }
            catch (Exception ex)
            {
                statusText = $"Не получилось удалить запись! {ex.Message}";
                _logger.Error($"Не получилось удалить запись по причине:{Environment.NewLine}{ex}");
            }
        }

        public void Update<T>(T selectedFoundItem, out string statusText) where T : DomainObject
        {
            try
            {
                _dbContext.Set<T>().Update(selectedFoundItem);
                _dbContext.SaveChanges();
                statusText = $"Запись изменений произведена";
                _logger.Info(statusText);
            }
            catch (Exception ex)
            {
                statusText = $"Запись изменений не произведена! {ex.Message}";
                _logger.Error($"Не удалось записать изменения по причине:{Environment.NewLine}{ex}");
            }
        }
        public void Add<T>(T selectedFoundItem, List<DomainObject> foundItems) where T : DomainObject, new()
        {
            if (foundItems is null)
                foundItems = new List<DomainObject>();
            foundItems.Add(selectedFoundItem);
            _logger.Info($"selectedFoundItem - {selectedFoundItem}");
        }
    }
}

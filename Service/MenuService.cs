using DAL;
using Model;
using System.Collections.ObjectModel;

namespace Service
{
    public class MenuService
    {
        private MenuDao menuDao = new();

        public List<MenuCard> GetAllMenuCards()
        {
            return menuDao.GetAllMenuCards();
        }

        public MenuCard GetMenuCardById(int cardId)
        {
            return menuDao.GetMenuCardById(cardId);
        }

        public List<Category> GetAllCategories()
        {
            return menuDao.GetAllCategories();
        }

        public Category GetCategoryById(int id)
        {
            return menuDao.GetCategoryById(id);
        }

        public List<MenuItem> GetAllMenuItems()
        {
            return menuDao.GetAllMenuItems();
        }

        public MenuItem GetMenuItemById(int itemId)
        {
            return menuDao.GetMenuItemById(itemId);
        }

        public void UpdateStock(ObservableCollection<OrderItem> orderItems)
        {
            foreach (OrderItem orderItem in orderItems)
                menuDao.UpdateStock(orderItem.Item.ItemId, (int)(orderItem.Item.StockAmount - orderItem.Quantity));
        }
    }
}

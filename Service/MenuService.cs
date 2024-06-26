using DAL;
using Model;

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

        public void UpdateStockOfMenuItems(List<OrderItem> orderItems)
        {
            Dictionary<int, int> newMenuItemStocks = new(); //First int is the MenuItem database ID, second int is the new stock 
            foreach (OrderItem orderItem in orderItems)
            {
                if (!newMenuItemStocks.ContainsKey(orderItem.Item.ItemId))
                    newMenuItemStocks.Add(orderItem.Item.ItemId, (int)orderItem.Item.StockAmount);

                newMenuItemStocks[orderItem.Item.ItemId] -= (int)orderItem.Quantity;
            }

            foreach (KeyValuePair<int, int> valuePair in newMenuItemStocks)
                menuDao.UpdateStockOfMenuItem(valuePair.Key, valuePair.Value);
        }
    }
}

using DAL;
using Model;

namespace Service
{
    public class MenuService
    {
        private MenuDao menuDao = new();

        public List<MenuItem> GetAllMenuItems()
        {
            return menuDao.GetAllMenuItems();
        }

        public Dictionary<MenuType, Dictionary<CategoryType, List<MenuItem>>> GetFullMenu()
        {
            List<MenuItem> allMenuItems = GetAllMenuItems();
            Dictionary<MenuType, Dictionary<CategoryType, List<MenuItem>>> menu = new();

            foreach (MenuItem item in allMenuItems)
            {
                if (!menu.ContainsKey(item.Category.MenuCard.MenuType))
                {
                    menu.Add(item.Category.MenuCard.MenuType, new Dictionary<CategoryType, List<MenuItem>>());
                    menu[item.Category.MenuCard.MenuType].Add(item.Category.CategoryType, new List<MenuItem>());
                }
                else if (!menu[item.Category.MenuCard.MenuType].ContainsKey(item.Category.CategoryType))
                    menu[item.Category.MenuCard.MenuType].Add(item.Category.CategoryType, new List<MenuItem>());
                menu[item.Category.MenuCard.MenuType][item.Category.CategoryType].Add(item);
            }
            return menu;
        }

        public void UpdateStockOfMenuItems(List<OrderItem> orderItems)
        {
            Dictionary<int, int> newMenuItemStocks = new(); //Key is the MenuItem database ID, value is the new stock 
            foreach (OrderItem orderItem in orderItems)
            {
                if (!newMenuItemStocks.ContainsKey(orderItem.Item.ItemId))
                    newMenuItemStocks.Add(orderItem.Item.ItemId, (int)orderItem.Item.StockAmount);

                newMenuItemStocks[orderItem.Item.ItemId] -= (int)orderItem.Quantity;
            }

            foreach (KeyValuePair<int, int> keyValuePair in newMenuItemStocks)
                menuDao.UpdateStockOfMenuItem(keyValuePair.Key, keyValuePair.Value);
        }
    }
}

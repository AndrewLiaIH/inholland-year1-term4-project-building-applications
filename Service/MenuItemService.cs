using DAL;
using Model;

namespace Service
{
    // This class is written by Sia Iurashchuk
    public class MenuItemService
    {
        private MenuItemDao menuItemDao = new();

        public List<MenuItem> GetAllMenuItems()
        {
            return menuItemDao.GetAllMenuItems();
        }

        public MenuItem GetMenuItemById(int itemId)
        {
            return menuItemDao.GetMenuItemById(itemId);
        }
    }
}

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
    }
}

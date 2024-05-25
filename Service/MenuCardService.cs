using DAL;
using Model;

namespace Service
{
    // This class is written by Sia Iurashchuk
    internal class MenuCardService
    {
        private MenuCardDao menuCardDao = new();

        public List<MenuCard> GetAllMenuCards()
        {
            return menuCardDao.GetAllMenuCards();
        }

        public MenuCard GetMenuCardById(uint cardId)
        {
            return menuCardDao.GetMenuCardById(cardId);
        }
    }
}

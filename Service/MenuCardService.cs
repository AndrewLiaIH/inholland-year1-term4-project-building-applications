using DAL;
using Model;

namespace Service
{
    // This class is written by Sia Iurashchuk
    public class MenuCardService
    {
        private MenuCardDao menuCardDao = new();

        public List<MenuCard> GetAllMenuCards()
        {
            return menuCardDao.GetAllMenuCards();
        }

        public MenuCard GetMenuCardById(int cardId)
        {
            return menuCardDao.GetMenuCardById(cardId);
        }
    }
}

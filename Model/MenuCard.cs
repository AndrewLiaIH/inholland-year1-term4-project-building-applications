namespace Model
{
    public class MenuCard
    {
        public int CardId { get; private set; }
        public string MenuType { get; private set; }

        public MenuCard(int cardId, string menuType)
        {
            CardId = cardId;
            MenuType = menuType;
        }
    }
}
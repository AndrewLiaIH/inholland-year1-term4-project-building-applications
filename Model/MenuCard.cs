namespace Model
{
    public class MenuCard
    {
        public uint CardId { get; private set; }
        public string MenuType { get; private set; }

        public MenuCard(uint cardId, string menuType)
        {
            CardId = cardId;
            MenuType = menuType;
        }
    }
}
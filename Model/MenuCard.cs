namespace Model
{
    // This class is created by Orest Pokotylenko
    public class MenuCard
    {
        public int CardId { get; private set; }
        public MenuType MenuType { get; private set; }

        public MenuCard(int cardId, MenuType menuType)
        {
            CardId = cardId;
            MenuType = menuType;
        }

        public override string ToString()
        {
            return $"MenuType: {MenuType}";
        }
    }
}
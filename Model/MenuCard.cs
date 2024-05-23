namespace Model
{
    public class MenuCard
    {
        public uint CardId { get; private set; }
        public MenuType MenuType { get; private set; }

        public MenuCard(uint cardId, MenuType menuType)
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
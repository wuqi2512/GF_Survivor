public static partial class Constant
{
    public static class Game
    {
        public static readonly string CoinSpriteAssetPath = "Assets/GameMain/Res/UISprites/Pictoicon_Coin_Crown.Png";
        public static readonly string DiamondSpriteAssetPath = "Assets/GameMain/Res/UISprites/Pictoicon_Gem_Diamond.Png";
        public static readonly int EquipmentMaxLevel = 10;
        public static readonly int[] EquipmentUpgradeCostCoin = new int[]
        {
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
        };

        public static readonly DropPool ChestDropPool;

        static Game()
        {
            ChestDropPool = new DropPool();
            ChestDropPool.Add(new DropPool.DropItem(DropItemType.Coin, 250), 10);
            ChestDropPool.Add(new DropPool.DropItem(DropItemType.Diamond, 5), 1);
            ChestDropPool.Add(new DropPool.DropItem(DropItemType.Equipment, 10000), 10);
            ChestDropPool.Add(new DropPool.DropItem(DropItemType.Equipment, 10001), 10);
            ChestDropPool.Add(new DropPool.DropItem(DropItemType.Equipment, 10002), 10);
            ChestDropPool.Add(new DropPool.DropItem(DropItemType.Equipment, 10003), 10);
            ChestDropPool.Add(new DropPool.DropItem(DropItemType.Equipment, 10004), 10);
            ChestDropPool.Add(new DropPool.DropItem(DropItemType.Equipment, 10005), 10);
            ChestDropPool.Add(new DropPool.DropItem(DropItemType.Equipment, 10006), 10);
            ChestDropPool.Add(new DropPool.DropItem(DropItemType.Equipment, 10007), 10);
            ChestDropPool.Add(new DropPool.DropItem(DropItemType.Equipment, 10008), 10);
            ChestDropPool.Add(new DropPool.DropItem(DropItemType.Equipment, 10009), 10);
            ChestDropPool.Add(new DropPool.DropItem(DropItemType.Equipment, 10010), 10);
            ChestDropPool.Add(new DropPool.DropItem(DropItemType.Equipment, 10011), 10);
        }
    }
}
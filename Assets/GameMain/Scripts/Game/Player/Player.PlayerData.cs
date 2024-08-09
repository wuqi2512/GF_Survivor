using UnityGameFramework.Runtime;

public partial class Player : GameFrameworkComponent
{
    private class PlayerData
    {
        public int Coin;
        public int Diamond;
        public int HeroId;
        public Equipment[] Equipments;
        public int[] Equipped;

        public struct Equipment
        {
            public int EquipmentId;
            public int Level;

            public Equipment(int equipmentId, int level)
            {
                EquipmentId = equipmentId;
                Level = level;
            }
        }
    }
}
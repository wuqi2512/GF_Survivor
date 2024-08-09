using GameFramework;
using UnityGameFramework.Runtime;

public partial class Player : GameFrameworkComponent
{
    private sealed class AchieveSerializer : GameFrameworkSerializer<PlayerData>
    {
        private static readonly byte[] Header = new byte[] { (byte)'G', (byte)'F', (byte)'A' };

        public AchieveSerializer()
        {

        }

        protected override byte[] GetHeader()
        {
            return Header;
        }
    }
}
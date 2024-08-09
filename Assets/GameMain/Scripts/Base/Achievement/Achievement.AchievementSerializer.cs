using GameFramework;
using System.Collections.Generic;
using UnityGameFramework.Runtime;

public partial class AchievementComponent : GameFrameworkComponent
{
    private sealed class AchievementSerializer : GameFrameworkSerializer<List<AchievementData>>
    {
        private static readonly byte[] Header = new byte[] { (byte)'A', (byte)'C', (byte)'H' };

        public AchievementSerializer()
        {

        }

        protected override byte[] GetHeader()
        {
            return Header;
        }
    }
}
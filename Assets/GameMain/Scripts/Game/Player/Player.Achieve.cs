using GameFramework;
using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityGameFramework.Runtime;

public partial class Player : GameFrameworkComponent
{
    private readonly string FileName = "PlayerAchieve.dat";
    private string FilePath;
    private AchieveSerializer Serializer;

    protected override void Awake()
    {
        base.Awake();

        FilePath = Utility.Path.GetRegularPath(Path.Combine(Application.persistentDataPath, FileName));
        Serializer = new AchieveSerializer();
        Serializer.RegisterSerializeCallback(0, Serialize);
        Serializer.RegisterDeserializeCallback(0, Deserialize);
    }

    public void Load()
    {
        PlayerData playerData = null;
        try
        {
            if (!File.Exists(FilePath))
            {
                playerData = GetDefaultPlayerData();
            }
            else
            {
                using (FileStream fileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                {
                    playerData = Serializer.Deserialize(fileStream);
                }
            }
        }
        catch (Exception exception)
        {
            playerData = GetDefaultPlayerData();
            Log.Warning("Load PlayerAchieve failure with exception '{0}'.", exception);
        }

        PlayerDataToPlaer(playerData);
    }

    public void Save()
    {
        try
        {
            using (FileStream fileStream = new FileStream(FilePath, FileMode.Create, FileAccess.Write))
            {
                Serializer.Serialize(fileStream, PlayerToPlayerData());
            }
        }
        catch (Exception exception)
        {
            Log.Warning("Save PlayerAchieve failure with exception '{0}'.", exception);
        }
    }

    private static PlayerData GetDefaultPlayerData()
    {
        PlayerData playerData = new PlayerData();
        playerData.Coin = 5000;
        playerData.Diamond = 100;
        playerData.HeroId = 10000;
        playerData.Equipments = new PlayerData.Equipment[0];
        playerData.Equipped = new int[6];
        for (int i = 0; i < 6; i++)
        {
            playerData.Equipped[i] = -1;
        }

        return playerData;
    }

    private static bool Serialize(Stream stream, PlayerData playerData)
    {
        using (BinaryWriter binaryWriter = new BinaryWriter(stream, Encoding.UTF8))
        {
            binaryWriter.Write7BitEncodedInt32(playerData.Coin);
            binaryWriter.Write7BitEncodedInt32(playerData.Diamond);
            binaryWriter.Write7BitEncodedInt32(playerData.HeroId);
            binaryWriter.Write7BitEncodedInt32(playerData.Equipments.Length);
            foreach (PlayerData.Equipment equipment in playerData.Equipments)
            {
                binaryWriter.Write7BitEncodedInt32(equipment.EquipmentId);
                binaryWriter.Write7BitEncodedInt32(equipment.Level);
            }
            foreach (int equipped in playerData.Equipped)
            {
                binaryWriter.Write7BitEncodedInt32(equipped);
            }
        }

        return true;
    }

    private static PlayerData Deserialize(Stream stream)
    {
        PlayerData playerData = new PlayerData();
        using (BinaryReader binaryReader = new BinaryReader(stream, Encoding.UTF8))
        {
            playerData.Coin = binaryReader.Read7BitEncodedInt32();
            playerData.Diamond = binaryReader.Read7BitEncodedInt32();
            playerData.HeroId = binaryReader.Read7BitEncodedInt32();
            int equipmentCount = binaryReader.Read7BitEncodedInt32();
            playerData.Equipments = new PlayerData.Equipment[equipmentCount];
            for (int i = 0; i < equipmentCount; i++)
            {
                int equipmentId = binaryReader.Read7BitEncodedInt32();
                int level = binaryReader.Read7BitEncodedInt32();
                playerData.Equipments[i] = new PlayerData.Equipment(equipmentId, level);
            }
            playerData.Equipped = new int[6];
            for (int i = 0; i < 6; i++)
            {
                playerData.Equipped[i] = binaryReader.Read7BitEncodedInt32();
            }
        }

        return playerData;
    }

    private PlayerData PlayerToPlayerData()
    {
        PlayerData playerData = new PlayerData();
        playerData.Coin = m_Coin;
        playerData.Diamond = m_Diamond;
        playerData.HeroId = m_Hero.Id;
        playerData.Equipments = new PlayerData.Equipment[m_EquipmentDatas.Count];
        for (int i = 0; i < m_EquipmentDatas.Count; i++)
        {
            var data = m_EquipmentDatas[i];
            playerData.Equipments[i] = new PlayerData.Equipment(data.EquipmentId, data.Level);
        }
        playerData.Equipped = new int[6];
        for (int i = 0; i < 6; i++)
        {
            int index = -1;
            if (m_EquippedDatas[i] != null)
            {
                index = m_EquipmentDatas.IndexOf(m_EquippedDatas[i]);
            }
            playerData.Equipped[i] = index;
        }

        return playerData;
    }

    private void PlayerDataToPlaer(PlayerData playerData)
    {
        m_Coin = playerData.Coin;
        m_Diamond = playerData.Diamond;
        m_Hero = GameEntry.Luban.Tables.TbHero.Get(playerData.HeroId);
        foreach (var equipment in playerData.Equipments)
        {
            m_EquipmentDatas.Add(EquipmentData.Create(equipment.EquipmentId, equipment.Level));
        }
        for (int i = 0; i < 6; i++)
        {
            int index = playerData.Equipped[i];
            if (index == -1)
            {
                continue;
            }

            m_EquippedDatas[i] = m_EquipmentDatas[index];
        }
    }
}
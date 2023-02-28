using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerStat
{
    static List<Dictionary<string, object>> stats;

    public int maxHP;
    public float fireRate;
    public int damage;
    public float knockbackForce;

    public PlayerStat(int _maxHP, float _fireRate, int _damage, float _knockbackForce)
    {
        maxHP = _maxHP;
        fireRate = _fireRate;
        damage = _damage;
        knockbackForce = _knockbackForce;
    }

    static List<Dictionary<string, object>> LoadStat()
    {
        if (JsonData.isFileExist("/Stats/playerStats.csv"))
        {
            Debug.Log("PlayerStat : Read from file");
            return CSVReader.Read("/Stats/playerStats");
        } else
        {
            Debug.Log("PlayerStat : Read from resources");
            return CSVReaderResources.Read("Stats/playerStats");
        }
    }

    public static int GetMaxLevel(Player.Type type)
    {
        if (stats == null)
        {
            stats = LoadStat();
        }

        return stats.Count - 1;
    }

    public static PlayerStat GetStat(Player.Type type, int level)
    {
        if (stats == null)
        {
            stats = LoadStat();
        }

        string key = type.ToString();

        int damage = Utils.ToInt(stats[level][key + "-damage"]);
        float fireRate = Utils.ToFloat(stats[level][key + "-fireRate"]);
        int hp = Utils.ToInt(stats[level][key + "-hp"]);
        float knockbackforce = Utils.ToFloat(stats[level][key + "-knockbackforce"]);

        return new PlayerStat(hp, fireRate, damage, knockbackforce);
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class EnemyStats
{
    public float moveSpeed;
    public int attackPower;
    public int pushPower;
    public float maxHP;
    public int value;
    public float mass;

    static List<Dictionary<string, object>> enemyStats;

    public EnemyStats()
    {

    }

    public EnemyStats(float _moveSpeed, int _attackPower, int _pushPower, float _maxHP, int _value, float  _mass)
    {
        moveSpeed = _moveSpeed;
        attackPower = _attackPower;
        pushPower= _pushPower;
        maxHP = _maxHP;
        value = _value;
        mass = _mass;
    }

    public static EnemyStats LoadEnemyStats(Enemy.Type type, int level)
    {
        if (enemyStats == null)
        {
            try
            {
                Debug.Log($"### {JsonData.isFileExist("/Stats/enemyStats.csv")}");
                if (JsonData.isFileExist("/Stats/enemyStats.csv"))
                {
                    enemyStats = CSVReader.Read("/Stats/enemyStats");
                } else
                {
                    enemyStats = CSVReaderResources.Read("Stats/enemyStats");
                }
            }
            catch
            {
                Debug.LogError($"csv 파일 읽어오기 실패 Enemy");
                return new EnemyStats();
            }
        }

        float _moveSpeed = Utils.ToFloat(enemyStats[level][type.ToString() + "-moveSpeed"]);
        int _attackPower = Utils.ToInt(enemyStats[level][type.ToString() + "-attackPower"]);
        int _pushPower = Utils.ToInt(enemyStats[level][type.ToString() + "-pushPower"]);
        int _maxHP = Utils.ToInt(enemyStats[level][type.ToString() + "-maxHP"]);
        int _value = Utils.ToInt(enemyStats[level][type.ToString() + "-value"]);
        float _mass = Utils.ToFloat(enemyStats[level][type.ToString() + "-mass"]);

        return new EnemyStats(_moveSpeed, _attackPower, _pushPower, _maxHP, _value, _mass);
    }
}

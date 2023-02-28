using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BossInfo
{
    public int id;
    public int level;
    public float time;
}

[System.Serializable]
public class BattleInfo
{
    public int[] enemyIDs;
    public int[] times;
    public float[] spawnRates;
    public int[] levels;

    public bool isBoss;
    public BossInfo boss;
}

public class StageDatas
{
    public List<BattleInfo> battleInfos;
}

struct EnemySpawnData {

    public EnemySpawnData(Dictionary<string, object> stageData)
    {
        Utils.ToInt(stageData["enemyID"]);
        Utils.ToInt(stageData["enemyLevel"]);
        Utils.ToInt(stageData["spawnCount"]);

        Utils.ToFloat(stageData["spawnStartTime"]);
        Utils.ToFloat(stageData["spawnRate"]);
    }
}

public class EnemySquadManager : MonoBehaviour
{
    public static EnemySquadManager I { get; private set; }

    [SerializeField] CoinPool[] coinPoolList;

    public KdTree<Enemy> EnemySquad = new KdTree<Enemy>();

    public float totalDamage;

    BattleInfo battleInfo;

    public int stage;

    float totalBattleTime = 0f;

    [SerializeField] Image image_stageProgress;

    int currSpawnLevel = 0;
    float currSpawnRate = 1f;

    private void Awake()
    {
        I = this;
    }

    BattleInfo getBattleInfo()
    {
        StageDatas stageDatas;
        if (JsonData.isFileExist("/Stage/stage-info.json"))
        {
            stageDatas = JsonData.Load<StageDatas>("/Stage/stage-info.json");

        } else
        {
            stageDatas = JsonData.LoadFromResources<StageDatas>("Stage/stage-info");
        }

        if (stageDatas.battleInfos.Count <= stage)
        {
            print("exceed---load");
            return null;
        } else
        {
            print("normal---load");
            BattleInfo battleInfo = stageDatas.battleInfos[stage];

            return battleInfo;
        }
    }

    IEnumerator UpdateCurrBattleInfos()
    {
        for (int i = 0; i < battleInfo.times.Length; ++i)
        {
            int time = battleInfo.times[i];

            currSpawnLevel = battleInfo.levels[i];
            currSpawnRate = battleInfo.spawnRates[i];

            yield return new WaitForSeconds(time);
        }

        isSpawn = false;
    }

    bool isSpawn = false;

    public void StartBattle(int _stage)
    {
        stage = _stage;

        EnemySquad.Clear();

        StopAllCoroutines();

        totalDamage = 0;

        battleInfo = getBattleInfo();
        print(battleInfo.isBoss);

        if (battleInfo.isBoss)
        {
            StartCoroutine(SpawnBoss(battleInfo.boss));
        }

        totalBattleTime = Utils.Sum(battleInfo.times);

        StartCoroutine(UpdateCurrBattleInfos());

        image_stageProgress.fillAmount = 1f;

        isSpawn = true;
    }

    IEnumerator SpawnBoss(BossInfo boss)
    {
        yield return new WaitForSeconds(boss.time);

        SpawnEnemy(EnemyPrefabs.I.bossList[boss.id], boss.level);
    }

    float prevSpawnTime = 0f;

    private void Update()
    {
        if (isSpawn)
        {
            float currTime = Time.time;
            if (currTime - prevSpawnTime >= currSpawnRate)
            {
                SpawnEnemy(battleInfo.enemyIDs[Random.Range(0, battleInfo.enemyIDs.Length)]);
                prevSpawnTime = currTime;
            }
        }

        image_stageProgress.fillAmount -= Time.deltaTime / totalBattleTime;
    }

    public void StartRest()
    {
        StopSpawn();

        for (int i = 0; i < EnemySquad.Count; i++)
        {
            PoolGeneral.I._Destroy(EnemySquad[i]);
        }
    }

    public float EndBattle(bool isWin)
    {
        StopSpawn();

        if (!isWin)
        {
            for (int i = 0; i < EnemySquad.Count; i++)
            {
                EnemySquad[i].ChangeState(Enemy.State.Win);
            }
        }

        return totalDamage;
    }

    void StopSpawn()
    {
        isSpawn = false;
        StopAllCoroutines();
    }

    public Enemy FindClosestEnemy(Vector3 target)
    {
        return EnemySquad.FindClosest(target);
    }

    void SpawnEnemy(int enemyID)
    {
        SpawnEnemy(EnemyPrefabs.I.enemyList[enemyID], currSpawnLevel);
    }

    void SpawnEnemy(Enemy enemyPrefab, int level)
    {
        Player _player = PlayerSquad.I.GetRandomPlayer();
        if (_player == null) return;

        Enemy _enemy = (Enemy)PoolGeneral.I._Instantiate(enemyPrefab, transform);

        Vector3 dir = new Vector3(
            Random.Range(-360f, 360f),
            0f,
            Random.Range(-360f, 360f)).normalized + PlayerSquad.I.moveDir * Random.Range(0.5f, 1.5f);

        dir = dir.normalized * Random.Range(5f, 7f);
        dir.y = 1.5f;

        Vector3 pos = PlayerSquad.I.transform.position + dir;

        _enemy.transform.position = pos;
        _enemy.Init(level);

        EnemySquad.Add(_enemy);
    }

    public void KillEnemy(Enemy killedEnemy, float value)
    {
        totalDamage += killedEnemy.maxHP;

        EnemySquad.RemoveAll(enemy => enemy == killedEnemy);

        if (!isSpawn && EnemySquad.Count == 0)
        {
            BattleSceneManager.I.EndBattle(true);
        }
    }

    Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, 0));

    public void SpawnCoin(Vector3 position, Quaternion _rotation, float value, int spawnCount)
    {
        float total = 0;

        int coinCount = spawnCount;

        float avgValue = value / coinCount;
        float minValue = avgValue / 2;
        float maxValue = avgValue + minValue;

        void spawnCoin(float value)
        {
            if (value <= 0)
            {
                Debug.LogError("coin point is 0");
                return;
            }

            int coinIndex = Random.Range(0, coinPoolList.Length);

            Coin coin = coinPoolList[coinIndex]._Instantiate(position, rotation);
            coin.SetPoint(value);
            coin.Spawn();
        }

        while (total < value)
        {
            float coinValue = Random.Range(minValue, maxValue);
            spawnCoin(coinValue);

            total += coinValue;
        }
    }
}

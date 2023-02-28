using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSquad : SingletonScene<PlayerSquad>
{
    public enum State
    {
        Battle = 0,
        Rest = 1,
        Stop = 2
    }

    public State state;

    public Player[] playerPrefabs_Pistol;
    public Player[] playerPrefabs_Bazooka;
    public Player[] playerPrefabs_Granade;
    public Player[] playerPrefabs_Magic;

    PlayerDataList playerDataList;

    string dataPath = "/Player.json";

    public List<Player> playerList = new List<Player>();
    public List<Player> alivePlayerList { get; private set; }

    [SerializeField] TextMeshProUGUI text_PlayerPrice;
    [SerializeField] TextMeshProUGUI text_PlayerCount;
    [SerializeField] TextMeshProUGUI text_TotalAttackPower;

    [SerializeField] UIBuyPopulation UI_BuyPopulation;

    [SerializeField] Joystick joystick;
    Transform camTransform;

    public float moveSpeed = 1.2f;
    public bool isMoving = false;
    public Vector3 moveDir;

    public int maxPlayerCount { get; private set; }

    [Header("Effects")]
    [SerializeField] ParticleSystem PS_merge;

    List<PlayerPosition> battlePositionList;
    List<PlayerPosition> restPositionList;

    public Vector3 topleft;
    public Vector3 bottomright;

    float feverEnforce = 1f;

    private void OnValidate()
    {
        UI_BuyPopulation = FindObjectOfType<UIBuyPopulation>();
    }    

    protected override void Awake()
    {
        base.Awake();

        camTransform = Camera.main.transform;
        battlePositionList = transform.GetChild(0).GetComponentsInChildren<PlayerPosition>().ToList();
        restPositionList = transform.GetChild(1).GetComponentsInChildren<PlayerPosition>().ToList();
    }

    private void Start()
    {
        initPlayers();
    }

    public void StartFever(SkillFeverManager.FeverData enforce)
    {
        feverEnforce = enforce.moveSpeed + 0.1f;

        for (int i = 0; i < alivePlayerList.Count; ++i)
        {
            alivePlayerList[i].StartFever(enforce);
        }

        setTotalAttackPower_Battle();
    }

    float originMoveSpeed;

    public void SpeedUP(float ratio, float duration)
    {
        originMoveSpeed = moveSpeed;
        moveSpeed *= ratio;

        for (int i = 0; i < alivePlayerList.Count; ++i)
        {
            alivePlayerList[i].SpeedUP(ratio);
        }

        StartCoroutine(EndSpeedDown(duration));
    }

    IEnumerator EndSpeedDown(float duration)
    {
        yield return new WaitForSeconds(duration);

        moveSpeed = originMoveSpeed;

        for (int i = 0; i < alivePlayerList.Count; ++i)
        {
            alivePlayerList[i].EndSpeedUP();
        }
    }

    public void EndFever()
    {
        feverEnforce = 1f;

        for (int i = 0; i < alivePlayerList.Count; ++i)
        {
            alivePlayerList[i].EndFever();
        }

        setTotalAttackPower_Battle();
    }

    public void ChangeState(State newState)
    {
        state = newState;
        
        switch (state)
        {
            case State.Battle:
                break;
            case State.Rest:
                break;

            default:
                Debug.LogError($"처리되지 않은 상태 {state}");
                break;
        }
    }

    private void Update()
    {
        switch (state)
        {
            case State.Battle:
                playerControl();
                break;
            case State.Rest:
                break;

            default:
                Debug.LogError($"처리되지 않은 상태 {state}");
                break;
        }
    }

    void playerControl()
    {
        float joyStickX = joystick.Horizontal;
        float joyStickZ = joystick.Vertical;

        if (joyStickX == 0 && joyStickZ == 0)
        {
            isMoving = false;
        }
        else
        {
            moveDir = (
            Utils.GetHorizontalDir(camTransform) * joyStickX +
            Utils.GetVerticalDir(camTransform) * joyStickZ)
            .normalized;

            transform.position = transform.position + moveDir * Time.deltaTime * moveSpeed * feverEnforce;

            Look(moveDir, 0.2f);

            isMoving = true;
        }
    }

    protected void Look(Vector3 lookDir, float turningSpeed)
    {
        Vector3 newDir = Vector3.RotateTowards(transform.forward, lookDir, turningSpeed * Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    public void BuyPlayer()
    {
        if (playerList.Count == maxPlayerCount)
        {
            Animator anim = text_PlayerCount.GetComponentInChildren<Animator>();
            anim.SetTrigger("Red");

            return;
        }
        
        else if (playerList.Count > maxPlayerCount)
        {
            Debug.LogError($"현재 player count :{playerList.Count} / maxium : {maxPlayerCount}");
            return;
        } 
        
        else
        {
            bool buySuccess = ShopManager.I.BuyPlayer();

            if (buySuccess)
            {
                SoundManager.I.BuyPlayer01.Play();
                SoundManager.I.BuyPlayer02.Play();

                Player.Type _type;

                // !!!! 테스트 
                if (GetCurrMaxLevel(Player.Type.Pistol) < 4)
                {
                    _type = Player.Type.Pistol;
                } else
                {
                    int val = Random.Range(0, 9);
                    if (val < 6)
                    {
                        _type = Player.Type.Pistol;
                    }
                    else
                    {
                        _type = Player.Type.Bazooka;
                    }
                }

                createNewPlayer(_type, 0);
            }
        }
    }

    public int GetCurrMaxLevel(Player.Type type)
    {
        int maxLevel = 0;

        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i].type != type) continue;

            int level = playerList[i].level;
            if (level > maxLevel)
            {
                maxLevel = level;
            }
        }

        return maxLevel;
    }

    int getPlayerCount(Player.Type type)
    {
        return playerList.Count((p) =>
        {
            return p.type == type;
        });
    }

    void showInfo(Player.Type type, int level)
    {
        int maxLevel = GetCurrMaxLevel(type);
        
        if (maxLevel < level)
        {
            UICharacterInfo.I.ShowInfo(GetPlayerPrefab(type, level));
        } else if (level == 0 && type == Player.Type.Bazooka && getPlayerCount(type) == 0)
        {
            UICharacterInfo.I.ShowInfo(GetPlayerPrefab(type, level));
        }
    }

    void setTotatAttackPower()
    {
        if (text_TotalAttackPower != null)
        {
            int attackPower = Utils.ToInt(calculateTotalAttackPower());
            text_TotalAttackPower.SetText($"{attackPower}");
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)text_TotalAttackPower.transform);
        }
    }

    void setTotalAttackPower_Battle()
    {
        if (text_TotalAttackPower != null)
        {
            int attackPower = Utils.ToInt(calculateTotalAttackPower_Battle());
            text_TotalAttackPower.SetText($"{attackPower}");
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)text_TotalAttackPower.transform);
        }
    }

    float calculateTotalAttackPower()
    {
        float totalAttackPower = 0;

        for (int i = 0; i < playerList.Count; i++)
        {
            Player.Type type = playerList[i].type;
            int level = playerList[i].level;

            PlayerStat stat = PlayerStat.GetStat(type, level);
            float attackPower = stat.damage / stat.fireRate;

            if (type == Player.Type.Bazooka)
            {
                attackPower *= 3f;
            }

            totalAttackPower += attackPower;
        }

        return totalAttackPower;
    }

    float calculateTotalAttackPower_Battle()
    {
        float totalAttackPower = 0f;

        for (int i = 0; i < alivePlayerList.Count; i++)
        {
            Player player = alivePlayerList[i];

            float damage = player.stat.damage * player.feverEnforceDamage;
            float fireRate = player.stat.fireRate / player.feverEnforceFireRate;

            float attackPower = damage / fireRate;

            totalAttackPower += attackPower;
        }

        return totalAttackPower;
    }

    void createNewPlayer(Player.Type type, int level)
    {
        showInfo(type, level);

        int id = createNewId();

        PlayerData playerData = new PlayerData(id, level, type);
        playerDataList.playerDataList.Add(playerData);
        saveData();

        spawnPlayer(playerData, transform.position + new Vector3(Random.Range(-1.2f, 1.2f), 0f, Random.Range(-1.2f, 1.2f)));

        setTotatAttackPower();
    }

    void createNewPlayer(Player.Type type, int level, Vector3 pos)
    {
        showInfo(type, level);

        int id = createNewId();

        PlayerData playerData = new PlayerData(id, level, type);
        playerDataList.playerDataList.Add(new PlayerData(id, level, type));
        saveData();

        spawnPlayer(playerData, pos);

        setTotatAttackPower();
    }

    public Player GetPlayerPrefab(Player.Type type, int level)
    {
        Player p;

        switch (type)
        {
            case Player.Type.Pistol:
                p = playerPrefabs_Pistol[level];
                break;
            case Player.Type.Granade:
                p = playerPrefabs_Granade[level];
                break;
            case Player.Type.Magic:
                p = playerPrefabs_Magic[level];
                break;
            case Player.Type.Bazooka:
                p = playerPrefabs_Bazooka[level];
                break;
            default:
                Debug.LogError($"처리되지 않은 타입 {type}");
                return null;
        }

        return p;
    }

    public Player GetRandomPlayer()
    {
        if (alivePlayerList.Count == 0) return null;

        return alivePlayerList[Random.Range(0, alivePlayerList.Count)];
    }

    public void StartBattle()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            Player player = playerList[i];

            player.ChangeState(Player.State.Battle);
        }

        alivePlayerList = playerList.ToList();

        setTotalAttackPower_Battle();
    }

    public void EndBattle()
    {

    }

    public void StartMerge()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            Player player = playerList[i];

            player.ChangeState(Player.State.Rest);
        }
    }

    public void OnPlayerDie(Player player)
    {
        bool successRemove = alivePlayerList.Remove(player);

        if (!successRemove)
        {
            Debug.LogError($"등록안되있음. {player}");
        }

        if (alivePlayerList.Count <= 0)
        {
            BattleSceneManager.I.EndBattle(false);
        }

        setTotalAttackPower_Battle();
    }

    public void MergePlayer(Player p1, Player p2)
    {
        if (!(p1.type == p2.type && p1.level == p2.level))
        {
            Debug.Log($"잘못된 머지 조건 발생 {p1.type} {p2.type} {p1.level} {p2.level}");
            return;
        }

        SoundManager.I.Merge01.Play();
        SoundManager.I.Merge02.Play();

        RemovePlayer(p1);
        RemovePlayer(p2);

        createNewPlayer(p1.type, p1.level + 1, (p1.transform.position + p2.transform.position) * 0.5f);
    }

    public void RemovePlayer(Player player)
    {
        playerList.Remove(player);
        player.placeRest.isAssigned = false;

        int removeCount = playerDataList.playerDataList.RemoveAll((PlayerData pdata) => pdata.id == player.id);

        saveData();

        Destroy(player.gameObject);
    }

    public void IndicateMergables(Player.Type type, int level)
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            Player p = playerList[i];

            if (p.type == type && p.level == level)
            {
                p.mergable.ChangeState(Mergable.State.Indicated);
            } else
            {
                p.mergable.ChangeState(Mergable.State.Idle);
            }
        }
    }

    public void DeIndicateMergables()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            Player p = playerList[i];

            p.mergable.ChangeState(Mergable.State.Idle);
        }
    }

    int createNewId()
    {
        int newId = 1;

        List<PlayerData> _playerDataList = playerDataList.playerDataList;
        while (true)
        {
            bool flag = true;
            for (int i = 0; i < _playerDataList.Count; i++)
            {
                if (_playerDataList[i].id == newId)
                {
                    flag = false;
                    break;
                }
            }

            if (flag)
            {
                return newId;
            }

            ++newId;
        }
    }

    void initPlayers()
    {
        if (!JsonData.isFileExist(dataPath))
        {
            playerDataList = new PlayerDataList(
                new List<PlayerData>(2)
                , 40);

            maxPlayerCount = playerDataList.maxiumPlayerCount;

            createNewPlayer(Player.Type.Pistol, 0);
            createNewPlayer(Player.Type.Pistol, 0);
        } else
        {
            playerDataList = JsonData.Load<PlayerDataList>(dataPath);

            if (playerDataList.maxiumPlayerCount == 0)
            {
                playerDataList.maxiumPlayerCount = 40;
                saveData();
            }

            maxPlayerCount = playerDataList.maxiumPlayerCount;

            for (int i = 0; i < playerDataList.playerDataList.Count; i++)
            {
                PlayerData playerData = playerDataList.playerDataList[i];
                spawnPlayer(playerData, transform.position + new Vector3(Random.Range(-1.2f, 1.2f), 0f, Random.Range(-1.2f, 1.2f)));
            }
        }

        setTotatAttackPower();
    }

    Player spawnPlayer(PlayerData playerData, Vector3 spawnPosition)
    {
        // Spawn
        Player playerPrefab = GetPlayerPrefab(playerData.type, playerData.level);
        Player _p = Instantiate(playerPrefab, spawnPosition, Utils.RandomRotationYaxis());
        _p.id = playerData.id;
        playerList.Add(_p);

        // Effect
        ParticleSystem _SpawnPS = (ParticleSystem)PoolGeneral.I._Instantiate(PS_merge, spawnPosition, Quaternion.identity);
        PoolGeneral.I._Destroy(_SpawnPS, 3f);
        _SpawnPS.Play();

        switch (state)
        {
            case State.Rest:
                text_PlayerCount.SetText($"{playerList.Count} / {maxPlayerCount}");
                PlayerPosition placeRest = restPositionList.Find(item =>
                    {
                        return !item.isAssigned;
                    }
                );

                _p.SetRestPlace(placeRest);

                _p.ChangeState(Player.State.Rest);
                break;
            case State.Battle:
                PlayerPosition placeBattle = battlePositionList.Find(item =>
                    {
                        return !item.isAssigned;
                    }
                );

                _p.SetBattlePlace(placeBattle);
                _p.ChangeState(Player.State.Battle);
                break;

            default:
                Debug.LogError($"처리되지 않은 state {state}");
                return null;
        }

        return _p;
    }

    public void AddPlayerCount()
    {
        ++maxPlayerCount;

        playerDataList.maxiumPlayerCount = maxPlayerCount;

        saveData();
        text_PlayerCount.SetText($"{playerList.Count} / {maxPlayerCount}");
    }

    Coroutine saveRoutine = null;

    void saveData()
    {
        if (saveRoutine != null)
        {
            StopCoroutine(saveRoutine);
        }

        saveRoutine = StartCoroutine(_saveData());
    }

    IEnumerator _saveData()
    {
        yield return new WaitForSeconds(0.5f);
        JsonData.SaveJson(JsonUtility.ToJson(playerDataList), dataPath);
    }
}

[System.Serializable]
class PlayerDataList
{
    public List<PlayerData> playerDataList;
    public int maxiumPlayerCount;

    public PlayerDataList(List<PlayerData> _playerDataList, int _maxiumPlayerCount)
    {
        playerDataList = _playerDataList;
        maxiumPlayerCount = _maxiumPlayerCount;
    }
}

[System.Serializable]
class PlayerData
{
    public int id;
    public int level;
    public Player.Type type;

    public PlayerData(int _id, int _level, Player.Type _type)
    {
        id = _id;
        level = _level;
        type = _type;
    }

    public void RemovePlayer()
    {

    }
}
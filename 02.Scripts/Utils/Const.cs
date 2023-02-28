using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layer
{
    public const int DEFAULT = 0;
    public const int TRANSPARENTFX = 1;
    public const int IGNORERAYCAST = 2;
    public const int ENEMY = 3;
    public const int WATER = 4;
    public const int UI = 5;
    public const int BULLET = 6;
    public const int PLAYER = 7;
    public const int TERRAIN = 8;
    public const int CHAIR = 9;
    public const int NONCOLLISIONOBJ = 10;
    public const int COIN = 11;
    public const int BOUNDARY_ENEMY = 12;
    public const int ONLYTERRAIN = 13;
    public const int MAGNETIC_FIELD = 14;
    public const int BOUNDARY = 15;
    public const int BULLETBOX = 16;
    public const int RANGESPHERE = 17;
    public const int COINACTIVATOR= 18;
    public const int ITEM = 19;
}
public class Tag
{
    public const string _ = "";
}

public class SceneName
{
    public const string PlayScene = "PlayScene";
    public const string BattleScene = "BattleScene";
    public const string LoadingScene = "LoadingScene";
}
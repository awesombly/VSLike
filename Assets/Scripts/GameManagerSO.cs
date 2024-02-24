using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "GameManagerSO_", menuName = "Scriptable Objects/GameManagerSO" )]
public class GameManagerSO : ScriptableObject
{
    public float respawnDelay;
    public Player playerPrefab;
    // PoolManager를 사용할 모든 프리팹들
    public List<Poolable/*Prefab*/> prefabList;
}

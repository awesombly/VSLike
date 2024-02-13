using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PacketType;

public class InGameScene : MonoBehaviour
{
    [SerializeField]
    private Transform spawnTransform;
    [SerializeField]
    private GameObject playerPrefab;

    private void Awake()
    {
        if ( spawnTransform == null )
        {
            spawnTransform = transform;
        }

        ProtocolSystem.Inst.Regist( SPAWN_PLAYER_ACK, AckSpawnPlayer );
        ProtocolSystem.Inst.Regist( SPAWN_ACTOR_ACK, AckSpawnEnemy );
    }

    private void Start()
    {
        InitLocalPlayer();
        ReqSpawnPlayer();
    }

    private void InitLocalPlayer()
    {
        Player player = FindObjectOfType<Player>();
        if ( player == null )
        {
            return;
        }

        player.IsLocal = true;
        GameManager.Inst.localPlayer = player;
    }

    private void ReqSpawnPlayer()
    {
        ACTOR_INFO protocol;
        protocol.isLocal = false;
        protocol.prefab = GameManager.Inst.GetPrefabIndex( playerPrefab );
        protocol.serial = 0;
        protocol.position = new VECTOR3( spawnTransform.position );
        protocol.rotation = new QUATERNION( spawnTransform.rotation );
        Network.Inst.Send( PacketType.SPAWN_PLAYER_REQ, protocol );
    }

    #region ACK Protocols
    private void AckSpawnPlayer( Packet _packet )
    {
        var data = Global.FromJson<ACTOR_INFO>( _packet );
        Player player = null;
        if ( data.isLocal && GameManager.Inst.localPlayer != null )
        {
            player = GameManager.Inst.localPlayer;
        }
        else
        {
            player = PoolManager.Inst.Get( data.prefab ) as Player;
        }

        player.IsLocal = data.isLocal;
        player.Serial = data.serial;
        player.transform.SetPositionAndRotation( data.position.To(), data.rotation.To() );
    }

    private void AckSpawnEnemy( Packet _packet )
    {
        var data = Global.FromJson<ACTOR_INFO>( _packet );
        Enemy enemy = PoolManager.Inst.Get( data.prefab ) as Enemy;
        enemy.Serial = data.serial;
        enemy.Initialize( new Vector3( data.position.x, data.position.y, data.position.z ) );
    }
    #endregion
}

using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using static PacketType;
public class ChatSystem : MonoBehaviour
{
    public ChatMessage prefab;
    private WNS.ObjectPool<ChatMessage> pool;

    public TMP_InputField  input;
    public Transform       contents;

    // Effect
    public RectTransform area;
    private Sequence enabledEffect, disabledEffect;
    private Vector2 enabledSize, disabledSize;


    private LinkedList<ChatMessage> msgList = new LinkedList<ChatMessage>();
    private readonly int MaxMesaageCount = 8;

    private void Awake()
    {
        ProtocolSystem.Inst.Regist( PACKET_CHAT_MSG, AckBroadcastMessage );

        pool = new WNS.ObjectPool<ChatMessage>( prefab, contents );
        input.interactable = false;
    }

    private void Start()
    {
        enabledSize = area.sizeDelta;
        enabledEffect = DOTween.Sequence().Pause().SetAutoKill( false );
        enabledEffect.Append( area.DOSizeDelta( enabledSize, .5f ) );

        disabledSize = new Vector2( area.sizeDelta.x, ( prefab.transform as RectTransform ).sizeDelta.y + 10f );
        disabledEffect = DOTween.Sequence().Pause().SetAutoKill( false );
        disabledEffect.Append( area.DOSizeDelta( disabledSize, .5f ) );

        area.sizeDelta = disabledSize;
    }

    private void EnableArea( bool _isEnabled )
    {
        if ( enabledEffect.IsPlaying()  ) enabledEffect.Pause();
        if ( disabledEffect.IsPlaying() ) disabledEffect.Pause();

        if ( _isEnabled )
        {
            input.interactable = true;
            input.ActivateInputField();
            enabledEffect.Restart();
        }
        else
        {
            input.text         = string.Empty;
            input.interactable = false;
            input.DeactivateInputField();
            disabledEffect.Restart();
        }
    }

    private void AckBroadcastMessage( Packet _packet )
    {
        CHAT_MESSAGE data = Global.FromJson<CHAT_MESSAGE>( _packet );

        ChatMessage msg  = pool.Spawn();
        msg.Initialize( data );
        msgList.AddLast( msg );
        if ( msgList.Count > MaxMesaageCount )
        {
            pool.Despawn( msgList.First.Value );
            msgList.RemoveFirst();
        }

        var player = GameManager.Inst.GetActor( data.serial ) as Player;
        if ( !ReferenceEquals( player, null ) )
             player.ReceiveMessage( data.message );
    }

    private void Update()
    {
        if ( Input.GetKeyDown( KeyCode.Return ) )
        {
            if ( input.interactable ) // 채팅 비활성화
            {
                if ( !ReferenceEquals( GameManager.LocalPlayer, null ) )
                     GameManager.LocalPlayer.UnmoveableCount--;

                if ( input.text.Trim() != string.Empty )
                {
                    CHAT_MESSAGE message;
                    message.serial = GameManager.LocalPlayer.Serial;
                    message.nickname = GameManager.LoginInfo == null ? string.Empty : GameManager.LoginInfo.Value.nickname;
                    message.message = input.text;
                    Network.Inst.Send( PACKET_CHAT_MSG, message );
                }

                EnableArea( false );
            }
            else // 채팅 활성화
            {
                if ( !ReferenceEquals( GameManager.LocalPlayer, null ) )
                     GameManager.LocalPlayer.UnmoveableCount++;

                EnableArea( true );
            }
        }
    }
}
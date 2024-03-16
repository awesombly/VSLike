using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using DG.Tweening;

[System.Serializable]
public enum SceneType
{
    None,
    Lobby,
    InGame_Logic,
    InGame_UI,
}

public class SceneBase : MonoBehaviour
{
    public static event Action OnGameStart;
    public static event Action OnBeforeSceneLoad;
    public static event Action OnAfterSceneLoad;

    public SceneType SceneType { get; protected set; }
    public static bool IsLock { get; set; }

    // 게임 시작시 자동 실행
    [RuntimeInitializeOnLoadMethod( RuntimeInitializeLoadType.BeforeSceneLoad )]
    private static void BeforeGameStart()
    {
        DOTween.Init( true, true, LogBehaviour.Default ).SetCapacity( 50, 20 );
        OnGameStart?.Invoke();
    }

    public void LoadScene( SceneType _sceneType, LoadSceneMode _loadMode = LoadSceneMode.Single )
    {
        GlobalEffect.Inst.FadeOut( () =>
        {
            OnBeforeSceneLoad?.Invoke();
            //DOTween.Clear( true );

            DOTween.CompleteAll( false );
            DOTween.KillAll( true );
            SceneManager.LoadScene( _sceneType.ToString(), _loadMode );
        } );
    }

    protected virtual void Awake()
    {
        IsLock    = false;
        SceneType = SceneType.None;
        EnabledInputSystem( true, true );
    }

    protected virtual void Start()
    {
        GlobalEffect.Inst.FadeIn();
    }

    protected virtual void OnEnable()
    {
        OnAfterSceneLoad?.Invoke();
    }

    #region Input
    public static void EnabledInputSystem( bool _keyboard, bool _mouse )
    {
#if !UNITY_ANDROID && !UNITY_IOS
        if ( _keyboard ) InputSystem.EnableDevice( Keyboard.current );
        else             InputSystem.DisableDevice( Keyboard.current );

        if ( _mouse )    InputSystem.EnableDevice( Mouse.current );
        else             InputSystem.DisableDevice( Mouse.current );
#endif
    }
    #endregion
}

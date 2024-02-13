using System.Collections.Generic;
using UnityEngine;

// null이 포함된 데이터를 JSON으로 만들면 서버가 뻗습니다.
// string 같은 클래스는 꼭 초기화 해주세요.
public enum PacketType : ushort
{
    NONE = 0,
    PACKET_HEARTBEAT,              // 주기적인 통신을 위한 패킷
    PACKET_CHAT_MSG,               // 채팅 메세지

    CONFIRM_LOGIN_REQ = 1000,      // 로그인 요청
    CONFIRM_LOGIN_ACK,             // 로그인 응답
    CONFIRM_ACCOUNT_REQ,           // 계정 생성 요청
    CONFIRM_ACCOUNT_ACK,           // 계정 생성 응답
    DUPLICATE_EMAIL_REQ,           // 이메일 중복확인 요청
    DUPLICATE_EMAIL_ACK,           // 이메일 중복확인 응답

    STAGE_INFO_REQ = 2000,         // 방 정보 요청
    STAGE_INFO_ACK,                // 방 정보 응답
    CREATE_STAGE_REQ,              // 방 생성 요청
    CREATE_STAGE_ACK,              // 방 생성 응답
    UPDATE_STAGE_INFO,             // 방 정보가 갱신됨
    INSERT_STAGE_INFO,             // 방 정보가 추가됨
    DELETE_STAGE_INFO,             // 방 정보가 삭제됨
    ENTRY_STAGE_REQ,               // 방 입장 요청
    ENTRY_STAGE_ACK,               // 방 입장 응답
    EXIT_STAGE_REQ,                // 방 퇴장 요청
    EXIT_STAGE_ACK,                // 방 퇴장 응답

    SPAWN_ENEMY_REQ = 5000,       // 적 스폰 요청
    SPAWN_ENEMY_ACK,              // 적 스폰 응답
};

public interface IProtocol { }
// Both 
public struct EMPTY : IProtocol { }
public struct MESSAGE : IProtocol { public string message; }
public struct CONFIRM : IProtocol { public bool isCompleted; }

public struct Personnel { public int current, maximum; }
public struct STAGE_INFO : IProtocol
{
    public ushort serial;
    public string title;
    public Personnel personnel;
}

public struct LOGIN_INFO : IProtocol
{
    public string nickname;
    public string email;
    public string password;
}

public struct SPAWN_ENEMY : IProtocol
{
    public int prefab;
    public int serial;
    public float x;
    public float y;
}

public struct SAMPLE : IProtocol
{
    public System.Numerics.Vector3 v3;
    public System.Numerics.Vector4 v4;
}
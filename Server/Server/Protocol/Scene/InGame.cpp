#include "InGame.h"
#include "Management/SessionManager.h"

void InGame::Bind()
{
	ProtocolSystem::Inst().Regist( SPAWN_PLAYER_REQ, SpawnPlayer );
	ProtocolSystem::Inst().Regist( SPAWN_ACTOR_REQ, SpawnActor );
	ProtocolSystem::Inst().Regist( EXIT_STAGE_REQ,  AckExitStage );
}

void InGame::SpawnPlayer( const Packet& _packet )
{
	ACTOR_INFO data = FromJson<ACTOR_INFO>( _packet );
	data.serial = Global::GetNewSerial();
	data.isLocal = true;
	SessionManager::Inst().Broadcast( UPacket( SPAWN_PLAYER_ACK, data ) );
	/// 발신자만 Send 기능 필요
	///data.isLocal = false;
	///SessionManager::Inst().BroadcastWithoutSelf( _packet.socket, UPacket( SPAWN_PLAYER_ACK, data ) );
}

void InGame::SpawnActor( const Packet& _packet )
{
	ACTOR_INFO data = FromJson<ACTOR_INFO>( _packet );
	data.serial = Global::GetNewSerial();
	SessionManager::Inst().Broadcast( UPacket( SPAWN_ACTOR_ACK, data ) );
}

void InGame::AckExitStage( const Packet& _packet )
{
	STAGE_INFO data = FromJson<STAGE_INFO>( _packet );

	Session* session = _packet.session;
	SessionManager::Inst().ExitStage( session, data );
	_packet.session->Send( UPacket( ENTRY_STAGE_ACK, stage->info ) );
}
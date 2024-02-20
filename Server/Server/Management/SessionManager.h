#pragma once
#include "Network/Session.h"
#include "Global/LogText.hpp"
#include "Stage/Stage.h"

class SessionManager : public Singleton<SessionManager>
{
private:
	std::queue<Session*> unAckSessions;
	std::list<Session*> sessions;
	// std::unordered_map<SOCKET, Session*> sessions;
	std::unordered_map<SerialType, Stage*> stages;
	std::mutex mtx;

public:
	SessionManager() = default;
	virtual ~SessionManager();

private:
	void ConfirmDisconnect();

public:
	bool Initialize();

	// Full Management
	void Push( Session* _session );
	void Erase( Session* _session );

	void Broadcast( const UPacket& _packet ) const;
	void BroadcastWithoutSelf( Session* _session, const UPacket& _packet ) const;
	void BroadcastWaitingRoom( const UPacket& _packet );
	std::list<Session*> GetSessions() const;

	// Stage Management
	Stage* EntryStage( Session* _session, const STAGE_INFO& _info );
	void ExitStage( Session* _session );
	const std::unordered_map<SerialType, Stage*>& GetStages() const;
};
#pragma once
#include "Protocol.h"
#include "../Global/Header.h"

class Network
{
protected:
	struct OVERLAPPEDEX : OVERLAPPED
	{
		u_int flag;

		OVERLAPPEDEX() : flag( MODE_RECV ) {} 
		
		enum : char
		{
			MODE_RECV = 0,
			MODE_SEND = 1,
		};
	};

	SOCKET socket;
	SOCKADDR_IN address;
	WSABUF wsaBuffer;

private:
	OVERLAPPEDEX ov;
	char buffer[HeaderSize + MaxDataSize];

public:
	Network() = default;
	Network( const SOCKET& _socket, const SOCKADDR_IN& _address );
	virtual ~Network() = default;

public:
	bool Initialize( int _port, const char* _ip );
	bool ClosedSocket();

public:
	bool Connect() const;
	//void Recieve();
	//void Send( const UPACKET& _packet );

public:
	const SOCKET& GetSocket();
	std::string GetAddress() const;
	std::string GetPort() const;

};
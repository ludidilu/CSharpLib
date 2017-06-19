using UnityEngine;
using System.Collections;
using System;
using System.Net.Sockets;
using System.Net;
using System.Collections.Generic;

public class SuperClient : MonoBehaviour {

	private enum CONNECT_STATUS{

		CONNECTING,
		CONNECTED,
		CONNECTOVER,
		CLOSED
	}

	private object locker = new object();

	private Socket socket;

	private Action<byte[]> getDataCallBack;

	private Action disconnectCallBack;

	private Action<bool,string> connectOverCallBack;

	private CONNECT_STATUS connectStatus = CONNECT_STATUS.CLOSED;

	public void Open(string _ip,int _port,int _timeout,Action<byte[]> _getDataCallBack,Action _disconnectCallBack,Action<bool,string> _connectOverCallBack){

		getDataCallBack = _getDataCallBack;

		disconnectCallBack = _disconnectCallBack;

		connectOverCallBack = _connectOverCallBack;

		IPAddress ipa = Dns.GetHostAddresses(_ip)[0];
		
		socket = new Socket(ipa.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

		IPEndPoint ipe = new IPEndPoint(ipa, _port);

		IAsyncResult ar = null;

		connectStatus = CONNECT_STATUS.CONNECTING;

		try
		{
			ar = socket.BeginConnect(ipe,ConnectCallBack,null);
		}
		catch (Exception e)
		{
			Action<bool,string> dele = connectOverCallBack;

			Clear ();

			dele(false,"error");

			return;
		}

		StartCoroutine(CheckConnectTimeout(ar,_timeout));
	}

	public void Send(byte[] _bytes){

		if (socket != null) {

			socket.BeginSend(_bytes,0,_bytes.Length,SocketFlags.None,SendOver,null);
		}
	}

	private void SendOver(IAsyncResult _ar){

		if (socket != null) {

			socket.EndSend (_ar);
		}
	}

	public void Close(){

		if (socket != null) {

			socket.BeginDisconnect(false,CloseOver,null);
		}
	}

	private void CloseOver(IAsyncResult _ar){

		if (socket != null) {

			socket.EndDisconnect(_ar);
		}

		Clear ();
	}

	private void ConnectCallBack(IAsyncResult _ar){

		lock (locker)
		{
			if (connectStatus == CONNECT_STATUS.CONNECTING)
			{
				socket.EndConnect(_ar);

				connectStatus = CONNECT_STATUS.CONNECTED;
			}
		}
	}

	private IEnumerator CheckConnectTimeout(IAsyncResult _ar,int _timeout){

		yield return new WaitForSeconds(_timeout);

		lock(locker)
		{
			if(connectStatus == CONNECT_STATUS.CONNECTING){
				
				Action<bool,string> dele = connectOverCallBack;

				Close();

				dele(false,"timeout");
			}
		}
	}

	private void Clear(){

		connectStatus = CONNECT_STATUS.CLOSED;

		if (socket != null) {

			socket.Close ();
			
			socket = null;
		}

		connectOverCallBack = null;

		getDataCallBack = null;

		disconnectCallBack = null;
	}

	// Update is called once per frame
	void Update () {
	
		if(connectStatus == CONNECT_STATUS.CONNECTED){

			connectStatus = CONNECT_STATUS.CONNECTOVER;

			connectOverCallBack(true,string.Empty);

		}else if(connectStatus == CONNECT_STATUS.CONNECTOVER){

			if (socket.Poll(10, SelectMode.SelectRead) && socket.Available == 0)
			{
				Action dele = disconnectCallBack;

				Clear();

				dele();
				
			}else{

				if(socket.Available > 0){

					byte[] bytes = new byte[socket.Available];

					socket.Receive(bytes,0,socket.Available,SocketFlags.None);

					getDataCallBack(bytes);
				}
			}
		}
	}

	void OnDestroy(){

		Clear ();
	}
}

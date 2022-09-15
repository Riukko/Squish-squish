using System;
using System.Net; 
using System.Net.Sockets; 
using System.Text; 
using System.Threading;
using UnityEditor;
using UnityEngine;

public class TCPServerTest : MonoBehaviour
{
	[SerializeField] private string m_hostIP = "127.0.0.1";
	[SerializeField] private int m_port = 25025;

	public delegate void IncomingMessageEvent(string message);
	public static event IncomingMessageEvent OnIncomingMessage;
	/// <summary> 	
	/// TCPListener to listen for incomming TCP connection
	/// requests. 	
	/// </summary> 	
	private TcpListener tcpListener; 
	/// <summary> 
	/// Background thread for TcpServer workload. 	
	/// </summary> 	
	private Thread tcpListenerThread;  	
	/// <summary> 	
	/// Create handle to connected tcp client. 	
	/// </summary> 	
	private TcpClient connectedTcpClient;
	
	void Start ()
	{
		// Start TcpServer background thread
		tcpListenerThread = new Thread (ListenForIncommingRequests) { IsBackground = true };
		tcpListenerThread.Start();
	}
	
	/// <summary>
	/// Runs in background TcpServerThread; Handles incoming TcpClient requests
	/// </summary>
	private void ListenForIncommingRequests()
	{
		try
		{
			// Create listener on localhost port 8052.
			tcpListener = new TcpListener(IPAddress.Parse(m_hostIP), m_port);
			tcpListener.Start();
			Debug.Log("Server is listening");
			Byte[] bytes = new Byte[1024];
			while (true)
			{
				connectedTcpClient = tcpListener.AcceptTcpClient();
				Debug.Log("Client is connect");
				// Get a stream object for reading
				using (NetworkStream stream = connectedTcpClient.GetStream())
				{
					int length;
					// Read incomming stream into byte array.
					while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
					{
						var incommingData = new byte[length];
						Array.Copy(bytes, 0, incommingData, 0, length);
						// Convert byte array to string message.
						string clientMessage = Encoding.ASCII.GetString(incommingData);
						//Debug.Log("client message received as: " + clientMessage);
						OnIncomingMessage?.Invoke(clientMessage);
					}
				}

				
			}
		}
		catch (SocketException socketException)
		{
			Debug.Log("SocketException " + socketException);
		}
	}

	private void OnApplicationQuit()
	{
		tcpListenerThread.Abort();
		
		if (connectedTcpClient != null)
		{
			connectedTcpClient.GetStream().Close();
			connectedTcpClient.Close();
		}

		if (tcpListener != null)
		{
			tcpListener.Stop();
		}
	}

	/// <summary> 	
	/// Send message to client using socket connection. 	
	/// </summary>
	[ContextMenu("Send Message")]
	private void SendMessage()
	{
		if (connectedTcpClient == null)
			return;
		
		try
		{	
			// Get a stream object for writing. 			
			NetworkStream stream = connectedTcpClient.GetStream(); 			
			if (stream.CanWrite)
			{
				string serverMessage = "This is a message from your server."; 			
				// Convert string message to byte array.                 
				byte[] serverMessageAsByteArray = Encoding.ASCII.GetBytes(serverMessage); 				
				// Write byte array to socketConnection stream.               
				stream.Write(serverMessageAsByteArray, 0, serverMessageAsByteArray.Length);               
				Debug.Log("Server sent his message - should be received by client");           
			}       
		}
		catch (SocketException socketException)
		{             
			Debug.Log("Socket exception: " + socketException);         
		} 	
	} 
}

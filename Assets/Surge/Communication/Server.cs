/// <summary>
/// SURGE FRAMEWORK
/// Author: Bob Berkebile
/// Email: bobb@pixelplacement.com
///
/// A super simple server for sending data back and forth.
///
/// </summary>

using UnityEngine;
using UnityEngine.Networking;
using System;

namespace Pixelplacement
{
	//[RequireComponent(typeof(NetworkDiscovery))]
	public class Server : NetworkDiscovery
	{
		#region Public Events:
		public static event Action OnPlayerConnected;
		public static event Action OnPlayerDisconnected;
		public static event Action<FloatMessage> OnFloat;
		public static event Action<FloatArrayMessage> OnFloatArray;
		public static event Action<IntMessage> OnInt;
		public static event Action<IntArrayMessage> OnIntArray;
		public static event Action<Vector2Message> OnVector2;
		public static event Action<Vector2ArrayMessage> OnVector2Array;
		public static event Action<Vector3Message> OnVector3;
		public static event Action<Vector3ArrayMessage> OnVector3Array;
		public static event Action<Vector4Message> OnVector4;
		public static event Action<Vector4ArrayMessage> OnVector4Array;
		public static event Action<RectMessage> OnRect;
		public static event Action<RectArrayMessage> OnRectArray;
		public static event Action<StringMessage> OnString;
		public static event Action<StringArrayMessage> OnStringArray;
		public static event Action<ByteMessage> OnByte;
		public static event Action<ByteArrayMessage> OnByteArray;
		public static event Action<ColorMessage> OnColor;
		public static event Action<ColorArrayMessage> OnColorArray;
		public static event Action<Color32Message> OnColor32;
		public static event Action<Color32ArrayMessage> OnColor32Array;
		public static event Action<RectByteArrayMessage> OnRectByteArray;
		public static event Action<BoolMessage> OnBool;
		public static event Action<BoolArrayMessage> OnBoolArray;
		#endregion

		#region Public Variables:
		[Tooltip("Must match the client's primary quality of service.")] public QosType primaryQualityOfService = QosType.Reliable;
		[Tooltip("Must match the client's secondary quality of service.")] public QosType secondaryQualityOfService = QosType.Reliable;
		[Tooltip("Optional name for this device to be sent to clients for connection identification.")] public string customDeviceId;
		[Tooltip("Must match the client's broadcasting port.")] public int broadcastingPort;
		public int maxConnections;
		public uint initialBandwidth;
		#endregion

		#region Private Variables:
		private static NetworkServerSimple _server = new NetworkServerSimple();
		private static string _randomIdKey = "RandomIdKey";
		#endregion

		#region Public Properties:
		public static int ConnectedCount
		{
			get;
			private set;
		}

		public static int PrimaryChannel
		{
			get;
			private set;
		}

		public static int SecondaryChannel
		{
			get;
			private set;
		}

		public static string DeviceId
		{
			get;
			private set;
		}

		public static bool Running
		{
			get;
			private set;
		}

		public static bool Connected
		{
			get
			{
				return ConnectedCount > 0;
			}
		}
		#endregion

		#region Private Properties:
		private int ServerPort
		{
			get
			{
				return base.broadcastPort + 1;
			}
		}
		#endregion

		#region Init:
		private void Reset()
		{
			maxConnections = 1;
			showGUI = false;
			broadcastingPort = 47777;
			initialBandwidth = 500000;
			primaryQualityOfService = QosType.Reliable;
			secondaryQualityOfService = QosType.UnreliableFragmented;
		}

		private void Awake()
		{
			//setup:
			broadcastPort = broadcastingPort;
			broadcastData = ServerPort.ToString();

			//set device id:
			if (string.IsNullOrEmpty(customDeviceId))
			{
				GenerateID();
				DeviceId = PlayerPrefs.GetString(_randomIdKey);
				broadcastData += "_" + DeviceId;
			}
			else
			{
				broadcastData += "_" + customDeviceId;
			}

			//HACK: this is a fix for the broadcastData bug where Unity will combine different 
			//data if the length is different because they internally reuse this object
			broadcastData += "~!~";

			Init();

			//configurations:
			ConnectionConfig config = new ConnectionConfig();
			PrimaryChannel = config.AddChannel(primaryQualityOfService);
			SecondaryChannel = config.AddChannel(secondaryQualityOfService);
			config.InitialBandwidth = initialBandwidth;

			HostTopology topology = new HostTopology(config, maxConnections);
			_server.Listen(ServerPort, topology);

			//event hooks:
			_server.RegisterHandler(MsgType.Connect, HandleConnect);
			_server.RegisterHandler(MsgType.Disconnect, HandleDisconnect);
			_server.RegisterHandler((short)NetworkMsg.FloatMsg, HandleFloat);
			_server.RegisterHandler((short)NetworkMsg.FloatArrayMsg, HandleFloatArray);
			_server.RegisterHandler((short)NetworkMsg.IntMsg, HandleInt);
			_server.RegisterHandler((short)NetworkMsg.IntArrayMsg, HandleIntArray);
			_server.RegisterHandler((short)NetworkMsg.Vector2Msg, HandleVector2);
			_server.RegisterHandler((short)NetworkMsg.Vector2ArrayMsg, HandleVector2Array);
			_server.RegisterHandler((short)NetworkMsg.Vector3Msg, HandleVector3);
			_server.RegisterHandler((short)NetworkMsg.Vector3ArrayMsg, HandleVector3Array);
			_server.RegisterHandler((short)NetworkMsg.Vector4Msg, HandleVector4);
			_server.RegisterHandler((short)NetworkMsg.Vector4ArrayMsg, HandleVector4Array);
			_server.RegisterHandler((short)NetworkMsg.RectMsg, HandleRect);
			_server.RegisterHandler((short)NetworkMsg.RectArrayMsg, HandleRectArray);
			_server.RegisterHandler((short)NetworkMsg.StringMsg, HandleString);
			_server.RegisterHandler((short)NetworkMsg.StringArrayMsg, HandleStringArray);
			_server.RegisterHandler((short)NetworkMsg.ByteMsg, HandleByte);
			_server.RegisterHandler((short)NetworkMsg.ByteArrayMsg, HandleByteArray);
			_server.RegisterHandler((short)NetworkMsg.ColorMsg, HandleColor);
			_server.RegisterHandler((short)NetworkMsg.ColorArrayMsg, HandleColorArray);
			_server.RegisterHandler((short)NetworkMsg.Color32Msg, HandleColor32);
			_server.RegisterHandler((short)NetworkMsg.Color32ArrayMsg, HandleColor32Array);
			_server.RegisterHandler((short)NetworkMsg.RectByteArrayMsg, HandleRectByteArray);
			_server.RegisterHandler((short)NetworkMsg.BoolMsg, HandleBool);
			_server.RegisterHandler((short)NetworkMsg.BoolArrayMsg, HandleBoolArray);

			//dont destroy:
			transform.parent = null;
			DontDestroyOnLoad(gameObject);
		}
		#endregion

		#region Cleanup:
		private void OnDestroy()
		{
			_server.Stop();
		}
		#endregion

		#region Flow
		private void OnEnable()
		{
			Running = true;
		}

		private void OnDisable()
		{
			Running = false;
		}
		#endregion

		#region Private Methods:
		private void Init()
		{
			Initialize();
			StartAsServer();
		}

		private void GenerateID()
		{
			//key already available:
			if (PlayerPrefs.HasKey(_randomIdKey)) return;

			string id = "";

			//create guid:
			Guid guid = Guid.NewGuid();
			string guidString = guid.ToString();

			//splint into strings:
			string[] parts = guidString.Split('-');

			//go through each block in the guid to create a 5 digit id:
			for (int i = 0; i < parts.Length; i++)
			{
				//holder for each int found:
				int finalNumber = 0;

				foreach (var item in parts[i])
				{
					//if it's a number then add it:
					int currentNumber = 0;
					if (int.TryParse(item.ToString(), out currentNumber))
					{
						finalNumber += currentNumber;
					}
				}

				//loop number so it is a single digit:
				finalNumber = (int)Mathf.Repeat(finalNumber, 9);

				//construct string:
				id += finalNumber.ToString();
			}

			PlayerPrefs.SetString(_randomIdKey, id);
		}
		#endregion

		#region Public Methods:
		public static void RegisterHandler(short msgType, NetworkMessageDelegate handler)
		{
			_server.RegisterHandler(msgType, handler);
		}

		public static void UnregisterHandler(short msgType)
		{
			_server.UnregisterHandler(msgType);
		}

		public static void Disconnect()
		{
			_server.DisconnectAllConnections();
		}

		public static void Send(short msgType, MessageBase message, int qualityOfServiceChannel = 0)
		{
			foreach (var item in _server.connections)
			{
				if (item != null)
				{
					item.SendByChannel(msgType, message, qualityOfServiceChannel);
				}
			}
		}

		public static void Send(float value, string id = "", int qualityOfServiceChannel = 0)
		{
			Send((short)NetworkMsg.FloatMsg, new FloatMessage(value, id), qualityOfServiceChannel);
		}

		public static void Send(float[] value, string id = "", int qualityOfServiceChannel = 0)
		{
			Send((short)NetworkMsg.FloatArrayMsg, new FloatArrayMessage(value, id), qualityOfServiceChannel);
		}

		public static void Send(int value, string id = "", int qualityOfServiceChannel = 0)
		{
			Send((short)NetworkMsg.IntMsg, new IntMessage(value, id), qualityOfServiceChannel);
		}

		public static void Send(int[] value, string id = "", int qualityOfServiceChannel = 0)
		{
			Send((short)NetworkMsg.IntArrayMsg, new IntArrayMessage(value, id), qualityOfServiceChannel);
		}

		public static void Send(Vector2 value, string id = "", int qualityOfServiceChannel = 0)
		{
			Send((short)NetworkMsg.Vector2Msg, new Vector2Message(value, id), qualityOfServiceChannel);
		}

		public static void Send(Vector2[] value, string id = "", int qualityOfServiceChannel = 0)
		{
			Send((short)NetworkMsg.Vector2ArrayMsg, new Vector2ArrayMessage(value, id), qualityOfServiceChannel);
		}

		public static void Send(Vector3 value, string id = "", int qualityOfServiceChannel = 0)
		{
			Send((short)NetworkMsg.Vector3Msg, new Vector3Message(value, id), qualityOfServiceChannel);
		}

		public static void Send(Vector3[] value, string id = "", int qualityOfServiceChannel = 0)
		{
			Send((short)NetworkMsg.Vector3ArrayMsg, new Vector3ArrayMessage(value, id), qualityOfServiceChannel);
		}

		public static void Send(Vector4 value, string id = "", int qualityOfServiceChannel = 0)
		{
			Send((short)NetworkMsg.Vector4Msg, new Vector4Message(value, id), qualityOfServiceChannel);
		}

		public static void Send(Vector4[] value, string id = "", int qualityOfServiceChannel = 0)
		{
			Send((short)NetworkMsg.Vector4ArrayMsg, new Vector4ArrayMessage(value, id), qualityOfServiceChannel);
		}

		public static void Send(Rect value, string id = "", int qualityOfServiceChannel = 0)
		{
			Send((short)NetworkMsg.RectMsg, new RectMessage(value, id), qualityOfServiceChannel);
		}

		public static void Send(Rect[] value, string id = "", int qualityOfServiceChannel = 0)
		{
			Send((short)NetworkMsg.RectArrayMsg, new RectArrayMessage(value, id), qualityOfServiceChannel);
		}

		public static void Send(string value, string id = "", int qualityOfServiceChannel = 0)
		{
			Send((short)NetworkMsg.StringMsg, new StringMessage(value, id), qualityOfServiceChannel);
		}

		public static void Send(string[] value, string id = "", int qualityOfServiceChannel = 0)
		{
			Send((short)NetworkMsg.StringArrayMsg, new StringArrayMessage(value, id), qualityOfServiceChannel);
		}

		public static void Send(byte value, string id = "", int qualityOfServiceChannel = 0)
		{
			Send((short)NetworkMsg.ByteMsg, new ByteMessage(value, id), qualityOfServiceChannel);
		}

		public static void Send(byte[] value, string id = "", int qualityOfServiceChannel = 0)
		{
			Send((short)NetworkMsg.ByteArrayMsg, new ByteArrayMessage(value, id), qualityOfServiceChannel);
		}

		public static void Send(Color value, string id = "", int qualityOfServiceChannel = 0)
		{
			Send((short)NetworkMsg.ColorMsg, new ColorMessage(value, id), qualityOfServiceChannel);
		}

		public static void Send(Color[] value, string id = "", int qualityOfServiceChannel = 0)
		{
			Send((short)NetworkMsg.ColorArrayMsg, new ColorArrayMessage(value, id), qualityOfServiceChannel);
		}

		public static void Send(Color32 value, string id = "", int qualityOfServiceChannel = 0)
		{
			Send((short)NetworkMsg.Color32Msg, new Color32Message(value, id), qualityOfServiceChannel);
		}

		public static void Send(Color32[] value, string id = "", int qualityOfServiceChannel = 0)
		{
			Send((short)NetworkMsg.Color32ArrayMsg, new Color32ArrayMessage(value, id), qualityOfServiceChannel);
		}

		public static void Send(Rect rect, byte[] bytes, string id = "", int qualityOfServiceChannel = 0)
		{
			Send((short)NetworkMsg.RectByteArrayMsg, new RectByteArrayMessage(rect, bytes, id), qualityOfServiceChannel);
		}

		public static void Send(bool value, string id = "", int qualityOfServiceChannel = 0)
		{
			Send((short)NetworkMsg.BoolMsg, new BoolMessage(value, id), qualityOfServiceChannel);
		}

		public static void Send(bool[] value, string id = "", int qualityOfServiceChannel = 0)
		{
			Send((short)NetworkMsg.BoolArrayMsg, new BoolArrayMessage(value, id), qualityOfServiceChannel);
		}
		#endregion

		#region Loops:
		private void Update()
		{
			_server.Update();
		}
		#endregion

		#region Event Handlers:
		private void HandleConnect(NetworkMessage message)
		{
			ConnectedCount++;

			if (OnPlayerConnected != null) OnPlayerConnected();
		}

		private void HandleDisconnect(NetworkMessage message)
		{
			ConnectedCount--;

			if (OnPlayerDisconnected != null) OnPlayerDisconnected();
		}

		private void HandleFloat(NetworkMessage message)
		{
			if (OnFloat != null) OnFloat(message.ReadMessage<FloatMessage>());
		}

		private void HandleFloatArray(NetworkMessage message)
		{
			if (OnFloatArray != null) OnFloatArray(message.ReadMessage<FloatArrayMessage>());
		}

		private void HandleInt(NetworkMessage message)
		{
			if (OnInt != null) OnInt(message.ReadMessage<IntMessage>());
		}

		private void HandleIntArray(NetworkMessage message)
		{
			if (OnIntArray != null) OnIntArray(message.ReadMessage<IntArrayMessage>());
		}

		private void HandleVector2(NetworkMessage message)
		{
			if (OnVector2 != null) OnVector2(message.ReadMessage<Vector2Message>());
		}

		private void HandleVector2Array(NetworkMessage message)
		{
			if (OnVector2Array != null) OnVector2Array(message.ReadMessage<Vector2ArrayMessage>());
		}

		private void HandleVector3(NetworkMessage message)
		{
			if (OnVector3 != null) OnVector3(message.ReadMessage<Vector3Message>());
		}

		private void HandleVector3Array(NetworkMessage message)
		{
			if (OnVector3Array != null) OnVector3Array(message.ReadMessage<Vector3ArrayMessage>());
		}

		private void HandleVector4(NetworkMessage message)
		{
			if (OnVector4 != null) OnVector4(message.ReadMessage<Vector4Message>());
		}

		private void HandleVector4Array(NetworkMessage message)
		{
			if (OnVector4Array != null) OnVector4Array(message.ReadMessage<Vector4ArrayMessage>());
		}

		private void HandleRect(NetworkMessage message)
		{
			if (OnRect != null) OnRect(message.ReadMessage<RectMessage>());
		}

		private void HandleRectArray(NetworkMessage message)
		{
			if (OnRectArray != null) OnRectArray(message.ReadMessage<RectArrayMessage>());
		}

		private void HandleString(NetworkMessage message)
		{
			if (OnString != null) OnString(message.ReadMessage<StringMessage>());
		}

		private void HandleStringArray(NetworkMessage message)
		{
			if (OnStringArray != null) OnStringArray(message.ReadMessage<StringArrayMessage>());
		}

		private void HandleByte(NetworkMessage message)
		{
			if (OnByte != null) OnByte(message.ReadMessage<ByteMessage>());
		}

		private void HandleByteArray(NetworkMessage message)
		{
			if (OnByteArray != null) OnByteArray(message.ReadMessage<ByteArrayMessage>());
		}

		private void HandleColor(NetworkMessage message)
		{
			if (OnColor != null) OnColor(message.ReadMessage<ColorMessage>());
		}

		private void HandleColorArray(NetworkMessage message)
		{
			if (OnColorArray != null) OnColorArray(message.ReadMessage<ColorArrayMessage>());
		}

		private void HandleColor32(NetworkMessage message)
		{
			if (OnColor32 != null) OnColor32(message.ReadMessage<Color32Message>());
		}

		private void HandleColor32Array(NetworkMessage message)
		{
			if (OnColor32Array != null) OnColor32Array(message.ReadMessage<Color32ArrayMessage>());
		}

		private void HandleRectByteArray(NetworkMessage message)
		{
			if (OnRectByteArray != null) OnRectByteArray(message.ReadMessage<RectByteArrayMessage>());
		}

		private void HandleBool(NetworkMessage message)
		{
			if (OnBool != null) OnBool(message.ReadMessage<BoolMessage>());
		}

		private void HandleBoolArray(NetworkMessage message)
		{
			if (OnBoolArray != null) OnBoolArray(message.ReadMessage<BoolArrayMessage>());
		}
		#endregion
	}
}
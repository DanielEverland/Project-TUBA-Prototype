/// <summary>
/// SURGE FRAMEWORK
/// Author: Bob Berkebile
/// Email: bobb@pixelplacement.com
///
/// Template messages and other things the client/server utilize.
///
/// </summary>

using UnityEngine;
using UnityEngine.Networking;

namespace Pixelplacement
{
	#region Msgs:
	public enum NetworkMsg { ServerAvailableMsg = 3000, FloatMsg, FloatArrayMsg, IntMsg, IntArrayMsg, Vector2Msg, Vector2ArrayMsg, Vector3Msg, Vector3ArrayMsg, Vector4Msg, Vector4ArrayMsg, RectMsg, RectArrayMsg, StringMsg, StringArrayMsg, ByteMsg, ByteArrayMsg, ColorMsg, ColorArrayMsg, Color32Msg, Color32ArrayMsg, RectByteArrayMsg, BoolMsg, BoolArrayMsg };
	#endregion

	#region Custom Messages:
	public class ServerAvailableMessage : MessageBase
	{
		public string ip;
		public int port;
		public string deviceId;

		public ServerAvailableMessage() { }

		public ServerAvailableMessage(string ip, int port, string deviceId)
		{
			this.ip = ip;
			this.port = port;
			this.deviceId = deviceId;
		}
	}

	public class FloatMessage : MessageBase
	{
		public float value;
		public string id;

		public FloatMessage() { }

		public FloatMessage(float value, string id)
		{
			this.value = value;
			this.id = id;
		}
	}

	public class FloatArrayMessage : MessageBase
	{
		public float[] value;
		public string id;

		public FloatArrayMessage() { }

		public FloatArrayMessage(float[] value, string id)
		{
			this.value = value;
			this.id = id;
		}
	}

	public class IntMessage : MessageBase
	{
		public int value;
		public string id;

		public IntMessage() { }

		public IntMessage(int value, string id)
		{
			this.value = value;
			this.id = id;
		}
	}

	public class IntArrayMessage : MessageBase
	{
		public int[] value;
		public string id;

		public IntArrayMessage() { }

		public IntArrayMessage(int[] value, string id)
		{
			this.value = value;
			this.id = id;
		}
	}

	public class Vector2Message : MessageBase
	{
		public Vector2 value;
		public string id;

		public Vector2Message() { }

		public Vector2Message(Vector2 value, string id)
		{
			this.value = value;
			this.id = id;
		}
	}

	public class Vector2ArrayMessage : MessageBase
	{
		public Vector2[] value;
		public string id;

		public Vector2ArrayMessage() { }

		public Vector2ArrayMessage(Vector2[] value, string id)
		{
			this.value = value;
			this.id = id;
		}
	}

	public class Vector3Message : MessageBase
	{
		public Vector3 value;
		public string id;

		public Vector3Message() { }

		public Vector3Message(Vector3 value, string id)
		{
			this.value = value;
			this.id = id;
		}
	}

	public class Vector3ArrayMessage : MessageBase
	{
		public Vector3[] value;
		public string id;

		public Vector3ArrayMessage() { }

		public Vector3ArrayMessage(Vector3[] value, string id)
		{
			this.value = value;
			this.id = id;
		}
	}

	public class Vector4Message : MessageBase
	{
		public Vector4 value;
		public string id;

		public Vector4Message() { }

		public Vector4Message(Vector4 value, string id)
		{
			this.value = value;
			this.id = id;
		}
	}

	public class Vector4ArrayMessage : MessageBase
	{
		public Vector4[] value;
		public string id;

		public Vector4ArrayMessage() { }

		public Vector4ArrayMessage(Vector4[] value, string id)
		{
			this.value = value;
			this.id = id;
		}
	}

	public class RectMessage : MessageBase
	{
		public Rect value;
		public string id;

		public RectMessage() { }

		public RectMessage(Rect value, string id)
		{
			this.value = value;
			this.id = id;
		}
	}

	public class RectArrayMessage : MessageBase
	{
		public Rect[] value;
		public string id;

		public RectArrayMessage() { }

		public RectArrayMessage(Rect[] value, string id)
		{
			this.value = value;
			this.id = id;
		}
	}

	public class ByteMessage : MessageBase
	{
		public byte value;
		public string id;

		public ByteMessage() { }

		public ByteMessage(byte value, string id)
		{
			this.value = value;
			this.id = id;
		}
	}

	public class ByteArrayMessage : MessageBase
	{
		public byte[] value;
		public string id;

		public ByteArrayMessage() { }

		public ByteArrayMessage(byte[] value, string id)
		{
			this.value = value;
			this.id = id;
		}
	}

	public class ColorMessage : MessageBase
	{
		public Color value;
		public string id;

		public ColorMessage() { }

		public ColorMessage(Color value, string id)
		{
			this.value = value;
			this.id = id;
		}
	}

	public class ColorArrayMessage : MessageBase
	{
		public Color[] value;
		public string id;

		public ColorArrayMessage() { }

		public ColorArrayMessage(Color[] value, string id)
		{
			this.value = value;
			this.id = id;
		}
	}

	public class Color32Message : MessageBase
	{
		public Color32 value;
		public string id;

		public Color32Message() { }

		public Color32Message(Color32 value, string id)
		{
			this.value = value;
			this.id = id;
		}
	}

	public class Color32ArrayMessage : MessageBase
	{
		public Color32[] value;
		public string id;

		public Color32ArrayMessage() { }

		public Color32ArrayMessage(Color32[] value, string id)
		{
			this.value = value;
			this.id = id;
		}
	}

	public class StringMessage : MessageBase
	{
		public string value;
		public string id;

		public StringMessage() { }

		public StringMessage(string value, string id)
		{
			this.value = value;
			this.id = id;
		}
	}

	public class StringArrayMessage : MessageBase
	{
		public string[] value;
		public string id;

		public StringArrayMessage() { }

		public StringArrayMessage(string[] value, string id)
		{
			this.value = value;
			this.id = id;
		}
	}

	public class RectByteArrayMessage : MessageBase
	{
		public Rect rect;
		public byte[] bytes;
		public string id;

		public RectByteArrayMessage() { }

		public RectByteArrayMessage(Rect rect, byte[] bytes, string id)
		{
			this.rect = rect;
			this.bytes = bytes;
			this.id = id;
		}
	}

	public class BoolMessage : MessageBase
	{
		public bool value;
		public string id;

		public BoolMessage() { }

		public BoolMessage(bool value, string id)
		{
			this.value = value;
			this.id = id;
		}
	}

	public class BoolArrayMessage : MessageBase
	{
		public bool[] value;
		public string id;

		public BoolArrayMessage() { }

		public BoolArrayMessage(bool[] value, string id)
		{
			this.value = value;
			this.id = id;
		}
	}
	#endregion
}
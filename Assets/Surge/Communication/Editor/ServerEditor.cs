/// <summary>
/// SURGE FRAMEWORK
/// Author: Bob Berkebile
/// Email: bobb@pixelplacement.com
///
/// Custom inspector for the Server.
///
/// </summary>

using UnityEditor;

namespace Pixelplacement
{
	[CustomEditor(typeof(Server))]
	public class ServerEditor : Editor
	{
		#region Private Variables
		Server _target;
		#endregion

		#region Init
		void OnEnable()
		{
			_target = target as Server;
		}
		#endregion

		#region GUI:
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(serializedObject.FindProperty("broadcastingPort"));
			EditorGUILayout.LabelField("Connection Port", (_target.broadcastingPort + 1).ToString());
			EditorGUILayout.PropertyField(serializedObject.FindProperty("maxConnections"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("initialBandwidth"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("customDeviceId"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("primaryQualityOfService"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("secondaryQualityOfService"));
			serializedObject.ApplyModifiedProperties();
		}
		#endregion
	}
}
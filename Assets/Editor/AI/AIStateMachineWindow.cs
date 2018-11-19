using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AIStateMachineWindow : EditorWindow
{
    private const string STATE_MACHINE_BACKGROUND_NAME = "Textures/StateMachineBackground";

    private float Scale { get => Target.CameraScale; set => Target.CameraScale = value; }
    private Vector2 CameraOffset { get => Target.CameraOffset; set => Target.CameraOffset = value; }
    private List<AIStateMachineNode> Nodes => Target.Nodes;

    private bool MiddleMouseDown { get; set; }
    private bool IsDragging => Event.current.type == EventType.MouseDrag;
    private Vector2 Center => new Vector2(position.width / 2, position.height / 2);
    private AIStateMachine Target { get; set; }

    private const float PIXELS_PER_UNIT = 32;
    private const float SCALE_COEFFICIENT = 0.1f;
    private const float SCALE_MAX = 10;
    private const float SCALE_MIN = 1;

    [MenuItem("Window/AI/State Machine", priority = 0)]
    private static void Init()
    {
        AIStateMachineWindow window = (AIStateMachineWindow)EditorWindow.GetWindow(typeof(AIStateMachineWindow));
        window.titleContent.text = "State Machine";
        window.Show();
    }
    public void OnEnable()
    {
        Selection.selectionChanged += QuerySelection;
        
        if(!Nodes.Any(x => x.GetType() == typeof(AIStateMachineStartNode)))
        {
            AIStateMachineStartNode startNode = ScriptableObject.CreateInstance<AIStateMachineStartNode>();
            Nodes.Add(startNode);
            AddObject(startNode);
        }
    }
    private void OnGUI()
    {
        using (var changeScope = new EditorGUI.ChangeCheckScope())
        {
            DrawBackground();

            if (Target == null)
            {
                EditorGUI.LabelField(new Rect(position.width / 2, position.height / 2, 200, 60), new GUIContent("No State Machine Selected"), Style.Text);
                return;
            }
            else
            {
                DrawNodes();
                PollInput();
            }

            if (changeScope.changed)
                EditorUtility.SetDirty(Target);
        }        
    }
    private void DrawNodes()
    {
        foreach (AIStateMachineNode node in Nodes)
        {
            DrawNode(node);
        }
    }
    private void DrawNode(AIStateMachineNode node)
    {
        Rect nodeRect = GetObjectRect(node.Position, node.Size);

        node.Draw(nodeRect);

        if(Event.current.type == EventType.MouseDown && nodeRect.Contains(Event.current.mousePosition))
        {
            SelectNode(node);
        }
    }
    private void SelectNode(AIStateMachineNode node)
    {
        Selection.activeObject = node;
    }
    private Rect GetObjectRect(Vector2 position, Vector2 size)
    {
        Vector2 sizeInPixels = new Vector2()
        {
            x = size.x * PIXELS_PER_UNIT * Scale,
            y = size.y * PIXELS_PER_UNIT * Scale,
        };

        Vector2 screenPoint = WorldToScreenPoint(position);
        Vector2 positionInPixels = new Vector2()
        {
            x = screenPoint.x - sizeInPixels.x / 2,
            y = screenPoint.y - sizeInPixels.y / 2,
        };

        return new Rect()
        {
            x = positionInPixels.x,
            y = positionInPixels.y,

            width = sizeInPixels.x,
            height = sizeInPixels.y,
        };
    }
    private void DrawBackground()
    {
        if(Event.current.type == EventType.Repaint)
        {
            Style.Background.Draw(new Rect(0, 0, position.width, position.height), GUIContent.none, 0);
        }            
    }
    private void QuerySelection()
    {
        AIStateMachine targetMachine = Selection.activeObject as AIStateMachine;

        if (targetMachine != null)
            Target = targetMachine;

        Repaint();
    }
    private void PollInput()
    {
        PollMouseInput();
        //PollScale();
        PollCameraOffset();
        PollCreateNewState();
        PollDeleteState();
    }
    private void PollDeleteState()
    {
        if(Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Delete)
        {
            AIStateMachineNode selectedNode = Selection.activeObject as AIStateMachineNode;

            if (selectedNode != null)
            {
                if (Nodes.Contains(selectedNode))
                {
                    Nodes.Remove(selectedNode);
                    RemoveObject(selectedNode);
                }
            }
        }
    }
    private void PollMouseInput()
    {
        PollMiddleMouseInput();
    }
    private void PollMiddleMouseInput()
    {
        Event e = Event.current;

        if (e.type == EventType.MouseDown && e.button == 2)
            MiddleMouseDown = true;

        if (e.type == EventType.MouseUp && e.button == 2)
            MiddleMouseDown = false;
    }
    private void PollCreateNewState()
    {
        Event e = Event.current;

        if (e.type == EventType.MouseDown && e.button == 1)
            CreateNewState();
    }
    private void CreateNewState()
    {
        AIStateMachineStateNode newState = ScriptableObject.CreateInstance<AIStateMachineStateNode>();
        newState.Position = ScreenToWorldPoint(Event.current.mousePosition);

        Nodes.Add(newState);
        AddObject(newState);
    }
    private void PollScale()
    {
        Event e = Event.current;

        if (e.isScrollWheel)
        {
            Scale = Mathf.Clamp(Scale - e.delta.y * SCALE_COEFFICIENT, SCALE_MIN, SCALE_MAX);

            Repaint();
        }
    }
    private void PollCameraOffset()
    {
        if (MiddleMouseDown && Event.current.type == EventType.MouseDrag)
        {
            CameraOffset += Event.current.delta;

            Repaint();
        }
    }
    private Vector2 WorldToScreenPoint(Vector2 worldPoint)
    {
        return new Vector2()
        {
            x = Center.x + CameraOffset.x + worldPoint.x * PIXELS_PER_UNIT * Scale,
            y = Center.y + CameraOffset.y + worldPoint.y * PIXELS_PER_UNIT * Scale,
        };
    }
    private Vector2 ScreenToWorldPoint(Vector2 screenPosition)
    {
        return (screenPosition - Center - CameraOffset) / PIXELS_PER_UNIT / Scale;
    }
    private void AddObject(Object obj)
    {
        obj.name = obj.GetType().Name;

        AssetDatabase.AddObjectToAsset(obj, Target);
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(obj));
        AssetDatabase.SaveAssets();
    }
    private void RemoveObject(Object obj)
    {
        DestroyImmediate(obj, true);
        AssetDatabase.SaveAssets();
    }

    private static class Style
    {
        public static GUIStyle Background;
        public static GUIStyle Text;

        static Style()
        {
            Background = new GUIStyle();
            Background.normal = new GUIStyleState();
            Background.normal.background = Resources.Load<Texture2D>(STATE_MACHINE_BACKGROUND_NAME);
            
            Text = new GUIStyle();
            Text.normal.textColor = Color.white;
        }
    }
}

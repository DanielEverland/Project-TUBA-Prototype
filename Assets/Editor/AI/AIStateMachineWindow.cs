using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AIStateMachineWindow : EditorWindow
{
    private const string STATE_MACHINE_BACKGROUND_NAME = "Textures/StateMachineBackground";

    private AIStateMachine Target { get; set; }
    
    private Vector2 CameraOffset { get => Target.CameraOffset; set => Target.CameraOffset = value; }
    private List<AIStateMachineNode> Nodes => Target.Nodes;
    private List<AIStateMachineTransition> Transitions => Target.Transitions;
    private AIStateMachineStartNode StartNode { get => Target.StartNode; set => Target.StartNode = value; }

    private bool MiddleMouseDown { get; set; }
    private bool IsDragging => Event.current.type == EventType.MouseDrag;
    private Vector2 Center => new Vector2(position.width / 2, position.height / 2);
    private AIStateMachineNode DraggedObject { get; set; }
    private AIStateMachineTransition TransitionBeingPlaced { get; set; }

    private const float PIXELS_PER_UNIT = 32;
    private const float SCALE_COEFFICIENT = 0.1f;
    private const float SCALE_MAX = 10;
    private const float SCALE_MIN = 1;
    private const float TRANSITION_HEIGHT = 0.5f;

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
    }
    private void OnGUI()
    {
        using (var changeScope = new EditorGUI.ChangeCheckScope())
        {
            DrawBackground();

            if (Target == null)
            {
                EditorGUI.LabelField(new Rect(position.width / 2 - 100, position.height / 2, 200, 60), new GUIContent("No State Machine Selected"), Style.Text);
                return;
            }
            else
            {
                QueryStartNode();
                DrawTransitions();
                DrawNodes();
                PollInput();
            }

            if (changeScope.changed)
                EditorUtility.SetDirty(Target);
        }        
    }
    private void DrawTransitions()
    {
        HandleTransitionBeingPlaced();

        foreach (AIStateMachineTransition transition in Transitions)
        {
            DrawTransition(transition);
        }
    }
    private void DrawTransition(AIStateMachineTransition transition)
    {
        Vector2 startPos = WorldToScreenPoint(transition.StartPosition);
        Vector2 endPos = WorldToScreenPoint(transition.EndPosition);
        Vector2 delta = endPos - startPos;
        Vector2 middlePoint = startPos + delta / 2;

        float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
        Vector2 size = new Vector2(delta.magnitude, TRANSITION_HEIGHT * PIXELS_PER_UNIT);
        Rect rect = new Rect(middlePoint - size / 2, size);
        
        EditorGUIUtility.RotateAroundPivot(angle, middlePoint);

        transition.Draw(rect);

        GUI.matrix = Matrix4x4.identity;
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

        if(Event.current.type == EventType.MouseDown && Event.current.button == 0 && nodeRect.Contains(Event.current.mousePosition))
        {
            SelectNode(node);
        }
    }
    private void SelectNode(AIStateMachineNode node)
    {
        Selection.activeObject = node;
    }
    private Rect GetObjectRect(AIStateMachineNode node)
    {
        return GetObjectRect(node.Position, node.Size);
    }
    private Rect GetObjectRect(Vector2 position, Vector2 size)
    {
        Vector2 sizeInPixels = new Vector2()
        {
            x = size.x * PIXELS_PER_UNIT,
            y = size.y * PIXELS_PER_UNIT,
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
    private void QueryStartNode()
    {
        if (!EditorUtility.IsPersistent(Target))
            return;

        if (!Nodes.Any(x => x.GetType() == typeof(AIStateMachineStartNode)))
        {
            AIStateMachineStartNode startNode = ScriptableObject.CreateInstance<AIStateMachineStartNode>();
            Nodes.Add(startNode);
            AddObject(startNode);
            StartNode = startNode;
        }
    }
    private void PollInput()
    {
        PollMouseInput();
        PollCameraOffset();
        PollCreateNewState();
        PollDeleteState();
        PollNodeDrag();
        PollDisableSelection();
        PollCreateNewTransition();
        PollPlaceTransition();
        PollSelectTransition();
    }
    private void PollSelectTransition()
    {
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            if(Selection.activeObject is AIStateMachineTransition)
                Selection.activeObject = null;

            foreach (AIStateMachineTransition transition in Transitions)
            {
                Vector2 position = transition.StartPosition + (transition.EndPosition - transition.StartPosition) / 2;
                Rect rect = GetObjectRect(position, Vector2.one);

                if (rect.Contains(Event.current.mousePosition))
                {
                    Selection.activeObject = transition;
                }
            }
        }            
    }
    private void PollPlaceTransition()
    {
        if (TransitionBeingPlaced == null)
            return;

        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            foreach (AIStateMachineNode node in Nodes)
            {
                if (node is AIStateMachineStateNode state)
                {
                    Rect rect = GetObjectRect(node);

                    if (rect.Contains(Event.current.mousePosition))
                    {
                        TransitionBeingPlaced.TargetState = state;
                        TransitionBeingPlaced = null;

                        return;
                    }
                }
            }
        }
    }
    private void HandleTransitionBeingPlaced()
    {
        if (TransitionBeingPlaced == null)
            return;

        TransitionBeingPlaced.EndPosition = ScreenToWorldPoint(Event.current.mousePosition);
        Repaint();
    }
    private void PollCreateNewTransition()
    {
        if (TransitionBeingPlaced != null)
            return;

        if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
        {
            foreach (AIStateMachineNode node in Nodes)
            {
                Rect rect = GetObjectRect(node);

                if (rect.Contains(Event.current.mousePosition))
                {
                    AIStateMachineTransition transition = ScriptableObject.CreateInstance<AIStateMachineTransition>();
                    transition.StartNode = node;
                    node.Transitions.Add(transition);

                    AddObject(transition);
                    Transitions.Add(transition);

                    TransitionBeingPlaced = transition;

                    return;
                }
            }
        }
    }
    private void PollDisableSelection()
    {
        AIStateMachineNode selectedNode = Selection.activeObject as AIStateMachineNode;

        if (selectedNode == null || !Nodes.Contains(selectedNode))
            return;

        Rect nodeRect = GetObjectRect(selectedNode);

        if (Event.current.type == EventType.MouseDown && !nodeRect.Contains(Event.current.mousePosition))
        {
            Selection.activeObject = null;
        }
    }
    private void PollNodeDrag()
    {
        AIStateMachineNode selectedNode = Selection.activeObject as AIStateMachineNode;

        if (selectedNode == null || !Nodes.Contains(selectedNode))
            return;

        Rect nodeRect = GetObjectRect(selectedNode);

        if (Event.current.type == EventType.MouseDown && nodeRect.Contains(Event.current.mousePosition))
        {
            DraggedObject = selectedNode;
        }
        else if(Event.current.type == EventType.MouseDown)
        {
            DraggedObject = null;
        }

        if (Event.current.type == EventType.MouseDrag && DraggedObject == selectedNode)
        {
            selectedNode.Position += Event.current.delta / PIXELS_PER_UNIT;
            Repaint();
        }
    }
    private void PollDeleteState()
    {
        if(Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Delete)
        {
            AIStateMachineObject selectedObject = Selection.activeObject as AIStateMachineObject;

            if (selectedObject != null)
            {
                if(selectedObject is AIStateMachineStateNode state)
                {
                    DeleteState(state);
                }
                else if(selectedObject is AIStateMachineTransition transition)
                {
                    DeleteTransition(transition);
                }
            }
        }
    }
    private void DeleteState(AIStateMachineStateNode state)
    {
        if (Nodes.Contains(state))
        {
            Nodes.Remove(state);
            RemoveObject(state);
        }

        for (int i = Transitions.Count - 1; i >= 0; i--)
        {
            AIStateMachineTransition transition = Transitions[i];

            if (transition.StartNode == state || transition.TargetState == state)
            {
                DeleteTransition(transition);
            }
        }
    }
    private void DeleteTransition(AIStateMachineTransition transition)
    {
        if (Transitions.Contains(transition))
        {
            transition.StartNode.Transitions.Remove(transition);

            Transitions.Remove(transition);
            RemoveObject(transition);
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
        {
            foreach (AIStateMachineNode node in Nodes)
            {
                Rect nodeRect = GetObjectRect(node);

                if (nodeRect.Contains(Event.current.mousePosition))
                    return;
            }

            CreateNewState();
        }
    }
    private void CreateNewState()
    {
        AIStateMachineStateNode newState = ScriptableObject.CreateInstance<AIStateMachineStateNode>();
        newState.Position = ScreenToWorldPoint(Event.current.mousePosition);

        Nodes.Add(newState);
        AddObject(newState);
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
            x = Center.x + CameraOffset.x + worldPoint.x * PIXELS_PER_UNIT,
            y = Center.y + CameraOffset.y + worldPoint.y * PIXELS_PER_UNIT,
        };
    }
    private Vector2 ScreenToWorldPoint(Vector2 screenPosition)
    {
        return (screenPosition - Center - CameraOffset) / PIXELS_PER_UNIT;
    }
    private void AddObject(Object obj)
    {
        obj.name = obj.GetType().Name;

        AssetDatabase.AddObjectToAsset(obj, Target);
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(obj));
    }
    private void RemoveObject(Object obj)
    {
        DestroyImmediate(obj, true);
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

using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AIStateMachineWindow : EditorWindow
{
    private const string STATE_MACHINE_BACKGROUND_NAME = "Textures/StateMachineBackground";
    private const string STATE_MACHINE_HEADER_NAME = "Textures/StateMachineHeaderBackground";

    private AIStateMachine Target { get; set; }
    
    private Vector2 CameraOffset { get => Target.CameraOffset; set => Target.CameraOffset = value; }
    private List<AINode> Nodes => Target.Nodes;
    private List<AITransition> Transitions => Target.Transitions;
    private AIStartNode StartNode { get => Target.StartNode; set => Target.StartNode = value; }

    private bool MiddleMouseDown { get; set; }
    private bool IsDragging => Event.current.type == EventType.MouseDrag;
    private Vector2 Center => new Vector2(position.width / 2, position.height / 2);
    private AINode DraggedObject { get; set; }
    private AITransition TransitionBeingPlaced { get; set; }

    private const float PIXELS_PER_UNIT = 32;
    private const float SCALE_COEFFICIENT = 0.1f;
    private const float SCALE_MAX = 10;
    private const float SCALE_MIN = 1;
    private const float TRANSITION_HEIGHT = 0.5f;
    private const float RENDERING_ROUND_TO_NEAREST = 8;
    private const float NODE_SIDE_PADDING = 10;

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
    private void Update()
    {
        Repaint();
        QuerySelection();
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
                DrawHeader();
            }

            if (changeScope.changed)
                EditorUtility.SetDirty(Target);
        }        
    }
    private void DrawTransitions()
    {
        HandleTransitionBeingPlaced();

        foreach (AITransition transition in Transitions)
        {
            DrawTransition(transition);
        }
    }
    private void DrawTransition(AITransition transition)
    {
        Vector2 startPos = WorldToScreenPoint(transition.StartPosition).RoundToNearest(RENDERING_ROUND_TO_NEAREST);
        Vector2 endPos = WorldToScreenPoint(transition.EndPosition).RoundToNearest(RENDERING_ROUND_TO_NEAREST);
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
        foreach (AINode node in Nodes)
        {
            DrawNode(node);
        }
    }
    private void DrawNode(AINode node)
    {
        Rect nodeRect = GetObjectRect(node);
        node.Draw(nodeRect);

        if(Event.current.type == EventType.MouseDown && Event.current.button == 0 && nodeRect.Contains(Event.current.mousePosition))
        {
            SelectNode(node);
        }
    }
    private void SelectNode(AINode node)
    {
        Selection.activeObject = node;
    }
    private Rect GetObjectRect(AINode node)
    {
        return GetObjectRect(node.Position, node.MinSize, node.Title, node.TextStyle);
    }
    private Rect GetObjectRect(Vector2 position, Vector2 size)
    {
        return GetObjectRect(position, size, string.Empty, null);
    }
    private Rect GetObjectRect(Vector2 position, Vector2 size, string content, GUIStyle style)
    {
        Vector2 minSizeInPixels = new Vector2()
        {
            x = size.x * PIXELS_PER_UNIT,
            y = size.y * PIXELS_PER_UNIT,
        };

        Vector2 preferredSizeInPixels = new Vector2();
        if (style != null)
        {
            preferredSizeInPixels = new Vector2()
            {
                x = style.CalcSize(new GUIContent(content)).x + NODE_SIDE_PADDING * 2,
                y = size.y * PIXELS_PER_UNIT,
            };
        }            

        Vector2 sizeInPixels = Vector2.Max(minSizeInPixels, preferredSizeInPixels);

        Vector2 screenPoint = WorldToScreenPoint(position);
        Vector2 positionInPixels = new Vector2()
        {
            x = screenPoint.x - sizeInPixels.x / 2,
            y = screenPoint.y - sizeInPixels.y / 2,
        };

        return new Rect()
        {
            x = positionInPixels.x.RoundToNearest(RENDERING_ROUND_TO_NEAREST),
            y = positionInPixels.y.RoundToNearest(RENDERING_ROUND_TO_NEAREST),

            width = sizeInPixels.x,
            height = sizeInPixels.y,
        };
    }
    private void DrawHeader()
    {
        if (Event.current.type == EventType.Repaint)
        {
            float width = GUIStyle.none.CalcSize(new GUIContent(Target.name)).x;
            Rect rect = new Rect()
            {
                width = width + 20,
                height = 25,
            };

            Style.Header.Draw(rect, GUIContent.none, 0);
            EditorGUI.LabelField(rect, new GUIContent(Target.name), Style.Text);
        }
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
        if(Selection.activeObject is AIStateMachine machine)
        {
            Target = machine;
        }
        else if(Selection.activeObject is GameObject gameObject)
        {
            Agent agent = gameObject.GetComponentInChildren<Agent>();

            if(agent != null)
                Target = agent.StateMachine;
        }

        Repaint();
    }
    private void QueryStartNode()
    {
        if (!EditorUtility.IsPersistent(Target))
            return;

        if (!Nodes.Any(x => x.GetType() == typeof(AIStartNode)))
        {
            AIStartNode startNode = ScriptableObject.CreateInstance<AIStartNode>();
            startNode.Position = new Vector2(0, 10);

            Nodes.Add(startNode);
            AddObject(startNode);
            StartNode = startNode;

            startNode.name = "Start Node";
        }
    }
    private void PollInput()
    {
        PollMouseInput();
        PollCameraOffset();
        PollDeleteState();
        PollNodeDrag();
        PollDisableSelection();
        PollSelectTransition();
        PollPlaceTransition();

        if (PollCreateNewState())
            return;
        else if (PollCreateNewTransition())
            return;
    }
    private void PollSelectTransition()
    {
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            if(Selection.activeObject is AITransition)
                Selection.activeObject = null;

            foreach (AITransition transition in Transitions)
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
            foreach (AINode node in Nodes)
            {
                if (node is AIState state && node != TransitionBeingPlaced.StartNode)
                {
                    Rect rect = GetObjectRect(node);

                    if (rect.Contains(Event.current.mousePosition))
                    {
                        PlaceTransition(state);

                        return;
                    }
                }
            }

            ClearTransition();
        }
    }
    private void ClearTransition()
    {
        DeleteTransition(TransitionBeingPlaced);
        TransitionBeingPlaced = null;
    }
    private void PlaceTransition(AIState targetState)
    {
        TransitionBeingPlaced.TargetState = targetState;
        TransitionBeingPlaced = null;
    }
    private void HandleTransitionBeingPlaced()
    {
        if (TransitionBeingPlaced == null)
            return;

        TransitionBeingPlaced.EndPosition = ScreenToWorldPoint(Event.current.mousePosition);
        Repaint();
    }
    private bool PollCreateNewTransition()
    {
        if (TransitionBeingPlaced != null)
            return false;

        if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
        {
            foreach (AINode node in Nodes)
            {
                Rect rect = GetObjectRect(node);

                if (rect.Contains(Event.current.mousePosition))
                {
                    AITransition transition = ScriptableObject.CreateInstance<AITransition>();
                    transition.StartNode = node;
                    node.Transitions.Add(transition);

                    AddObject(transition);
                    Transitions.Add(transition);

                    TransitionBeingPlaced = transition;

                    return true;
                }
            }
        }

        return false;
    }
    private void PollDisableSelection()
    {
        AINode selectedNode = Selection.activeObject as AINode;

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
        AINode selectedNode = Selection.activeObject as AINode;

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
            EditorUtility.SetDirty(selectedNode);
        }
    }
    private void PollDeleteState()
    {
        if(Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Delete)
        {
            AIStateMachineObject selectedObject = Selection.activeObject as AIStateMachineObject;

            if (selectedObject != null)
            {
                if(selectedObject is AIState state)
                {
                    DeleteState(state);
                }
                else if(selectedObject is AITransition transition)
                {
                    DeleteTransition(transition);
                }
            }
        }
    }
    private void DeleteState(AIState state)
    {
        if (Nodes.Contains(state))
        {
            Nodes.Remove(state);
            RemoveObject(state);
        }

        for (int i = Transitions.Count - 1; i >= 0; i--)
        {
            AITransition transition = Transitions[i];

            if (transition.StartNode == state || transition.TargetState == state)
            {
                DeleteTransition(transition);
            }
        }
    }
    private void DeleteTransition(AITransition transition)
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
    private bool PollCreateNewState()
    {
        Event e = Event.current;

        if (e.type == EventType.MouseDown && e.button == 1)
        {
            foreach (AINode node in Nodes)
            {
                Rect nodeRect = GetObjectRect(node);

                if (nodeRect.Contains(Event.current.mousePosition))
                    return false;
            }

            CreateNewState();
            return true;
        }

        return false;
    }
    private void CreateNewState()
    {
        AIState newState = ScriptableObject.CreateInstance<AIState>();
        newState.Position = ScreenToWorldPoint(Event.current.mousePosition);
        newState.name = "New State";

        Nodes.Add(newState);
        AddObject(newState);

        if (TransitionBeingPlaced != null)
            PlaceTransition(newState);
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
        if(obj is AIStateMachineObject stateObject)
        {
            stateObject.Machine = Target;
        }

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
        public static GUIStyle Header;
        public static GUIStyle Background;
        public static GUIStyle Text;

        static Style()
        {
            Header = new GUIStyle();
            Header.normal.background = Resources.Load<Texture2D>(STATE_MACHINE_HEADER_NAME);
            Header.border = new RectOffset(1, 1, 1, 1);

            Background = new GUIStyle();
            Background.normal = new GUIStyleState();
            Background.normal.background = Resources.Load<Texture2D>(STATE_MACHINE_BACKGROUND_NAME);
            
            Text = new GUIStyle();
            Text.normal.textColor = new Color(0.8f, 0.8f, 0.8f, 1);
            Text.alignment = TextAnchor.MiddleCenter;
        }
    }
}

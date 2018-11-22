using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
    public static partial class EditorGUILayoutHelper
    {
        private const float kImageSectionWidth = 44;
        private const float PADDING_TOP = 2;
        private const float PADDING_LEFT = 35;
        private const float PADDING_RIGHT = 60;
        private const float HEIGHT = 20;

        private const float LABEL_Y_OFFSET = 4;
        private const float LABEL_X_OFFSET = 44;

        public static string DrawHeaderTextField(string text)
        {
            Rect overlayRect = new Rect()
            {
                x = PADDING_LEFT,
                y = PADDING_TOP,
                width = Screen.width - (PADDING_LEFT + PADDING_RIGHT),
                height = HEIGHT,
            };

            if (Event.current.type == EventType.Repaint)
                Styles.inspectorBigInner.Draw(overlayRect, GUIContent.none, 0);

            Rect labelRect = new Rect()
            {
                x = LABEL_X_OFFSET,
                y = LABEL_Y_OFFSET,
                width = overlayRect.width,
                height = overlayRect.height,
            };
            
            return EditorGUI.TextField(labelRect, text, Styles.textFieldHeader);
        }

        static class Styles
        {
            public static readonly GUIContent open = EditorGUIUtility.TrTextContent("Open");
            public static readonly GUIStyle textFieldHeader = new GUIStyle("LargeLabel");
            public static readonly GUIStyle inspectorBig = new GUIStyle("In BigTitle");
            public static readonly GUIStyle inspectorBigInner = "IN BigTitle inner";
            public static readonly GUIStyle centerStyle = new GUIStyle();
            public static readonly GUIStyle postLargeHeaderBackground = "IN BigTitle Post";

            static Styles()
            {
                centerStyle.alignment = TextAnchor.MiddleCenter;
                // modify header bottom padding on a mutable copy here
                // this was done rather than modifying the style asset itself in order to minimize possible side effects where the style was already used
                inspectorBig.padding.bottom -= 1;
                
                textFieldHeader.active = EditorStyles.textField.active;
            }
        }
    }
}

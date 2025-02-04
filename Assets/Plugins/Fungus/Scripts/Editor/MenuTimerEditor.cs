﻿// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEditor;
using UnityEngine;

namespace Fungus.EditorUtils
{
    [CustomEditor (typeof(MenuTimer))]
    public class MenuTimerEditor : CommandEditor 
    {
        protected SerializedProperty durationProp;
        protected SerializedProperty targetBlockProp;

        public override void OnEnable()
        {
            base.OnEnable();

            durationProp = serializedObject.FindProperty("_duration");
            targetBlockProp = serializedObject.FindProperty("targetBlock");
        }
        
        public override void DrawCommandGUI()
        {
            var flowchart = FlowchartWindow.GetFlowchart();
            if (flowchart == null)
            {
                return;
            }
            
            serializedObject.Update();
            
            EditorGUILayout.PropertyField(durationProp);
            
            BlockEditor.BlockField(targetBlockProp,
                                         new GUIContent("Target Block", "Block to call when timer expires"), 
                                         new GUIContent("<None>"), 
                                         flowchart);
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}

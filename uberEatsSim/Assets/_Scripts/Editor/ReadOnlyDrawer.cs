using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Save the current GUI state (to restore it later)
        GUI.enabled = false;

        // Draw the property field normally, but with GUI.enabled = false
        // This makes the field look grayed out and uneditable.
        EditorGUI.PropertyField(position, property, label, true);

        // Restore the GUI state to its previous setting
        GUI.enabled = true;
    }

    // Ensure the property takes up the correct amount of space
    //public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    //{
    //    return EditorGUI.GetPropertyHeight(property, label, true);
    //}
}

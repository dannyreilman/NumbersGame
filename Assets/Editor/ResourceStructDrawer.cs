using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

[CustomPropertyDrawer(typeof(ResourceStruct))]
public class ResourceStructDrawer : PropertyDrawer
{
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        // Properly configure height for expanded contents.
		SerializedProperty resourceArray = property.FindPropertyRelative("resourceArray");
        return EditorGUI.GetPropertyHeight(resourceArray, label, resourceArray.isExpanded);
    }

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		SerializedProperty resourceArray = property.FindPropertyRelative("resourceArray");
		EditorGUI.PropertyField(position, resourceArray, label, true);
	}
}

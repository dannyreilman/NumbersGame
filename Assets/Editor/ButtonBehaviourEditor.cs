using UnityEngine;
using System.Collections;
using UnityEditor;

/**
 * Add prop variables, onenable initializations, and drawPropertiesexcluding to add enum values
 */

[CustomEditor(typeof(ButtonBehaviour))]
public class ButtonBehaviourEditor : Editor 
{
    //Flat props
    SerializedProperty flatUpProp;

    //Width props
    SerializedProperty widthUpProp;

    void OnEnable () 
    {
        flatUpProp = serializedObject.FindProperty ("flatUpgradeAmount");
        widthUpProp = serializedObject.FindProperty ("widthIncrease");
    }

    public override void OnInspectorGUI()
    {
        ButtonBehaviour myTarget = (ButtonBehaviour)target;
        
        DrawPropertiesExcluding(serializedObject, new string[]{ "flatUpgradeAmount", "widthIncrease" });
        EditorGUI.BeginChangeCheck();

        switch(myTarget.upgradeType)
        {
            case ButtonBehaviour.UpgradeType.None:
                //Print nothing
            break;
            case ButtonBehaviour.UpgradeType.Flat:
                EditorGUILayout.PropertyField (flatUpProp, true);
            break;
 			case ButtonBehaviour.UpgradeType.Width:
                EditorGUILayout.PropertyField (widthUpProp, true);
            break;
            default:
                Debug.Log("UpgradeType not implemented in custom inspector");
            break;
        }

        // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
        serializedObject.ApplyModifiedProperties ();

    }
}


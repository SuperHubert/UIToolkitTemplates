using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

[CustomEditor(typeof(ScriptableSettings))]
public class ScriptableSettingsEditor : Editor
{
    public VisualTreeAsset inspectorUxml;
    
    public override VisualElement CreateInspectorGUI()
    {
        var root = new VisualElement();
        inspectorUxml.CloneTree(root);
        
        var foldout = new Foldout() {viewDataKey = "ScriptableSettingsInspectorFoldout", text = "Full Inspector"};
        
        InspectorElement.FillDefaultInspector(foldout, serializedObject,this);
        
        var loadButton = root.Q<Button>("load-button");
        loadButton.RegisterCallback<ClickEvent>(evt => ((ScriptableSettings)target).LoadSettings());
        
        var saveButton = root.Q<Button>("save-button");
        saveButton.RegisterCallback<ClickEvent>(evt => ((ScriptableSettings)target).SaveSettings());
        
        root.Add(foldout);
        return root;
    }
}

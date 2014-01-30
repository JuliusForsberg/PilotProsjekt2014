using UnityEditor;

[CustomEditor(typeof(TrapPlace))]
public class GridInspector : Editor
{

    public void OnInspectorGUI()
    {
        TrapPlace targetScript = (TrapPlace)target;
        targetScript.gridSizeX = EditorGUILayout.IntSlider("Val-you", targetScript.gridSizeX, 1, 10);
        //targetScript.gridSizeX = EditorGUILayout.IntField("Total Grid Squares X", targetScript.gridSizeX);
    }
}
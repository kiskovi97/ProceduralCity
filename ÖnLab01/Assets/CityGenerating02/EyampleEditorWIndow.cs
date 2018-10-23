using UnityEngine;
using UnityEditor;

public class EyampleEditorWIndow : EditorWindow {

    string myString = "Hello World";
    
    [MenuItem("Window/Example")]
	public static void ShowWindow()
    {
        GetWindow<EyampleEditorWIndow>("Example");
    }

    private void OnGUI()
    {
        // Window Code
        GUILayout.Label("THis is a Label", EditorStyles.boldLabel);
        GUILayout.Space(20);
        myString = EditorGUILayout.TextField("Name", myString);

        if (GUILayout.Button("Press Me"))
        {
            myString = "Pressed";
            for (int x = 0; x != 10; x++)
            {
                GameObject go = new GameObject("MyCreatedGO" + x);
                go.transform.position = new Vector3(x, 0, 0);
            }
        }
    }
}

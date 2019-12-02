using UnityEditor;
using UnityEngine;
using Runner.Control;

public class EditorGUISidiaTest : EditorWindow
{
    public GameObject objectSelected = null;
    public static float valueFloat = 0.0f;
    public SwipeManager customType = null;
    public Vector3 valueDistance = new Vector3(0, 0, 0);

    [MenuItem("Sidia Test/Tier 4")]
    static void Init()
    {
        EditorWindow window = GetWindow<EditorGUISidiaTest>("Tier 4");
        window.position = new Rect(0, 0, 300, 300);
        window.Show();
    }

    void OnInspectorUpdate()
    {
        Repaint();
    }

    [System.Obsolete]
    void OnGUI()
    {
        // GameObject
        GUILayout.Label("Selection GameObject");
        objectSelected = (GameObject)EditorGUILayout.ObjectField(objectSelected, typeof(GameObject));
        if (objectSelected)
            if (GUILayout.Button("Find Object"))
                Selection.objects = EditorUtility.CollectDependencies(new GameObject[] { objectSelected });
        EditorGUILayout.Space();

        // Float
        GUILayout.Label("Value Float");
        valueFloat = EditorGUILayout.Slider(valueFloat, 0, 1);
        EditorGUILayout.Space();

        //Script
        GUILayout.Label("Selection SwipeManager");
        customType = (SwipeManager)EditorGUILayout.ObjectField(customType, typeof(SwipeManager));
        if (customType)
            if (GUILayout.Button("Find SwipeManager"))
                Selection.objects = EditorUtility.CollectDependencies(new SwipeManager[] { customType });
        EditorGUILayout.Space();

        //Sorting GameObject
        GUILayout.Label("Sorting GameObject");
        valueDistance = EditorGUILayout.Vector3Field("Distance between Obj:", valueDistance);
        if (GUILayout.Button("Sorting Selected GameObjects"))
        {
            GameObject[] gameObjectsSelected = Selection.gameObjects;
            if (gameObjectsSelected != null && gameObjectsSelected.Length > 0)
            {
                Vector3 startPosition = gameObjectsSelected[0].transform.position;
                Vector3 startPositionScale = gameObjectsSelected[0].transform.localScale;
                Debug.Log(startPositionScale);

                for (int i = 1; i < gameObjectsSelected.Length; i++)
                {
                    gameObjectsSelected[i].transform.position = new Vector3(startPosition.x + startPositionScale.x / 2 + valueDistance.x,
                        startPosition.y + startPositionScale.y / 2 + valueDistance.y,
                        startPosition.z + startPositionScale.z / 2 + valueDistance.z);

                    startPosition = gameObjectsSelected[i].transform.position;
                    startPositionScale = gameObjectsSelected[i].transform.localScale;
                }
            }
        }
    }


}
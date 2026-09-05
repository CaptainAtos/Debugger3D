using UnityEngine;
using UnityEditor;

public class DungoenGenarateEditorTool : EditorWindow
{
    private DungeonGenerator targetGenerator;
    private int seed;

    [MenuItem("Tools/Dungeon Generator")]
    public static void ShowWindow()
    {
        GetWindow<DungoenGenarateEditorTool>("Dungeon Generator");
    }

    private void OnGUI()
    {
        targetGenerator = (DungeonGenerator)EditorGUILayout.ObjectField(
            "Dungeon Generator", targetGenerator, typeof(DungeonGenerator), true);

        if (targetGenerator == null)
        {
            EditorGUILayout.HelpBox("Bitte DungeonGenerator.cs zuweisen, um das Tool nutzen zu können.", MessageType.Info);
            return;
        }

        seed = EditorGUILayout.IntField("Seed", seed);
        targetGenerator.maxRooms = EditorGUILayout.IntField("Max Rooms", targetGenerator.maxRooms);

        if (GUILayout.Button("Zufälligen Seed würfeln"))
        {
            seed = Random.Range(0, int.MaxValue);
        }

        if (GUILayout.Button("Generate"))
        {
            targetGenerator.GameManager(seed);
        }

        EditorGUILayout.HelpBox("Sollte ein passender Seed gefunden werden, notieren und manuell ins den DungeonGenerator übertragen", MessageType.Info);
    }
}
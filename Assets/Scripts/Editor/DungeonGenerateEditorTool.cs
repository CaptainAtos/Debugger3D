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
        targetGenerator.maxRooms = EditorGUILayout.IntSlider("Max Rooms", targetGenerator.maxRooms, 20, 100);

        if (GUILayout.Button("Zufälligen Seed würfeln"))
        {
            targetGenerator.seed = Random.Range(0, int.MaxValue);
            seed = targetGenerator.seed;
        }

        EditorGUILayout.HelpBox(" Alle Daten werden mit dem Dungeongenerator.cs synchronisiert\n Keine Manuelle Übernahme nötig", MessageType.Info);

        if (GUILayout.Button("Generate"))
        {
            targetGenerator.GameManager(seed);
        }

        if (GUILayout.Button("Delete Dungeon"))
        {
            targetGenerator.ClearDungeon();
        }
    }
}
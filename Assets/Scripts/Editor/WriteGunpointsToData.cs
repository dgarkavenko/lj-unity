using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class WriteGunpointsToData : EditorWindow {

    [MenuItem("LJUtils/Gunpoints")]
    public static void OpenWindow()
    {
        EditorWindow.GetWindow(typeof(WriteGunpointsToData), true, "Gunpoints", true);
    }

    public GameObject source;
    public int gunId = 0;

    void OnGUI()
    {
        source = (GameObject) EditorGUILayout.ObjectField(source, typeof(Object), true);

        if (source != null)
        {
            GUILayout.BeginHorizontal();
            gunId = EditorGUILayout.IntField(gunId);
            if(GUILayout.Button("Write")){

                var guns = GameplayData.Instance.guns;
                if (guns.Length > gunId)
                {                    

                    List<Vector2> positions = new List<Vector2>();
                    
                    foreach (Transform t in source.transform)
                    {
                        positions.Add(t.localPosition);
                    }

                    positions.Sort((a, b) => -1 * a.y.CompareTo(b.y));
                    guns[gunId].gunpoints = positions.ToArray();

                }
                else
                {
                    Debug.LogWarning(string.Format("Gun ID{0} not found", gunId));
                }

            }
            GUILayout.EndHorizontal();
        }
    }
}

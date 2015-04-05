using UnityEngine;
using System.Collections;
using UnityEditor;

public class GameplayData : ScriptableObject {

    static GameplayData instance;

    public static GameplayData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load("Data/Config") as GameplayData;
    
                if (instance == null)
                {
                    instance = ScriptableObject.CreateInstance<GameplayData>();
                    AssetDatabase.CreateAsset(instance, "Assets/Resources/Data/Config.asset");
                    AssetDatabase.SaveAssets();
                }
            }		

            return instance;
        }
    }

    public GunData[] guns;
	public ToolData[] tools;

    
}

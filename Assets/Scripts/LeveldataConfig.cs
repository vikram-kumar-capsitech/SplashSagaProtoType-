using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/Level")]
public class LevelDataConfig : ScriptableObject
{
    [System.Serializable]
    public class SpawnObject
    {
        public GameObject prefab;
        public Vector3 position;
        public Vector3 rotation;
    }

    [System.Serializable]
    public class LevelData
    {
        public int levelNumber;
        public bool levelUnLock;
        public bool levelComplete;
        public SpawnObject[] objects;
    }

    public LevelData[] levels;  
}

using UnityEngine;

[System.Serializable]
public class SpawnObject
{
    public string prefab;
    public float x;
    public float y;
}

[System.Serializable]
public class LevelData
{
    public int levelNumber;
    public bool levelLock;
    public bool levelComplete;
    public SpawnObject[] objects;
}

public static class LevelsData
{
    public static LevelData[] levels =
    {
        new LevelData
        {
            levelNumber = 1,
            levelLock = true,
            levelComplete = true,
            objects = new SpawnObject[]
            {
                new SpawnObject { prefab = "Sponge", x = 0, y = 3.5f },
                new SpawnObject { prefab = "Tile", x = -1.5f, y = -1 },
                new SpawnObject { prefab = "Tile", x = 1.5f, y = -1 },
                new SpawnObject { prefab = "Triangle Tile", x = -0.2f, y = 1.5f },
                new SpawnObject { prefab = "Basin", x = 0, y = -5.5f }
            }
        },

        new LevelData
        {
            levelNumber = 2,
            levelLock = true,
            levelComplete = true,
            objects = new SpawnObject[]
            {
                new SpawnObject { prefab = "Ball", x = -1, y = 3 },
                new SpawnObject { prefab = "Block", x = 1, y = 1 },
                new SpawnObject { prefab = "Block", x = 3, y = 2 }
            }
        },

        new LevelData
        {
            levelNumber = 3,
            levelLock = true,
            levelComplete = true,
            objects = new SpawnObject[]
            {
                new SpawnObject { prefab = "Ball", x = 0, y = 4 },
                new SpawnObject { prefab = "Block", x = 2, y = 2 }
            }
        },

        new LevelData
        {
            levelNumber = 4,
            levelLock = true,
            levelComplete = true,
            objects = new SpawnObject[]
            {
                new SpawnObject { prefab = "Ball", x = 0, y = 4 },
                new SpawnObject { prefab = "Block", x = 2, y = 2 }
            }
        },

        new LevelData
        {
            levelNumber = 5,
            levelLock = true,
            levelComplete = true,
            objects = new SpawnObject[]
            {
                new SpawnObject { prefab = "Ball", x = 0, y = 4 },
                new SpawnObject { prefab = "Block", x = 2, y = 2 }
            }
        }
    };
}

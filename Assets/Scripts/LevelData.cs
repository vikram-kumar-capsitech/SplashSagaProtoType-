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
                new SpawnObject { prefab = "Sponge", x = 0, y = 3.6f },
                new SpawnObject { prefab = "Square ", x = 0.1f, y = 1.8f },
                new SpawnObject { prefab = "Basin ", x = 0f, y = -6.5f }


            }
        },

        new LevelData
        {
            levelNumber = 2,
            levelLock = true,
            levelComplete = true,
            objects = new SpawnObject[]
            {

                    new SpawnObject { prefab = "Sponge", x = 0, y = 2.43f },
                    new SpawnObject { prefab = "V-Shape", x = -0.6f  , y = 3.1f },

                  new SpawnObject { prefab = "Basin ", x = 0f, y = -6.5f }
            }
        },

        new LevelData
        {
            levelNumber = 3,
            levelLock = true,
            levelComplete = true,
            objects = new SpawnObject[]
            {
                new SpawnObject { prefab = "Sponge", x = 0, y = 3.6f },
                new SpawnObject { prefab = "Triangle Tile", x = -0.2f, y = 1.35f },
                 new SpawnObject { prefab = "Tile", x = -1.69f, y = -0.82f },
                 new SpawnObject { prefab = "Tile", x = 1.56f, y = -0.82f },
                  new SpawnObject { prefab = "Basin ", x = 0f, y = -6.5f }
            }
        },

        new LevelData
        {
            levelNumber = 4,
            levelLock = true,
            levelComplete = true,
            objects = new SpawnObject[]
            {
                new SpawnObject { prefab = "Ball", x = 0, y = 2.5f },
                new SpawnObject { prefab = "L-Shape", x = 0f, y =1.15f },
                new SpawnObject { prefab = "L-Shape (1)", x = 0f, y = 3f },
                new SpawnObject { prefab = "Basin ", x = 0f, y = -6.5f }
            }
        },

        new LevelData
        {
            levelNumber = 5,
            levelLock = true,
            levelComplete = true,
            objects = new SpawnObject[]
            {
                new SpawnObject { prefab = "Ball", x = 0, y = 3.5f },
                new SpawnObject { prefab = "Vertical (1)", x = -3f, y = 3.5f },
                new SpawnObject { prefab = "Vertical", x = 3.17f, y = 3.5f  },
                new SpawnObject { prefab = "Square ", x = 0f, y = 1.5f  },
                new SpawnObject { prefab = "Basin1 ", x = 0f, y = -6.42f }


            }
        }
    };
}

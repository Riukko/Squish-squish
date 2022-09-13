using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRenderer : MonoBehaviour
{

    [SerializeField]
    [Range(1, 50)]
    private int width = 10;

    [SerializeField]
    [Range(1, 50)]
    private int height = 10;

    [SerializeField]
    private float size = 1f;

    [SerializeField]
    private Transform floorPrefab = null;

    [SerializeField]
    private Transform bridge1Prefab = null;
    [SerializeField]
    private Transform bridge2Prefab = null;
    [SerializeField]
    private Transform bridge3Prefab = null;
    [SerializeField]
    private Transform bridge4Prefab = null;

    [SerializeField]
    private Transform targetPrefab = null;

    void Start()
    {
        var maze = MazeGenerator.Generate(width, height);
        Draw(maze);

        // SPAWN TARGET PREFAB
        var target = Instantiate(targetPrefab, transform) as Transform;
        target.position = new Vector3(width - 1, 0.55f, height - 1);

    }

    private void Draw(WallState[,] maze)
    {
        //var floor = Instantiate(floorPrefab, transform);
        //floor.localScale = new Vector3(width, 1, height);

        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                var cell = maze[i, j];
                var position = new Vector3(i, 0, j);

                int numberOfWalls = 0;
                if (cell.HasFlag(WallState.UP))
                    numberOfWalls++;
                if (cell.HasFlag(WallState.DOWN))
                    numberOfWalls++;
                if (cell.HasFlag(WallState.LEFT))
                    numberOfWalls++;
                if (cell.HasFlag(WallState.RIGHT))
                    numberOfWalls++;

                if (numberOfWalls == 0)
                {
                    var bridge = Instantiate(bridge4Prefab, transform) as Transform;
                    bridge.position = position;
                }
                else if (numberOfWalls == 1)
                {
                    var bridge = Instantiate(bridge3Prefab, transform) as Transform;
                    bridge.position = position;
                    if (cell.HasFlag(WallState.RIGHT))
                        bridge.eulerAngles = new Vector3(0, 90, 0);
                    else if (cell.HasFlag(WallState.DOWN))
                        bridge.eulerAngles = new Vector3(0, 180, 0);
                    else if (cell.HasFlag(WallState.LEFT))
                        bridge.eulerAngles = new Vector3(0, 270, 0);
                } 
                else if (cell.HasFlag(WallState.UP) && cell.HasFlag(WallState.DOWN))
                {
                    var bridge = Instantiate(bridge1Prefab, transform) as Transform;
                    bridge.position = position;
                }
                else if (cell.HasFlag(WallState.LEFT) && cell.HasFlag(WallState.RIGHT))
                {
                    var bridge = Instantiate(bridge1Prefab, transform) as Transform;
                    bridge.position = position;
                    bridge.eulerAngles = new Vector3(0, 90, 0);
                }
                else
                {
                    var bridge = Instantiate(bridge2Prefab, transform) as Transform;
                    bridge.position = position;
                    if (cell.HasFlag(WallState.LEFT) && cell.HasFlag(WallState.UP))
                        bridge.eulerAngles = new Vector3(0, bridge.eulerAngles.y + 270, 0);
                    else if (cell.HasFlag(WallState.LEFT) && cell.HasFlag(WallState.DOWN))
                        bridge.eulerAngles = new Vector3(0, bridge.eulerAngles.y + 180, 0);
                    else if (cell.HasFlag(WallState.RIGHT) && cell.HasFlag(WallState.DOWN))
                        bridge.eulerAngles = new Vector3(0, bridge.eulerAngles.y + 90, 0);
                }

                

                /*
                contar flags
                flags = 0  -> bridge 4
                flags = 1 -> bridge 3
                flags = 2 || 3 -> bridge 1 || 2
                orientar 
                bridge 4 -> skip
                bridge 3 -> if flag.down -> skip
                            if flag.right -> rotate 90
                            up -> rot 180
                            left -> rot 270
                bridge 2 -> if flag.up && flag.right -> skip
                            up & left -> rot 90
                            left & down -> rot 180
                            down & right -> rot 270

                bridge 1 -> if 2 flags
                                up & down -> rot 90
                            if 3 flags
                                up || down -> rot 90
                 */


            }

        }

    }

}

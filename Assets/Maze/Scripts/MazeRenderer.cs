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
    private List<Transform> bridgePrefab = new List<Transform>();

    [SerializeField]
    private List<Transform> bridgeHPrefab = new List<Transform>();

    [SerializeField]
    private List<Material> ballMatsList = new List<Material>();

    [SerializeField]
    private Transform targetPrefab = null;

    [SerializeField]
    private GameObject playerPrefab = null;

    [SerializeField]
    private List<Color> colors = new List<Color>();
    [SerializeField]
    private Material skyboxMat = null;
    [SerializeField]
    private Material fogMat = null;
    [SerializeField]
    private Material mazeMat = null;

    private Material ballMat = null;

    void Start()
    {
        if (PlayerPrefs.GetInt("difficulty") == 1)
        {
            bridgePrefab[0] = bridgeHPrefab[0];
            bridgePrefab[1] = bridgeHPrefab[1];
            bridgePrefab[2] = bridgeHPrefab[2];
            bridgePrefab[3] = bridgeHPrefab[3];
        }

        ballMat = ballMatsList[Random.Range(0, ballMatsList.Count)];
        playerPrefab.GetComponent<Renderer>().material = ballMat;

        RandomColor();

        var maze = MazeGenerator.Generate(width, height);
        Draw(maze);

        // SPAWN TARGET PREFAB
        var target = Instantiate(targetPrefab, transform) as Transform;
        target.position = new Vector3(width - 1, 2.6f, height - 1);

    }

    private void Draw(WallState[,] maze)
    {
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
                    var bridge = Instantiate(bridgePrefab[3], transform) as Transform;
                    bridge.position = position;
                }
                else if (numberOfWalls == 1)
                {
                    var bridge = Instantiate(bridgePrefab[2], transform) as Transform;
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
                    var bridge = Instantiate(bridgePrefab[0], transform) as Transform;
                    bridge.position = position;
                }
                else if (cell.HasFlag(WallState.LEFT) && cell.HasFlag(WallState.RIGHT))
                {
                    var bridge = Instantiate(bridgePrefab[0], transform) as Transform;
                    bridge.position = position;
                    bridge.eulerAngles = new Vector3(0, 90, 0);
                }
                else
                {
                    var bridge = Instantiate(bridgePrefab[1], transform) as Transform;
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

    private void RandomColor()
    {
        float H, S, V;
        var color1 = colors[Random.Range(0, colors.Count)];
        Color.RGBToHSV(color1,out H, out S, out V);
        var color2 = Color.HSVToRGB((H + 0.33f) % 1, 0.75f, 1);
        var color3 = Color.HSVToRGB((H + 0.66f) % 1, 0.75f, 1);

        mazeMat.color = color2;
        skyboxMat.SetColor("_Color2", color1);
        ballMat.color = color3;
        fogMat.SetColor("_FogColor", Color.HSVToRGB(H,0.5f,V));
    }

}

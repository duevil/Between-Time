using UnityEngine;

public class MazeBuilder : MonoBehaviour
{

    [SerializeField]
    private GameObject pipe_curved;

    [SerializeField]
    private GameObject pipe_straight;

    [SerializeField]
    private GameObject pipe_straight_end;

    [SerializeField]
    private GameObject pipe_x_junction;

    [SerializeField]
    private GameObject pipe_t_junction;

    private char[,] _labyrinth = {
        {'3', 'b', '9', '1', '5', '5', '5', '3'},
        {'a', 'c', '2', 'c', '5', '3', '9', '6'},
        {'c', '3', 'c', '3', '9', '6', 'a', 'b'},
        {'b', 'a', 'd', '6', 'c', '3', 'a', 'a'},
        {'a', 'c', '3', '9', '3', 'a', 'c', '2'},
        {'8', '5', '6', 'a', 'c', '6', 'd', '2'},
        {'c', '3', 'd', '4', '5', '5', '3', 'a'},
        {'d', '4', '5', '5', '5', '5', '6', 'c'}
    };
    private int _labySize = 8;

    private float _cellToCelldistance = 0.3738f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buildLabyrinth();
    }

    private void buildLabyrinth()
    {
        var pos = gameObject.transform.position;
        float startZ = pos.z;

        for(int i = 0; i<_labySize; i++)
        {
            for (int j = 0; j<_labySize; j++)
            {
                var pipeType = _labyrinth[i,j];
                GameObject pipe;

                switch (pipeType)
                {
                    case '1':
                        pipe = pipeType1();
                        Instantiate(pipe, new Vector3(pos.x, pos.y, pos.z), pipe.transform.rotation, gameObject.transform);
                        break;
                    case '2':
                        pipe = pipeType2();
                        Instantiate(pipe, new Vector3(pos.x, pos.y, pos.z), pipe.transform.rotation, gameObject.transform);
                        break;
                    case '3':
                        pipe = pipeType3();
                        Instantiate(pipe, new Vector3(pos.x, pos.y, pos.z), pipe.transform.rotation, gameObject.transform);
                        break;
                    case '4':
                        pipe = pipeType4();
                        Instantiate(pipe, new Vector3(pos.x, pos.y, pos.z), pipe.transform.rotation, gameObject.transform);
                        break;
                    case '5':
                        pipe = pipeType5();
                        Instantiate(pipe, new Vector3(pos.x, pos.y, pos.z), pipe.transform.rotation, gameObject.transform);
                        break;
                    case '6':
                        pipe = pipeType6();
                        Instantiate(pipe, new Vector3(pos.x, pos.y, pos.z), pipe.transform.rotation, gameObject.transform);
                        break;
                    /*
                    case '7':
                        pipe = pipeType7();
                        Instantiate(pipe, new Vector3(pos.x, pos.y, pos.z), pipe.transform.rotation, gameObject.transform);
                        break;
                    */
                    case '8':
                        pipe = pipeType8();
                        Instantiate(pipe, new Vector3(pos.x, pos.y, pos.z), pipe.transform.rotation, gameObject.transform);
                        break;
                    case '9':
                        pipe = pipeType9();
                        Instantiate(pipe, new Vector3(pos.x, pos.y, pos.z), pipe.transform.rotation, gameObject.transform);
                        break;
                    case 'a':
                        pipe = pipeTypeA();
                        Instantiate(pipe, new Vector3(pos.x, pos.y, pos.z), pipe.transform.rotation, gameObject.transform);
                        break;
                    case 'b':
                        pipe = pipeTypeB();
                        Instantiate(pipe, new Vector3(pos.x, pos.y, pos.z), pipe.transform.rotation, gameObject.transform);
                        break;
                    case 'c':
                        pipe = pipeTypeC();
                        Instantiate(pipe, new Vector3(pos.x, pos.y, pos.z), pipe.transform.rotation, gameObject.transform);
                        break;
                    case 'd':
                        pipe = pipeTypeD();
                        Instantiate(pipe, new Vector3(pos.x, pos.y, pos.z), pipe.transform.rotation, gameObject.transform);
                        break;
                }
                pos = new Vector3(pos.x, pos.y, pos.z + _cellToCelldistance);
            }
            pos = new Vector3(pos.x, pos.y - _cellToCelldistance, startZ);
        }
    }

    private GameObject pipeType1()
    {
        GameObject pipe = pipe_t_junction;
        pipe.transform.rotation = Quaternion.Euler(180, -90, -90);
        return pipe;
    }

    private GameObject pipeType2()
    {
        GameObject pipe = pipe_t_junction;
        pipe.transform.rotation = Quaternion.Euler(0, -90, 0);
        return pipe;
    }

    private GameObject pipeType3()
    {
        GameObject pipe = pipe_curved;
        pipe.transform.rotation = Quaternion.Euler(0, -90, 0);
        return pipe;
    }

    private GameObject pipeType4()
    {
        GameObject pipe = pipe_t_junction;
        pipe.transform.rotation = Quaternion.Euler(0, 90, -90);
        return pipe;
    }

    private GameObject pipeType5()
    {
        GameObject pipe = pipe_straight;
        pipe.transform.rotation = Quaternion.Euler(-90, 0, 0);
        return pipe;
    }

    private GameObject pipeType6()
    {
        GameObject pipe = pipe_curved;
        pipe.transform.rotation = Quaternion.Euler(0, -90, -90);
        return pipe;
    }

    /*
    private GameObject pipeType7()
    {
        
    }
    */

    private GameObject pipeType8()
    {
        GameObject pipe = pipe_t_junction;
        pipe.transform.rotation = Quaternion.Euler(0, 90, 0);
        return pipe;
    }

    private GameObject pipeType9()
    {
        GameObject pipe = pipe_curved;
        pipe.transform.rotation = Quaternion.Euler(0, 90, 0);
        return pipe;
    }

    private GameObject pipeTypeA()
    {
        GameObject pipe = pipe_straight;
        pipe.transform.rotation = Quaternion.Euler(180, 0, 0);
        return pipe;
    }

    private GameObject pipeTypeB()
    {
        GameObject pipe = pipe_straight_end;
        pipe.transform.rotation = Quaternion.Euler(180, 0, 0);
        return pipe;
    }

    private GameObject pipeTypeC()
    {
        GameObject pipe = pipe_curved;
        pipe.transform.rotation = Quaternion.Euler(0, 90, -90);
        return pipe;
    }

    private GameObject pipeTypeD()
    {
        GameObject pipe = pipe_straight_end;
        pipe.transform.rotation = Quaternion.Euler(-90, 180, 0);
        return pipe;
    }

    public float getCellToCellDistance()
    {
        return _cellToCelldistance;
    }
}

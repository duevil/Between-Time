using UnityEngine;
using UnityEngine.Serialization;

namespace BetweenTime._Scripts.steampunk
{
    public class MazeBuilder : MonoBehaviour
    {

        [SerializeField]
        private GameObject pipeCurved;

        [SerializeField]
        private GameObject pipeStraight;

        [SerializeField]
        private GameObject pipeStraightEnd;

        [SerializeField]
        private GameObject pipeXJunction;

        [SerializeField]
        private GameObject pipeTJunction;

        private readonly char[,] _labyrinth = {
            {'3', 'b', '9', '1', '5', '5', '5', '3'},
            {'a', 'c', '2', 'c', '5', '3', '9', '6'},
            {'c', '3', 'c', '3', '9', '6', 'a', 'b'},
            {'b', 'a', 'd', '6', 'c', '3', 'a', 'a'},
            {'a', 'c', '3', '9', '3', 'a', 'c', '2'},
            {'8', '5', '6', 'a', 'c', '6', 'd', '2'},
            {'c', '3', 'd', '4', '5', '5', '3', 'a'},
            {'d', '4', '5', '5', '5', '5', '6', 'c'}
        };
        private const int LabySize = 8;

        private const float CellToCelldistance = 0.3738f;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            BuildLabyrinth();
        }

        private void BuildLabyrinth()
        {
            var pos = gameObject.transform.position;
            var startZ = pos.z;

            for(var i = 0; i<LabySize; i++)
            {
                for (var j = 0; j<LabySize; j++)
                {
                    var pipeType = _labyrinth[i,j];
                    GameObject pipe;

                    switch (pipeType)
                    {
                        case '1':
                            pipe = PipeType1();
                            Instantiate(pipe, new Vector3(pos.x, pos.y, pos.z), pipe.transform.rotation, gameObject.transform);
                            break;
                        case '2':
                            pipe = PipeType2();
                            Instantiate(pipe, new Vector3(pos.x, pos.y, pos.z), pipe.transform.rotation, gameObject.transform);
                            break;
                        case '3':
                            pipe = PipeType3();
                            Instantiate(pipe, new Vector3(pos.x, pos.y, pos.z), pipe.transform.rotation, gameObject.transform);
                            break;
                        case '4':
                            pipe = PipeType4();
                            Instantiate(pipe, new Vector3(pos.x, pos.y, pos.z), pipe.transform.rotation, gameObject.transform);
                            break;
                        case '5':
                            pipe = PipeType5();
                            Instantiate(pipe, new Vector3(pos.x, pos.y, pos.z), pipe.transform.rotation, gameObject.transform);
                            break;
                        case '6':
                            pipe = PipeType6();
                            Instantiate(pipe, new Vector3(pos.x, pos.y, pos.z), pipe.transform.rotation, gameObject.transform);
                            break;
                        /*
                    case '7':
                        pipe = pipeType7();
                        Instantiate(pipe, new Vector3(pos.x, pos.y, pos.z), pipe.transform.rotation, gameObject.transform);
                        break;
                    */
                        case '8':
                            pipe = PipeType8();
                            Instantiate(pipe, new Vector3(pos.x, pos.y, pos.z), pipe.transform.rotation, gameObject.transform);
                            break;
                        case '9':
                            pipe = PipeType9();
                            Instantiate(pipe, new Vector3(pos.x, pos.y, pos.z), pipe.transform.rotation, gameObject.transform);
                            break;
                        case 'a':
                            pipe = PipeTypeA();
                            Instantiate(pipe, new Vector3(pos.x, pos.y, pos.z), pipe.transform.rotation, gameObject.transform);
                            break;
                        case 'b':
                            pipe = PipeTypeB();
                            Instantiate(pipe, new Vector3(pos.x, pos.y, pos.z), pipe.transform.rotation, gameObject.transform);
                            break;
                        case 'c':
                            pipe = PipeTypeC();
                            Instantiate(pipe, new Vector3(pos.x, pos.y, pos.z), pipe.transform.rotation, gameObject.transform);
                            break;
                        case 'd':
                            pipe = PipeTypeD();
                            Instantiate(pipe, new Vector3(pos.x, pos.y, pos.z), pipe.transform.rotation, gameObject.transform);
                            break;
                    }
                    pos = new Vector3(pos.x, pos.y, pos.z + CellToCelldistance);
                }
                pos = new Vector3(pos.x, pos.y - CellToCelldistance, startZ);
            }
        }

        private GameObject PipeType1()
        {
            var pipe = pipeTJunction;
            pipe.transform.rotation = Quaternion.Euler(180, -90, -90);
            return pipe;
        }

        private GameObject PipeType2()
        {
            var pipe = pipeTJunction;
            pipe.transform.rotation = Quaternion.Euler(0, -90, 0);
            return pipe;
        }

        private GameObject PipeType3()
        {
            var pipe = pipeCurved;
            pipe.transform.rotation = Quaternion.Euler(0, -90, 0);
            return pipe;
        }

        private GameObject PipeType4()
        {
            var pipe = pipeTJunction;
            pipe.transform.rotation = Quaternion.Euler(0, 90, -90);
            return pipe;
        }

        private GameObject PipeType5()
        {
            var pipe = pipeStraight;
            pipe.transform.rotation = Quaternion.Euler(-90, 0, 0);
            return pipe;
        }

        private GameObject PipeType6()
        {
            var pipe = pipeCurved;
            pipe.transform.rotation = Quaternion.Euler(0, -90, -90);
            return pipe;
        }
        
    /*
        private GameObject PipeType7()
        {

        }
    */

        private GameObject PipeType8()
        {
            var pipe = pipeTJunction;
            pipe.transform.rotation = Quaternion.Euler(0, 90, 0);
            return pipe;
        }

        private GameObject PipeType9()
        {
            var pipe = pipeCurved;
            pipe.transform.rotation = Quaternion.Euler(0, 90, 0);
            return pipe;
        }

        private GameObject PipeTypeA()
        {
            var pipe = pipeStraight;
            pipe.transform.rotation = Quaternion.Euler(180, 0, 0);
            return pipe;
        }

        private GameObject PipeTypeB()
        {
            var pipe = pipeStraightEnd;
            pipe.transform.rotation = Quaternion.Euler(180, 0, 0);
            return pipe;
        }

        private GameObject PipeTypeC()
        {
            var pipe = pipeCurved;
            pipe.transform.rotation = Quaternion.Euler(0, 90, -90);
            return pipe;
        }

        private GameObject PipeTypeD()
        {
            var pipe = pipeStraightEnd;
            pipe.transform.rotation = Quaternion.Euler(-90, 180, 0);
            return pipe;
        }
    }
}

using UnityEngine;

namespace BetweenTime._Scripts.steampunk
{
    /// <summary>
    ///     Builds a maze using the provided pipe models and a predefined maze layout
    /// </summary>
    public class MazeBuilder : MonoBehaviour
    {
        // Distance between cells in the maze
        private const float CellToCellDistance = 0.3738f;

        // Maze layout; each uint represents a row of the maze of 8 cells
        // with 4 bits per cell (1 hex digit) representing the walls
        private static readonly uint[] Maze =
        {
            0x355519B3,
            0x6935C2CA,
            0xBA693C3C,
            0xAA3C6DAB,
            0x2CA393CA,
            0x2D6CA658,
            0xA3554D3C,
            0xC655554D
        };

        [Tooltip("The pipe model for a curved pipe")] [SerializeField]
        private GameObject pipeCurved;

        [Tooltip("The pipe model for a straight pipe")] [SerializeField]
        private GameObject pipeStraight;

        [Tooltip("The pipe model for a straight pipe with an end")] [SerializeField]
        private GameObject pipeStraightEnd;

        [Tooltip("The pipe model for a cross junction")] [SerializeField]
        private GameObject pipeXJunction;

        [Tooltip("The pipe model for a T junction")] [SerializeField]
        private GameObject pipeTJunction;

        /// <summary>
        ///     Parses the maze layout and builds the maze in the scene by
        ///     calculating the position and rotation of each pipe and
        ///     instantiating the corresponding pipe model with an attached collider
        /// </summary>
        private void Start()
        {
            var position = gameObject.transform.position;
            foreach (var row in Maze)
            {
                for (var i = 0; i < Maze.Length; i++)
                {
                    // Extract the walls from the row
                    var walls = (row >> (i * 4)) & 0xF;
                    // Wall layout to corresponding pipe model
                    var pipe = walls switch
                    {
                        0x0 => pipeXJunction,
                        0x1 or 0x2 or 0x4 or 0x8 => pipeTJunction,
                        0x3 or 0x6 or 0x9 or 0xC => pipeCurved,
                        0x7 or 0xB or 0xD or 0xE => pipeStraightEnd,
                        0x5 or 0xA => pipeStraight,
                        _ => null
                    };
                    // 2D clockwise rotation of the models in degrees
                    var rotation = walls switch
                    {
                        0x1 or 0x3 or 0x5 or 0xD => 90,
                        0x2 or 0x6 or 0xB => 180,
                        0x4 or 0x7 or 0xC => 270,
                        _ => 0 // 0x0 or 0x8 or 0x9 or 0xA or 0xE
                    };
                    var instance = Instantiate(pipe, position, Quaternion.Euler(0, 90, rotation), gameObject.transform);
                    instance.AddComponent<BoxCollider>();
                    position.z += CellToCellDistance;
                }

                // Reset the horizontal position to the start of the row
                position.z -= Maze.Length * CellToCellDistance;
                // Move the vertical position to the next row
                position.y -= CellToCellDistance;
            }
        }
    }
}
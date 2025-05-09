using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Visutronik
{
    public class TriangleController : MonoBehaviour
    {
        #region ----- Variables -----

        #region --- enum, const and readonly ---
        private enum TrianglePositon { Left, Top, Right, Bottom }
        private const float MoveXY = -0.5f;
        private readonly Vector3 VecRotLeft = new Vector3(1, 0, 0);
        private readonly Vector3 VecRotRight = new Vector3(-1, 0, 0);
        private readonly Vector3 VecRotTop = new Vector3(0, 1, 0);
        private readonly Vector3 VecRotBottom = new Vector3(0, -1, 0);

        #endregion

        #region --- Variables for the inspector ---

        [SerializeField, Tooltip("The texture for the wall")]
        private Texture Image;

        [SerializeField, Tooltip("The numbers of tiles per side, ex. 3 -> 3x3 tiles -> 36 triangles")]
        private int NumberOfTilesPerSide = 6;

        [SerializeField]
        private Shader RippleShader;

        [SerializeField, Tooltip("Speed of the rotation of the triangles")] 
        private float RotationSpeed = 50;

        [SerializeField, Tooltip("True...the tiles don't stop the player")]
        private bool TilesAreConvex = false;

        #endregion

        #region --- private Variables ---

        private int NumberOfTiles = 0;
        private GameObject ParentQuad;
        private List<MyTriangleTile> Tiles = new List<MyTriangleTile>();
        private float TillSize, StartX, StartY;
        private Vector3 Tilling;
        private float MaxDistance;
        private float SpeedDistance = 2;

        #endregion

        #endregion


        // Start is called before the first frame update
        void Start()
        {
            ParentQuad = this.gameObject;
            NumberOfTiles = NumberOfTilesPerSide * NumberOfTilesPerSide;

            //Calculate size, tilling and start position
            TillSize = 1.0f / NumberOfTilesPerSide;
            int n = NumberOfTilesPerSide / 2;

            Tilling = new Vector2(TillSize, TillSize);

            StartX = TillSize * n * -1;
            StartY = TillSize * n;

            if (NumberOfTilesPerSide % 2 == 0)
            {
                float pos = TillSize / 2.0f;
                StartX += pos;
                StartY -= pos;
            }

            //calculate values for the effect
            Vector3 scaleParent = ParentQuad.transform.localScale;
            MaxDistance = Mathf.Sqrt(scaleParent.x * scaleParent.x + scaleParent.y * scaleParent.y);
            SpeedDistance = Mathf.Sqrt(scaleParent.x * scaleParent.y) / 2.0f;

            //disable the MeshRenderer
            ParentQuad.GetComponent<MeshRenderer>().enabled = false;

            //create tiles
            int row = 0;
            int col = 0;
            for (int i = 0; i < NumberOfTiles; i++)
            {
                Tiles.Add(CreateTile(i, col, row, TrianglePositon.Left));
                Tiles.Add(CreateTile(i, col, row, TrianglePositon.Top));
                Tiles.Add(CreateTile(i, col, row, TrianglePositon.Right));
                Tiles.Add(CreateTile(i, col, row, TrianglePositon.Bottom));

                if (col == NumberOfTilesPerSide - 1)
                {
                    row++;
                    col = 0;
                }
                else
                {
                    col++;
                }
            }
        }

        //Create tile method
        private MyTriangleTile CreateTile(int i, int col, int row, TrianglePositon pos)
        {
            //New gameobject as child
            GameObject g = GameObject.CreatePrimitive(PrimitiveType.Quad);
            g.name += "_" + i + "_" + pos.ToString();
            g.transform.parent = ParentQuad.transform;
            g.transform.localScale = new Vector3(TillSize, TillSize, 1);

            //Add our TriangleTileScript
            TriangleTileScript tts = g.AddComponent<TriangleTileScript>();
            tts.RotationSpeed = RotationSpeed;
            tts.MaxRot = 30;

            //Create a triangle Mesh
            Mesh mesh = new Mesh();
            mesh.vertices = new Vector3[]
            {
                new Vector3(0 + MoveXY, 0 + MoveXY,0),
                new Vector3(0 + MoveXY, 1 + MoveXY,0),
                new Vector3(1 + MoveXY, 1 + MoveXY,0),
                new Vector3(1 + MoveXY, 0 + MoveXY,0),
                new Vector3(0.5f + MoveXY, 0.5f + MoveXY,0),
            };
            mesh.uv = new Vector2[]
            {
                new Vector2(0,0),
                new Vector2(0,1),
                new Vector2(1,1),
                new Vector2(1,0),
                new Vector2(0.5f,0.5f),
            };

            //Triangles depends on the position (indeces in clockwise direction)
            switch (pos)
            {
                case TrianglePositon.Left:
                    mesh.triangles = new int[] { 0, 1, 4 };
                    tts.VecRot = VecRotLeft;
                    break;

                case TrianglePositon.Right:
                    mesh.triangles = new int[] { 2, 3, 4 };
                    tts.VecRot = VecRotRight;
                    break;

                case TrianglePositon.Top:
                    mesh.triangles = new int[] { 1, 2, 4 };
                    tts.VecRot = VecRotTop;
                    break;

                case TrianglePositon.Bottom:
                    mesh.triangles = new int[] { 0, 4, 3 };
                    tts.VecRot = VecRotBottom;
                    break;
            }

            //Set the mesh
            g.GetComponent<MeshFilter>().mesh = mesh;

            //Set parameters of the MeshCollider
            MeshCollider mc = g.GetComponent<MeshCollider>();
            mc.convex = TilesAreConvex;
            mc.isTrigger = TilesAreConvex;
            mc.sharedMesh = mesh;

            //Calculate offset values (0,0 -> left down)
            Vector2 offset = new Vector2(col * TillSize, 1 - TillSize * (row + 1));

            //Create material of our shader
            Material m = new Material(RippleShader);
            m.SetTexture("_Texture", Image);
            m.SetVector("_Tilling", Tilling);
            m.SetVector("_Offset", offset);
            g.GetComponent<MeshRenderer>().material = m;

            //Calculate x and y for the local position of the tile
            float x = StartX + TillSize * col;
            float y = StartY - TillSize * row;
            g.transform.localPosition = new Vector3(x, y, -0.001f);

            //Recalculate normals of the mesh
            mesh.RecalculateNormals();

            //Add the tile to the list
            return new MyTriangleTile()
            {
                Pos = g.transform.position,
                TTS = tts,
            };
        }


        private void OnTriggerEnter(Collider other)
        {
            StartCoroutine("RunEffect", other.ClosestPoint(other.transform.position));
        }

        //Start effect as coroutine
        IEnumerator RunEffect(Vector3 pos)
        {
            float actualDistance = 0.5f;
            bool effectIsRunning = true;
            while (effectIsRunning == true)
            {
                //get tiles which in the area of the distance and didn't started yet
                foreach (MyTriangleTile tt in Tiles.Where(
                    x => x.TTS.Blocked == false
                    && Mathf.Abs(Vector3.Distance(x.Pos, pos) - actualDistance) < 0.5))
                {
                    tt.TTS.EffectIsRunning = true;
                    tt.TTS.Blocked = true;
                }
                actualDistance += Time.deltaTime * SpeedDistance;

                if (actualDistance > MaxDistance)
                {
                    effectIsRunning = false;
                }

                yield return null;
            }
        }
    }

    public class MyTriangleTile
    {
        public Vector3 Pos { get; set; }
        public TriangleTileScript TTS { get; set; }
    }
}

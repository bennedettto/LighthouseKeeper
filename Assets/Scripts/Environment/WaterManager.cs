using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace LighthouseKeeper.Environment
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class WaterManager : MonoBehaviour
    {
        public Transform target;
        Vector3 lastPosition;
        float revalidationDistance = 5f;

        public int gridSize = 20;
        public int halfExtent = 200;

        Mesh mesh;
        MeshFilter meshFilter;

        Dictionary<int, int> _vertexCountMapping;
        // Dictionary<Vector3, int> vertexMapping = new Dictionary<Vector3, int>();


        int Get(int x, int y) => _vertexCountMapping.TryGetValue(x * 3 * halfExtent + y, out int value) ? value : 1;
        void Set(int x, int y, int value) => _vertexCountMapping[x * 3 * halfExtent + y] = value;

        [ContextMenu("Initialize Water")]
        void Start()
        {
            meshFilter = GetComponent<MeshFilter>();
            mesh = new Mesh();
            meshFilter.mesh = mesh;

            _vertexCountMapping = new Dictionary<int, int>(4 * (halfExtent+1)* (halfExtent+1) / (gridSize * gridSize));

            UpdateMapping();
            UpdateMesh();
            lastPosition = target.position;
        }



        void Update()
        {
            if (Vector3.Distance(target.position, lastPosition) < revalidationDistance) return;

            lastPosition = target.position;
            if (!UpdateMapping()) return;

            UpdateMesh();
        }


        int VertexCount(Vector3 position)
            => Vector3.Distance(position, target.position) switch
            {
                < 25 => 8,
                < 50 => 4,
                < 75 => 2,
                _    => 1,
            };


        bool UpdateMapping()
        {
            bool changed = false;
            for (int z = -halfExtent; z <= halfExtent; z += gridSize)
                for (int x = -halfExtent; x <= halfExtent; x += gridSize)
                {
                    int vertexCount = VertexCount(new Vector3(x + gridSize * 0.5f, 0, z + gridSize * 0.5f));
                    if (Get(x, z) == vertexCount) continue;

                    changed = true;
                    Set(x, z, vertexCount);
                }
            return changed;
        }


        void UpdateMesh()
        {
            mesh.Clear();

            var vertices = new List<Vector3>();
            var triangles = new List<int>();
            var uvs = new List<Vector2>();
            var normals = new List<Vector3>();

            int vertexIndex = 0;

            for (int z = -halfExtent; z < halfExtent; z += gridSize)
                for (int x = -halfExtent; x < halfExtent; x += gridSize)
                {
                    if (math.sqrt(x * x + z * z) < 100) continue; // interior of island
                    AddGrid(vertices, triangles, uvs, normals, x, z, Get(x, z), ref vertexIndex);
                }

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.normals = normals.ToArray();
            mesh.uv = uvs.ToArray();
        }


        void AddGrid(List<Vector3> vertices, List<int> triangles, List<Vector2> uvs, List<Vector3> normals, int x, int z, int vertexCount,
                            ref int vertexIndex)
        {
            int startIndex = vertexIndex;
            for (int i = x; i <= x + vertexCount; i++)
            {
                float xx = math.remap(x, x + vertexCount, x, x + gridSize, i);
                for (int j = z; j <= z + vertexCount; j++)
                {
                    float zz = math.remap(z, z + vertexCount, z, z + gridSize, j);
                    vertices.Add(new Vector3(xx, 0, zz));
                    uvs.Add(new Vector2(math.remap(-halfExtent, halfExtent, 0, 1, xx),
                                        math.remap(-halfExtent, halfExtent, 0, 1, zz)));
                    normals.Add(new Vector3(0, 1, 0));
                    vertexIndex++;
                }
            }


            int rightCount = Get(x + gridSize, z);
            int leftCount  = Get(x - gridSize, z);
            int upCount    = Get(x, z + gridSize);
            int downCount  = Get(x, z - gridSize);
            for (int i = 0; i < vertexCount; i++)
                for (int j = 0; j < vertexCount; j++)
                {
                    if (j == 0             && downCount < vertexCount) continue;
                    if (j == vertexCount-1 && upCount   < vertexCount) continue;
                    if (i == 0             && leftCount < vertexCount) continue;
                    if (i == vertexCount-1 && rightCount < vertexCount) continue;

                    int bottomLeft = startIndex + i * (vertexCount + 1) + j;
                    int bottomRight = bottomLeft + 1;
                    int topLeft = bottomLeft + vertexCount + 1;
                    int topRight = topLeft + 1;

                    triangles.Add(topLeft);
                    triangles.Add(bottomLeft);
                    triangles.Add(topRight);

                    triangles.Add(topRight);
                    triangles.Add(bottomLeft);
                    triangles.Add(bottomRight);
                }
            if (downCount < vertexCount)
            {
                int j = 0;
                for (int i = 0; i < vertexCount; i += 2)
                {
                    int bottomLeft = startIndex + i * (vertexCount + 1) + j;
                    int bottomMiddle = bottomLeft + vertexCount + 1;
                    int bottomRight = bottomLeft + 2 * vertexCount + 2;
                    int topLeft = bottomLeft + 1;
                    int topMiddle = bottomMiddle + 1;
                    int topRight = bottomRight + 1;

                    triangles.Add(bottomLeft);
                    triangles.Add(topMiddle);
                    triangles.Add(bottomRight);

                    if (leftCount >= vertexCount || i > 0)
                    {
                        triangles.Add(bottomLeft);
                        triangles.Add(topLeft);
                        triangles.Add(topMiddle);
                    }

                    if (rightCount >= vertexCount || i < vertexCount - 2)
                    {
                        triangles.Add(bottomRight);
                        triangles.Add(topMiddle);
                        triangles.Add(topRight);
                    }
                }
            }
            if (upCount < vertexCount)
            {
                int j = vertexCount - 1;
                for (int i = 0; i < vertexCount; i += 2)
                {
                    int bottomLeft = startIndex + i * (vertexCount + 1) + j;
                    int bottomMiddle = bottomLeft + vertexCount + 1;
                    int bottomRight = bottomLeft + 2 * vertexCount + 2;
                    int topLeft = bottomLeft + 1;
                    int topMiddle = bottomMiddle + 1;
                    int topRight = bottomRight + 1;

                    triangles.Add(topLeft);
                    triangles.Add(topRight);
                    triangles.Add(bottomMiddle);

                    if (leftCount >= vertexCount || i > 0)
                    {
                        triangles.Add(bottomLeft);
                        triangles.Add(topLeft);
                        triangles.Add(bottomMiddle);
                    }

                    if (rightCount >= vertexCount || i < vertexCount - 2)
                    {
                        triangles.Add(bottomMiddle);
                        triangles.Add(topRight);
                        triangles.Add(bottomRight);
                    }
                }
            }
            if (rightCount < vertexCount)
            {
                int i = vertexCount - 1;
                for (int j = 0; j < vertexCount; j += 2)
                {
                    int bottomLeft = startIndex + i * (vertexCount + 1) + j;
                    int bottomRight = bottomLeft + vertexCount + 1;
                    int middleLeft = bottomLeft + 1;
                    int middleRight = bottomRight + 1;
                    int topLeft = middleLeft + 1;
                    int topRight = middleRight + 1;

                    triangles.Add(middleLeft);
                    triangles.Add(topRight);
                    triangles.Add(bottomRight);

                    if (downCount >= vertexCount || j > 0)
                    {
                        triangles.Add(bottomLeft);
                        triangles.Add(middleLeft);
                        triangles.Add(bottomRight);
                    }

                    if (upCount >= vertexCount || j < vertexCount - 2)
                    {
                        triangles.Add(middleLeft);
                        triangles.Add(topLeft);
                        triangles.Add(topRight);
                    }
                }
            }
            if (leftCount < vertexCount)
            {
                int i = 0;
                for (int j = 0; j < vertexCount; j += 2)
                {
                    int bottomLeft = startIndex + i * (vertexCount + 1) + j;
                    int bottomRight = bottomLeft + vertexCount + 1;
                    int middleLeft = bottomLeft + 1;
                    int middleRight = bottomRight + 1;
                    int topLeft = middleLeft + 1;
                    int topRight = middleRight + 1;

                    triangles.Add(bottomLeft);
                    triangles.Add(topLeft);
                    triangles.Add(middleRight);

                    if (downCount >= vertexCount || j > 0)
                    {
                        triangles.Add(bottomLeft);
                        triangles.Add(middleRight);
                        triangles.Add(bottomRight);
                    }

                    if (upCount >= vertexCount || j < vertexCount - 2)
                    {
                        triangles.Add(middleRight);
                        triangles.Add(topLeft);
                        triangles.Add(topRight);
                    }
                }
            }
        }
    }
}
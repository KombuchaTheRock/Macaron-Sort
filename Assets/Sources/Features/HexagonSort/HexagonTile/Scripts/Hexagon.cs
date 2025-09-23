using System;
using UnityEngine;

namespace Sources.Features.HexagonSort.HexagonTile.Scripts
{
    public class Hexagon : MonoBehaviour
    {
        public HexagonTileType TileType { get; private set; }
        public float Height { get; private set; }

        public int Score { get; private set; }

        public void Awake()
        {
            MeshFilter meshRenderer = GetComponentInChildren<MeshFilter>();
            Height = GetMeshHeight(meshRenderer.mesh);
        }

        public void Initialize(HexagonTileType tileType, int score)
        {
            TileType = tileType;
            Score = score;
        }

        private float GetMeshHeight(Mesh mesh)
        {
            if (mesh == null) return 0f;

            Bounds bounds = mesh.bounds;
            return bounds.size.y;
        }

        public void SetParent(Transform parent) =>
            transform.SetParent(parent);
    }
}
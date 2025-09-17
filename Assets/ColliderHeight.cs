using UnityEngine;

public class ColliderHeight : MonoBehaviour
{
    private MeshCollider _meshCollider;
    private Mesh _originalMesh;
    private Mesh _modifiedMesh;
    private Vector3[] _originalVertices;
    
    public float OriginalHeight => _meshCollider.bounds.size.y;
    
    private void Awake()
    {
        _meshCollider = GetComponent<MeshCollider>();

        if (_meshCollider == null || _meshCollider.sharedMesh == null) 
            return;
        
        _originalMesh = _meshCollider.sharedMesh;
        _originalVertices = _originalMesh.vertices;
            
        _modifiedMesh = Instantiate(_originalMesh);
        _meshCollider.sharedMesh = _modifiedMesh;
    }
    
    public void SetHeight(float heightMultiplier)
    {
        if (_modifiedMesh == null) 
            return;
        
        Vector3[] vertices = _modifiedMesh.vertices;
        float minOriginalY = FindMinY(_originalVertices);
        
        for (int i = 0; i < vertices.Length; i++)
        {
            float originalY = _originalVertices[i].y;
            
            float offsetFromBottom = originalY - minOriginalY;
            vertices[i].y = originalY + offsetFromBottom * heightMultiplier;
        }
        
        UpdateMesh(vertices);
    }
    
    private float FindMinY(Vector3[] vertices)
    {
        float minY = vertices[0].y;
        
        for (int i = 1; i < vertices.Length; i++)
            if (vertices[i].y < minY) 
                minY = vertices[i].y;
        
        return minY;
    }
    
    private void UpdateMesh(Vector3[] vertices)
    {
        _modifiedMesh.vertices = vertices;
        _modifiedMesh.RecalculateNormals();
        _modifiedMesh.RecalculateBounds();
        
        _meshCollider.sharedMesh = null;
        _meshCollider.sharedMesh = _modifiedMesh;
    }
}

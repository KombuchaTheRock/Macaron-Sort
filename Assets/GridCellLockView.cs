using UnityEngine;

public class GridCellLockView : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;

    private void Awake() => 
        Hide();

    private void Update()
    {
        if (transform.rotation.y != 0)
            transform.rotation = Quaternion.Euler(90, 0, 0);
    }

    public void Hide() =>
        _meshRenderer.enabled = false;

    public void Show() =>
        _meshRenderer.enabled = true;
}
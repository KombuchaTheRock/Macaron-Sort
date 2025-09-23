using DG.Tweening;
using Sources.Features.HexagonSort.HexagonStackSystem.StackGenerator.Scripts;
using TMPro;
using UnityEngine;

public class StackSizeViewer : MonoBehaviour
{
    private HexagonStack _stack;
    [SerializeField] private TextMeshPro _text;

    public void Initialize(HexagonStack stack) =>
        _stack = stack;

    private void Update()
    {
        _text.transform.rotation = Quaternion.LookRotation(-Vector3.forward);
        //LookAtCameraY(_text.transform);
    }

    public void Hide() =>
        _text.gameObject.SetActive(false);

    public void Show() =>
        _text.gameObject.SetActive(true);

    public void UpdateStackSize()
    {
        if (_stack == null)
            return;

        int hexagonsCount = _stack.Hexagons.Count;

        float newY = hexagonsCount * _stack.OffsetBetweenTiles + 0.5f;
        Vector3 newPosition = new(_text.transform.position.x, newY, _text.transform.position.z);

        _text.transform.position = newPosition;
        _text.transform.DOScale(1, hexagonsCount * 0.02f)
            .From(0)
            .SetEase(Ease.OutBounce)
            .SetLink(gameObject)
            .Play();

        _text.text = hexagonsCount.ToString();
    }
    
     
    public void LookAtCameraY(Transform target)
    {
        Vector3 directionToCamera = Camera.main.transform.position - transform.position;
        
        directionToCamera.y = 0;
        
        if (directionToCamera != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);
            target.transform.rotation = targetRotation;
        }
    }
}
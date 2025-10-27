using UnityEngine;

public abstract class CellLockView : MonoBehaviour
{
    public void Show() => 
        gameObject.SetActive(true);

    public void Hide() => 
        gameObject.SetActive(false);

    private void Update()
    {
        if (transform.rotation.y != 0)
            transform.rotation = Quaternion.Euler(90, 0, 0);
    }
}
using UnityEngine;
using DG.Tweening;

public class Item : MonoBehaviour
{
    public float floatHeight = 0.5f;
    public float floatDuration = 1.5f;
    public float rotateSpeed = 45f;

    public bool isKey = true;

    void Start()
    {
        AnimateItem();
    }

    void AnimateItem()
    {
        // Floating up and down
        transform.DOMoveY(transform.position.y + floatHeight, floatDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);

        // Continuous rotation around Y axis
        transform.DORotate(new Vector3(0, 360, 0), 6f, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);
    }

    public void ItemInteract()
    {
        AudioManager.Instance.PlaySound("Pickup");
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {          
        if(other.gameObject.CompareTag("Player"))
        {
            if (isKey)
            {
                Player playeScript = other.gameObject.GetComponent<Player>();
                playeScript.IncrementKeyCount();    // increase key count
                ItemInteract();
            }
        }
    }
}

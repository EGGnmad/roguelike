using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DroppedItem : MonoBehaviour
{
    [SerializeField] private Item _item;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            player.inventory.AddItem(_item);
            Destroy(gameObject);
        }
    }
}

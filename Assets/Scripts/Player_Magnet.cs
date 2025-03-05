using UnityEngine;

public class Player_Magnet : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Coin>(out Coin coin))
        {
            coin.MoveToPlayer();
        }
    }
}

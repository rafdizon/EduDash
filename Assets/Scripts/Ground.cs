using UnityEngine;

public class Ground : MonoBehaviour
{
    private MeshRenderer mr;

    private void Awake()
    {
        mr = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        float speed = GameManager.Instance.gameSpeed / transform.localScale.x;
        mr.material.mainTextureOffset += Vector2.right * speed * Time.deltaTime;
    }
}

using UnityEngine;

public class IceBlock : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject go = collision.gameObject;
        if(go.CompareTag("Player"))
        {
            if(go.GetComponent<IcePlayerController>().CanDestroyBlock())
            {
                gameObject.SetActive(false);
            }
        }
    }
}

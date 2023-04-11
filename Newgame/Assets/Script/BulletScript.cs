using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isRight;
    void Start()
    {
        Destroy(gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(( isRight ? Vector3.right : Vector3.left) * Time.deltaTime * 5f);
        
    }
    public void setIsRight(bool isRight)
    {
        this.isRight = isRight;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Snail"))
        {
            Destroy(gameObject);
        }
    }
}

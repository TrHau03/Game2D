using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailScript : MonoBehaviour
{
    public float left,right;
    private bool isRight;
    public GameObject Player;
    private float countLife;

    // Start is called before the first frame update
    void Start()
    {
        countLife = 2;

    }

    // Update is called once per frame
    void Update()
    {
        var SnailX = transform.position.x;
        if(Player != null){
            var Playerx = Player.transform.position.x;
            if(Playerx > left && Playerx < right){
                if(Playerx < SnailX){
                    isRight = false;
                }else if(Playerx > SnailX){
                    isRight = true;
                }
        }
        }
        if(SnailX < left){
                    isRight = true;
                }else if(SnailX > right){
                    isRight = false;
                }
        Vector2 scale = transform.localScale;
        if(isRight){
            scale.x *= scale.x  < 0 ? 1 : -1;
            transform.Translate(Vector3.right *  2f * Time.deltaTime);
        }else{
            scale.x *= scale.x  < 0 ? -1 : 1;
            transform.Translate(Vector3.left * 2f * Time.deltaTime);
        }
        transform.localScale = scale;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("bullet"))
        {
            --countLife;
            if (countLife == 0)
            {
                Destroy(gameObject);
            }
        }
    }
}

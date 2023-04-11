using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSCript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Player;
    public float Startt,End;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var PlayerX = Player.transform.position.x;
        var PlayerY = Player.transform.position.y;
        var Camx = transform.position.x;
        var Camy = transform.position.y;
        if(PlayerX > Startt && PlayerX < End){
            Camx = PlayerX;
            if(PlayerY > 0.45){
            Camy = PlayerY;
            }else{
                Camy = 0.45f;
            }
        }else
        {
            if(PlayerX < Startt)
            {
                Camx = Startt;
                Camy = 0.45f;
            };
            if(PlayerX > End) Camx = End;
        }
        transform.position = new Vector3(Camx, Camy, -10);
        
    }
}

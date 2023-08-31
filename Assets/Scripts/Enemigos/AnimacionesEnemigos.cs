using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimacionesEnemigos : MonoBehaviour
{

    //Para referencias
    Animator am;
    SpriteRenderer sr;
    MovimientoEnemigos me;

    // Start is called before the first frame update
    void Start()
    {
        am = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        me = GetComponent<MovimientoEnemigos>();
    }

    // Update is called once per frame
    void Update()
    {
        if (me.jugador.position.x < transform.position.x)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }
}

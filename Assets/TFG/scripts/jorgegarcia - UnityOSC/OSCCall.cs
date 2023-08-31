using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OSCCall : MonoBehaviour
{
    // Vamos a colocar este script en un objeto que este siempre en escena durante toda la partida, por ejemplo la camara principal

    // Start is called before the first frame update
    void Start()
    {
        //inicializamos la instancia del manejador de supercollider
        OSCHandler.Instance.Init();
    }
}

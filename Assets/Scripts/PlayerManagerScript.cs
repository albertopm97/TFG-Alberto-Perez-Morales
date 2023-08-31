using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManagerScript : MonoBehaviour
{
    public PlayerInputManager playerInputManager;

    [Header("Sonido")]
    public AudioSource loopJuego;

    void Update()
    {
        //No se deberia gestionar audio aqui, pero GameManager esta en PlayerParent
        if(GameObject.FindObjectOfType<EstadisticasJugador>() != null)
        {
            comprobarLoop();
        }
        
    }

    public void comprobarLoop()
    {
        if (GameManager.instancia.estadoDelJuegoActual == GameManager.estadoDelJuego.Gameplay && !loopJuego.isPlaying)
        {
            loopJuego.Play();
        }
        else if (GameManager.instancia.estadoDelJuegoActual == GameManager.estadoDelJuego.GameOver && loopJuego.isPlaying)
        {
            loopJuego.Stop();
        }
    }
}


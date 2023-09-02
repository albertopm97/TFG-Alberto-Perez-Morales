using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instancia;

    public enum estadoDelJuego
    {
        Gameplay, Pausa, GameOver, SubirNivel
    }

    public estadoDelJuego estadoDelJuegoActual;
    public estadoDelJuego estadoDelJuegoPrevio;

    public bool juegoFinalizado = false;
    public Text jugadorGanador;
    public bool mejorandoEquipamiento = false;
    public int jugadorSubiendoNivel;

    [Header("UI")]
    public GameObject menuPausa;
    public GameObject menuFinJuego;
    public GameObject menuSubirNivel;

    [Header("Cronometro")]
    float tiempoCrono;
    public Text tiempoCronoUI;

    [Header("Sonido")]
    public AudioSource loopJuego;

    public GameObject referenciaJugador;

    //Al comienzo de la partida desactivamos los menus 
    void Awake()
    {
        //Vamos a aplicar el patron singleton para evitar que haya mas de 1 instancia del gamemanager al mismo tiempo
        if(instancia == null)
        {
            instancia = this;
        }
        else
        {
            Debug.LogWarning("Borrada instancia extra del Game Manager");
        }

        desactivarMenus();
    }

    void Update()
    {
        switch (estadoDelJuegoActual)
        {
            case estadoDelJuego.Gameplay:
                
                comprobadorPausa();
                actualizarCrono();
                if (!loopJuego.isPlaying)
                {
                    loopJuego.Play();
                }
                break;

            case estadoDelJuego.Pausa:
                
                comprobadorPausa();
                loopJuego.Pause();
                break;

            case estadoDelJuego.GameOver:

                if (!juegoFinalizado)
                {
                    juegoFinalizado = true;
                    Time.timeScale = 0f;
                    Debug.Log("FIN DEL JUEGO");
                    tiempoCronoUI.text = "";
                    mostrarPantallaFinal();
                    loopJuego.Stop();

                    //Mandamos mensaje de fin de música a supercollider
                    OSCHandler.Instance.SendMessageToClient("SuperCollider", "/finalizarMusica", juegoFinalizado);
                }
                break;

            case estadoDelJuego.SubirNivel:

                if (!mejorandoEquipamiento)
                {
                    mejorandoEquipamiento = true;
                    Time.timeScale = 0f;
                    Debug.Log("Jugador mejorando equipo");
                    menuSubirNivel.SetActive(true);
                }
                break;

            default:
                Debug.LogWarning("Estado que no existe");
                break;
        }
    }

    public void cambiarEstadoActual(estadoDelJuego nuevoEstado)
    {
        estadoDelJuegoActual = nuevoEstado;
    }

    public void pausarJuego()
    {
        estadoDelJuegoPrevio = estadoDelJuegoActual;
        cambiarEstadoActual(estadoDelJuego.Pausa);
        Time.timeScale = 0f; //Literalmente para el tiempo
        menuPausa.SetActive(true);
        Debug.Log("Juego Pausado");
    }

    public void reanudarJuego()
    {
        if(estadoDelJuegoActual == estadoDelJuego.Pausa)
        {
            cambiarEstadoActual(estadoDelJuegoPrevio);
            Time.timeScale = 1f;
            menuPausa.SetActive(false);
            Debug.Log("Juego reanudado");
        }
    }

    void comprobadorPausa()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(estadoDelJuegoActual == estadoDelJuego.Pausa)
            {
                reanudarJuego();
            }
            else
            {
                pausarJuego();
            }
        }
    }

    void desactivarMenus()
    {
        menuPausa.SetActive(false);
        menuFinJuego.SetActive(false);
        menuSubirNivel.SetActive(false);
    }

    public void gameOver()
    {
        cambiarEstadoActual(estadoDelJuego.GameOver);
    }

    public void mostrarPantallaFinal() 
    {
        menuFinJuego.SetActive(true);
    }

    void actualizarCrono()
    {
        tiempoCrono += Time.deltaTime;

        actualizarCronoUI();
    }

    void actualizarCronoUI()
    {
        int minutos = Mathf.FloorToInt(tiempoCrono/60);
        int segundos = Mathf.FloorToInt(tiempoCrono % 60);

        tiempoCronoUI.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    public void inicioMenuMejora()
    {
        cambiarEstadoActual(estadoDelJuego.SubirNivel);
        referenciaJugador.SendMessage("EliminarOpcionesYAplicarMejoras");
    }
    public void finMenuMejora()
    {
        mejorandoEquipamiento = false;
        Time.timeScale = 1f;
        menuSubirNivel.SetActive(false);
        cambiarEstadoActual(estadoDelJuego.Gameplay);
    }
}

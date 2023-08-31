using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EstadisticasJugador : MonoBehaviour
{
    [Header("Clase por defecto")]
    public ScriptableObjectPersonaje infoPJDefecto;
    ScriptableObjectPersonaje infoPersonaje;

    //Estadisticas generales actuales
    public float vidaActual;
    [HideInInspector]
    public float recuperacionActual;
    [HideInInspector]
    public float velocidadMovimientoActual;
    [HideInInspector]
    public float poderActual;
    [HideInInspector]
    public float rapidezProyectilActual;
    [HideInInspector]
    public float imanObjetosActual;

    //Gestion de experiencia
    [Header("Experiencia/Nivel")]
    public int experiencia = 0;
    public int nivel = 1;
    public int maxExperiencia;

    //Para gestion de frames de invulnerabilidad (no recibir daño constante y rapido)
    public float duracionInv;
    float temporizadorInv;
    bool esInvulnerable;


    //Para gestion de subidas de niveles y cambios en la experiencia máxima
    public List<RangoNivel> rangosNivel;

    //Para la gestion del inventario
    GestorInventario inventario;
    public int ranuraArma;
    public int ranuraPasivo;

    //pruebas
    public GameObject arma2;
    public GameObject pasivo1, pasivo2;


    //Para la barra de vida
    [Header("UI")]
    public Image barraHp;
    public Image barraExp;

    //Clase para gestion de los rangos de nivel. En cada rango, la experiencia necesaria para subir de nivel varia de forma distinta
    //Cabe destacar que el siguiente sistema de subida de lv no es de desarrollo propio, es el utilizado en el juego original "vampire survivor"
    // de lv 1 a 2 se necesitan 5 xp
    //desde lv 2 a lv 20 se incrementa la experienca necesaria en 10 cada lv (15 xp de lv 2 a 3, 25xp de lv 3 a 4, ...)
    //lv 21 a 40 el incremento es de 13xp
    //lv 40 en adelante incremento de 16xp


    [System.Serializable]
    public class RangoNivel
    {
        public int nivelInicial;
        public int nivelFinal;
        public int incrementoMaxExperiencia;
    }

    [Header("Sonido")]
    public AudioSource sonidoHit;
    public AudioSource sonidoMuerte;
    public AudioSource sonidoLvUp;
    public AudioSource sonidoMoneda;

    private void Start()
    {
        //Inicializamos la experiencia maxima incial como el primer incremento máximo de experiencia (0 + incremento = incremento)
        maxExperiencia = rangosNivel[0].incrementoMaxExperiencia;

        actualizarBarraHP();


        /**--------------TFG------------**/

        //Llamada para iniciar la musica al activarse este sript en el juego
        float iniciarMusica = 1.0f;
        OSCHandler.Instance.SendMessageToClient("SuperCollider", "/iniciarMusica", iniciarMusica);
    }

    void Update()
    {
        if(temporizadorInv > 0)
        {
            temporizadorInv -= Time.deltaTime;
        }
        else if(esInvulnerable)
        {
            esInvulnerable = false;
        }

        curaPasiva();
        actualizarBarraExp();
    }

    //Al llamarse al script
    private void Awake()
    {
        if(SelectorPersonaje.instancia != null)
        {
            infoPersonaje = SelectorPersonaje.instancia.datosPj;
        }
        else
        {
            infoPersonaje = infoPJDefecto;
        }
        

        vidaActual = infoPersonaje.VidaMaxima;
        recuperacionActual = infoPersonaje.Recuperacion;
        velocidadMovimientoActual = infoPersonaje.VelocidadMovimiento;
        poderActual = infoPersonaje.Poder;
        rapidezProyectilActual = infoPersonaje.RapidezProyectil;
        imanObjetosActual = infoPersonaje.ImanObjetos;

        inventario = GetComponent<GestorInventario>();

        equiparNuevaArma(infoPersonaje.ArmaInicial);

        if(SelectorPersonaje.instancia != null)
        {
            SelectorPersonaje.instancia.DestruirSingleton();
        }
    }

    public void incrementarExperiencia(int cantidad)
    {
        experiencia += cantidad;
        sonidoMoneda.Play();
        comprobarLvUp();
    }

    void comprobarLvUp()
    {
        if(experiencia >= maxExperiencia)
        {
            sonidoLvUp.Play();
            nivel++;
            Debug.Log("LV UP");
            experiencia -= maxExperiencia;
            

            int incrementoMaxExperiencia = 0;

            //iteramos por los distintos rangos de nivel hasta encontrar en el que esta el jugador, tomando el incremento de nivel correspondiente
            foreach(RangoNivel r in rangosNivel)
            {
                if(nivel >= r.nivelInicial && nivel <= r.nivelFinal)
                {
                    incrementoMaxExperiencia = r.incrementoMaxExperiencia;

                    //no es que sea la forma mas bonita, pero en juegos hay que optimizar al maximo, si encontramos el rango correspondiente forzamos salida del bucle
                    break;
                }
            }
            //aplicamos el incremento
            maxExperiencia += incrementoMaxExperiencia;
            GameManager.instancia.inicioMenuMejora();
        }
    }

    public void RecibirAtaque(float dmg)
    {
        //Recibimos el ataque si no estamos en frames de invencibilidad
        if (!esInvulnerable)
        {
            vidaActual -= dmg;

            //Reiniciamos el temporizador de I-frames
            temporizadorInv = duracionInv;
            esInvulnerable = true;
            sonidoHit.Play();

            if (vidaActual <= 0)
            {
                sonidoMuerte.Play();
                Morir();
            }

            actualizarBarraHP();
        }
    }

    void actualizarBarraHP()
    {
        barraHp.fillAmount = vidaActual / infoPersonaje.VidaMaxima;
    }

    void actualizarBarraExp()
    {
        barraExp.fillAmount = experiencia / maxExperiencia;
    }

    public void Morir()
    {
        if (!GameManager.instancia.juegoFinalizado)
        {
            GameManager.instancia.gameOver();
        }
    }

    public void curar(float vidaACurar)
    {
        //Solo curar cuando se haya perdido vida
        if(vidaActual < infoPersonaje.VidaMaxima)
        {
            vidaActual += vidaACurar;
            Debug.Log("TE HAS CURADO");

            //Si al curar la vida sobrepasa el maximo, dejar el maximo en su lugar
            if (vidaActual > infoPersonaje.VidaMaxima)
            {
                vidaActual = infoPersonaje.VidaMaxima;
            }

            actualizarBarraHP();
        }
    }

    //La idea es que si te falta vida haya una curación pasiva en función del factor de recuperación
    void curaPasiva()
    {
        if(vidaActual < infoPersonaje.VidaMaxima)
        {
            vidaActual += recuperacionActual * Time.deltaTime;

            //Incluso con la comprobación anterior, a veces se pasa un poco de la vida maxima
            //Es necesaria una segunda comprobación para dejar aplicado el máximo
            if(vidaActual > infoPersonaje.VidaMaxima)
            {
                vidaActual = infoPersonaje.VidaMaxima;
            }

            actualizarBarraHP();
        }
    }

    public void equiparNuevaArma(GameObject arma)
    {
        //Si el inventario esta lleno no tomamos mas armas
        if(ranuraArma >= inventario.ranurasArmas.Count - 1)
        {
            Debug.LogError("Inventario de armas lleno");
            return;
        }

        //Arma a equipar
        GameObject nuevaArma = Instantiate(arma, transform.position, Quaternion.identity);
        nuevaArma.transform.SetParent(transform); //deja el arma inicial como hijo del jugador*/
        inventario.equiparArma(ranuraArma, nuevaArma.GetComponent<ControladorArmas>());

        ranuraArma++;
    }

    public void equiparNuevoPasivo(GameObject pasivo)
    {
        //Si el inventario esta lleno no tomamos mas armas
        if (ranuraPasivo >= inventario.ranurasObjetos.Count -1)
        {
            Debug.LogError("Inventario de pasivos lleno");
            return;
        }

        //Objeto a equipar
        GameObject nuevoObjeto = Instantiate(pasivo, transform.position, Quaternion.identity);
        nuevoObjeto.transform.SetParent(transform); //deja el arma inicial como hijo del jugador*/
        inventario.equiparPasivo(ranuraPasivo, nuevoObjeto.GetComponent<ObjetoPasivo>());

        ranuraPasivo++;
    }

    /*
    private void OnDestroy()
    {
        float finalizarMusica = 1.0f;
        OSCHandler.Instance.SendMessageToClient("SuperCollider", "/finalizarMusica", finalizarMusica);
    }*/
}

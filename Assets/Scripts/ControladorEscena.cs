using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorEscena : MonoBehaviour
{
    public void cambiarEscena(string nombre)
    {
        SceneManager.LoadScene(nombre);
        Time.timeScale = 1;
    }
}

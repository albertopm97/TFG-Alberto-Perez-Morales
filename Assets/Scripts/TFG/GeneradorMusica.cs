using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorMusica : MonoBehaviour
{
    public AudioClip[] notas; // Array de clips de sonido (notas)
    public int[] chordPattern; // Array de índices para representar los acordes

    // Start is called before the first frame update
    void Start()
    {
        // Generar una melodía aleatoria
        int[] melodyPattern = GenerateRandomMelody(100); // Por ejemplo, generamos una melodía de 16 notas

        // Reproducir la melodía generada
        //PlayMelody(melodyPattern);
        StartCoroutine(PlayMelody(melodyPattern));
    }

    int[] GenerateRandomMelody(int length)
    {
        int[] melody = new int[length];
        for (int i = 0; i < length; i++)
        {
            melody[i] = Random.Range(0, notas.Length); // Asignar una nota aleatoria
        }
        return melody;
    }

    IEnumerator PlayMelody(int[] melody)
    {
        AudioSource enReproduccion;
        foreach (int noteIndex in melody)
        {
            if (noteIndex >= 0 && noteIndex < notas.Length)
            {
                AudioSource.PlayClipAtPoint(notas[noteIndex], transform.position);
                yield return new WaitForSeconds(0.5f);
                // Otra opción: GetComponent<AudioSource>().PlayOneShot(notes[noteIndex]);
            }
        }
    }


    /*Para arpegios*/

    int[] GenerateArpeggio(int chordIndex)
    {
        int[] arpeggio = new int[4]; // Por ejemplo, generamos un arpegio de 4 notas

        // El arpegio se construirá a partir del acorde especificado en chordIndex
        // Por ejemplo, si chordIndex = 0, el arpegio será construido a partir del primer acorde en el array chordPattern

        // Supongamos que el acorde es representado por un array de índices de notas (por ejemplo, [0, 4, 7] para un acorde C)
        int[] chordNotes = GetChordNotes(chordIndex);

        // Generar el arpegio ascendente
        for (int i = 0; i < arpeggio.Length; i++)
        {
            arpeggio[i] = chordNotes[i % chordNotes.Length]; // Repetir las notas del acorde en el arpegio
        }

        return arpeggio;
    }

    int[] GetChordNotes(int chordIndex)
    {
        // Aquí deberías implementar una lógica para obtener los índices de notas que representan el acorde.
        // Por ejemplo, puedes tener un array de acordes predefinidos y usar chordIndex para seleccionar uno de ellos.
        // Retorna un array de índices que representen las notas del acorde seleccionado.
        // Por ejemplo, para un acorde C, podrías tener algo como return new int[] { 0, 4, 7 }; (C, E, G)

        // NOTA: La implementación de GetChordNotes depende de cómo estés representando los acordes en tu proyecto.

        // Ejemplo simple para un arpegio de acorde C mayor
        return new int[] { 0, 4, 7 }; // (C, E, G)
    }

    void PlayArpeggio(int[] arpeggio)
    {
        foreach (int noteIndex in arpeggio)
        {
            if (noteIndex >= 0 && noteIndex < notas.Length)
            {
                AudioSource.PlayClipAtPoint(notas[noteIndex], transform.position);
                // Otra opción: GetComponent<AudioSource>().PlayOneShot(notes[noteIndex]);
            }
        }
    }
}

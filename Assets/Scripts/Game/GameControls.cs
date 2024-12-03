using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControls : MonoBehaviour
{
    // Tecla para alternar o fullscreen (exemplo: F11)
    private KeyCode teclaFullscreen = KeyCode.F11;

    void Update()
    {
        // Verifica se a tecla foi pressionada
        if (Input.GetKeyDown(teclaFullscreen))
        {
            // Alterna entre fullscreen e modo janela
            ToggleFullscreen();
        }
    }

    void ToggleFullscreen()
    {
        // Verifica se o jogo já está no modo fullscreen
        if (Screen.fullScreen)
        {
            // Se estiver em fullscreen, muda para modo janela
            Screen.fullScreen = false;
        }
        else
        {
            // Se não estiver em fullscreen, muda para fullscreen
            Screen.fullScreen = true;
        }
    }
}

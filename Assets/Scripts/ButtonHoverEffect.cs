using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonHoverEffectTMP : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI buttonText; // O componente TextMeshProUGUI do botão
    public TMP_FontAsset hoverFont;      // A fonte a ser usada no hover
    public TMP_FontAsset defaultFont;    // A fonte padrão
    private Color originalColor;

    void Start()
    {
        // Salva a cor original do texto
        originalColor = buttonText.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Muda a fonte e a cor do texto
        buttonText.font = hoverFont;
        buttonText.color = Color.white; // Muda a cor do texto para vermelho
        AddUnderline();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Restaura a fonte e a cor original do texto
        buttonText.font = defaultFont;
        buttonText.color = originalColor; // Restaura a cor original
        RemoveUnderline();
    }

    private void AddUnderline()
    {
        // Adiciona sublinhado ao texto
        buttonText.text = "<u>" + buttonText.text + "</u>"; // Adiciona o sublinhado
    }

    private void RemoveUnderline()
    {
        // Remove o sublinhado do texto
        buttonText.text = buttonText.text.Replace("<u>", "").Replace("</u>", "");
    }
}

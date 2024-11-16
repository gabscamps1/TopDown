using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Money : MonoBehaviour
{
    public int moneyAmount;

    [Header("Balanço Vertical")]
    public float amplitude = 0.5f; // Altura do movimento
    public float speed = 1.0f;    // Velocidade do movimento

    [Header("Balanço Horizontal (opcional)")]
    public bool enableHorizontalSwing = false;
    public float horizontalAmplitude = 0.5f;
    public float horizontalSpeed = 1.0f;

    private Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        // Salva a posição inicial do objeto
        startPosition = transform.position;

        // Calcula a quantia de dinheiro.
        moneyAmount = Random.Range(1,6);
    }

    void Update()
    {
        // Movimento vertical
        float verticalOffset = Mathf.Sin(Time.time * speed) * amplitude;

        // Movimento horizontal opcional
        float horizontalOffset = enableHorizontalSwing ? Mathf.Cos(Time.time * horizontalSpeed) * horizontalAmplitude : 0;

        // Aplica o movimento
        transform.position = startPosition + new Vector3(horizontalOffset, verticalOffset, 0);
    }


}




using TMPro;
using UnityEngine;

public class RandomNameGenerator : MonoBehaviour
{
    public TextMeshProUGUI tmpText; // Refer�ncia para o componente TextMeshProUGUI
    public float changeInterval = 1.0f; // Intervalo de tempo entre as trocas, em segundos

    private string[] firstNames = {
                                    "James",
                                    "John",
                                    "William",
                                    "Charles",
                                    "George",
                                    "Frank",
                                    "Joseph",
                                    "Richard",
                                    "Edward",
                                    "Donald",
                                    "Thomas",
                                    "Paul",
                                    "Harold",
                                    "Raymond",
                                    "Jack",
                                    "Henry",
                                    "Albert",
                                    "Arthur",
                                    "Kenneth",
                                    "David",
                                    "Eugene",
                                    "Howard",
                                    "Fred",
                                    "Harry",
                                    "Walter",
                                    "Willie",
                                    "Ralph",
                                    "Lawrence",
                                    "Roy",
                                    "Joe",
                                    "Clarence",
                                    "Louis",
                                    "Carl",
                                    "Earl",
                                    "Francis",
                                    "Ernest",
                                    "Leonard",
                                    "Stanley",
                                    "Samuel",
                                    "Alfred",
                                    "Herbert",
                                    "Anthony",
                                    "Norman",
                                    "Melvin",
                                    "Philip",
                                    "Daniel",
                                    "Bernard",
                                    "Lee",
                                    "Marvin",
                                    "Peter",
                                    "Ray",
                                    "Leroy",
                                    "Clifford",
                                    "Floyd",
                                    "Edwin",
                                    "Vernon",
                                    "Chester",
                                    "Michael",
                                    "Martin",
                                    "Glenn",
                                    "Leo",
                                    "Jimmie",
                                    "Bill",
                                    "Theodore",
                                    "Wayne",
                                    "Clyde",
                                    "Allen",
                                    "Alvin",
                                    "Milton",
                                    "Lloyd",
                                    "Lewis",
                                    "Gene",
                                    "Victor",
                                    "Ben",
                                    "Vincent",
                                    "Gerald",
                                    "Leslie",
                                    "Arnold",
                                    "Don",
                                    "Keith",
                                    "Wilbur",
                                    "Clinton",
                                    "Stephen",
                                    "Gilbert",
                                    "Horace",
                                    "Jessie",
                                    "Sidney",
                                    "Wallace",
                                    "Charlie",
                                    "Herman",
                                    "Frederick",
                                    "Harvey",
                                    "Douglas",
                                    "Elmer",
                                    "Johnnie",
                                    "Leon",
                                    "Cecil",
                                    "Jess",
                                    "Alex",
                                    "Roland",
                                    "Tony"
    };

    private string[] lastNames = {
                                    "Johnson",
                                    "Williams",
                                    "Brown",
                                    "Jones",
                                    "Garcia",
                                    "Miller",
                                    "Davis",
                                    "Rodriguez",
                                    "Martinez",
                                    "Hernandez",
                                    "Lopez",
                                    "Gonzalez",
                                    "Wilson",
                                    "Anderson",
                                    "Thomas",
                                    "Taylor",
                                    "Moore",
                                    "Jackson",
                                    "Martin",
                                    "Lee",
                                    "Perez",
                                    "Thompson",
                                    "White",
                                    "Harris",
                                    "Sanchez",
                                    "Clark",
                                    "Ramirez",
                                    "Lewis",
                                    "Robinson",
                                    "Walker",
                                    "Young",
                                    "Allen",
                                    "King",
                                    "Wright",
                                    "Scott",
                                    "Torres",
                                    "Nguyen",
                                    "Hill",
                                    "Flores",
                                    "Green",
                                    "Adams",
                                    "Nelson",
                                    "Baker",
                                    "Hall",
                                    "Rivera",
                                    "Campbell",
                                    "Mitchell",
                                    "Carter",
                                    "Roberts",
                                    "Gomez",
                                    "Phillips",
                                    "Evans",
                                    "Turner",
                                    "Diaz",
                                    "Parker",
                                    "Cruz",
                                    "Edwards",
                                    "Collins",
                                    "Reyes",
                                    "Stewart",
                                    "Morris",
                                    "Morales",
                                    "Murphy",
                                    "Cook",
                                    "Rogers",
                                    "Gutierrez",
                                    "Ortiz",
                                    "Morgan",
                                    "Cooper",
                                    "Peterson",
                                    "Bailey",
                                    "Reed",
                                    "Kelly",
                                    "Howard",
                                    "Ramos",
                                    "Kim",
                                    "Cox",
                                    "Ward",
                                    "Richardson",
                                    "Watson",
                                    "Brooks",
                                    "Chavez",
                                    "Wood",
                                    "James",
                                    "Bennett",
                                    "Gray",
                                    "Mendoza",
                                    "Ruiz",
                                    "Hughes",
                                    "Price",
                                    "Alvarez",
                                    "Castillo",
                                    "Sanders",
                                    "Patel",
                                    "Myers",
                                    "Long",
                                    "Ross",
                                    "Foster",
                                    "Jimenez"
    };

    private float timer = 0f; // Temporizador para controlar o intervalo

    void Update()
    {
        // Incrementa o temporizador com o tempo passado desde o �ltimo frame
        timer += Time.deltaTime;

        // Verifica se o temporizador atingiu o intervalo definido
        if (timer >= changeInterval)
        {
            // Gera um nome e sobrenome aleat�rios
            string randomFirstName = firstNames[Random.Range(0, firstNames.Length)];
            string randomLastName = lastNames[Random.Range(0, lastNames.Length)];

            // Atualiza o texto no componente TextMeshPro
            tmpText.text = $"{randomFirstName} {randomLastName}";

            // Reseta o temporizador
            timer = 0f;
        }
    }
}

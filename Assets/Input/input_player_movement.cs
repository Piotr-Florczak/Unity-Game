using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class input_player_movement : MonoBehaviour
{
    Vector2 dane_wejscowe;
    Rigidbody2D fizyka;
    [SerializeField] float przyspiesznie = 6;

    void Start()
    {
        fizyka = GetComponent<Rigidbody2D>(); // pobiera wszystkie funkcje komponentu "Rigidbody2d" do zmiennej "fizyka"
    }

    // Update is called once per frame
    void Update()
    {
        Run();
    }

    void OnMove(InputValue value)             // fukncja wywo�uje si� automatycznie kiedy zostaje wcisni�ty klwiesz akcji typu W,A,S,D. Oraz przyjmuje (ze �rodowiska unity) arugumenty typu vector. np kiedy wcisniemy 'D' zmienna typu inputvalue b�dzie si� r�wna� (1,0).
    {
        dane_wejscowe = value.Get<Vector2>(); // zmienna inputvalue dzia�a tylko w ciele funkcji onMove dlatego stworzy�em zmienn� globaln� typu Vecotr2 do kt�rej przypisuje te warto�ci ponadto InputValue jest obiektem kt�ry posiada wile zmiennych, dlatego getem pobieramy zmienn� typu vacotr
    }
    void Run()
    {
        Vector2 predkosc_gracza = new Vector2(dane_wejscowe.x, fizyka.velocity.y);      //w bieganiu postaci interesuje nas tylko o� X, dlatego nie zmieniamy nic w osi Y .dlaetgo do y przypisujemy defaultowe dane kt�re aktualnie po�iada komponent 
        fizyka.velocity = predkosc_gracza * przyspiesznie;                              // do komponentu rigidbody przypisujemy akualn� pr�dkos� gracza 
    }
}

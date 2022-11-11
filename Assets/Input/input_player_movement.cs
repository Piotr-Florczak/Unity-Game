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

    void OnMove(InputValue value)             // fukncja wywo³uje siê automatycznie kiedy zostaje wcisniêty klwiesz akcji typu W,A,S,D. Oraz przyjmuje (ze œrodowiska unity) arugumenty typu vector. np kiedy wcisniemy 'D' zmienna typu inputvalue bêdzie siê równaæ (1,0).
    {
        dane_wejscowe = value.Get<Vector2>(); // zmienna inputvalue dzia³a tylko w ciele funkcji onMove dlatego stworzy³em zmienn¹ globaln¹ typu Vecotr2 do której przypisuje te wartoœci ponadto InputValue jest obiektem który posiada wile zmiennych, dlatego getem pobieramy zmienn¹ typu vacotr
    }
    void Run()
    {
        Vector2 predkosc_gracza = new Vector2(dane_wejscowe.x, fizyka.velocity.y);      //w bieganiu postaci interesuje nas tylko oœ X, dlatego nie zmieniamy nic w osi Y .dlaetgo do y przypisujemy defaultowe dane które aktualnie poœiada komponent 
        fizyka.velocity = predkosc_gracza * przyspiesznie;                              // do komponentu rigidbody przypisujemy akualn¹ prêdkosæ gracza 
    }
}

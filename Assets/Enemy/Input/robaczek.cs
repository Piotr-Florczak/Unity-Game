using UnityEngine;

public class robaczek : MonoBehaviour
{
    [SerializeField]
    private float speed = 2.0f; // Szybkość ruchu przeciwnika

    [SerializeField]
    private float jumpForce = 5.0f; // Siła skoku przeciwnika

    [SerializeField]
    private float groundCheckDistance = 0.5f; // Dystans sprawdzania podłoża

    [SerializeField]
    private float edgeCheckDistance = 1.0f; // Dystans sprawdzania krawędzi przed przeciwnikiem

    [SerializeField]
    private float jumpIntervalMin = 2.0f; // Minimalny czas między skokami

    [SerializeField]
    private float jumpIntervalMax = 5.0f; // Maksymalny czas między skokami

    private Rigidbody2D rb;
    private float direction = 1.0f; // Kierunek ruchu przeciwnika (1.0f - prawo, -1.0f - lewo)
    private float timeUntilNextJump; // Czas do następnego skoku

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Pobierz komponent Rigidbody2D przeciwnika
        timeUntilNextJump = Random.Range(jumpIntervalMin, jumpIntervalMax); // Losuj czas do następnego skoku
    }

    private void FixedUpdate()
    {
        // Ustaw prędkość Rigidbody2D w osi X
        rb.velocity = new Vector2(speed * direction, rb.velocity.y);

        // Rzuć promień w dół, aby sprawdzić czy przeciwnik ma kontakt z podłożem
        RaycastHit2D hitGround = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance);

        // Rzuć promienie w kierunku ruchu, aby sprawdzić czy przeciwnik nie spadnie z krawędzi (z obu stron modelu)
        Vector2 edgeCheckDirection = new Vector2(direction, 0);
        RaycastHit2D hitEdge1 = Physics2D.Raycast(transform.position + new Vector3(0, -0.5f, 0), edgeCheckDirection, edgeCheckDistance);
        RaycastHit2D hitEdge2 = Physics2D.Raycast(transform.position + new Vector3(0, 0.5f, 0), edgeCheckDirection, edgeCheckDistance);

        // Jeśli przeciwnik utraci kontakt z podłożem lub zbliży się do krawędzi, zmień kierunek
        if (hitGround.collider == null || (hitEdge1.collider == null && hitEdge2.collider == null))
        {
            direction *= -1;
        }

        // Jeśli przeciwnik ma kontakt z podłożem, zmniejsz czas do następnego skoku
        if (hitGround.collider != null)
        {
            timeUntilNextJump -= Time.fixedDeltaTime;
            if (timeUntilNextJump <= 0)
            {
                // Skacz, gdy czas do następnego skoku się skończy
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                // Losuj czas do następnego skoku
                timeUntilNextJump = Random.Range(jumpIntervalMin, jumpIntervalMax);
            }
        }
    }
    // Opcjonalnie: rysuj linie w Scene View w celu wizualizacji raycastingu
    private void OnDrawGizmos()
{
    Gizmos.color = Color.red;
    Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    Gizmos.DrawLine(transform.position + new Vector3(0, -0.5f, 0), transform.position + new Vector3(0, -0.5f, 0) + new Vector3(direction, 0, 0) * edgeCheckDistance);
    Gizmos.DrawLine(transform.position + new Vector3(0, 0.5f, 0), transform.position + new Vector3(0, 0.5f, 0) + new Vector3(direction, 0, 0) * edgeCheckDistance);
}

}


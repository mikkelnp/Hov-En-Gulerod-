using UnityEngine;

public class Veggie : MonoBehaviour
{
    // Ansvar: Bogstavet ved selv hvilken plads den er over og det korrekte svar.

    [SerializeField] private string veggieType;

    private TomPlads nuvaerendeTomPlads;
    private Rigidbody2D rb; // Fordi vi skal fryse position efter korrekt placering.

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Gør den collider, bogstavet nu er over, til den nuværende:
        nuvaerendeTomPlads = collision.GetComponent<TomPlads>();

        Debug.Log("Bogstav er nu over: " + collision.name);
    }

    public void TjekOmKorrektPlads()
    {
        if (nuvaerendeTomPlads != null)
        {
            if (nuvaerendeTomPlads.korrektBogstav == veggieType)
            {
                Debug.Log("Korrekt bogstav på denne plads!");
                transform.position = nuvaerendeTomPlads.transform.position;
                rb.constraints = RigidbodyConstraints2D.FreezeAll; // Fryser al bevægelse
                gameObject.layer = 6;
                gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                nuvaerendeTomPlads.DestroySelf();
                GetComponent<AudioSource>().Play();
            }
            else
            {
                Debug.Log("Forkert plads, hvad er der i vejen med dig?");
            }
        }
    }
}

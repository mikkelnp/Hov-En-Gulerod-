using TMPro;
using UnityEngine;

public class CanvasText : MonoBehaviour
{
    [SerializeField] GameObject movesLeftText;
    [SerializeField] GameObject amountMatchedText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateMovesLeftText(int movesLeft)
    {
        movesLeftText.GetComponent<TextMeshProUGUI>().text = movesLeft + "";
    }
    public void UpdateAmountMatchedText(int amountMatched, int amountNeeded)
    {
        amountMatchedText.GetComponent<TextMeshProUGUI>().text = amountMatched + "/" + amountNeeded;

    }
}

using UnityEngine;
using TMPro;
/*
    I want this code to assign a random number(1-8) to 16 blocks(only 2 can have the same numbers)

    each number is gonna have its own symbol in it

    when a block is interacted with it stays "activated" until another block is chose, 
    if the both blocks are the same symbol, then the block get marked as done
    */
public class MatchingBlock : MonoBehaviour, IInteractable
{
    public int blockID;
    public int symbolNumber;

    public TextMeshPro symbolText;

    public Renderer blockRenderer;
    public Color matchedColor = Color.green;

    private bool isRevealed = false;
    private bool isMatched = false;
    private Color originalColor;

    public bool IsMatched => isMatched;

    void Start()
    {
        if (blockRenderer != null)
        {
            originalColor = blockRenderer.material.color;
        }

        if (symbolText != null)
        {
            symbolText.gameObject.SetActive(false);
        }
    }

    public void Interact()
    {
        if (isMatched)
        {
            return;
        }

        FindAnyObjectByType<MatchingManager>().SelectBlock(this);

        Debug.Log("block " + blockID + " revealed the symbol" + symbolNumber);
        Debug.Log("Block " + blockID + " interacted with!");
        Debug.Log("Symbol number: " + symbolNumber);
    }

    public void SetMatched()
    {
        isMatched = true;
        isRevealed = true;

        if (symbolText != null)
        {
            symbolText.text = symbolNumber.ToString();
            symbolText.gameObject.SetActive(true);
        }

        if (blockRenderer != null)
        {
            blockRenderer.material.color = matchedColor;
        }
        Debug.Log("Block " + blockID + " is matched!!!!");
    }

    public void RevealSymbol()
    {
        isRevealed = true;
        
        if (symbolText != null)
        {
            symbolText.text = symbolNumber.ToString();
            symbolText.gameObject.SetActive(true);
        }
    }

    public void HideSymbol()
    {
        if (isMatched)
        {
            return;
        }
        
        isRevealed = false;

        if (symbolText != null)
        {
            symbolText.gameObject.SetActive(false);
        }
    }
}

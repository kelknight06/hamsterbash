using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchingManager : MonoBehaviour
{
    public MatchingBlock[] blocks;
    public GameTimer1 gameTimer;
    public TMPro.TMP_Text roundText;

    private MatchingBlock firstBlock;
    private MatchingBlock secondBlock;

    private bool isCheckingMatch = false;
    private bool allBlocksMatched = false;
    public bool AllBlocksMatched => allBlocksMatched;

    private int matchedPairs = 0;
    public int totalRounds = 1;
    private int currentRound = 1;

    public bool FinalRoundCompleted => currentRound >= totalRounds && allBlocksMatched;

    void Start()
    {
        RandomizeSymbols();
        UpdateRoundText();
    }

    void UpdateRoundText()
    {
        roundText.text = currentRound + "/" + totalRounds;
    }

    void RandomizeSymbols()
    {
        List<int> symbols = new List<int>();

        for (int i = 1; i <= 8; i++)
        {
            symbols.Add(i);
            symbols.Add(i);
        }

        
        for (int i = 0; i < symbols.Count; i++)
        {
            int randomIndex = Random.Range(i, symbols.Count);

            int temp = symbols[i];
            symbols[i] = symbols[randomIndex];
            symbols[randomIndex] = temp;
        }

        
        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i].symbolNumber = symbols[i];
        }
    }

    public void SelectBlock(MatchingBlock block)
    {
        if (isCheckingMatch || block.IsMatched)
            return;

        if (block == firstBlock)
            return;

        block.RevealSymbol();

        if (firstBlock == null)
        {
            firstBlock = block;
            return;
        }

        secondBlock = block;

        if (firstBlock.symbolNumber == secondBlock.symbolNumber)
        {
            firstBlock.SetMatched();
            secondBlock.SetMatched();
            matchedPairs = matchedPairs + 1;
            if (matchedPairs == blocks.Length / 2)
                {
                    allBlocksMatched = true;
                    CompleteRound();
                }

            ResetSelection();
        }
        else
        {
            StartCoroutine(HideMismatchedBlocks());
        }
    }

    IEnumerator HideMismatchedBlocks()
    {
        isCheckingMatch = true;

        yield return new WaitForSeconds(1.5f);

        firstBlock.HideSymbol();
        secondBlock.HideSymbol();

        ResetSelection();

        isCheckingMatch = false;
    }
    

    void ResetSelection()
    {
        firstBlock = null;
        secondBlock = null;
    }

    public void ResetBoard()
    {
        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i].ResetBlock();
        }
        matchedPairs = 0;
        allBlocksMatched = false;
        RandomizeSymbols();
    }

    public void CompleteRound()
    {
        if (currentRound >= totalRounds)
        {
            return;
        }

        currentRound++;
        UpdateRoundText();
        gameTimer.AddRoundTime();
        ResetBoard();
    }
}

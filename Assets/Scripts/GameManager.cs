using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [Header("T.U.M. Buff System Setup")]
    public List<BuffData> allAvailableBuffs; // Assign all BuffData assets here
    [SerializeField] private GameObject P1Choice1;
    [SerializeField] private GameObject P1Choice2;
    [SerializeField] private GameObject P2Choice1;
    [SerializeField] private GameObject P2Choice2;
    // Button3 is removed as the T.U.M. design uses only 2 random choices

    [Header("Game State")]
    public int player1Deaths;
    public int player2Deaths;
    [SerializeField] private Transform player1Spawn;
    [SerializeField] private Transform player2Spawn;

    private int deadPlayerIndex; // The player who lost the Stock (0 or 1)
    private BuffData offeredBuff1;
    private BuffData offeredBuff2;
    
    // Cached PlayerCharacter components for T.U.M. logic
    private PlayerCharacter player1Char;
    private PlayerCharacter player2Char;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        player1Deaths = 0;
        player2Deaths = 0;
    }

    private void Start()
    {
        // UI Setup
        // NOTE: You'll need separate scripts to update the text/icon on Button1 and Button2
        P1Choice1 = GameObject.FindWithTag("P1C1");
        P1Choice2 = GameObject.FindWithTag("P1C2");
        P1Choice1.SetActive(false);
        P1Choice2.SetActive(false);
        P2Choice1 = GameObject.FindWithTag("P2C1");
        P2Choice2 = GameObject.FindWithTag("P2C2");
        P2Choice1.SetActive(false);
        P2Choice2.SetActive(false);
        
        // Find and Cache PlayerCharacter references
        player1Char = GameObject.FindWithTag("Player1")?.GetComponent<PlayerCharacter>();
        player2Char = GameObject.FindWithTag("Player2")?.GetComponent<PlayerCharacter>();

        if (player1Char == null || player2Char == null)
        {
             Debug.LogError("Ensure players have the PlayerCharacter script attached and correct tags.");
        }
    }

    public void PlayerDied(int playerIndex)
    {
        deadPlayerIndex = playerIndex; 
        
        PlayerCharacter deadPlayerComponent = (playerIndex == 0) ? player1Char : player2Char;
        
        // --- C. Buff Reset Logic (Step 1: Stock Loss while Active) ---
        if (deadPlayerComponent != null && deadPlayerComponent.isBuffActive)
        {
            deadPlayerComponent.ResetBuff(); // Buff is instantly lost upon death
            Debug.Log($"P{playerIndex + 1} died while buff was active. Buff consumed.");
        }

        // Increment Death count (Stock Loss)
        if (playerIndex == 0)
        {
            player1Deaths++;
        }
        else
        {
            player2Deaths++;
        }

        if (player1Deaths >= 4 || player2Deaths >= 4) // Check for match end (assuming 2 stocks total)
        {
            SceneManager.LoadScene("Post Fight");
            
        }
        else
        {
            // Reset position, health, and freeze time for the Selection Phase

            // Handle Player Movement/Control enabling/disabling (You will need to adjust this based on your control scripts)
            if (deadPlayerComponent != null)
            {
                // Reset health
                deadPlayerComponent.GetComponent<DummyStats>().HP = 100;
                // Reset position
                deadPlayerComponent.transform.position = (playerIndex == 0) ? player1Spawn.position : player2Spawn.position;
                
                // Disable controls during the low-tension selection phase
                //player1Char.enabled = false;
                //player2Char.enabled = false;
            }
            
            // --- A. Buff Selection Logic (Step 2: New Offer) ---
            // DDA check: Only offer the buff if the player is still the losing player in Stock count

            if ((playerIndex == 0) || (playerIndex == 1))
            {
                OfferBuffChoices(playerIndex);
            }
            else
            {
                // If stocks are equal or the dead player is leading, just resume the game
                ResumeGame(); 
            }
        }
    }

    private void OfferBuffChoices(int playerIndex)
    {
        //Debug.Log("Dead player is " + playerIndex);
        // 1. Get two random, unique buffs
        List<BuffData> pool = new List<BuffData>(allAvailableBuffs);
        if (pool.Count < 2) 
        {
            Debug.LogError("Not enough unique buffs defined in the allAvailableBuffs list!");
            ResumeGame();
            return; 
        }

        // Randomly select two unique buffs
        offeredBuff1 = pool[Random.Range(0, pool.Count)];
        Debug.Log(offeredBuff1);
        pool.Remove(offeredBuff1);
        offeredBuff2 = pool[Random.Range(0, pool.Count)];

        switch (deadPlayerIndex)
        {
            case 0:
                // 2. Present the pop-up (UI)
                P1Choice1.SetActive(true);
                P1Choice2.SetActive(true);
        
                // TODO: A separate UI script needs to be updated here to show:
                P1Choice1.GetComponent<TextMeshProUGUI>().text = "Press L1 for " +offeredBuff1.buffName;
                P1Choice2.GetComponent<TextMeshProUGUI>().text = "Press L2 for " +offeredBuff2.buffName;
                break;
            case 1:
                // 2. Present the pop-up (UI)
                P2Choice1.SetActive(true);
                P2Choice2.SetActive(true);
        
                // TODO: A separate UI script needs to be updated here to show:
                P2Choice1.GetComponent<TextMeshProUGUI>().text = "Press L1 for " + offeredBuff1.buffName;
                P2Choice2.GetComponent<TextMeshProUGUI>().text = "Press L2 for " +offeredBuff2.buffName;
                break;
        }
    }
    
    // --- UI Button Callbacks (Linked to the buttons in the Unity Inspector) ---

    // Called when the player selects the first button
    public void P1Choice1Selected()
    {
        Debug.Log("P1C1");
        if (deadPlayerIndex == 0)
        {
            SelectBuff(offeredBuff1);
            Debug.Log(offeredBuff1);
        }
    }
    
    // Called when the player selects the second button
    public void P1Choice2Selected()
    {
        Debug.Log("P1C2");
        if (deadPlayerIndex == 0)
        {
            SelectBuff(offeredBuff2);
            Debug.Log(offeredBuff2);
        }
    }
    public void P2Choice1Selected()
    {
        Debug.Log("P2C1");
        if (deadPlayerIndex == 1)
        {
            SelectBuff(offeredBuff1);
            Debug.Log(offeredBuff1);
        }
    }
    
    // Called when the player selects the second button
    public void P2Choice2Selected()
    {
        Debug.Log("P2C2");
        if (deadPlayerIndex == 1)
        {
            SelectBuff(offeredBuff2);
            Debug.Log(offeredBuff2);
        }
    }

    private void SelectBuff(BuffData chosenBuff)
    {
        // 3. Pass the chosen buff to the PlayerCharacter for storage
        PlayerCharacter targetPlayer = (deadPlayerIndex == 0) ? player1Char : player2Char;
        
        if (targetPlayer != null)
        {
            targetPlayer.StoreBuff(chosenBuff);
            targetPlayer.TUM_Activate();
        }
        
        ResumeGame();
    }
    
    private void ResumeGame()
    {
        // Reset and resume game flow
        P1Choice1.SetActive(false);
        P1Choice2.SetActive(false);
        P2Choice1.SetActive(false);
        P2Choice2.SetActive(false);
        
        // Re-enable player controls
        if (player1Char != null) player1Char.enabled = true;
        if (player2Char != null) player2Char.enabled = true;
        
        // Clear the offered buffs
        offeredBuff1 = null;
        offeredBuff2 = null;
    }
}
﻿using UnityEngine;
using UnityEngine.Events;

public class TestingPlayer : MonoBehaviour
{
    public int maxLevel = 5;
    public int playerLevel = 1;
    public int playerGold = 0;
    public UnityEvent OnLevelUp; //On level Up event
    public UnityEvent OnGoldChange; //On player's gold Changes event
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Gain level
    public void gainLevel(int cost)
    {
        //Check if Player's level is MAX
        if (checkLevel())
        {
            //Check if Player's gold is enough or not
            if (checkGold(cost)) //If enough gold, take gold off from Player
            {
                removeGold(cost);
                addOneLevel();
            }
            else
            {
                //TODO
                //An output that shows player's gold is not enough to buy level-up
                Debug.Log("Not enough gold");
            }
        }
        else
        {
            //TODO
            //An output saying player's level is max
            Debug.Log("Player's level has already reached max");
        }
    }

    //method: check if gold is enough 
    public bool checkGold(int gold)
    {
        if (playerGold >= gold)
            return true;

        return false;
    }

    //method: Gain gold
    public void gainGold(int gold)
    {
        playerGold += gold;
        OnGoldChange.Invoke();
    }

    //method: remove gold 
    public void removeGold(int gold)
    {
        playerGold -= gold;
        OnGoldChange.Invoke();
    }

    //method: check is player level is max
    public bool checkLevel()
    {
        if (playerLevel < maxLevel)
            return true;

        return false;
    }

    //method: increase level by 1
    public void addOneLevel()
    {
        playerLevel++;
        OnLevelUp.Invoke();
    }

    //Getter for Return Player's level
    public int GetPlayerLevel()
    {
        return playerLevel;
    }


    //Don't know how this will look in final version of Player script, but should be here
    public int GetPlayerGold()
    {
        return playerGold;
    }
}
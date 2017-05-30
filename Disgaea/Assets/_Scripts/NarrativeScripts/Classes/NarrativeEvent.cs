using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrativeEvent// : MonoBehaviour
{
    // MUST MATCH JSON - see DataTest.json
    public List<Dialogue> dialogues;

}

// Data that must match JSON data
public struct Dialogue
{
    public CharacterType characterType;
    public string name;
    public string atlasImageName;
    public string dialogueText;
    public bool bMultiLines;
}

// Not sure if we need this atm... but maybe in the future
public enum CharacterType
{
    Laharl = 0,
    Etna = 1,
    Angel = 2,
    Archer = 3,
    Brawler = 4,
    FireMage = 5,
    FireSkull = 6,
    Flonne = 7,
    Gordon = 8,
    Healer = 9,
    Hoggmeiser = 10,
    Jennifer = 11,
    KurtisPrinny = 12,
    Maderas = 13,
    MagicKnight = 14,
    Majin = 15,
    Marjoly = 16,
    Ninja = 17,
    Plenair = 18,
    Priere = 19,
    Prinny = 20,
    Samurai = 21,
    Scout = 22,
    Thief = 23,
    Thursday = 24,
    Warrior = 25,
    Empty 

}
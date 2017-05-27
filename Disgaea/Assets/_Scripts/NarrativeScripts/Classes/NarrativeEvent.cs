using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrativeEvent// : MonoBehaviour
{
    // MUST MATCH JSON - see DataTest.json
    public List<Dialogue> dialogues;

}

public struct Dialogue
{
    public CharacterType characterType;
    public string name;
    public string atlasImageName;
    public string dialogueText;
    public bool bMultiLines;
}

public enum CharacterType
{
    Laharl,
    Etna
}
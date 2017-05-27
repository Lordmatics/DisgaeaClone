using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JSONFactory;

public class TestJSON : MonoBehaviour
{

	void Start ()
    {
        NarrativeEvent testEvent = JSONAssembly.RunJSONFactoryForIndex(1);
        Debug.Log(testEvent.dialogues[0].characterType);
        Debug.Log(testEvent.dialogues[0].name);
        Debug.Log(testEvent.dialogues[0].dialogueText);
        Debug.Log(testEvent.dialogues[0].atlasImageName);


    }

}

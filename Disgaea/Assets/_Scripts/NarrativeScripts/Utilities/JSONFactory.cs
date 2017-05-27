using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO; // Read Write
using LitJson; // Custom JSON library dll

// List of JSON file with path extensions
// Only NarrativeManager should be able to use this script
// Take in scene number, output Narrative Event - Black Box
// Validation and Exception handling

namespace JSONFactory
{
    class JSONAssembly
    {
        private static Dictionary<string, string> _resourceList = new Dictionary<string, string>
        {
            {"TestOne", "/Resources/NarrativeData/DataTest.json" },
            {"TestTwo", "/Resources/NarrativeData/ConsecutiveTest.json" }
        };

        public static NarrativeEvent RunJSONFactoryForIndex(string dictionaryIndex)
        {
            string resourcePath = PathForScene(dictionaryIndex);

            if(IsValidJSON(resourcePath) == true)
            {
                string jsonString = File.ReadAllText(Application.dataPath + resourcePath);
                NarrativeEvent narEvent = JsonMapper.ToObject<NarrativeEvent>(jsonString);
                return narEvent;
            }
            else
            {
                throw new Exception("The JSON is not valid, please check the schema and file extension");
            }
        }

        private static string PathForScene(string dictionaryIndex)
        {
            string resourcePathResult;
            if(_resourceList.TryGetValue(dictionaryIndex, out resourcePathResult))
            {
                return _resourceList[dictionaryIndex];
            }
            else
            {
                //return "Failed";
                throw new Exception("The scene number you provided is not in the resource list. Please check the JSONFactory namespace");
            }
        }

        private static bool IsValidJSON(string path)
        {
            return (Path.GetExtension(path) == ".json") ? true : false;
        }
    }
}


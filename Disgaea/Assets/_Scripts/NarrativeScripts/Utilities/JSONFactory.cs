﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO; // Read Write
using LitJson; // Custom JSON library dll

// List of JSON file with path extensions
// Validation and Exception handling

namespace JSONFactory
{
    class JSONAssembly
    {
        // Look up Dictionary - Extensible for any JSON purposes
        private static Dictionary<string, string> _resourceList = new Dictionary<string, string>
        {
            {"TestOne", "/Resources/NarrativeData/DataTest.json" },
            {"TestTwo", "/Resources/NarrativeData/ConsecutiveTest.json" },
            {"TestThree", "/Resources/NarrativeData/AnotherConsecutiveTest.json" },
            {"Shop_Weapon", "/Resources/NarrativeData/Shop_Weapon.json" },
            {"Shop_Armour", "/Resources/NarrativeData/Shop_Armour.json" },
            {"Shop_Healing", "/Resources/NarrativeData/Shop_Healing.json" },
            {"Dark_Assembly", "/Resources/NarrativeData/Dark_Assembly.json" },
            {"Level_Portal", "/Resources/NarrativeData/Level_Portal.json" },
            {"Item_World", "/Resources/NarrativeData/Item_World.json" },
            {"Test_Event", "/Resources/NarrativeData/Test_Event.json" }


        };

        // Load the conversation at a given dictionary key
        // + Validity checks
        public static NarrativeEvent RunJSONFactoryForIndex(string dictionaryIndex)
        {
            string resourcePath = PathForData(dictionaryIndex);

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

        // Validity check on the dictionary look up
        private static string PathForData(string dictionaryIndex)
        {
            string resourcePathResult;
            if(_resourceList.TryGetValue(dictionaryIndex, out resourcePathResult))
            {
                return _resourceList[dictionaryIndex];
            }
            else
            {
                //return "Failed";
                throw new Exception("The look up key you provided is not in the resource list. Please check the JSONFactory namespace");
            }
        }

        // Validity check for json file extension
        private static bool IsValidJSON(string path)
        {
            return (Path.GetExtension(path) == ".json") ? true : false;
        }
    }
}


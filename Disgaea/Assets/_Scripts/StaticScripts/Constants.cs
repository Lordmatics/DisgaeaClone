using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Constants
{
    struct AnimationTuple
    {
        public string parameter;
        public bool value;

        public  AnimationTuple(string param,bool val)
        {
            parameter = param;
            value = val;
        }
    }

    internal class AnimationTuples
    {
        internal static AnimationTuple introAnimation = new AnimationTuple("EnterConversation", true);
        internal static AnimationTuple exitAnimation = new AnimationTuple("EnterConversation", false);
    }
}

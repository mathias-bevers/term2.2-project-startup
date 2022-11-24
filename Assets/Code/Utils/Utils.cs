using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Utils {

    public static float Map(float x, float in_min, float in_max, float out_min, float out_max)
    {
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }

    public static bool NullCheck(Object obj)
    {
        return ReferenceEquals(obj, null);
    }

    public static string[] ChopBaseText(string textToChop, int characterLimit)
    {
	    List<string> parts = new() { textToChop };

	    while (parts.Last().Length > characterLimit)
	    {
		    string chopping = parts.Last();
		    parts.Remove(chopping);

		    for (int i = characterLimit; i >= 0; --i)
		    {
			    if (chopping[i] != ' ') { continue; }

			    parts.Add(chopping[..i]);
			    parts.Add(chopping[(i + 1)..]);
			    break;
		    }
	    }

	    return parts.ToArray();
    }
	
    public static string StyleEndCharacter(Color color, string toColor) => $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{toColor}</color>";

}

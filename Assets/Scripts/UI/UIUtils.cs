using UnityEngine;
using System.Collections;

public class UIUtils   {

	public static void ShowCanvasGroup(CanvasGroup cg)
	{
		if (cg == null)
		{
			Debug.Log ("Messing Canvas Group");
		}
		else
		{
			cg.alpha = 1;
			cg.interactable = true;
			cg.blocksRaycasts = true;
		}
	}
	
	public static void HideCanvasGroup(CanvasGroup cg)
	{
		if (cg == null)
		{
			Debug.Log ("Messing Canvas Group");
		}
		else
		{
			cg.alpha = 0;
			cg.interactable = false;
			cg.blocksRaycasts = false;
		}
	}
}

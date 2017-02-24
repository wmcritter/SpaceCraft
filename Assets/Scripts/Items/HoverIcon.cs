using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HoverIcon : MonoBehaviour {

	public Text stackText;
	public int itemCount;
	
	// Update is called once per frame
	void Update () 
	{
		stackText.rectTransform.position = gameObject.GetComponent<RectTransform>().position + new Vector3(5,-5);
		stackText.text = itemCount > 1 ? itemCount.ToString() : string.Empty;	
	}
}

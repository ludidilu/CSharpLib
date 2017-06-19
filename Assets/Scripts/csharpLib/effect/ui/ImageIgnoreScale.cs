using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ImageIgnoreScale : MonoBehaviour {

	private Material mat;

	private Canvas canvas;

	// Use this for initialization
	void Start () {
	
		mat = GetComponent<Image>().material;

		canvas = GetComponentInParent<Canvas>();
	}
	
	// Update is called once per frame
	void Update () {

		mat.SetVector("unScale",new Vector4(1 / transform.lossyScale.x * canvas.transform.localScale.x,1 / transform.lossyScale.y * canvas.transform.localScale.y,1 / transform.lossyScale.z * canvas.transform.localScale.z,1));	

		transform.position = canvas.transform.localPosition;
	}
}

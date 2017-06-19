using UnityEngine;
using System.Collections;

public class SpriteScale9Grid : MonoBehaviour {

	private Transform center;
	private Transform left;
	private Transform right;
	private Transform top;
	private Transform bottom;
	private Transform left_top;
	private Transform left_bottom;
	private Transform right_top;
	private Transform right_bottom;

	private SpriteRenderer centerRenderer;
	private SpriteRenderer leftRenderer;
	private SpriteRenderer rightRenderer;
	private SpriteRenderer topRenderer;
	private SpriteRenderer bottomRenderer;
	private SpriteRenderer left_topRenderer;
	private SpriteRenderer left_bottomRenderer;
	private SpriteRenderer right_topRenderer;
	private SpriteRenderer right_bottomRenderer;

	private float W;
	private float H;
	private float L;
	private float R;
	private float T;
	private float B;
	private float OW;
	private float OH;

	[SerializeField]
	private Sprite sp;

	void Awake(){

		if(sp != null){

			Init(sp);
		}
	}

	public void Init(Sprite _sp){
		
		W = _sp.rect.width;
		H = _sp.rect.height;
		L = _sp.border.x;
		R = _sp.border.z;
		T = _sp.border.w;
		B = _sp.border.y;

		if(center == null){

			//中心块
			GameObject center_go = new GameObject ("center");
			center = center_go.transform;
			centerRenderer = center_go.AddComponent<SpriteRenderer> ();
			center.SetParent (transform, false);

			//左边块
			GameObject left_go = new GameObject ("left");
			left = left_go.transform;
			leftRenderer = left_go.AddComponent<SpriteRenderer> ();
			left.SetParent (transform, false);

			//右边块
			GameObject right_go = new GameObject("right");
			right = right_go.transform;
			rightRenderer = right_go.AddComponent<SpriteRenderer> ();
			right.SetParent (transform, false);

			//上边块
			GameObject top_go = new GameObject("top");
			top = top_go.transform;
			topRenderer = top_go.AddComponent<SpriteRenderer> ();
			top.SetParent (transform, false);

			//下边块
			GameObject bottom_go = new GameObject("bottom");
			bottom = bottom_go.transform;
			bottomRenderer = bottom_go.AddComponent<SpriteRenderer> ();
			bottom.SetParent (transform, false);

			//左上块
			GameObject left_top_go = new GameObject("left_top");
			left_top = left_top_go.transform;
			left_topRenderer = left_top_go.AddComponent<SpriteRenderer> ();
			left_top.SetParent (transform, false);

			//左下块
			GameObject left_bottom_go = new GameObject("left_bottom");
			left_bottom = left_bottom_go.transform;
			left_bottomRenderer = left_bottom_go.AddComponent<SpriteRenderer> ();
			left_bottom.SetParent (transform, false);

			//右上块
			GameObject right_top_go = new GameObject("right_top");
			right_top = right_top_go.transform;
			right_topRenderer = right_top_go.AddComponent<SpriteRenderer> ();
			right_top.SetParent (transform, false);

			//右下块
			GameObject right_bottom_go = new GameObject("right_bottom");
			right_bottom = right_bottom_go.transform;
			right_bottomRenderer = right_bottom_go.AddComponent<SpriteRenderer> ();
			right_bottom.SetParent (transform, false);
		}

		Refresh(_sp);
	}

	private void Refresh(Sprite _sp){
//		if (!_sp.packed) {
//			OW = 0;
//			OH = 0;
//			W = _sp.rect.width;
//			H = _sp.rect.height;
//			L = _sp.border.x;
//			R = _sp.border.z;
//			T = _sp.border.w;
//			B = _sp.border.y;
//		} else {
			OW = _sp.textureRect.x;
			OH = _sp.textureRect.y;
			W = _sp.textureRect.width;
			H = _sp.textureRect.height;
			L = _sp.border.x - _sp.textureRectOffset.x;
			R = _sp.border.z - (_sp.rect.width - _sp.textureRectOffset.x - _sp.textureRect.width);
			T = _sp.border.w - (_sp.rect.height - _sp.textureRectOffset.y - _sp.textureRect.height);
			B = _sp.border.y - _sp.textureRectOffset.y;
//		}

		center.localPosition = Vector3.zero;
		left.localPosition = new Vector3 (-(L / 200 + (W - L - R) / 200), 0);
		right.localPosition = new Vector3 ((W - L - R) / 200 + R / 200, 0);
		top.localPosition = new Vector3 (0, (H - T - B) / 200 + T / 200);
		bottom.localPosition = new Vector3 (0, -((H - T - B) / 200 + B / 200));
		left_top.localPosition = new Vector3 (-(L / 200 + (W - L - R) / 200), (H - T - B) / 200 + T / 200);
		left_bottom.localPosition = new Vector3 (-(L / 200 + (W - L - R) / 200), -((H - T - B) / 200 + B / 200));
		right_top.localPosition = new Vector3 ((W - L - R) / 200 + R / 200, (H - T - B) / 200 + T / 200);
		right_bottom.localPosition = new Vector3 ((W - L - R) / 200 + R / 200, -((H - T - B) / 200 + B / 200));

		centerRenderer.sprite = Sprite.Create (_sp.texture, new Rect (L + OW, B + OH, W - L - R, H - T - B), new Vector2 (0.5f, 0.5f));
		leftRenderer.sprite = Sprite.Create (_sp.texture, new Rect (OW, B + OH, L, H - T - B), new Vector2 (0.5f, 0.5f));
		rightRenderer.sprite = Sprite.Create (_sp.texture, new Rect (W - R + OW, B + OH, R, H - T - B), new Vector2 (0.5f, 0.5f));
		topRenderer.sprite = Sprite.Create (_sp.texture, new Rect (L + OW, H - T + OH, W - L - R, T), new Vector2 (0.5f, 0.5f));
		bottomRenderer.sprite = Sprite.Create (_sp.texture, new Rect (L + OW, OH, W - L - R, B), new Vector2 (0.5f, 0.5f));
		left_topRenderer.sprite = Sprite.Create (_sp.texture, new Rect (OW, H - T + OH, L, T), new Vector2 (0.5f, 0.5f));
		left_bottomRenderer.sprite = Sprite.Create (_sp.texture, new Rect (OW, OH, L, B), new Vector2 (0.5f, 0.5f));
		right_topRenderer.sprite = Sprite.Create (_sp.texture, new Rect (W - R + OW, H - T + OH, R, T), new Vector2 (0.5f, 0.5f));
		right_bottomRenderer.sprite = Sprite.Create (_sp.texture, new Rect (W - R + OW, OH, R, B), new Vector2 (0.5f, 0.5f));
	}

	public void ChangeSizeForLeft(float length){
		float scale = length / (W - L - R);	
		center.localScale += new Vector3 (scale, 0, 0);
		top.localScale += new Vector3 (scale, 0, 0);
		bottom.localScale += new Vector3 (scale, 0, 0);
		center.localPosition -= new Vector3 (length / 200, 0, 0);
		top.localPosition -= new Vector3 (length / 200, 0, 0);
		bottom.localPosition -= new Vector3 (length / 200, 0, 0);
		left_top.localPosition -= new Vector3 (length / 100, 0, 0);
		left.localPosition -= new Vector3 (length / 100, 0, 0);
		left_bottom.localPosition -= new Vector3 (length / 100, 0, 0);
	}

	public void ChangeSizeForRight(float length){
		float scale = length / (W - L - R);	
		center.localScale += new Vector3 (scale, 0, 0);
		top.localScale += new Vector3 (scale, 0, 0);
		bottom.localScale += new Vector3 (scale, 0, 0);
		center.localPosition += new Vector3 (length / 200, 0, 0);
		top.localPosition += new Vector3 (length / 200, 0, 0);
		bottom.localPosition += new Vector3 (length / 200, 0, 0);
		right_top.localPosition += new Vector3 (length / 100, 0, 0);
		right.localPosition += new Vector3 (length / 100, 0, 0);
		right_bottom.localPosition += new Vector3 (length / 100, 0, 0);
	}

	public void ChangeSizeForTop(float length){
		float scale = length / (H - T - B);	
		center.localScale += new Vector3 (0, scale, 0);
		left.localScale += new Vector3 (0, scale, 0);
		right.localScale += new Vector3 (0, scale, 0);
		center.localPosition += new Vector3 (0, length / 200, 0);
		left.localPosition += new Vector3 (0, length / 200, 0);
		right.localPosition += new Vector3 (0, length / 200, 0);
		top.localPosition += new Vector3 (0, length / 100, 0);
		left_top.localPosition += new Vector3 (0, length / 100, 0);
		right_top.localPosition += new Vector3 (0, length / 100, 0);
	
	}

	public void ChangeSizeForBottom(float length){
		float scale = length / (H - T - B);	
		center.localScale += new Vector3 (0, scale, 0);
		left.localScale += new Vector3 (0, scale, 0);
		right.localScale += new Vector3 (0, scale, 0);
		center.localPosition -= new Vector3 (0, length / 200, 0);
		left.localPosition -= new Vector3 (0, length / 200, 0);
		right.localPosition -= new Vector3 (0, length / 200, 0);
		bottom.localPosition -= new Vector3 (0, length / 100, 0);
		left_bottom.localPosition -= new Vector3 (0, length / 100, 0);
		right_bottom.localPosition -= new Vector3 (0, length / 100, 0);
	}

	public void ChangeSizeForBoth(Vector2 size){
		float width = size.x;
		float height = size.y;
		ChangeSizeForBoth (width, height);
	}

	public void ChangeSizeForBoth(float width,float height){
		float scaleX = width / (W - L - R);
		center.localScale += new Vector3 (scaleX, 0, 0);
		top.localScale += new Vector3 (scaleX, 0, 0);
		bottom.localScale += new Vector3 (scaleX, 0, 0);
		left.localPosition -= new Vector3 (width / 200, 0, 0);
		left_top.localPosition -= new Vector3 (width / 200, 0, 0);
		left_bottom.localPosition -= new Vector3 (width / 200, 0, 0);
		right.localPosition += new Vector3 (width / 200, 0, 0);
		right_top.localPosition += new Vector3 (width / 200, 0, 0);
		right_bottom.localPosition += new Vector3 (width / 200, 0, 0);
		float scaleY = height / (H - T - B);
		center.localScale += new Vector3 (0, scaleY, 0);
		left.localScale += new Vector3 (0, scaleY, 0);
		right.localScale += new Vector3 (0, scaleY, 0);
		top.localPosition += new Vector3 (0, height / 200, 0);
		left_top.localPosition += new Vector3 (0, height / 200, 0);
		right_top.localPosition += new Vector3 (0, height / 200, 0);
		bottom.localPosition -= new Vector3 (0, height / 200, 0);
		left_bottom.localPosition -= new Vector3 (0, height / 200, 0);
		right_bottom.localPosition -= new Vector3 (0, height / 200, 0);
	}

}

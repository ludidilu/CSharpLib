using UnityEngine;
using System.Collections;
using superFunction;
using publicTools;

public class ClickMove : MonoBehaviour {

	public enum Direction{
		
		UP,
		DOWN,
		RIGHT,
		LEFT
	}

	public const string MOVE = "clickMove";

	public const string UP = "clickUp";

	[SerializeField]
	private Canvas canvas;
	
	[SerializeField]
	private RectTransform rect;

	private Rect clickArea;

	private bool isDown = false;

	private float topRight;

	private float topLeft;

	private float bottomRight;

	private float bottomLeft;

	void Awake(){

	}

	void Start(){
		
		if(rect != null){
			
			clickArea = new Rect(rect.rect.x + rect.anchoredPosition.x,rect.rect.y + rect.anchoredPosition.y,rect.rect.width,rect.rect.height);
		}
	}

	void Update(){
		
		if(isDown){
			
			if(!Input.GetMouseButton(0)){

				isDown = false;

				SuperFunction.Instance.DispatchEvent(gameObject,UP);
				
				return;
			}

			Vector3 v = PublicTools.MousePositionToCanvasPosition(canvas,Input.mousePosition);
			
			Vector3 topRight = Vector3.Cross(new Vector3(clickArea.xMax,clickArea.yMax,0),v);

			Vector3 topLeft = Vector3.Cross(new Vector3(clickArea.xMin,clickArea.yMax,0),v);

			Vector3 bottomRight = Vector3.Cross(new Vector3(clickArea.xMax,clickArea.yMin,0),v);

			Vector3 bottomLeft = Vector3.Cross(new Vector3(clickArea.xMin,clickArea.yMin,0),v);

			Direction direction;

			if(topLeft.z > 0 && topRight.z < 0){

				direction = Direction.DOWN;

			}else if(topRight.z > 0 && bottomRight.z < 0){

				direction = Direction.LEFT;

			}else if(bottomRight.z > 0 && bottomLeft.z < 0){

				direction = Direction.UP;

			}else{

				direction = Direction.RIGHT;
			}

			SuperFunction.Instance.DispatchEvent(gameObject,MOVE,direction);
			
		}else if(Input.GetMouseButtonDown(0)){
			
			if(rect != null){
				
				Vector3 v = PublicTools.MousePositionToCanvasPosition(canvas,Input.mousePosition);
				
				if(clickArea.Contains(v)){
					
					isDown = true;
				}
				
			}else{
				
				isDown = true;
			}
		}
	}

	void OnDisable(){
		
		isDown = false;
	}
}

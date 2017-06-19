using UnityEngine;
using System.Collections;
using publicTools;
using superFunction;

public class Joystick4Direction : MonoBehaviour {

	[SerializeField]
	private Canvas canvas;

	[SerializeField]
	private RectTransform rect;

	private Rect clickArea;

	private Vector2 downPos;

	private bool isDown = false;

	private float maxValue;

	void Awake(){

	}

	void Start(){

		maxValue = JoystickData.moveMaxValue * canvas.scaleFactor;

		clickArea = new Rect(rect.rect.x + rect.anchoredPosition.x,rect.rect.y + rect.anchoredPosition.y,rect.rect.width,rect.rect.height);
	}

	private void Down(){

		isDown = true;

		downPos = Input.mousePosition;

		SuperFunction.Instance.DispatchEvent(gameObject,JoystickData.DOWN,this,downPos);
	}

	private void Up(){

		isDown = false;

		SuperFunction.Instance.DispatchEvent(gameObject,JoystickData.UP,this);
	}

	void Update(){

		if(isDown){

			if(!Input.GetMouseButton(0)){
				
				Up ();

				return;
			}

			float dx = Input.mousePosition.x - downPos.x;

			float dy = Input.mousePosition.y - downPos.y;

			JoystickData.Direction direction;

			float moveDis;

			if(Mathf.Abs(dx) > Mathf.Abs(dy)){

				dx = Mathf.Clamp(dx,-maxValue,maxValue);

				if(dx > 0){

					direction = JoystickData.Direction.RIGHT;

				}else{

					direction = JoystickData.Direction.LEFT;
				}

				moveDis = dx;

			}else{

				dy = Mathf.Clamp(dy,-maxValue,maxValue);

				if(dy > 0){
					
					direction = JoystickData.Direction.UP;
					
				}else{
					
					direction = JoystickData.Direction.DOWN;
				}

				moveDis = dy;
			}

			SuperFunction.Instance.DispatchEvent(gameObject,JoystickData.MOVE,this,direction,moveDis,downPos);

		}else if(Input.GetMouseButtonDown(0)){
				
			Vector3 v = PublicTools.MousePositionToCanvasPosition(canvas,Input.mousePosition);

			if(clickArea.Contains(v)){
				
				Down();
			}
		}
	}
	
	void OnDisable(){
		
		isDown = false;
	}
}

using UnityEngine;
using System.Collections;
using publicTools;
using superFunction;

public class JoystickFree3 : MonoBehaviour {
	
	[SerializeField]
	private Canvas canvas;
	
	[SerializeField]
	private RectTransform rect;

	[SerializeField]
	private bool canJoystickMove;
	
	private Rect clickArea;
	
	private Vector2 downPos;
	
	private bool isDown = false;
	
	public float maxValue{private set;get;}
	
	void Awake(){
		
		
	}
	
	void Start(){
		
		maxValue = JoystickData.moveMaxValue * canvas.scaleFactor;
		
		Vector3 v1 = PublicTools.MousePositionToCanvasPosition(canvas,Vector3.zero);
		
		Vector3 v2 = PublicTools.MousePositionToCanvasPosition(canvas,new Vector3(1,0,0));
		
		//这个fix是表示鼠标每移动1在canvas中移动fix距离
		float fix = v2.x - v1.x;
		
		clickArea = new Rect(rect.rect.x + rect.anchoredPosition.x,rect.rect.y + rect.anchoredPosition.y,rect.rect.width,rect.rect.height);
		
		clickArea.width = clickArea.width - maxValue * 2 * fix;
		
		clickArea.height = clickArea.height - maxValue * 2 * fix;
		
		clickArea.center = clickArea.center + new Vector2(maxValue * fix,maxValue * fix);
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

			Vector2 dir = new Vector2(Input.mousePosition.x - downPos.x,Input.mousePosition.y - downPos.y);
			
			float dis = Vector2.Distance(downPos,Input.mousePosition);
			
			if(dis > maxValue){

				if(canJoystickMove){
				
					downPos = Vector2.Lerp(downPos,Input.mousePosition,(dis - maxValue) / dis);
					
					FixDownPos();
				}

				dis = maxValue;
			}

			dir = dir.normalized * dis / maxValue;

			SuperFunction.Instance.DispatchEvent(gameObject,JoystickData.MOVE,this,dir,downPos);
			
		}else if(Input.GetMouseButtonDown(0)){
			
			if(rect != null){
				
				Vector3 v = PublicTools.MousePositionToCanvasPosition(canvas,Input.mousePosition);
				
				if(clickArea.Contains(v)){
					
					Down();
				}
				
			}else{
				
				Down ();
			}
		}
	}
	
	private void FixDownPos(){
		
		Vector2 v = PublicTools.MousePositionToCanvasPosition(canvas,downPos);
		
		if(!clickArea.Contains(v)){
			
			if(v.x < clickArea.xMin){
				
				v = new Vector3(clickArea.xMin,v.y);
				
			}else if(v.x > clickArea.xMax){
				
				v = new Vector2(clickArea.xMax,v.y);
			}
			
			if(v.y < clickArea.yMin){
				
				v = new Vector3(v.x,clickArea.yMin);
				
			}else if(v.y > clickArea.yMax){
				
				v = new Vector2(v.x,clickArea.yMax);
			}
			
			downPos = PublicTools.CanvasPostionToMousePosition(canvas,v);
		}
	}
	
	void OnDisable(){
		
		isDown = false;
	}
}

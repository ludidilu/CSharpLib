using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using superFunction;

namespace superList{
	
	public class SuperScrollRect : ScrollRect,IPointerExitHandler {

		public static GameObject eventDispatcher;

		public const string CLOSE_MOVE = "SuperListCloseMove";
		
		public const string OPEN_MOVE = "SuperListOpenMove";

		public static bool canDrag{

			get{

				return SuperScrollRectScript.Instance.canDrag > 0;
			}

			set{

				SuperScrollRectScript.Instance.canDrag = SuperScrollRectScript.Instance.canDrag + (value ? 1 : -1);
			}
		}

		public bool isRestrain = false;

		private bool isRestrainDrag;

		private bool isOneTouchDrag;
		
		public override void OnBeginDrag(PointerEventData eventData) {

			if(!canDrag || Input.touchCount > 1){

				return;
			}

			if(isRestrain){

				isRestrainDrag = true;
			}

			isOneTouchDrag = true;

			base.OnBeginDrag(eventData);
		}
		
		public override void OnDrag(PointerEventData eventData) {

			if(!canDrag){
				
				return;
			}
			
			if(Input.touchCount > 1){
				
				isOneTouchDrag = false;
				
				return;
			}

			if(isOneTouchDrag && (!isRestrain || isRestrainDrag)){

				base.OnDrag(eventData);
			}
		}
		
		public void OnPointerExit (PointerEventData eventData)
		{
			if(isRestrain){

				base.OnEndDrag(eventData);
				
				isRestrainDrag = false;
			}
		}

        public void DirectContentAnchoredPosition(Vector2 anchoredPosition)
        {
            SetContentAnchoredPosition(anchoredPosition);
        }

		protected override void Awake ()
		{
			base.Awake();

			if (movementType == ScrollRect.MovementType.Elastic) {

				if (eventDispatcher == null) {
					
					eventDispatcher = new GameObject ("SuperListEventDispatcher");
					
					GameObject.DontDestroyOnLoad (eventDispatcher);
				}

				SuperFunction.Instance.AddEventListener (eventDispatcher, OPEN_MOVE, OpenMove);
				
				SuperFunction.Instance.AddEventListener (eventDispatcher, CLOSE_MOVE, CloseMove);
			}
		}

		protected override void OnDestroy (){
				
			SuperFunction.Instance.RemoveEventListener(eventDispatcher,OPEN_MOVE,OpenMove);
			
			SuperFunction.Instance.RemoveEventListener(eventDispatcher,CLOSE_MOVE,CloseMove);
		}

		public void CloseMove(int _index){
			
			movementType = ScrollRect.MovementType.Clamped;
		}
		
		public void OpenMove(int _index){
			
			movementType = ScrollRect.MovementType.Elastic;
		}
	}
}
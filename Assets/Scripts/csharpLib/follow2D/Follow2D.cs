using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Follow2D{

	class Follow2DNode{

		public Vector2 pos;
		public int dir;
	}

	private int num;

	private LinkedList<Follow2DNode> pool = new LinkedList<Follow2DNode>();

	private LinkedList<Follow2DNode> path = new LinkedList<Follow2DNode>();

	private Action<int,Vector2,int> changeCallBack;

	private Vector2 headPos;

	private float gap;

	public void Init(Vector2 _pos,int _num,float _gap,Action<int,Vector2,int> _changeCallBack){

		headPos = _pos;

		gap = _gap;

		changeCallBack = _changeCallBack;

		SetNum(_num);
	}

	public void SetNum(int _num){

		num = _num;

		if(num == 0){

			ResetPos();
		}
	}

	public void ResetPos(){

		while(path.First != null){

			LinkedListNode<Follow2DNode> node = path.First;

			path.RemoveFirst();

			pool.AddLast(node);
		}
	}
	
	public void AddPathPoint(int _data){

		if(num > 0){

			LinkedListNode<Follow2DNode> node;

			if(pool.Count > 0){

				node = pool.Last;

				pool.RemoveLast();
			}
			else{

				node = new LinkedListNode<Follow2DNode>(new Follow2DNode());
			}

			node.Value.pos = headPos;

			node.Value.dir = _data;
			
			path.AddLast(node);
		}
	}
	
	public void SetPos(Vector2 _pos){

		headPos = _pos;

		Vector2 next = _pos;
		
		float tmpDis = 0;

		LinkedListNode<Follow2DNode> lastNode = path.Last;

		for(int i = 0 ; i < num; i++){
			
			float dis = (i + 1) * gap;
			
			bool posChange = false;

			LinkedListNode<Follow2DNode> node = lastNode;

			while(node != null){

				Follow2DNode pair = node.Value;

				Vector2 p = pair.pos;

				float dis2 = Vector2.Distance(next,p);

				float tmpDis2 = tmpDis + dis2;

				if(tmpDis2 > dis){

					lastNode = node;

					Vector2 result = Vector2.Lerp(p,next,(tmpDis2 - dis) / dis2);

					changeCallBack(i,result,pair.dir);

					posChange = true;

					break;
				}

				tmpDis = tmpDis2;

				next = p;

				node = node.Previous;
			}

			if(!posChange){
				
				return;
				
			}else{
				
				if(i == num - 1){

					lastNode = lastNode.Previous;

					while(lastNode != null){

						LinkedListNode<Follow2DNode> tmpNode = lastNode.Previous;

						path.Remove(lastNode);

						pool.AddLast(lastNode);

						lastNode = tmpNode;
					}
				}
			}
		}
	}
}

using UnityEngine;
using System.Collections.Generic;
using assetManager;
using System;

namespace gameObjectFactory
{
    public class GameObjectFactoryUnit
	{
		private string name;

		private GameObject data;

		private int type = -1;

		public int useNum{private set;get;}

		private List<Action<GameObject>> callBackList = new List<Action<GameObject>>();

		private List<Action> callBackList2 = new List<Action>();

		public GameObjectFactoryUnit (string _name)
		{
			name = _name;
		}

        public void GetGameObject (Action _callBack){
			
			if (type == -1) {
				
				type = 0;
				
				callBackList2.Add (_callBack);
				
				AssetManager.Instance.GetAsset<GameObject> (name, GetResouece);
				
			} else if (type == 0) {
				
				callBackList2.Add (_callBack);
				
			} else {
				
				_callBack ();
			}
		}

		public GameObject GetGameObject (Action<GameObject> _callBack){

			if (type == -1) {

				type = 0;

				callBackList.Add (_callBack);

				AssetManager.Instance.GetAsset<GameObject> (name, GetResouece);

				return null;

			} else if (type == 0) {

				callBackList.Add (_callBack);

				return null;

			} else {

				GameObject result = GameObject.Instantiate<GameObject> (data);

				if (_callBack != null) {

					_callBack (result);
				}

				return result;
			}
		}

		private void GetResouece (GameObject _go)
		{
			data = _go;

			type = 1;

            for (int i = 0; i < callBackList.Count; i++)
            {
                Action<GameObject> callBack = callBackList[i];

                if (callBack != null)
                {
                    if (_go != null)
                    {
                        GameObject result = GameObject.Instantiate(data);

                        callBack(result);
                    }
                    else {

                        callBack(null);
                    }
                }
            }

			callBackList.Clear ();

            for (int i = 0; i < callBackList2.Count; i++)
            {
                Action callBack = callBackList2[i];

                if (callBack != null)
                {
                    callBack();
                }
            }

			callBackList2.Clear ();
		}

		public void AddUseNum ()
		{
			useNum++;
		}

		public void DelUseNum ()
		{
			useNum--;
		}

		public void Dispose ()
		{
			if (data != null) {

				data = null;
			}
		}
	}
}
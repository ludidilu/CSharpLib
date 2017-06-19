using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace qtools.qhierarchy
{
	public class QObjectList: MonoBehaviour
	{
		public void merge(QObjectList anotherInstance)
		{ 
			lockedObjects.AddRange(anotherInstance.lockedObjects);
			editModeVisibileObjects.AddRange(anotherInstance.editModeVisibileObjects);
			editModeInvisibleObjects.AddRange(anotherInstance.editModeInvisibleObjects);
        } 
        
        public List<GameObject> lockedObjects = new List<GameObject>();
		public List<GameObject> editModeVisibileObjects = new List<GameObject>();
		public List<GameObject> editModeInvisibleObjects = new List<GameObject>();

		public static bool inited = false;
	}
}
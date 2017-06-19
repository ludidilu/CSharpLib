using UnityEngine;
using System.Collections;
using UnityEditor;

namespace qtools
{
	public class QAlphabetNaturalSort: BaseHierarchySort 
	{
		public override int Compare(GameObject lhs, GameObject rhs)
		{ 
			if (lhs == rhs) return 0;
			if (lhs == null) return -1;
			if (rhs == null) return 1;
			int result = EditorUtility.NaturalCompare(lhs.name, rhs.name);
			return result;
		}
		
		public override GUIContent content
		{
			get
			{ 
				return new GUIContent("Abc");
			}
		}
	}
}
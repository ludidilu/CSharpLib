using UnityEngine;

namespace superFunction
{
    public class SuperFunctionControl : MonoBehaviour
    {
        public bool isDestroy = false;

        void OnDestroy()
        {
            if (!isDestroy)
            {
                SuperFunction.Instance.DestroyGameObject(gameObject);
            }
        }
    }
}
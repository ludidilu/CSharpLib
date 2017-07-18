using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace shader
{
    public class UIRenderQueue : MonoBehaviour
    {
        private static Dictionary<string, Material> dic = new Dictionary<string, Material>();

        [SerializeField]
        private int renderQueue;

        [SerializeField]
        private bool isIndependent;

        // Use this for initialization
        void Awake()
        {
            MaskableGraphic graphic = GetComponent<MaskableGraphic>();

            if (isIndependent)
            {
                Material material = Instantiate(graphic.material);

                material.renderQueue = renderQueue;

                graphic.material = material;
            }
            else
            {
                string materialName = graphic.material.name + "_" + renderQueue.ToString();

                Material material;

                if (dic.TryGetValue(materialName, out material))
                {
                    graphic.material = material;
                }
                else
                {
                    material = Instantiate(graphic.material);

                    material.name = materialName;

                    material.renderQueue = renderQueue;

                    dic.Add(materialName, material);

                    graphic.material = material;
                }
            }

            Destroy(this);
        }
    }
}
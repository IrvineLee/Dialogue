using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dico.Helper
{
	public class ShaderHelper : MonoBehaviour
	{
		public static MaterialPropertyBlock GetPropertyBlock_InitializeColor(Renderer rend, Color color, string propertyName = "_Color")
        {
			MaterialPropertyBlock propBlock = new MaterialPropertyBlock();

			rend.GetPropertyBlock(propBlock);
			propBlock.SetColor(propertyName, color);
			rend.SetPropertyBlock(propBlock);

			return propBlock;
		}
    }
}

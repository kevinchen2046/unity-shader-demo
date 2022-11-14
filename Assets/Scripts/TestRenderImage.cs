using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class TestRenderImage : MonoBehaviour
{

	#region Variables
	public Shader curShader;
	public float grayScaleAmout = 1.0f;
	private Material curMaterial;

	Material material
	{
		get
		{
			if (curMaterial == null)
			{
				curMaterial = new Material(curShader);
				curMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return curMaterial;
		}
	}
	#endregion
	// Use this for initialization
	void Start()
	{
		if (!SystemInfo.supportsImageEffects)
		{
			enabled = false;
		}

		if (!curShader && !curShader.isSupported)
		{
			enabled = false;
		}
		Camera.main.depthTextureMode = DepthTextureMode.Depth;
	}

	// Update is called once per frame
	void Update()
	{
		grayScaleAmout = Mathf.Clamp(grayScaleAmout, 0.0f, 1.0f);
	}

	void OnRenderImage(RenderTexture source, RenderTexture target)
	{
		if (curShader != null)
		{
			material.SetFloat("_LuminosityAmount", grayScaleAmout);
			Graphics.Blit(source, target, material);
			//Debug.Log("OnRenderImage: " + grayScaleAmout);
		}
		else
		{
			Graphics.Blit(source, target);
		}
	}

	void OnDisable()
	{
		if (curMaterial)
		{
			DestroyImmediate(curMaterial);
		}
	}
}
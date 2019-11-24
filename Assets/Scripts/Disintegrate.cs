using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disintegrate : MonoBehaviour
{
	public float DisintegrationPeriod = 10f;
	public Texture2D Pattern;

    void OnEnable()
    {
		StartCoroutine(DisintegrateCoroutine());
    }

	public IEnumerator DisintegrateCoroutine()
	{
		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		foreach (var renderer in renderers)
		{
			Material material = new Material(Shader.Find("Custom/Disintegrate2"));
			material.SetTexture("_MainTex", renderer.material.mainTexture);
			material.SetTexture("_PatternTex", Pattern);
			renderer.material = material;
		}


		float t = 0f;
		while (t < DisintegrationPeriod)
		{
			foreach (var renderer in renderers)
			{
				renderer.material.SetFloat("_Threshold", t / DisintegrationPeriod);
			}
			t += Time.deltaTime;
			yield return null;
		}

		foreach (var renderer in renderers)
		{
			Destroy(renderer.material);
		}
		Destroy(gameObject);
	}
}

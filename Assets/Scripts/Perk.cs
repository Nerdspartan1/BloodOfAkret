using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Perk",menuName = "Perk")]
public class Perk : ScriptableObject
{
	public string Name;
	public int Price;
	public Sprite Icon;
	[TextArea(4,6)]
	public string Description;
}

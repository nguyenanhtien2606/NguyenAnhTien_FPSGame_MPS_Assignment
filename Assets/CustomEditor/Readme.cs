using System;
using UnityEngine;

public class Readme : ScriptableObject
{
	public Texture2D icon;
	public string title;
	public Section[] sections;
	public bool loadedLayout;

	[Serializable]
	public class Section
	{
		public string heading, linkText, url;
		[TextArea(10,15)] public string text;
	}
}

using UnityEngine;

[ExecuteInEditMode()] 
public class ClockHand : MonoBehaviour {
	
	public static float angle = 0;

	public Texture2D texture = null;
	public float currentAngel = 0;

	public Vector2 size = new Vector2(128, 128);
	public Vector2 relativePos = new Vector2(0, 0);

	Vector2 pos;
	Rect rect;
	Vector2 pivot;

	void Start() {
		UpdateSettings();
	}
	
	void UpdateSettings() {
		Vector2 cornerPos = new Vector2(Screen.width, 0);

		pos = cornerPos + relativePos;
		rect = new Rect(pos.x - size.x * 0.5f, pos.y - size.y * 0.5f, size.x, size.y);
		pivot = new Vector2(rect.xMin + rect.width * 0.5f, rect.yMin + rect.height);
	}
	
	void OnGUI() {
		if (Application.isEditor) { UpdateSettings();}
		Matrix4x4 matrixBackup = GUI.matrix;
		GUIUtility.RotateAroundPivot(angle, pivot);
		GUI.DrawTexture(rect, texture);
		GUI.matrix = matrixBackup;
	}
}
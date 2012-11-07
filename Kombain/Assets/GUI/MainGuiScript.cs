using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class MainGuiScript : MonoBehaviour {

	public enum LayoutType{
		leftBottom,
		rightBottom,
		leftTop,
		rightTop,
		leftMiddle,
		rightMiddle,
		topMiddle,
		bottomMiddle,
		middle,
		none
	};

	public enum ScaleType {
		onlyWidth,
		onlyHeight,
		width,
		height,
		bouth,
		defaultScale
	};
  
	public GUISkin defaultGuiSkin;
	public GUIStyle guiStyle;
	
	public int xOffset = 0;
	public int yOffset = 0;
	
	public int width = 100;
	public int height = 100;
	
	public const float SOURCE_WIDTH = 1024.0f;
	public const float SOURCE_HEIGHT = 768.0f;
	
	protected float rootLayoutWidth = 0;
	protected float rootLayoutHeight = 0;
	
	private LayoutType currentLayoutType = LayoutType.none;
	private ScaleType currentScaleType = ScaleType.defaultScale;
	
	static private List<Rect> rectList = new List<Rect> ();
	
	public void setLayoutSize(float width, float height){
		rootLayoutWidth = width;
		rootLayoutHeight = height;		
	}
	
	protected Rect getBox (bool passEvent = true){
		Rect rect = scaledRect (xOffset, yOffset, width, height, passEvent);
		return rect;	
	}
	
	protected void beginLayout (LayoutType type, int depth){
		beginLayout (type, ScaleType.defaultScale, depth);    
	}

	protected void beginLayout (LayoutType type){
		beginLayout (type, ScaleType.defaultScale, 10);
	}
	
	private float getRootLayoutWidth(){
		if(rootLayoutWidth == 0){
			return Screen.width;
		}
		
		return rootLayoutWidth;
	}
	
	private float getRootLayoutHeight(){
		if(rootLayoutHeight == 0){
			return Screen.height;
		}
		
		return rootLayoutHeight;
	}
	
	protected void beginLayout (LayoutType type, ScaleType scaleType, int depth) {
		GUI.depth = depth;
		GUI.skin = defaultGuiSkin;
		currentLayoutType = type;
		currentScaleType = scaleType;
	}

	private Rect scaledRect (float xOffset, float yOffset, float width, float height, bool passEvent = true) {
		return scaledRect (getRect (xOffset, yOffset, width, height, currentScaleType, currentLayoutType), passEvent); 
	}

	private Rect scaledRect (Rect rect, bool passEvent = true){
		if (!passEvent) {
			blockEvent (rect);
		}
		return rect;
	}

	private Rect getRect (float xOffset, float yOffset, float width, float height, LayoutType layoutType){
		return getRect (xOffset, yOffset, width, height, ScaleType.defaultScale, layoutType);
	}

	private Rect getRect (float xOffset, float yOffset, float width, float height, ScaleType scaleType, LayoutType layoutType){
    
		Vector2 scale = getScale (scaleType);
		Vector2 offset = getOffset (width, height, scale, layoutType);

		return new Rect (offset.x + xOffset * scale.x, offset.y + yOffset * scale.y, width * scale.x, height * scale.y);
	}

	private Vector2 getScale (){
		return getScale (ScaleType.defaultScale);
	}

	private Vector2 getScale (ScaleType scaleType){

		Vector2 scale = new Vector2 (); 

		switch (scaleType) {
		case ScaleType.onlyWidth:
			scale.x = getRootLayoutWidth() / SOURCE_WIDTH;
			scale.y = 1f;
			break;

		case ScaleType.onlyHeight:
			scale.x = getRootLayoutHeight() / SOURCE_HEIGHT;
			scale.y = 1f;
			break;
		case ScaleType.width:
			scale.x = scale.y = getRootLayoutWidth() / SOURCE_WIDTH;
			break;

		case ScaleType.height:
			scale.y = scale.x = getRootLayoutHeight() / SOURCE_HEIGHT;
			break;
		case ScaleType.bouth:
			scale.x = getRootLayoutWidth() / SOURCE_WIDTH;
			scale.y = getRootLayoutHeight() / SOURCE_HEIGHT;
			break;
		default:
			if ((getRootLayoutWidth() / getRootLayoutHeight()) < (SOURCE_WIDTH / SOURCE_HEIGHT)) { //screen is taller
				scale.y = scale.x = getRootLayoutWidth() / SOURCE_WIDTH;
			} else { // screen is wider
				scale.y = scale.x = getRootLayoutHeight() / SOURCE_HEIGHT;
			}
			break;
		}

		return scale;
	}

	private void setScale (Vector2 scale){
		GUI.matrix = Matrix4x4.TRS (Vector2.zero, Quaternion.identity, new Vector3 (scale.x, scale.y, 1));  
	}

	private float getMaxHeight (){
		return SOURCE_HEIGHT / getScale (ScaleType.defaultScale).y;
	}

	private float getMaxWidth (){
		return SOURCE_WIDTH / getScale (ScaleType.defaultScale).x;
	}

	private void blockEvent (Rect rect){
		if (!rectList.Contains (rect)) {
			rectList.Add (rect);
		}
	}

	private Vector2 getOffset (float width, float height, LayoutType currentLayoutType){
		return getOffset (width, height, getScale (), currentLayoutType);  
	}

	private Vector2 getOffset (float width, float height, Vector2 scale, LayoutType currentLayoutType){
    
		Vector3 offset = Vector3.zero;

		switch (currentLayoutType) {
		case LayoutType.rightBottom:
			offset.x = getRootLayoutWidth() - width * scale.x;
			offset.y = getRootLayoutHeight() - height * scale.y;
			break;
		case LayoutType.leftBottom:
			offset.x = 0;
			offset.y = getRootLayoutHeight() - height * scale.y;
			break;
		case LayoutType.rightTop:
			offset.x = getRootLayoutWidth() - width * scale.x;
			offset.y = 0;
			break;
		case LayoutType.leftTop:
			offset.x = 0;
			offset.y = 0;
			break;
		case LayoutType.rightMiddle:
			offset.x = getRootLayoutWidth() - width * scale.x;
			offset.y = getRootLayoutHeight() / 2 - height * scale.y / 2;
			break;
		case LayoutType.leftMiddle:
			offset.x = 0;
			offset.y = getRootLayoutHeight() / 2 - height * scale.y / 2;
			break;
		case LayoutType.topMiddle:
			offset.x = getRootLayoutWidth() / 2 - width * scale.x / 2;
			offset.y = 0;
			break;
		case LayoutType.bottomMiddle:
			offset.x = getRootLayoutWidth() / 2 - width * scale.x / 2;
			offset.y = getRootLayoutHeight() - height * scale.y;
			break;
		case LayoutType.middle:
			offset.x = getRootLayoutWidth() / 2 - width * scale.x / 2;
			offset.y = getRootLayoutHeight() / 2 - height * scale.y / 2;
			break;
		}

		return offset;
	}
	
	void LateUpdate (){
		rectList.Clear ();
	}
	
	/**
   * Is coprdinate in gui rect
   * @return bool
   * */
	public static bool touchInsideGUI (Vector3 coordinates){

		foreach (Rect rect in rectList) {

			float fMouseX = coordinates.x;
			float fMouseY = Screen.height - coordinates.y;

			if (rect.Contains (new Vector2 (fMouseX, fMouseY))) {
				return true;
			}
		}
		return false;
	}

}
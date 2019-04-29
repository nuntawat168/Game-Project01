using UnityEngine;
using UnityEditor;
using System.Collections;

namespace UnderWaterEffect {


[CustomEditor(typeof(UnderWater))] 
public class UnderWaterEditor : Editor {

public Texture2D m_logo;
SerializedObject   serObj;
SerializedProperty AssignSkybox;
SerializedProperty cubemap1;
SerializedProperty enableSkyFog;
SerializedProperty skyFogColor;
SerializedProperty skyFog;
SerializedProperty enableCaustics;
SerializedProperty CausticColor;
SerializedProperty CausticBrightness;
SerializedProperty CausticSize;
SerializedProperty CausticSpeedX;
SerializedProperty CausticSpeedY;
SerializedProperty Water;
SerializedProperty effectColor;
SerializedProperty effectValue;
SerializedProperty enableUnderwaterFog;
SerializedProperty underwaterFogColor;
SerializedProperty underwaterFog;
SerializedProperty WaterTextureSpeed;
SerializedProperty WaterBumpmapSpeed;
SerializedProperty DistortionMapSpeed;
SerializedProperty DistortionMap_Speed;
SerializedProperty TransparencyUnderWater;
SerializedProperty TransparencyOverWater;

GUIStyle style;
	
	void OnEnable()
	{
		m_logo	= (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Underwater Effect/Editor/Graphics/ue_logo.png", typeof(Texture2D));
      		
		serObj         			= new SerializedObject (target);	
		AssignSkybox 			= serObj.FindProperty("AssignSkybox");
		cubemap1 				= serObj.FindProperty("cubemap1");
		enableSkyFog		 	= serObj.FindProperty("enableSkyFog");
		skyFogColor			 	= serObj.FindProperty("skyFogColor");
		skyFog		 			= serObj.FindProperty("skyFog");
		enableCaustics 			= serObj.FindProperty("enableCaustics");
		CausticColor 			= serObj.FindProperty("CausticColor");
		CausticBrightness 		= serObj.FindProperty("CausticBrightness");
		CausticSize 			= serObj.FindProperty("CausticSize");
		CausticSpeedX	 		= serObj.FindProperty("CausticSpeedX");
		CausticSpeedY	 		= serObj.FindProperty("CausticSpeedY");
		Water 					= serObj.FindProperty("water");
		effectColor			 	= serObj.FindProperty("effectColor");
		effectValue			 	= serObj.FindProperty("effectValue");
		enableUnderwaterFog 	= serObj.FindProperty("enableUnderwaterFog");
		underwaterFogColor 		= serObj.FindProperty("underwaterFogColor");
		underwaterFog	 		= serObj.FindProperty("underwaterFog");
		WaterTextureSpeed	 	= serObj.FindProperty("WaterTextureSpeed");
		WaterBumpmapSpeed	 	= serObj.FindProperty("WaterBumpmapSpeed");
		DistortionMapSpeed	 	= serObj.FindProperty("DistortionMapSpeed");
		DistortionMap_Speed 	= serObj.FindProperty("DistortionMap_Speed");
		TransparencyUnderWater 	= serObj.FindProperty("TUWV");
		TransparencyOverWater 	= serObj.FindProperty("TOWV");
		
	}
	
	public override void OnInspectorGUI () {
        
		serObj.Update();
		
		if (m_logo != null)
        {
            Rect rect = GUILayoutUtility.GetRect(m_logo.width, m_logo.height);
            GUI.DrawTexture(rect, m_logo, ScaleMode.ScaleToFit);
        }
		
		GUILayout.BeginVertical("Box");
        GUILayout.Label("Sky Settings", EditorStyles.boldLabel);

		EditorGUILayout.PropertyField (AssignSkybox, new GUIContent("Skybox Material"));
		EditorGUILayout.PropertyField (cubemap1, new GUIContent("Water Cubemap"));
		EditorGUILayout.PropertyField (enableSkyFog, new GUIContent("Enable Sky Fog"));
		skyFogColor.colorValue = EditorGUILayout.ColorField ("Sky Fog Color", skyFogColor.colorValue);
		skyFog.floatValue = EditorGUILayout.Slider ("Sky Fog Density",  skyFog.floatValue,  0.0f, 0.2f);
					
		GUILayout.Space(5.0f);
        GUILayout.EndVertical();


		GUILayout.BeginVertical("Box");
        GUILayout.Label("Caustic Settings", EditorStyles.boldLabel);

		EditorGUILayout.PropertyField (enableCaustics, new GUIContent("Enable Caustics"));
		CausticColor.colorValue = EditorGUILayout.ColorField ("Caustic Color", CausticColor.colorValue);
		CausticBrightness.floatValue = EditorGUILayout.Slider ("Caustic Brightness",  CausticBrightness.floatValue,  0.1f, 1.5f);
		CausticSize.floatValue = EditorGUILayout.Slider ("Caustic Size",  CausticSize.floatValue,  0.001f, 0.01f);
		CausticSpeedX.floatValue = EditorGUILayout.Slider("Caustic Speed X", CausticSpeedX.floatValue, -6.0f, 6.0f);
		CausticSpeedY.floatValue = EditorGUILayout.Slider("Caustic Speed Y", CausticSpeedY.floatValue, -0.3f, 0.3f);
		
		GUILayout.Space(5.0f);
        GUILayout.EndVertical();
		
		
		GUILayout.BeginVertical("Box");
		GUILayout.Label("Water Settings", EditorStyles.boldLabel);
		
		EditorGUILayout.PropertyField (Water, new GUIContent("Select Water"));
		effectColor.colorValue = EditorGUILayout.ColorField ("Muddiness Color", effectColor.colorValue);
		effectValue.floatValue = EditorGUILayout.Slider ("Muddiness Intensity",  effectValue.floatValue,  0.0f, 0.5f);
		EditorGUILayout.PropertyField (enableUnderwaterFog, new GUIContent("Enable Underwater Fog"));
		underwaterFogColor.colorValue = EditorGUILayout.ColorField ("Underwater Fog Color", underwaterFogColor.colorValue);
		underwaterFog.floatValue = EditorGUILayout.Slider ("Underwater Fog Density",  underwaterFog.floatValue,  0.0f, 0.2f);
		WaterTextureSpeed.vector2Value = EditorGUILayout.Vector2Field ("Water Texture Speed", WaterTextureSpeed.vector2Value);
		WaterBumpmapSpeed.vector2Value = EditorGUILayout.Vector2Field ("Water Bumpmap Speed", WaterBumpmapSpeed.vector2Value);
		DistortionMapSpeed.vector2Value = EditorGUILayout.Vector2Field ("Water Distortion Speed(X)", DistortionMapSpeed.vector2Value);
		DistortionMap_Speed.vector2Value = EditorGUILayout.Vector2Field ("Water Distortion Speed(Y)", DistortionMap_Speed.vector2Value);
		TransparencyOverWater.floatValue = EditorGUILayout.Slider ("Transparency Above Water",  TransparencyOverWater.floatValue,  0.5f, 1.0f);
		TransparencyUnderWater.floatValue = EditorGUILayout.Slider ("Transparency Under Water",  TransparencyUnderWater.floatValue,  0.5f, 0.95f);

		GUILayout.Space(5.0f);
        GUILayout.EndVertical();
		
		
		serObj.ApplyModifiedProperties();
		

	}
	}	
}

using UnityEngine;
using System.Collections;

namespace UnderWaterEffect {

public enum underwater { 
    UnderWater_1 = 0,
	UnderWater_2 = 1,
	UnderWater_3 = 2,
	UnderWater_4 = 3,	
}

[ExecuteInEditMode]
[RequireComponent(typeof(Plane))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[AddComponentMenu("Underwater Effects!/Water/Apply effects to water")]


public class UnderWater : MonoBehaviour {
	
	private UWEffects script;
	private UWClone clonescript;
	public Material AssignSkybox;
	public bool enableSkyFog = true;
	public Color skyFogColor = Color.grey;
	public float skyFog = 0.1f;
	private Color NoFogColor = Color.clear;
	private GameObject AssignCaustic;
	private GameObject WaterPlane;
	private Material Caustic;
	public bool enableCaustics = true;
	public Color CausticColor= Color.white;
	public float CausticBrightness = 0.01f;
	public float CausticSize = 0.01f;
	public float CausticSpeedX = 0.01f;
    public float CausticSpeedY = 0.01f;
	
	public underwater water;
	public Material material1;
	public Material material2;
	public Material material3;
	public Material material4;
	private Material Effect;
	public Color effectColor = new Color (0.086f, 0.216f, 0.714f);
	public float effectValue;
	private float m_UnderwaterCheckOffset = 0.000001F;
	public bool enableUnderwaterFog = true;
	public Color underwaterFogColor = Color.cyan;
	public float underwaterFog = 0.1f;
	public Vector2 WaterTextureSpeed = new Vector2(0.01F, 0.01F);
	public Vector2 WaterBumpmapSpeed = new Vector2(0.01F, 0.01F);
	public Vector2 DistortionMapSpeed = new Vector2(0.01F, 0.01F);
	public Vector2 DistortionMap_Speed = new Vector2(0.01F, 0.01F);
	private Vector2 textureUV;
	private Vector2 bumpMapUV;
	private Vector2 distortionMap1UV;
	private Vector2 distortionMap2UV;
	private bool InUnderwater = false;
	public Cubemap cubemap1;
	private float rot;
	private float FOWV = -1.0F;
	private float FUWV = 1.0F;
	public float TOWV = 0.99F;
	public float TUWV = 0.80F;

	protected virtual void Start ()
 
	{
	
		RenderSettings.fog =true;

		Caustic = Resources.Load("Material/Caustic") as Material;
		
		if (Caustic != null){
			Caustic.shader = Shader.Find("UnderWater/Caustics");
			}

		else {
			Resources.UnloadAsset(Caustic);
		
    	}

		AssignCaustic = GameObject.Find("Caustic");

		
		Effect = Resources.Load("Material/Effect") as Material;
		
		if (Effect != null){
			Effect.shader = Shader.Find("Hidden/UnderWater/Effects");
		}
		
		else {
			Resources.UnloadAsset(Effect);
					
		}	
		
		WaterPlane = GameObject.Find("Water");
		
		material1 = Resources.Load("Material/UnderWater_1") as Material;
		material2 = Resources.Load("Material/UnderWater_2") as Material;
		material3 = Resources.Load("Material/UnderWater_3") as Material;
		material4 = Resources.Load("Material/UnderWater_4") as Material;

			if (material1 != null && material2 != null && material3 != null && material4 != null){ 
		
		material1.shader = Shader.Find("UnderWater/UW_1");	
		material2.shader = Shader.Find("UnderWater/UW_2");
		material3.shader = Shader.Find("UnderWater/UW_3");
		material4.shader = Shader.Find("UnderWater/UW_4");
			
		switch(water)
			{
			case underwater.UnderWater_1:
				GetComponent<Renderer>().sharedMaterial = material1;
				break;
			case underwater.UnderWater_2:
				GetComponent<Renderer>().sharedMaterial = material2;
				break;
			case underwater.UnderWater_3:
				GetComponent<Renderer>().sharedMaterial = material3;
				break;
			case underwater.UnderWater_4:
				GetComponent<Renderer>().sharedMaterial = material4;
				break;
			}
		}
			
		else {
			Resources.UnloadAsset(material1);
			Resources.UnloadAsset(material2);
			Resources.UnloadAsset(material3);
			Resources.UnloadAsset(material4);
		}
		
	}
	void Update () { 


Caustic.SetColor("_Color", CausticColor);		
Caustic.SetFloat("_Brightness", CausticBrightness);
Caustic.SetFloat("_Size", CausticSize);

Caustic.SetFloat("_ScrollX", CausticSpeedX);		
Caustic.SetFloat("_ScrollY", CausticSpeedY);		
		
Effect.SetFloat("_EffectFadeValue", effectValue);
Effect.SetColor("_DepthEffectColor", effectColor);		

textureUV.x = Time.time * WaterTextureSpeed.x;
textureUV.y = Time.time * WaterTextureSpeed.y;
		
bumpMapUV.x = Time.time * WaterBumpmapSpeed.x;
bumpMapUV.y = Time.time * WaterBumpmapSpeed.y;			

distortionMap1UV.x = Time.time * DistortionMapSpeed.x;
distortionMap1UV.y = Time.time * DistortionMapSpeed.y;

distortionMap2UV.x = Time.time * DistortionMap_Speed.x;
distortionMap2UV.y = Time.time * DistortionMap_Speed.y;
		
		
GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", textureUV);
GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_BumpMap", bumpMapUV);            
GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_DistortionMap1", distortionMap1UV);		
GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_DistortionMap2", distortionMap2UV);		

}

	
	
	
	public bool IsUnderwater(Camera cam) {
		return cam.transform.position.y + m_UnderwaterCheckOffset < transform.position.y ? true : false;	
	}



	public void OnWillRenderObject()
	{
		
		Camera cam = Camera.main;
		
		if(IsUnderwater(cam)) 
		{
			if(Camera.main == cam && !cam.gameObject.GetComponent(typeof(UWEffects)) ) {
					cam.gameObject.AddComponent(typeof(UWEffects));	
				}
				
				UWEffects effect = (UWEffects)cam.gameObject.GetComponent(typeof(UWEffects));				
				if(effect) {
					effect.enabled = true;
				
				}
				

				if 
					(!WaterPlane.gameObject.GetComponent(typeof(UWClone))){
						WaterPlane.gameObject.AddComponent(typeof(UWClone));
					}


				transform.localScale = new Vector3(200.0f, -1.0f, 200.0f);
				GetComponent<Renderer>().sharedMaterial.SetTexture("_Cube",cubemap1);
				Quaternion rot = Quaternion.Euler (0, 180, 0);
				Matrix4x4 m = new Matrix4x4 ();
				m.SetTRS(Vector3.zero, rot,new Vector3(1,1,1) );
				GetComponent<Renderer>().sharedMaterial.SetMatrix ("_Rotation", m);
				GetComponent<Renderer>().sharedMaterial.SetFloat("_Flip", FUWV);
				GetComponent<Renderer>().sharedMaterial.SetFloat("_ReflectPower", TUWV);
				GetComponent<Renderer>().sharedMaterial.shader.maximumLOD = 50;	
				
			
				if(!InUnderwater){
					
					InUnderwater = true;
					
				
				if(!AssignCaustic.gameObject.GetComponent(typeof(Projector)))  {
					AssignCaustic.gameObject.AddComponent(typeof(Projector)); 
						
				
				
				Projector proj = (Projector)AssignCaustic.gameObject.GetComponent(typeof(Projector));				
				if(proj) {
					proj.enabled = true;
					proj.orthographic = true;
					proj.orthographicSize = 500.0f;
					proj.material = (Caustic);
				 }
				}
				if(!AssignCaustic.gameObject.GetComponent(typeof(UWCaustics))) {
					AssignCaustic.gameObject.AddComponent(typeof(UWCaustics));
				
					
				
				UWCaustics animated = (UWCaustics)AssignCaustic.gameObject.GetComponent(typeof(UWCaustics));				
				if(animated) {
					animated.enabled = true;
	
				 }
				} 
				
					
					//Enable caustic
					if(enableCaustics) {
					enableCaustics = true;
					
						 	
					AssignCaustic.GetComponent<Projector>().enabled = true;
					
				}
				else{
					AssignCaustic.GetComponent<Projector>().enabled = false;
				}
				
								
				if(enableUnderwaterFog) {
					enableUnderwaterFog = true;
					
					//Enable underwater fog
					RenderSettings.fog = true;
					RenderSettings.fogMode = FogMode.ExponentialSquared;
					RenderSettings.fogDensity = underwaterFog;
					RenderSettings.fogColor = underwaterFogColor;
					cam.clearFlags = CameraClearFlags.SolidColor;
					cam.backgroundColor = underwaterFogColor;
				}
				else{
					RenderSettings.fog = false;
					RenderSettings.fogDensity = 0.0f;
					RenderSettings.fogColor = NoFogColor;
					cam.clearFlags = CameraClearFlags.SolidColor;
					cam.backgroundColor = underwaterFogColor;
					
				}
					
			}
		}
		else{
			
			UWEffects effect = (UWEffects)cam.gameObject.GetComponent(typeof(UWEffects));
				if(effect && effect.enabled) {
					effect.enabled = false;
				}
			
				transform.localScale = new Vector3(200.0f, 1.0f, 200.0f);
				GetComponent<Renderer>().sharedMaterial.SetTexture("_Cube",cubemap1);
				Quaternion rot = Quaternion.Euler (0, 0, 0);
				Matrix4x4 m = new Matrix4x4 ();
				m.SetTRS(Vector3.zero, rot,new Vector3(1,1,1) );
				GetComponent<Renderer>().sharedMaterial.SetMatrix ("_Rotation", m);
				GetComponent<Renderer>().sharedMaterial.SetFloat("_Flip", FOWV);
				GetComponent<Renderer>().sharedMaterial.SetFloat("_ReflectPower", TOWV);
				GetComponent<Renderer>().sharedMaterial.shader.maximumLOD = 100;	
				
			
				if(InUnderwater){
				
					InUnderwater = false;	
					
					//Disable caustic
				AssignCaustic.GetComponent<Projector>().enabled = false;
				
				
					
				
					if(enableSkyFog) {
					enableSkyFog = true;
				
					//Disable underwater fog
					RenderSettings.fog = true;
					RenderSettings.fogMode = FogMode.ExponentialSquared;
					RenderSettings.fogDensity = skyFog;
					RenderSettings.fogColor = skyFogColor;
					RenderSettings.skybox = AssignSkybox;
					cam.clearFlags = CameraClearFlags.Skybox;
					cam.GetComponent<Skybox>();
				
				}else{
					
					RenderSettings.fog = false;
					RenderSettings.fogDensity = 0.0f;
					RenderSettings.fogColor = NoFogColor;
					RenderSettings.skybox = AssignSkybox;
					cam.clearFlags = CameraClearFlags.Skybox;
					cam.GetComponent<Skybox>();
				}					
							
				}
			}
		}
	}
}
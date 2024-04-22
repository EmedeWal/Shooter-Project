/*=========================================================
	PARTICLE PRO FX volume one 
	PPFXScale.cs
	
	Scale Particle variables according to scale factor.
	Runs only in editor.
	
	(c) 2014
=========================================================*/

using UnityEngine; 
using System.Collections; 

#if UNITY_EDITOR 
using UnityEditor; 
#endif 

[ExecuteInEditMode] 
public class PPFXScale : MonoBehaviour { 
	
	//default value
	[SerializeField] private float _particleScale = 1.0f; 
	
	void Update () 
	{ 
		#if UNITY_EDITOR 
		//Update scale
		if (_particleScale > 0)
		{
			//scale gameobject
			transform.localScale = new Vector3(_particleScale, _particleScale, _particleScale); 

			//scale legacy particle systems 
			#if !UNITY_5_5_OR_NEWER
			ScaleLegacySystems(scaleFactor); 
			#endif
			//scale shuriken particle systems 
			ScaleShurikenSystems(_particleScale); 
			//scale trail renders 
			ScaleTrailRenderers(_particleScale); 
			//scale shockwave 
			ScaleShockwave(_particleScale);
		} 
		#endif 
	}

    //void ScaleShurikenSystems(float _scaleFactor)
    //{ 
    //	#if UNITY_EDITOR 
    //	//get all shuriken systems
    //	ParticleSystem[] _systems = GetComponentsInChildren<ParticleSystem>(); 

    //	foreach (ParticleSystem _system in _systems)
    //	{ 		
    //		//scale global values
    //		#if UNITY_5_5_OR_NEWER
    //			var _s = _system.main.startSpeed;
    //			_s.curveMultiplier *= _scaleFactor; 

    //			var _s2 = _system.main.startSize;
    //			_s2.curveMultiplier *= _scaleFactor; 

    //			var _s3 = _system.main.gravityModifier;
    //			_s3.curveMultiplier *= _scaleFactor; 
    //		#else
    //			_system.startSpeed *= _scaleFactor; 
    //			_system.startSize *= _scaleFactor; 
    //			_system.gravityModifier *= _scaleFactor; 
    //		#endif

    //		//use serialized objects to access particle values
    //		SerializedObject _so = new SerializedObject(_system); 

    //		_so.FindProperty("ShapeModule.radius").floatValue *= _scaleFactor; 
    //		_so.FindProperty("ShapeModule.boxX").floatValue *= _scaleFactor;
    //		_so.FindProperty("ShapeModule.boxY").floatValue *= _scaleFactor;
    //		_so.FindProperty("ShapeModule.boxZ").floatValue *= _scaleFactor;
    //		_so.FindProperty("VelocityModule.x.scalar").floatValue *= _scaleFactor;
    //		_so.FindProperty("VelocityModule.y.scalar").floatValue *= _scaleFactor;
    //		_so.FindProperty("VelocityModule.z.scalar").floatValue *= _scaleFactor;
    //		_so.FindProperty("ClampVelocityModule.magnitude.scalar").floatValue *= _scaleFactor;
    //		_so.FindProperty("ClampVelocityModule.x.scalar").floatValue *= _scaleFactor;
    //		_so.FindProperty("ClampVelocityModule.y.scalar").floatValue *= _scaleFactor;
    //		_so.FindProperty("ClampVelocityModule.z.scalar").floatValue *= _scaleFactor;
    //		_so.FindProperty("ForceModule.x.scalar").floatValue *= _scaleFactor;
    //		_so.FindProperty("ForceModule.y.scalar").floatValue *= _scaleFactor;
    //		_so.FindProperty("ForceModule.z.scalar").floatValue *= _scaleFactor;
    //		_so.FindProperty("ColorBySpeedModule._range").vector2Value *= _scaleFactor;
    //		_so.FindProperty("SizeBySpeedModule._range").vector2Value *= _scaleFactor;
    //		_so.FindProperty("RotationBySpeedModule._range").vector2Value *= _scaleFactor;
    //		_so.ApplyModifiedProperties();
    //	} 
    //	#endif 
    //} 

    void ScaleShurikenSystems(float _scaleFactor)
    {
#if UNITY_EDITOR
        ParticleSystem[] _systems = GetComponentsInChildren<ParticleSystem>();

        foreach (ParticleSystem _system in _systems)
        {
            // Scale main module properties
            var main = _system.main;
            main.startSpeedMultiplier *= _scaleFactor;
            main.startSizeMultiplier *= _scaleFactor;
            main.gravityModifierMultiplier *= _scaleFactor;

            // Use serialized objects to access and scale other particle system modules
            SerializedObject _so = new SerializedObject(_system);

            //ScaleSerializedProperty(_so, "ShapeModule.radius", _scaleFactor);
            //ScaleSerializedProperty(_so, "ShapeModule.boxX", _scaleFactor);
            //ScaleSerializedProperty(_so, "ShapeModule.boxY", _scaleFactor);
            //ScaleSerializedProperty(_so, "ShapeModule.boxZ", _scaleFactor);
            ScaleSerializedProperty(_so, "VelocityModule.x.scalar", _scaleFactor);
            ScaleSerializedProperty(_so, "VelocityModule.y.scalar", _scaleFactor);
            ScaleSerializedProperty(_so, "VelocityModule.z.scalar", _scaleFactor);
            ScaleSerializedProperty(_so, "ClampVelocityModule.magnitude.scalar", _scaleFactor);
            ScaleSerializedProperty(_so, "ClampVelocityModule.x.scalar", _scaleFactor);
            ScaleSerializedProperty(_so, "ClampVelocityModule.y.scalar", _scaleFactor);
            ScaleSerializedProperty(_so, "ClampVelocityModule.z.scalar", _scaleFactor);
            ScaleSerializedProperty(_so, "ForceModule.x.scalar", _scaleFactor);
            ScaleSerializedProperty(_so, "ForceModule.y.scalar", _scaleFactor);
            ScaleSerializedProperty(_so, "ForceModule.z.scalar", _scaleFactor);
            ScaleSerializedProperty(_so, "ColorBySpeedModule.range", _scaleFactor, isVector2: true);
            ScaleSerializedProperty(_so, "SizeBySpeedModule.range", _scaleFactor, isVector2: true);
            ScaleSerializedProperty(_so, "RotationBySpeedModule.range", _scaleFactor, isVector2: true);

            _so.ApplyModifiedProperties();
        }
#endif
    }

    void ScaleSerializedProperty(SerializedObject so, string propertyName, float scaleFactor, bool isVector2 = false)
    {
        SerializedProperty prop = so.FindProperty(propertyName);
        if (prop != null)
        {
            if (isVector2 && prop.propertyType == SerializedPropertyType.Vector2)
            {
                prop.vector2Value *= scaleFactor;
            }
            else if (!isVector2 && prop.propertyType == SerializedPropertyType.Float)
            {
                prop.floatValue *= scaleFactor;
            }
            else
            {
                Debug.LogError("Property type mismatch or incorrect property name: " + propertyName);
            }
        }
        else
        {
            Debug.LogError("Failed to find property: " + propertyName);
        }
    }

#if !UNITY_5_5_OR_NEWER
	void ScaleLegacySystems(float _scaleFactor) 
	{ 
#if UNITY_EDITOR
		//get all emitters 
		
		ParticleEmitter[] _emitters = GetComponentsInChildren<ParticleEmitter>(); 
		//get all animators
		ParticleAnimator[] _animators = GetComponentsInChildren<ParticleAnimator>(); 
		//apply scaling to emitters 
		foreach (ParticleEmitter _emitter in _emitters) 
		{ 
			//scale values
			_emitter.minSize *= _scaleFactor; 
			_emitter.maxSize *= _scaleFactor; 
			_emitter.worldVelocity *= _scaleFactor; 
			_emitter.localVelocity *= _scaleFactor; 
			_emitter.rndVelocity *= _scaleFactor; 
			
			//acces other values through a serialized object 
			SerializedObject _so = new SerializedObject(_emitter); 
			_so.FindProperty("m_Ellipsoid").vector3Value *= _scaleFactor; 
			_so.FindProperty("tangentVelocity").vector3Value *= _scaleFactor; 
			_so.ApplyModifiedProperties(); } //apply scaling to animators 
			
			//apply scaling
			foreach (ParticleAnimator _animator in _animators) 
			{ 
				_animator.force *= _scaleFactor; 
				_animator.rndForce *= _scaleFactor; 
			} 
#endif
	} 
#endif

    void ScaleTrailRenderers(float _scaleFactor) 
	{ 
		//get all animators
		TrailRenderer[] _trails = GetComponentsInChildren<TrailRenderer>(); 
		
		//apply scaling
		foreach (TrailRenderer _trail in _trails)
		{ 
			_trail.startWidth *= _scaleFactor; 
			_trail.endWidth *= _scaleFactor;
		} 
	} 
	
	
	void ScaleShockwave(float _scaleFactor)
	{		
		//get all shockwave components
		PPFXShockwave _swave = GetComponentInChildren<PPFXShockwave>();
		
		//apply scaling
		if(_swave!=null)
		{
			_swave.scale *= _scaleFactor;
		}
	}
	

}

﻿// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;

namespace Fungus
{
    /// <summary>
    /// Randomly shakes a GameObject's rotation by a diminishing amount over time.
    /// </summary>
    [CommandInfo("iTween", 
                 "Shake Scale", 
                 "Randomly shakes a GameObject's rotation by a diminishing amount over time.")]
    [AddComponentMenu("")]
    [ExecuteInEditMode]
    public class ShakeScale : iTweenCommand
    {
        [Tooltip("A scale offset in space the GameObject will animate to")]
        [SerializeField] protected Vector3Data _amount;

        #region Public members

        public override void DoTween()
        {
            Hashtable tweenParams = new Hashtable();
            tweenParams.Add("name", _tweenName.Value);
            tweenParams.Add("amount", _amount.Value);
            tweenParams.Add("time", _duration.Value);
            tweenParams.Add("easetype", easeType);
            tweenParams.Add("looptype", loopType);
            tweenParams.Add("oncomplete", "OniTweenComplete");
            tweenParams.Add("oncompletetarget", gameObject);
            tweenParams.Add("oncompleteparams", this);
            iTween.ShakeScale(_targetObject.Value, tweenParams);
        }

        public override bool HasReference(Variable variable)
        {
            return _amount.vector3Ref == variable || base.HasReference(variable);
        }

        #endregion

        #region Backwards compatibility

        [HideInInspector] [FormerlySerializedAs("amount")] public Vector3 amountOLD;

        protected override void OnEnable()
        {
            base.OnEnable();

            if (amountOLD != default(Vector3))
            {
                _amount.Value = amountOLD;
                amountOLD = default(Vector3);
            }
        }

        #endregion
    }    
}
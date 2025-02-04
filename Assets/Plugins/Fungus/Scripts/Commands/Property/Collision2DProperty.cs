﻿/*This script has been, partially or completely, generated by the Fungus.GenerateVariableWindow*/
using UnityEngine;


namespace Fungus
{
    // <summary>
    /// Get or Set a property of a Collision2D component
    /// </summary>
    [CommandInfo("Property",
                 "Collision2D",
                 "Get or Set a property of a Collision2D component")]
    [AddComponentMenu("")]
    public class Collision2DProperty : BaseVariableProperty
    {
		//generated property
        public enum Property 
        { 
            Rigidbody, 
            OtherRigidbody, 
            Transform, 
            GameObject, 
            RelativeVelocity, 
            Enabled, 
            Collider, 
            OtherCollider, 
            ContactCount, 
        }

		
        [SerializeField]
        protected Property property;
		
        [SerializeField]
        [VariableProperty(typeof(Collision2DVariable))]
        protected Collision2DVariable collision2DVar;

        [SerializeField]
        [VariableProperty(typeof(Collider2DVariable),
                          typeof(Rigidbody2DVariable),
                          typeof(TransformVariable),
                          typeof(GameObjectVariable),
                          typeof(Vector2Variable),
                          typeof(BooleanVariable),
                          typeof(IntegerVariable))]
        protected Variable inOutVar;

        public override void OnEnter()
        {
            var ioc2d = inOutVar as Collider2DVariable;
            var iorb2d = inOutVar as Rigidbody2DVariable;
            var iot = inOutVar as TransformVariable;
            var iogo = inOutVar as GameObjectVariable;
            var iov2 = inOutVar as Vector2Variable;
            var iob = inOutVar as BooleanVariable;
#if UNITY_2019_2_OR_NEWER
            var ioi = inOutVar as IntegerVariable;
#endif


            var target = collision2DVar.Value;

            switch (getOrSet)
            {
                case GetSet.Get:
                    switch (property)
                    {
                        case Property.Collider:
                            ioc2d.Value = target.collider;
                            break;
                        case Property.OtherCollider:
                            ioc2d.Value = target.otherCollider;
                            break;
                        case Property.Rigidbody:
                            iorb2d.Value = target.rigidbody;
                            break;
                        case Property.OtherRigidbody:
                            iorb2d.Value = target.otherRigidbody;
                            break;
                        case Property.Transform:
                            iot.Value = target.transform;
                            break;
                        case Property.GameObject:
                            iogo.Value = target.gameObject;
                            break;
                        case Property.RelativeVelocity:
                            iov2.Value = target.relativeVelocity;
                            break;
                        case Property.Enabled:
                            iob.Value = target.enabled;
                            break;
#if UNITY_2019_2_OR_NEWER
                        case Property.ContactCount:
                            ioi.Value = target.contactCount;
                            break;
#endif
                        default:
                            Debug.Log("Unsupported get or set attempted");
                            break;
                    }

                    break;
                case GetSet.Set:
                    switch (property)
                    {
                        default:
                            Debug.Log("Unsupported get or set attempted");
                            break;
                    }

                    break;
                default:
                    break;
            }

            Continue();
        }

        public override string GetSummary()
        {
            if (collision2DVar == null)
            {
                return "Error: no collision2DVar set";
            }
            if (inOutVar == null)
            {
                return "Error: no variable set to push or pull data to or from";
            }

            return getOrSet.ToString() + " " + property.ToString();
        }

        public override Color GetButtonColor()
        {
            return new Color32(235, 191, 217, 255);
        }

        public override bool HasReference(Variable variable)
        {
            if (collision2DVar == variable || inOutVar == variable)
                return true;

            return false;
        }

    }
}
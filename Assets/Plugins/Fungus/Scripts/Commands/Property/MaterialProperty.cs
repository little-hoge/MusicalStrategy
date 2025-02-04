﻿/*This script has been, partially or completely, generated by the Fungus.GenerateVariableWindow*/
using UnityEngine;


namespace Fungus
{
    // <summary>
    /// Get or Set a property of a Material component
    /// </summary>
    [CommandInfo("Property",
                 "Material",
                 "Get or Set a property of a Material component")]
    [AddComponentMenu("")]
    public class MaterialProperty : BaseVariableProperty
    {
		//generated property
        public enum Property 
        { 
            Color, 
            MainTexture, 
            MainTextureOffset, 
            MainTextureScale, 
            RenderQueue, 
            DoubleSidedGI, 
            EnableInstancing, 
            PassCount, 
        }

		
        [SerializeField]
        protected Property property;
		
        [SerializeField]
        [VariableProperty(typeof(MaterialVariable))]
        protected MaterialVariable materialVar;

        [SerializeField]
        [VariableProperty(typeof(ColorVariable),
                          typeof(TextureVariable),
                          typeof(Vector2Variable),
                          typeof(IntegerVariable),
                          typeof(BooleanVariable))]
        protected Variable inOutVar;

        public override void OnEnter()
        {
            var iocol = inOutVar as ColorVariable;
            var iotex = inOutVar as TextureVariable;
            var iov2 = inOutVar as Vector2Variable;
            var ioi = inOutVar as IntegerVariable;
            var iob = inOutVar as BooleanVariable;


            var target = materialVar.Value;

            switch (getOrSet)
            {
                case GetSet.Get:
                    switch (property)
                    {
                        case Property.Color:
                            iocol.Value = target.color;
                            break;
                        case Property.MainTexture:
                            iotex.Value = target.mainTexture;
                            break;
                        case Property.MainTextureOffset:
                            iov2.Value = target.mainTextureOffset;
                            break;
                        case Property.MainTextureScale:
                            iov2.Value = target.mainTextureScale;
                            break;
                        case Property.RenderQueue:
                            ioi.Value = target.renderQueue;
                            break;
                        case Property.DoubleSidedGI:
                            iob.Value = target.doubleSidedGI;
                            break;
                        case Property.EnableInstancing:
                            iob.Value = target.enableInstancing;
                            break;
                        case Property.PassCount:
                            ioi.Value = target.passCount;
                            break;
                        default:
                            Debug.Log("Unsupported get or set attempted");
                            break;
                    }

                    break;
                case GetSet.Set:
                    switch (property)
                    {
                        case Property.Color:
                            target.color = iocol.Value;
                            break;
                        case Property.MainTexture:
                            target.mainTexture = iotex.Value;
                            break;
                        case Property.MainTextureOffset:
                            target.mainTextureOffset = iov2.Value;
                            break;
                        case Property.MainTextureScale:
                            target.mainTextureScale = iov2.Value;
                            break;
                        case Property.RenderQueue:
                            target.renderQueue = ioi.Value;
                            break;
                        case Property.DoubleSidedGI:
                            target.doubleSidedGI = iob.Value;
                            break;
                        case Property.EnableInstancing:
                            target.enableInstancing = iob.Value;
                            break;
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
            if (materialVar == null)
            {
                return "Error: no materialVar set";
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
            if (materialVar == variable || inOutVar == variable)
                return true;

            return false;
        }

    }
}
using UnityEngine;
using System.Collections.Generic;
using DefaultNamespace.ArmController;
using Unity.VisualScripting;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

namespace DefaultNamespace.UI
{
    public class HoldingPartUI : UIDisplayer
    {
        [SerializeField] private List<Texture2D> texturesForHoldingPart;

        private VisualElement _holdingPartUILeft;
        private VisualElement _holdingPartUIRight;


        private const string PART_HOLDER_ROOT_NAME = "Part_Holder_Root";
        private const string PART_HOLDER_VISUAL_LEFT_NAME = "Left_Holder";
        private const string PART_HOLDER_VISUAL_RIGHT_NAME = "Right_Holder";
        
        protected override string ROOT_NAME => PART_HOLDER_ROOT_NAME;

        protected override void FindUIReferences()
        {
            base.FindUIReferences();
            _holdingPartUILeft = FindVisualElement<VisualElement>(PART_HOLDER_VISUAL_LEFT_NAME);
            _holdingPartUIRight = FindVisualElement<VisualElement>(PART_HOLDER_VISUAL_RIGHT_NAME);
            Open();
        }
        
        public void SetHoldingPartUI(EArmPosition armPosition, EPartType partType)
        {
            int textureIndex = (int)partType;
            if (armPosition == EArmPosition.LEFT)
            {
                _holdingPartUILeft.style.backgroundImage = Background.FromTexture2D(texturesForHoldingPart[textureIndex]);
            }
            else
            {
                _holdingPartUIRight.style.backgroundImage = Background.FromTexture2D(texturesForHoldingPart[textureIndex]);
            }
        }

        public void VoidHoldingPartUI(EArmPosition armPosition)
        {
            if (armPosition == EArmPosition.LEFT)
            {
                _holdingPartUILeft.style.backgroundImage = default;
            }
            else
            {
                _holdingPartUIRight.style.backgroundImage = default;
            }
        }
    }
}
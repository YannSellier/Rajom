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
        [SerializeField] private List<Texture2D> texturesForHoldingHead;

        private VisualElement _holdingPartUILeft;
        private VisualElement _holdingPartUIRight;
        private VisualElement _holdingHeadUILeft;
        private VisualElement _holdingHeadUIRight;


        private const string PART_HOLDER_ROOT_NAME = "Part_Holder_Root";
        private const string PART_HOLDER_VISUAL_LEFT_NAME = "LeftPart_Holder";
        private const string HEAD_HOLDER_VISUAL_LEFT_NAME = "LeftHead_Holder";
        private const string PART_HOLDER_VISUAL_RIGHT_NAME = "RightPart_Holder";
        private const string HEAD_HOLDER_VISUAL_RIGHT_NAME = "RightHead_Holder";
        
        protected override string ROOT_NAME => PART_HOLDER_ROOT_NAME;

        protected override void FindUIReferences()
        {
            base.FindUIReferences();
            _holdingPartUILeft = FindVisualElement<VisualElement>(PART_HOLDER_VISUAL_LEFT_NAME);
            _holdingHeadUILeft = FindVisualElement<VisualElement>(HEAD_HOLDER_VISUAL_LEFT_NAME);
            _holdingPartUIRight = FindVisualElement<VisualElement>(PART_HOLDER_VISUAL_RIGHT_NAME);
            _holdingHeadUIRight = FindVisualElement<VisualElement>(HEAD_HOLDER_VISUAL_RIGHT_NAME);
            
            Open();
        }
        
        public void SetHoldingPartUI(EArmPosition armPosition, Part part)
        {
            EPartType partType = part?.GetPartType() ?? EPartType.ALIM;
            
            int textureIndex = (int)partType;
            if (armPosition == EArmPosition.LEFT)
            {
                _holdingPartUILeft.style.display = part != null ? DisplayStyle.Flex : DisplayStyle.None;
                _holdingPartUILeft.style.backgroundImage = Background.FromTexture2D(texturesForHoldingPart[textureIndex]);
            }
            else
            {
                _holdingPartUIRight.style.display = part != null ? DisplayStyle.Flex : DisplayStyle.None;
                _holdingPartUIRight.style.backgroundImage = Background.FromTexture2D(texturesForHoldingPart[textureIndex]);
            }
        }
        public void SetHoldingHeadUI(EArmPosition armPosition, HeadPickUp headPickUp)
        {

            EHeadType headType = headPickUp?.getHead()?.GetHeadType() ?? EHeadType.HAMMER;
            
            int textureIndex = (int)headType;
            if (armPosition == EArmPosition.LEFT)
            {
                _holdingHeadUILeft.style.display = headPickUp != null ? DisplayStyle.Flex : DisplayStyle.None;
                _holdingHeadUILeft.style.backgroundImage = Background.FromTexture2D(texturesForHoldingHead[textureIndex]);
            }
            else
            {
                _holdingHeadUIRight.style.display = headPickUp != null ? DisplayStyle.Flex : DisplayStyle.None;
                _holdingHeadUIRight.style.backgroundImage = Background.FromTexture2D(texturesForHoldingHead[textureIndex]);
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
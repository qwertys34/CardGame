using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.scripts
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CardMovementScr : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private float maxYPos;
        public Transform defaultParent, defaultTempCardParent;
        private Camera _camera;
        private Vector3 _offset;
        private GameObject _tempCardGO;
        private float _startClickPosY;
        private bool _isDraggable;
        
        private void Awake()
        {
            _tempCardGO = GameObject.Find("TempCardGO");
            _camera = Camera.main;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            var placeType = transform.parent.GetComponent<DropPlaceScr>().Type;
            _isDraggable = placeType is PlaceType.SelfHand or PlaceType.SelfField
                           && GameManagerScr.G.IsPlayerTurn;
            if (!_isDraggable)
                return;
            
            _offset = transform.position - _camera.ScreenToWorldPoint(eventData.position);
            
            defaultParent = defaultTempCardParent = transform.parent;
            
            _tempCardGO.transform.SetParent(defaultParent);
            _tempCardGO.transform.SetSiblingIndex(transform.GetSiblingIndex());
            
            transform.SetParent(defaultParent.parent);
            GetComponent<CanvasGroup>().blocksRaycasts = false;
            _startClickPosY = transform.position.y;
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            if (!_isDraggable)
                return;
            
            var cardPos = _camera.ScreenToWorldPoint(eventData.position);
            transform.position = cardPos + _offset;

            if (_tempCardGO.transform.parent != defaultTempCardParent)
                _tempCardGO.transform.SetParent(defaultTempCardParent);
                
            CheckPosition();
        }
    
        public void OnEndDrag(PointerEventData eventData)
        {
            if (!_isDraggable)
                return;
            
            transform.SetParent(defaultParent);
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            
            transform.SetSiblingIndex(_tempCardGO.transform.GetSiblingIndex());
            _tempCardGO.transform.SetParent(GameObject.Find("Canvas").transform);
            _tempCardGO.transform.localPosition = new Vector3(2115, 0, 0);
        }

        private void CheckPosition()
        {
            int newIndex = defaultTempCardParent.childCount;

            for (int i = 0; i < defaultTempCardParent.childCount; i++)
            {
                if (transform.position.x < defaultTempCardParent.GetChild(i).position.x
                    && transform.position.y - _startClickPosY < maxYPos)
                {
                    newIndex = i;

                    if (_tempCardGO.transform.GetSiblingIndex() < newIndex)
                        newIndex--;

                    break;
                }
            }
            
            _tempCardGO.transform.SetSiblingIndex(newIndex);
        }
    }
}

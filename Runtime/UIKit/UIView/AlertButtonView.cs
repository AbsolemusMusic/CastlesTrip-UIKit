using UnityEngine;
using UnityEngine.UI;

namespace CastlesTrip.UIKit
{
    public class AlertButtonView : MonoBehaviour
    {
        [SerializeField]
        private Button mainButton;

        [SerializeField]
        private Text text;

        private IAlertButtonView m_delegate;

        public void Init(IAlertButtonView _del, AnswerAlertData data)
        {
            m_delegate = _del;
            text.text = data.answerText;
            text.color = data.textColor;
        }

        private void Awake() => mainButton.onClick.AddListener(() => m_delegate?.OnMainTapped(this));

        private void OnDestroy() => mainButton.onClick.RemoveListener(() => m_delegate?.OnMainTapped(this));
    }

    public interface IAlertButtonView
    {
        public void OnMainTapped(AlertButtonView view);
    }
}
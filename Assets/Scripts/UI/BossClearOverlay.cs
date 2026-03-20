using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Platformer.UI
{
    public class BossClearOverlay : MonoBehaviour
    {
        static BossClearOverlay instance;
        GUIStyle titleStyle;
        GUIStyle buttonStyle;

        public static void Show()
        {
            if (instance != null)
                return;

            var root = new GameObject("BossClearOverlay");
            instance = root.AddComponent<BossClearOverlay>();
            instance.BuildUI();
        }

        void BuildUI()
        {
            EnsureEventSystem();

            var canvas = gameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 1000;

            gameObject.AddComponent<CanvasScaler>();
            gameObject.AddComponent<GraphicRaycaster>();

            var panel = CreateUIObject("Panel", gameObject.transform);
            var panelImage = panel.AddComponent<Image>();
            panelImage.color = new Color(0f, 0f, 0f, 0.75f);
            StretchToParent(panel.GetComponent<RectTransform>());

            Debug.Log("BossClearOverlay.BuildUI (OnGUI mode)");
        }

        void OnGUI()
        {
            EnsureGuiStyles();

            var width = Screen.width;
            var height = Screen.height;

            var titleRect = new Rect(0f, height * 0.26f, width, 90f);
            GUI.Label(titleRect, "Thanks for playing!", titleStyle);

            var buttonWidth = 320f;
            var buttonHeight = 80f;
            var buttonRect = new Rect((width - buttonWidth) * 0.5f, height * 0.42f, buttonWidth, buttonHeight);
            if (GUI.Button(buttonRect, "Back to Main", buttonStyle))
                BackToMain();
        }

        void EnsureGuiStyles()
        {
            if (titleStyle != null && buttonStyle != null)
                return;

            titleStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 48,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = Color.white }
            };

            buttonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 28,
                alignment = TextAnchor.MiddleCenter
            };
        }

        static GameObject CreateUIObject(string name, Transform parent)
        {
            var obj = new GameObject(name);
            obj.transform.SetParent(parent, false);
            obj.AddComponent<RectTransform>();
            return obj;
        }

        static void StretchToParent(RectTransform rectTransform)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.anchoredPosition = Vector2.zero;
        }

        static void EnsureEventSystem()
        {
            if (FindObjectOfType<EventSystem>() != null)
                return;

            var eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }

        void BackToMain()
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}

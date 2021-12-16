using System.Collections.Generic;
using Bencodex.Types;
using Libplanet;
using Nekoyume.BlockChain;
using Nekoyume.Game;
using Nekoyume.Model.State;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using Event = UnityEngine.Event;

namespace Editor
{
    public class StateViewer : EditorWindow
    {
        [SerializeField] TreeViewState treeViewState;

        private StateView stateView;
        private SearchField searchField;

        private string searchString;

        [MenuItem("Tools/Libplanet/View State")]
        private static void ShowWindow()
        {   
            var window = GetWindow<StateViewer>();
            window.titleContent = new GUIContent("State Viewer");
            window.Show();
        }

        private void OnEnable()
        {
            PreventNullElements();
        }

        private void OnGUI()
        {
            Rect rect = new Rect(0, 0, position.width, 20);
            PreventNullElements();

            var result = searchField.OnGUI(rect, searchString);
            searchString = result;

            DoProcess(searchString);

            rect.y += 20;
            rect.height = position.height - 20;

            stateView.OnGUI(rect);
        }

        private void DoProcess(string searchString)
        {
            var current = Event.current;
            if (current.type == EventType.KeyUp && current.keyCode == KeyCode.Return)
            {
                OnConfirm(searchString);
            }
        }

        private void OnConfirm(string searchString)
        {
            if (CheckPlaying())
            {
                RegisterAliases();
            }
        }

        private bool CheckPlaying()
        {
            if (!Application.isPlaying || !Game.instance.IsInitialized)
            {
                Debug.Log("You can use this feature in only play mode.");
                return false;
            }

            return true;
        }

        private void PreventNullElements()
        {
            if (searchField is null)
            {
                searchField = new SearchField();
            }

            if (treeViewState is null)
            {
                treeViewState = new TreeViewState();
            }

            if (stateView is null)
            {
                stateView = new StateView(treeViewState);
            }
        }

        private void RegisterAliases()
        {            
        }
    }
}

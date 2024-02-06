using UnityEngine;

namespace CT.UIKit
{
    public class Nib : MonoBehaviour
    {
        public const string NibVPath = "Prefabs/Views/";
        public const string NibVCPath = "Prefabs/ViewControllers/";
        public const string NibTVCPath = "Prefabs/TableViewCells/";

        public static T LoadViewNib<T>()
        {
            string path = NibVPath + typeof(T).ToString();
            GameObject nib = Instantiate(Resources.Load(path)) as GameObject;
            if (nib.TryGetComponent(out Canvas nibCanvas))
            {
                nibCanvas.worldCamera = Camera.main;
            }
            return nib.GetComponent<T>();
        }

        public static T LoadVCNib<T>()
        {
            string path = NibVCPath + typeof(T).ToString();
            GameObject nib = Instantiate(Resources.Load(path)) as GameObject;
            if (nib.TryGetComponent(out Canvas nibCanvas))
            {
                nibCanvas.worldCamera = Camera.main;
                nibCanvas.sortingOrder = GetNearOrderLayer();
            }
            return nib.GetComponent<T>();
        }

        public static T LoadCellNib<T>(UITableView tableView, IndexPath indexPath)
        {
            string path = NibTVCPath + typeof(T).ToString();
            GameObject nib = Instantiate(Resources.Load(path)) as GameObject;
            Transform nibTr = nib.transform;
            nibTr.SetParent(tableView.LayoutGroup.transform);
            nibTr.Reset();

            if (nib.TryGetComponent(out UITableViewCell cell))
            {
                cell.TVCDelegate = tableView;
                cell.Init(indexPath);
            }

            return nib.GetComponent<T>();
        }

        private static int GetNearOrderLayer()
        {
            Canvas[] all = FindObjectsOfType<Canvas>();
            int near = -1;
            foreach (Canvas canvas in all)
            {
                if (near >= canvas.sortingOrder)
                    continue;

                near = canvas.sortingOrder;
            }
            return near + 1;
        }
    }

}
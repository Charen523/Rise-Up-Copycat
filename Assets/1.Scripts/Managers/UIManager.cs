using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public enum eUIPosition
{
    Background,
    Popup
}

public class UIManager : Singleton<UIManager>
{
    private List<Transform> parents;
    [SerializeField] private List<UIBase> uiList = new List<UIBase>();

#pragma warning disable CS1998
    public async Task Init()
#pragma warning restore CS1998
    {

    }

    public static void SetParents(List<Transform> parents)
    {
        Instance.parents = parents;
        Instance.parents.Add(Instance.transform);
    }

    /// <typeparam name="T">UIBase�� ��ӹ��� Ŭ���� �̸�</typeparam>
    /// <param name="param">()�ȿ� �� ��� ��</param>
    /// <returns></returns>
    public async static Task<T> Show<T>(params object[] param) where T : UIBase
    {
        var ui = Instance.uiList.Find(obj => obj.name == typeof(T).ToString());

        if (ui == null)
        {
            var prefab = await ResourceManager.Instance.LoadAsset<T>(typeof(T).ToString(), eAddressableType.UI);
            ui = Instantiate(prefab, Instance.parents[(int)prefab.uiPosition]);
            ui.name = ui.name.Replace("(Clone)", "");

            Instance.uiList.Add(ui);
        }
        ui.opened.Invoke(param);
        ui.gameObject.SetActive(ui.isActiveInCreated);
        ui.isActiveInCreated = true;
        return (T)ui;
    }

    public static void Hide<T>(bool isPlay = true, params object[] param) where T : UIBase
    {
        var ui = Instance.uiList.Find(obj => obj.name == typeof(T).ToString());
        if (ui != null)
        {
            ui.closed.Invoke(param);
            if (ui.isDestroyAtClosed)
            {
                Instance.uiList.Remove(ui);
                Destroy(ui.gameObject);
            }
            else
            {
                ui.SetActive(false);
            }
        }
    }

    /// <summary>
    /// ���� ������ UI�� �������� �޼���
    /// </summary>
    /// <typeparam name="T">UI ��ũ��Ʈ �̸�</typeparam>
    /// <returns>UI ��ũ��Ʈ</returns>
    public static T Get<T>() where T : UIBase
    {
        return (T)Instance.uiList.Find(obj => obj.name == typeof(T).ToString());
    }

    public static bool IsOpened<T>() where T : UIBase
    {
        return Instance.uiList.Exists(obj => obj.name == typeof(T).ToString());
    }
}